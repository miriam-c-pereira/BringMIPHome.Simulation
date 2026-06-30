#nullable enable
namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SimulationHost : ISimulationTelemetry
    {
        public SimConfig Config { get; private set; }
        public Random Random { get; private set; }
        public SimState State { get; private set; } = new SimState();

        // ISimulationTelemetry Implementation
        public float TotalEnergy => this.State?.Energy ?? 0f;
        public float TimeLeft => this.State?.TimeLeft ?? 0f;
        public LocationType CurrentLocation => this.State?.CurrentLocation ?? LocationType.None;
        public DoneReasonType DoneReason => this.State?.Done ?? DoneReasonType.NotDone;
        public IReadOnlyList<RoverBattery> Batteries => this.State.Batteries;

        // Injected interfaces for decoupled communication
        private readonly IRoverController roverController;
        private readonly IEnumerable<IChargingStationController> chargingStationControllers;
        private readonly IMissionControlController mcController;


        public event Action<SimState, SimState, SimEvent> OnStepCompleted = (b, a, e) => { };

        public SimulationHost(SimConfig config, IRoverController roverController, IEnumerable<IChargingStationController> chargingStations, IMissionControlController missionControlController)
        {
            this.Config = config ?? throw new ArgumentNullException(nameof(config));
            this.roverController = roverController ?? throw new ArgumentNullException(nameof(roverController));
            this.chargingStationControllers = chargingStations ?? throw new ArgumentNullException(nameof(chargingStations));
            this.mcController = missionControlController ?? throw new ArgumentNullException(nameof(missionControlController));

            this.Random = this.Config.RandomSeed != null
                ? new Random(this.Config.RandomSeed.Value)
                : new Random();

            this.ResetState();
        }

        private void ResetState()
        {
            var gameState = new SimState
            {
                Batteries = new List<RoverBattery>
                {
                    new RoverBattery { Id = 1, Energy = this.Config.EnergyInit / 2 },
                    new RoverBattery { Id = 2, Energy = this.Config.EnergyInit / 2 },
                },
                TimeLeft = this.Config.TimeInit,
                CurrentLocation = LocationType.Start,
                Done = DoneReasonType.NotDone,
                ChargingStations = new List<StationState>()
            };

            var roles = this.GetShuffleRoles();
            var roleIndex = 0;

            foreach (var chargingStationParams in this.Config.ChargingStations)
            {
                RoleParams? roleParams = null;

                if (chargingStationParams.Location == LocationType.None)
                {
                    throw new InvalidOperationException("chargingStationParams.Location cannot be None.");
                }

                if (chargingStationParams.Location != LocationType.Start)
                {
                    roleParams = this.GetRoleParamsByRoleType(roles[roleIndex]);
                    roleIndex++;
                }

                gameState.ChargingStations.Add(new StationState(chargingStationParams, roleParams!));
            }

            this.State = gameState;
        }

        public SimEvent? ExecuteAction(ActionType action)
        {
            if (this.State.Done != DoneReasonType.NotDone)
            {
                return null;
            }

            var before = this.State.GetSnapshot();

            var validActions = this.State.CurrentChargingStation.GetValidActions();
            if (!validActions.Contains(action))
            {
                throw new InvalidOperationException($"Invalid action {action} at {this.State.CurrentLocation}");
            }

            switch (action)
            {
                case ActionType.GoToStation1:
                case ActionType.GoToStation2:
                case ActionType.GoToStation3:
                    this.ExecuteGoto(action);
                    break;

                case ActionType.Extract:
                    this.ExecuteExtract();
                    break;

                case ActionType.Upload:
                    this.ExecuteUpload();
                    break;
            }

            this.UpdateDone();

            var after = this.State.GetSnapshot();

            var gameEvent = new SimEvent(before, action, after);
            this.OnStepCompleted?.Invoke(before, after, gameEvent);
            return gameEvent;
        }

        private void UpdateDone()
        {
            if (this.State.Energy <= 0f)
            {
                this.State.Done = DoneReasonType.EnergyDepleted;
                return;
            }

            if (this.State.TimeLeft <= 0f)
            {
                this.State.Done = DoneReasonType.TimeExpired;
                return;
            }

            if (this.Config.TargetEnergy != null && this.State.Energy >= this.Config.TargetEnergy.Value)
            {
                this.State.Done = DoneReasonType.TargetEnergyReached;
                return;
            }
        }

        private void ExecuteGoto(ActionType action)
        {
            var station = this.State.CurrentChargingStation;
            LocationType newLocation;

            switch (action)
            {
                case ActionType.GoToStation1:
                    newLocation = LocationType.Station1;
                    break;

                case ActionType.GoToStation2:
                    newLocation = LocationType.Station2;
                    break;

                case ActionType.GoToStation3:
                    newLocation = LocationType.Station3;
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported action: {action} currentLocation={this.State.CurrentLocation}");
            }

            var stationAccessSequence = station.ChargingStationParams?.StationAccessSequences.FirstOrDefault(l => l.Location == newLocation);

            if (stationAccessSequence != null)
            {
                this.roverController.NavigateToStation(stationAccessSequence);
            }
            else
            {
                this.mcController.ConsoleMessage($"No auto-navigation available to {newLocation}.", null);
            }

            this.SetEnergy(this.State.Energy - this.Config.GoToEnergyCost);
            this.State.TimeLeft -= this.Config.GoToTimeCost;

            this.State.CurrentChargingStation.ApplyGoto(newLocation);
            this.State.CurrentLocation = newLocation;
        }

        private void ExecuteExtract()
        {
            var stationState = this.State.CurrentChargingStation;

            this.SetEnergy(this.State.Energy - this.Config.ExtractEnergyCost);
            this.State.TimeLeft -= this.Config.ExtractTimeCost;

            stationState.ApplyExtract();

            // Pack arguments dynamically without changing the method's interface signature
            var args = new ExtractionVisualsArgs(
                stationState.Location,
                stationState.TotalRewards,
                stationState.Accumulator,
                1f / 100f,
                1.5f
            );

            var ctl = this.chargingStationControllers.FirstOrDefault(x => x.Location == stationState.Location);
            if (ctl == null)
            {
                this.mcController.ConsoleMessage($"Charging station controller for {stationState.Location} not found.", null);
                return;
            }

            ctl.PlayExtractionVisuals(args);

            if (stationState.LastExtractOutcome.BecameDepleted)
            {
                this.ApplyDepletion();
            }
        }

        private void ExecuteUpload()
        {
            var station = this.State.CurrentChargingStation;

            this.SetEnergy(this.State.Energy - this.Config.UploadEnergyCost + station.Accumulator);
            this.State.TimeLeft -= this.Config.UploadTimeCost;

            var args = new UploadVisualsArgs(
                station.Location,
                station.TotalRewards,
                0.15f
            );

            var ctl = this.chargingStationControllers.FirstOrDefault(x => x.Location == station.Location);
            if (ctl == null)
            {
                this.mcController.ConsoleMessage($"Charging station controller for {station.Location} not found.", null);
                return;
            }

            ctl.PlayUploadVisuals(args);
            
            station.ApplyUpload();
        }

        private void SetEnergy(float energy)
        {
            var batteries = this.State.Batteries;
            var energyPerBattery = energy / batteries.Count;
            foreach (var battery in batteries)
            {
                battery.Energy = energyPerBattery;
            }
        }

        private void ApplyDepletion()
        {
            switch (this.Config.RoleReassignmentRule)
            {
                case RoleReassignmentRule.LocalRotation:
                    this.ApplyLocalRotation();
                    break;

                case RoleReassignmentRule.Random:
                    this.ApplyRandomReassignment();
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported RoleReassignmentRule: {this.Config.RoleReassignmentRule}");
            }
        }

        private void ApplyLocalRotation()
        {
            var currentStation = this.State.CurrentChargingStation;
            var currentStationRole = currentStation.RoleParams.Role;

            var depletedStation = this.GetChargingStationByRole(RoleType.Depleted);
            var breadCrumbStation = this.GetChargingStationByRole(RoleType.BreadCrumb);
            var jackpotStation = this.GetChargingStationByRole(RoleType.Jackpot);

            var depletedRoleParams = this.GetRoleParamsByRoleType(RoleType.Depleted);
            var breadCrumbRoleParams = this.GetRoleParamsByRoleType(RoleType.BreadCrumb);
            var jackpotRoleParams = this.GetRoleParamsByRoleType(RoleType.Jackpot);

            switch (currentStationRole)
            {
                case RoleType.Jackpot:
                    jackpotStation.SwitchRole(depletedRoleParams);
                    breadCrumbStation.SwitchRole(jackpotRoleParams);
                    depletedStation.SwitchRole(breadCrumbRoleParams);
                    break;

                case RoleType.BreadCrumb:
                    breadCrumbStation.SwitchRole(depletedRoleParams);
                    jackpotStation.SwitchRole(breadCrumbRoleParams);
                    depletedStation.SwitchRole(jackpotRoleParams);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported current station role: {currentStationRole}");
            }
        }

        private void ApplyRandomReassignment()
        {
            var currentStation = this.State.CurrentChargingStation;
            var currentStationRole = currentStation.RoleParams.Role;

            var depletedStation = this.GetChargingStationByRole(RoleType.Depleted);
            var breadCrumbStation = this.GetChargingStationByRole(RoleType.BreadCrumb);
            var jackpotStation = this.GetChargingStationByRole(RoleType.Jackpot);

            var depletedRoleParams = this.GetRoleParamsByRoleType(RoleType.Depleted);
            var breadCrumbRoleParams = this.GetRoleParamsByRoleType(RoleType.BreadCrumb);
            var jackpotRoleParams = this.GetRoleParamsByRoleType(RoleType.Jackpot);

            var switchRole = this.Random.NextDouble() > 0.5;

            switch (currentStationRole)
            {
                case RoleType.Jackpot:
                    jackpotStation.SwitchRole(depletedRoleParams);
                    breadCrumbStation.SwitchRole(switchRole ? jackpotRoleParams : breadCrumbRoleParams);
                    depletedStation.SwitchRole(switchRole ? breadCrumbRoleParams : jackpotRoleParams);
                    break;

                case RoleType.BreadCrumb:
                    breadCrumbStation.SwitchRole(depletedRoleParams);
                    jackpotStation.SwitchRole(switchRole ? breadCrumbRoleParams : jackpotRoleParams);
                    depletedStation.SwitchRole(switchRole ? jackpotRoleParams : breadCrumbRoleParams);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported currentStationRole: {currentStationRole}");
            }
        }

        private RoleParams GetRoleParamsByRoleType(RoleType role)
        {
            var roleParams = this.Config.Roles.FirstOrDefault(x => x.Role == role);
            if (roleParams == null)
            {
                throw new InvalidOperationException($"{role} role not found in Config.Roles");
            }

            return roleParams;
        }

        private StationState GetChargingStationByRole(RoleType role)
        {
            var station = this.State.ChargingStations.FirstOrDefault(l => l.RoleParams?.Role == role);
            if (station == null)
            {
                throw new InvalidOperationException($"{role} role not found in any location");
            }

            return station;
        }

        private RoleType[] GetShuffleRoles()
        {
            var roles = Enum.GetValues(typeof(RoleType)).Cast<RoleType>().ToList();

            for (var i = roles.Count - 1; i > 0; i--)
            {
                var j = this.Random.Next(i + 1);
                var tmp = roles[i];
                roles[i] = roles[j];
                roles[j] = tmp;
            }

            return roles.ToArray();
        }
    }
}