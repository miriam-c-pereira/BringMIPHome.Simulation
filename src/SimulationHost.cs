#nullable enable
namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SimulationHost : ISimulationHost
    {
        public ISimulationTelemetry Telemetry => this.telemetry;

        public event EventHandler<SimulationEvent>? SimulationEvent;

        private readonly ChargingStationAnimationConfig chargingStationAnimationConfig;
        private readonly SimConfig config;
        private readonly SimUiConfig simUiConfig;
        private readonly Random random;
        private readonly SimState state;
        private readonly SimulationTelemetry telemetry;
        private ActionType currentAction = ActionType.None;
        private SimState stateBeforeAction = new SimState();
        private readonly PermutationGenerator permutationGenerator;
        private ExtractionOutcome? lastExtractionOutcome;
        private bool isInitialized;

        // Injected interfaces for decoupled communication
        private IRoverController? roverController;
        private IEnumerable<IChargingStationController>? chargingStationControllers;
        private IMissionControlController? missionControlController;

        public SimulationHost(SimConfig simConfig, SimUiConfig simUiConfig, ChargingStationAnimationConfig animConfig)
        {
            if (simConfig == null)
            {
                throw new ArgumentNullException(nameof(simConfig));
            }

            this.chargingStationAnimationConfig = animConfig ?? throw new ArgumentNullException(nameof(animConfig));


            EnsureConfigIsValid(simConfig);

            this.config = simConfig;
            this.simUiConfig = simUiConfig;

            this.random = this.config.RandomSeed != null
                ? new Random(this.config.RandomSeed.Value)
                : new Random();

            this.permutationGenerator = new PermutationGenerator(this.random, this.config.Roles.Count);

            var batteries = new List<RoverBattery>
            {
                new RoverBattery { Id = 1, Energy = this.config.EnergyInit / 2.0f },
                new RoverBattery { Id = 2, Energy = this.config.EnergyInit / 2.0f },
            };

            // Generate a random assignment of role IDs to station indices.
            // The array index represents the station index, and the value at that index
            // is the role ID assigned to that station.
            // Example: [1, 0, 2] means:
            //   CurrentStation 0 -> Role 1
            //   CurrentStation 1 -> Role 0
            //   CurrentStation 2 -> Role 2
            var stationRoleIds = this.permutationGenerator.GetRandomCombination();

            var chargingStations = new List<StationState>();
            // Assign roles to all charging stations except
            // the Start location (the final charging station), as it is not assigned a role. 
            for (var index = 0; index < this.config.ChargingStations.Count - 1; index++)
            {
                var chargingStationParams = this.config.ChargingStations[index];

                var roleParams = this.config.Roles.First(x => x.Id == stationRoleIds[index]);
                chargingStations.Add(new StationState(chargingStationParams, roleParams!));
            }

            //add the Start location as the last station in the list, with no role assigned
            chargingStations.Add(new StationState(this.config.ChargingStations.Last(), null));

            var simState = new SimState
            {
                Batteries = batteries,
                TimeLeft = this.config.TimeInit,
                CurrentLocation = LocationType.Start,
                DoneReason = DoneReasonType.NotDone,
                ChargingStations = chargingStations,
            };

            this.state = simState;

            var currentStation = this.GetCurrentStation();

            this.telemetry = new SimulationTelemetry
            {
                TimeLeft = this.state.TimeLeft,
                DoneReason = this.state.DoneReason,
                ValidActions = this.GetValidActions(),
                CurrentAction = this.currentAction,
                rover = new RoverTelemetry
                {
                    LeftBatteryIsDetached = this.state.Batteries[0].IsDetached,
                    LeftBatteryEnergy = this.state.Batteries[0].Energy,
                    RightBatteryIsDetached = this.state.Batteries[1].IsDetached,
                    RightBatteryEnergy = this.state.Batteries[1].Energy,
                },
                currentStation = new StationTelemetry
                {
                    Location = currentStation.Location,
                    Accumulator = currentStation.Accumulator,
                }
            };
        }


        public void InitializeControllers(IRoverController rover, IEnumerable<IChargingStationController> chargingStations, IMissionControlController missionControl)
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("Controllers are already initialized.");
            }

            this.missionControlController = missionControl ?? throw new ArgumentNullException(nameof(missionControl));
            this.roverController = rover ?? throw new ArgumentNullException(nameof(rover));
            this.chargingStationControllers = chargingStations ?? throw new ArgumentNullException(nameof(chargingStations));

            this.missionControlController.Initialize(this);

            this.isInitialized = true;

            this.roverController.RoverEvent += this.OnRoverEvent;
            this.roverController.Initialize(this);

            foreach (var chargingStationController in this.chargingStationControllers)
            {
                var locationUiConfig = this.simUiConfig.Locations.FirstOrDefault(x => x.Location == chargingStationController.Location);
                if (locationUiConfig == null)
                {
                    throw new InvalidOperationException($"Location UI config for {chargingStationController.Location} not found.");
                }
                chargingStationController.ChargingStationEvent += OnChargingStationEvent;
                chargingStationController.Initialize(this, locationUiConfig, this.chargingStationAnimationConfig);
            }
        }


        public void Tick(float deltaTime)
        {
            this.state.TimeLeft = Math.Max(0, this.state.TimeLeft - deltaTime);
            this.telemetry.TimeLeft = this.state.TimeLeft;

            if (this.telemetry.TimeLeft <= 0f)
            {
                this.state.DoneReason = DoneReasonType.TimeExpired;
                this.telemetry.DoneReason = this.state.DoneReason;
                this.telemetry.ValidActions = this.GetValidActions();

                this.SimulationEvent?.Invoke(this, new DoneEvent
                {
                    Done = this.state.DoneReason,
                    TotalEnergy = this.telemetry.Rover.TotalBatteryEnergy,
                    TimeLeft = this.telemetry.TimeLeft
                });
            }
        }


        public bool TryStartAction(ActionType action)
        {
            if (!this.isInitialized)
            {
                throw new InvalidOperationException("Controllers are not initialized.");
            }

            if (this.telemetry.DoneReason != DoneReasonType.NotDone)
            {
                return false;
            }

            var validActions = this.GetValidActions();
            if (!validActions.Contains(action))
            {
                return false;
            }

            this.stateBeforeAction = this.state.GetSnapshot();

            this.telemetry.ValidActions = Array.Empty<ActionType>();

            this.currentAction = action;
            this.telemetry.CurrentAction = this.currentAction;

            this.ExecuteAction(action, true);

            return true;
        }




        private void OnChargingStationEvent(object sender, ChargingStationEvent e)
        {
            if (!this.isInitialized)
            {
                throw new InvalidOperationException("Controllers are not initialized.");
            }

            switch (e)
            {
                case ExtractionStartedEvent extractionStartedEvent:
                    break;

                case ExtractionStoppedEvent extractionStoppedEvent:
                    this.NotifyActionCompleted(ActionType.Extract, sender as IController);
                    break;

                case UploadingStartedEvent uploadingStartedEvent:
                    break;

                case UploadingStoppedEvent uploadingStoppedEvent:
                    this.NotifyActionCompleted(ActionType.Upload, sender as IController);
                    break;

                default:
                    break;
            }
        }

        private void OnRoverEvent(object sender, RoverEvent e)
        {
            if (!this.isInitialized)
            {
                throw new InvalidOperationException("Controllers are not initialized.");
            }

            switch (e)
            {
                case NavigationStartedEvent navigationStartedEvent:
                    break;

                case NavigationStoppedEvent navigationStoppedEvent:
                    
                    ActionType gotoAction;
                    switch (navigationStoppedEvent.Args.NavigationRoute.TargetLocation)
                    {
                        case LocationType.Station1:
                            gotoAction = ActionType.GoToStation1;
                            break;
                        
                        case LocationType.Station2:
                            gotoAction = ActionType.GoToStation2;
                            break;
                        
                        case LocationType.Station3:
                            gotoAction = ActionType.GoToStation3;
                            break;
                        
                        case LocationType.Station4:
                            gotoAction = ActionType.GoToStation4;
                            break;
                        
                        default:
                            throw new InvalidOperationException("Invalid navigation target location.");
                    }
                    
                    this.NotifyActionCompleted(gotoAction, sender as IController);
                    break;

                case PositionUpdatedEvent positionUpdatedEvent:
                    this.telemetry.rover.Position = positionUpdatedEvent.Position;
                    this.telemetry.rover.LinearVelocity = positionUpdatedEvent.Velocity;
                    break;

                case RotationUpdatedEvent rotationUpdatedEvent:
                    this.telemetry.rover.Heading = rotationUpdatedEvent.Heading;
                    //this.telemetry.rover.AngularVelocity = rotationUpdatedEvent.AngularVelocity;
                    break;

                default:
                    break;
            }
        }


        private void NotifyActionCompleted(ActionType action, IController? controller)
        {
            if (!this.isInitialized)
            {
                throw new InvalidOperationException("Controllers are not initialized.");
            }

            if (action != this.currentAction)
            {
                throw new InvalidOperationException($"Expected action {this.currentAction}, but received completion for {action}");
            }

            this.ExecuteAction(this.currentAction, false);

            var after = this.state.GetSnapshot();

            var actionEvent = new ActionEvent(this.stateBeforeAction, this.currentAction, after);

            this.SimulationEvent?.Invoke(this, actionEvent);

            this.currentAction = ActionType.None;

            //Note: avoid firing twice if the simulation is already done (TimeExpired)
            if (this.state.DoneReason == DoneReasonType.NotDone)
            {
                if (this.telemetry.Rover.TotalBatteryEnergy <= 0f)
                {
                    this.state.DoneReason = DoneReasonType.EnergyDepleted;
                }
                else if (this.config.TargetEnergy != null && this.telemetry.Rover.TotalBatteryEnergy >= this.config.TargetEnergy.Value)
                {
                    this.state.DoneReason = DoneReasonType.TargetEnergyReached;
                }

                this.telemetry.DoneReason = this.state.DoneReason;

                if (this.state.DoneReason != DoneReasonType.NotDone)
                {
                    this.SimulationEvent?.Invoke(this, new DoneEvent
                    {
                        Done = this.telemetry.DoneReason,
                        TotalEnergy = this.telemetry.Rover.TotalBatteryEnergy,
                        TimeLeft = this.telemetry.TimeLeft
                    });
                }
            }

            if (this.state.DoneReason == DoneReasonType.NotDone)
            {
                this.telemetry.ValidActions = this.GetValidActions();
            }
        }



        private void ExecuteAction(ActionType action, bool isStarting)
        {
            switch (action)
            {
                case ActionType.GoToStation1:
                    this.ExecuteGoto(ActionType.GoToStation1, isStarting, LocationType.Station1);
                    break;

                case ActionType.GoToStation2:
                    this.ExecuteGoto(ActionType.GoToStation2, isStarting, LocationType.Station2);
                    break;

                case ActionType.GoToStation3:
                    this.ExecuteGoto(ActionType.GoToStation3, isStarting, LocationType.Station3);
                    break;

                case ActionType.GoToStation4:
                    this.ExecuteGoto(ActionType.GoToStation4, isStarting, LocationType.Station4);
                    break;

                case ActionType.Extract:
                    this.ExecuteExtract(isStarting);
                    break;

                case ActionType.Upload:
                    this.ExecuteUpload(isStarting);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported action: {action} currentLocation={this.state.CurrentLocation}");
            }
        }

        private void ExecuteGoto(ActionType action, bool starting, LocationType newLocation)
        {
            if (starting)
            {
                //leaving the current station

                //reset its state for the next visit
                var departingStation = this.GetCurrentStation();
                departingStation.LastExtractOutcome = null;
                departingStation.TotalRewards = 0;
                departingStation.TotalFailures = 0;
                departingStation.TotalUploads = 0;
                departingStation.ConsecutiveFailures = 0;
                departingStation.ConsecutiveRewards = 0;
                departingStation.Accumulator = 0;

                //change the CurrentLocation 
                this.state.CurrentLocation = LocationType.None;

                UpdateStationTelemetry(this.telemetry.currentStation, new StationState());

                var locationUiConfig = this.simUiConfig.Locations.FirstOrDefault(x => x.Location == departingStation.Location);
                if (locationUiConfig == null)
                {
                    throw new InvalidOperationException($"No LocationUiConfig available for {departingStation.Location}.");
                }

                var navigationRoute = locationUiConfig.NavigationRoutes?.FirstOrDefault(l => l.TargetLocation == newLocation);
                if (navigationRoute != null)
                {
                    if (this.roverController == null)
                    {
                        throw new InvalidOperationException("Rover controller is not initialized.");
                    }

                    this.roverController.StartAction(action, new RoverGotoArgs { NavigationRoute = navigationRoute });
                }
                else
                {
                    throw new InvalidOperationException($"No NavigationRoute available to {newLocation}.");
                }
            }
            else
            {
                //arrived newLocation
                this.state.CurrentLocation = newLocation;
                var arrivedStation = this.GetCurrentStation();

                UpdateStationTelemetry(this.telemetry.currentStation, arrivedStation);
                this.AddEnergy(-this.config.GoToEnergyCost);
            }
        }


        private void ExecuteUpload(bool starting)
        {
            var currentStation = this.GetCurrentStation();
            if (starting)
            {
                var stationController = this.GetChargingStationController(currentStation.Location);
                var args = new UploadingArgs { ConsecutiveRewards = currentStation.ConsecutiveRewards };
                stationController.StartAction(ActionType.Upload, args);
            }
            else
            {
                this.AddEnergy(-this.config.UploadEnergyCost + currentStation.Accumulator);

                currentStation.Accumulator = 0f;
                currentStation.TotalUploads++;
                UpdateStationTelemetry(this.telemetry.currentStation, currentStation);
            }
        }


        private void ExecuteExtract(bool starting)
        {
            var currentStation = this.GetCurrentStation();

            if (starting)
            {
                if (currentStation.RoleParams == null)
                {
                    throw new InvalidOperationException($"Current station {currentStation.Location} has no role parameters.");
                }

                this.lastExtractionOutcome = this.GetExtractionOutcome(currentStation);

                var stationController = this.GetChargingStationController(currentStation.Location);
                var args = new ExtractionArgs
                {
                    Accumulator = this.lastExtractionOutcome.ExtractedAmount,
                    ConsecutiveRewards = this.lastExtractionOutcome.ConsecutiveRewards,
                    ConsecutiveFailures = this.lastExtractionOutcome.ConsecutiveFailures,
                };
                stationController.StartAction(ActionType.Extract, args);
            }
            else
            {
                if (this.lastExtractionOutcome == null)
                {
                    throw new InvalidOperationException("No extraction outcome available.");
                }

                this.AddEnergy(-this.config.ExtractEnergyCost);

                ApplyExtractionOutcome(currentStation, this.lastExtractionOutcome);
                UpdateStationTelemetry(this.telemetry.currentStation, currentStation);

                if (this.lastExtractionOutcome.BecameDepleted)
                {
                    this.ApplyDepletion();
                }
            }
        }

        private static void ApplyExtractionOutcome(StationState targetStation, ExtractionOutcome outcome)
        {
            if (outcome.ExtractedAmount > 0)
            {
                //reward was extracted
                targetStation.Accumulator = outcome.ExtractedAmount;
                targetStation.ConsecutiveRewards += 1;
                targetStation.ConsecutiveFailures = 0;
                targetStation.TotalRewards += 1;
            }
            else
            {
                //no reward was extracted

                targetStation.Accumulator = 0;
                targetStation.ConsecutiveRewards = 0;
                targetStation.ConsecutiveFailures += 1;
                targetStation.TotalFailures += 1;
            }
        }

        private ExtractionOutcome GetExtractionOutcome(StationState station)
        {
            if (station.RoleParams == null)
            {
                throw new InvalidOperationException($"Station {station.Location} has no role parameters.");
            }

            var extractionOutcome = new ExtractionOutcome { ExtractedAmount = 0, BecameDepleted = false };

            if (station.IsDepleted)
            {
                //Note: Do not mark BecameDepleted as true because IsDepleted is already true
                return extractionOutcome;
            }

            var isReward = this.random.NextDouble() < station.RoleParams.RewardProbability;
            if (isReward)
            {
                extractionOutcome.ExtractedAmount = station.Accumulator * station.RoleParams.RewardAccumulatorMul + station.RoleParams.RewardAccumulatorAdd;
                extractionOutcome.ConsecutiveRewards = station.ConsecutiveRewards + 1;
            }
            else
            {
                extractionOutcome.ExtractedAmount = 0;
                extractionOutcome.ConsecutiveFailures = station.ConsecutiveFailures + 1;
            }

            extractionOutcome.BecameDepleted = this.random.NextDouble() < station.RoleParams.DepletionSwitchProbability;


            return extractionOutcome;
        }


        private void ApplyDepletion()
        {
            var currentStation = this.GetCurrentStation();
            var currentStationIx = -1;

            //Last index is always the Start location, so we only iterate to Count-1
            for (var ix = 0; ix < this.state.ChargingStations.Count - 1; ix++)
            {
                var stationState = this.state.ChargingStations[ix];
                if (stationState == currentStation)
                {
                    currentStationIx = ix;
                    break;
                }
            }

            // Get the current roles of all charging stations except the Start location e.g. 1,0,2
            var currentRoles = this.state.ChargingStations.Where(x => x.Location != LocationType.Start).Select(x => x.RoleParams?.Id ?? 0).ToArray();
            var newReassignment = this.permutationGenerator.GetRandomFeasibleReassignment(currentRoles, currentStationIx);

            //Last index is always the Start location, so we only iterate to Count-1
            for (var ix = 0; ix < this.state.ChargingStations.Count - 1; ix++)
            {
                var newRoleId = newReassignment[ix];
                var newRole = this.config.Roles[newRoleId];
                if (newRole == null)
                {
                    throw new InvalidOperationException($"Role with ID {newRoleId} not found.");
                }

                var stationState = this.state.ChargingStations[ix];
                stationState.RoleParams = newRole;
            }
        }


        private void AddEnergy(float energyToAdd)
        {
            // Count only attached batteries
            var attached = this.state.Batteries.Count(b => !b.IsDetached);
            if (attached == 0)
            {
                return;
            }

            // Split the incoming energy equally
            var share = energyToAdd / attached;

            // Add the share to each attached battery
            for (var index = 0; index < this.state.Batteries.Count; index++)
            {
                var battery = this.state.Batteries[index];
                if (!battery.IsDetached)
                {
                    battery.Energy += share;

                    //update telemetry for the corresponding battery
                    switch (index)
                    {
                        case 0:
                            this.telemetry.rover.LeftBatteryEnergy = battery.Energy;
                            break;
                        case 1:
                            this.telemetry.rover.RightBatteryEnergy = battery.Energy;
                            break;
                    }
                }
            }
        }


        private static void EnsureConfigIsValid(SimConfig config)
        {
            //CHARGING STATIONS

            //minimum requirement of 3 charging stations (Station1, Station2, Start)
            if (config.ChargingStations == null || config.ChargingStations.Count < 3)
            {
                throw new Exception("At least three charging stations must be defined in the configuration.");
            }

            //maximum requirement of 5 charging stations (Station1, Station2, Station3, Station4, Start)
            if (config.ChargingStations == null || config.ChargingStations.Count > 5)
            {
                throw new Exception("No more than five charging stations can be defined in the configuration.");
            }

            // Validate that all station locations (excluding the Start location) are unique and sequential.
            // The Start location is validated separately because it must always be the final entry in the list.
            for (var lx = 0; lx < config.ChargingStations.Count - 1; lx++)
            {
                var loc = (LocationType)lx; //0 - > Station1, 1 -> Station2, 2 -> Station3, 3 -> Station4
                if (config.ChargingStations[lx].Location != loc)
                {
                    throw new Exception($"TargetLocation {loc} not found.");
                }
            }

            // Ensure the final charging station represents the Start location.
            if (config.ChargingStations.Last().Location != LocationType.Start)
            {
                throw new Exception("The last charging station must be located at the Start location.");
            }

            //ROLES

            if (config.Roles == null || config.Roles.Count < 2)
            {
                throw new Exception("At least two roles must be defined in the configuration.");
            }

            // Validate that role IDs are unique and sequential, starting at 0 with no gaps or duplicates
            for (var rx = 0; rx < config.Roles.Count; rx++)
            {
                if (config.Roles[rx].Id != rx)
                {
                    throw new Exception($"Role {rx} not found.");
                }
            }

            // one to-one mapping between charging stations and roles, excluding the Start location.
            if (config.ChargingStations.Count - 1 != config.Roles.Count)
            {
                throw new Exception("The number of charging stations (excluding Start) must match the number of roles.");
            }
        }

        private static void UpdateStationTelemetry(StationTelemetry dest, StationState source)
        {
            dest.Location = source.Location;
            dest.Accumulator = source.Accumulator;
            dest.ConsecutiveFailures = source.ConsecutiveFailures;
            dest.ConsecutiveRewards = source.ConsecutiveRewards;
            dest.TotalFailures = source.TotalFailures;
            dest.TotalRewards = source.TotalRewards;
            dest.TotalUploads = source.TotalUploads;
        }

        private IReadOnlyList<ActionType> GetValidActions()
        {
            if (this.state.CurrentLocation == LocationType.None)
            {
                // The rover is currently in transit between stations, so no actions are valid.
                return new ActionType[] { };
            }

            var currentStation = this.GetCurrentStation();

            var actions = new List<ActionType>();

            //Only allow navigation to other stations if the current station's accumulator is empty (the player needs to upload to avoid losing the rewards)
            if (currentStation.Accumulator > 0)
            {
                if (this.state.CurrentLocation != LocationType.Station1)
                {
                    actions.Add(ActionType.GoToStation1);
                }

                if (this.state.CurrentLocation != LocationType.Station2)
                {
                    actions.Add(ActionType.GoToStation2);
                }

                //Note: Start location is always the last station in the list and counts as +1
                if (this.state.ChargingStations.Count > 3)
                {
                    if (this.state.CurrentLocation != LocationType.Station3)
                    {
                        actions.Add(ActionType.GoToStation3);
                    }
                }

                if (this.state.ChargingStations.Count > 4)
                {
                    if (this.state.CurrentLocation != LocationType.Station4)
                    {
                        actions.Add(ActionType.GoToStation4);
                    }
                }
            }

            if (this.state.CurrentLocation != LocationType.Start && currentStation.RoleParams != null)
            {
                if (currentStation.RoleParams.Id != RoleType.Depleted)
                {
                    actions.Add(ActionType.Extract);
                }

                //if the accumulator has nothing there is no point to upload nothing and lose energy!
                if (currentStation.Accumulator > 0f)
                {
                    actions.Add(ActionType.Upload);
                }
            }

            return actions;
        }

        private IChargingStationController GetChargingStationController(LocationType location)
        {
            if (!this.isInitialized || this.chargingStationControllers == null)
            {
                throw new InvalidOperationException("Controllers are not initialized.");
            }

            var stationController = this.chargingStationControllers.FirstOrDefault(x => x.Location == location);
            if (stationController == null)
            {
                throw new InvalidOperationException($"Charging station controller for {location} not found.", null);
            }

            return stationController;
        }

        private StationState GetCurrentStation()
        {
            var station = this.state.ChargingStations.First(x => x.Location == this.state.CurrentLocation);
            return station;
        }
    }
}