namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;
    
    public class ExtractionOutcome
    {
        public bool BecameDepleted { get; set; }
        public float ExtractedAmount { get; set; }
    }

    public class StationState
    {
        public LocationType Location { get; private set; }
        public ChargingStationParams ChargingStationParams { get; private set; }
        public RoleParams RoleParams { get; private set; }

        public float Accumulator { get; private set; }
        public float TotalRewards { get; private set; }
        public ExtractionOutcome LastExtractOutcome { get; private set; }

        /// <summary>
        /// Standard constructor for original initialization.
        /// </summary>
        public StationState(ChargingStationParams chargingStationParams, RoleParams roleParams)
        {
            if (chargingStationParams == null)
            {
                throw new ArgumentNullException(nameof(chargingStationParams));
            }

            if (chargingStationParams.Location == LocationType.None)
            {
                throw new InvalidOperationException("ChargingStationParams.Location cannot be None.");
            }

            this.Location = chargingStationParams.Location;
            this.ChargingStationParams = chargingStationParams;
            this.RoleParams = roleParams; // Can be null if Location is LocationType.Start

            this.Accumulator = chargingStationParams.AccumulatorInitialValue;
            this.TotalRewards = 0f;
            this.LastExtractOutcome = new ExtractionOutcome { BecameDepleted = false, ExtractedAmount = 0f };
        }

        /// <summary>
        /// Deep copy constructor for duplication and simulation snapshots.
        /// </summary>
        public StationState(StationState source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.Location = source.Location;
            this.ChargingStationParams = source.ChargingStationParams;
            this.RoleParams = source.RoleParams;
            this.Accumulator = source.Accumulator;
            this.TotalRewards = source.TotalRewards;

            this.LastExtractOutcome = new ExtractionOutcome
            {
                BecameDepleted = source.LastExtractOutcome.BecameDepleted,
                ExtractedAmount = source.LastExtractOutcome.ExtractedAmount
            };
        }

        public List<ActionType> GetValidActions()
        {
            var actions = new List<ActionType>();

            // Always allow moving away from the current location to a proper station
            if (this.Location != LocationType.Station1) actions.Add(ActionType.GoToStation1);
            if (this.Location != LocationType.Station2) actions.Add(ActionType.GoToStation2);
            if (this.Location != LocationType.Station3) actions.Add(ActionType.GoToStation3);

            // Gameplay mechanics constraints based on role parameters
            if (this.Location != LocationType.Start && this.RoleParams != null)
            {
                if (this.RoleParams.Role != RoleType.Depleted)
                {
                    actions.Add(ActionType.Extract);
                }

                if (this.Accumulator > 0f)
                {
                    actions.Add(ActionType.Upload);
                }
            }

            return actions;
        }

        public void ApplyGoto(LocationType targetLocation)
        {
            // Reset extraction histories upon leaving the station context
            this.LastExtractOutcome.BecameDepleted = false;
            this.LastExtractOutcome.ExtractedAmount = 0f;
        }

        public void ApplyExtract()
        {
            if (this.RoleParams == null) return;

            // Example mathematical evaluation matching your rule constraints
            float gathered = this.RoleParams.RewardAccumulatorAdd;
            this.Accumulator += gathered;
            this.TotalRewards += gathered;

            this.LastExtractOutcome.ExtractedAmount = gathered;

            // Toggle depletion state markers safely under rule criteria
            if (this.RoleParams.Role == RoleType.Jackpot || this.RoleParams.Role == RoleType.BreadCrumb)
            {
                // This boolean triggers the outer host to process role rotation updates
                this.LastExtractOutcome.BecameDepleted = true;
            }
        }

        public void ApplyUpload()
        {
            // Clear the local resource buffer storage safely upon explicit upload completion
            this.Accumulator = 0f;
        }

        public void SwitchRole(RoleParams newRoleParams)
        {
            this.RoleParams = newRoleParams ?? throw new ArgumentNullException(nameof(newRoleParams));

            // Clean slate evaluation tracking loops post reassignment shifts
            this.LastExtractOutcome.BecameDepleted = false;
        }
    }
}