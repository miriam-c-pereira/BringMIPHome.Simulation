#nullable enable
namespace BringMIPHome.Simulation
{
    using System;

    public class StationState
    {
        public LocationType Location { get; set; }
        public ChargingStationParams ChargingStationParams { get; set; }
        public RoleParams? RoleParams { get; set; }
        public float Accumulator { get; set; }
        public int TotalRewards { get; set; } = 0;
        public int TotalFailures { get; set; } = 0;
        public int TotalUploads { get; set; }
        public int ConsecutiveFailures { get; set; } = 0;
        public int ConsecutiveRewards { get; set; } = 0;
        public ExtractionOutcome? LastExtractOutcome { get; set; } = null;
        public bool IsDepleted { get; set; } = false;

        public StationState()
        {
            this.Location = LocationType.None;
            this.ChargingStationParams = new ChargingStationParams();
        }
        
        /// <summary>
        /// Standard constructor for original initialization.
        /// </summary>
        public StationState(ChargingStationParams chargingStationParams, RoleParams? roleParams)
        {
            if (chargingStationParams == null)
            { 
                throw new ArgumentNullException(nameof(chargingStationParams));
            }

            this.Location = chargingStationParams.Location;
            this.ChargingStationParams = chargingStationParams;

            // Can be null if TargetLocation is TargetLocation.Start
            this.RoleParams = roleParams;

            this.Accumulator = chargingStationParams.AccumulatorInitialValue;
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
            this.TotalFailures = source.TotalFailures;
            this.TotalUploads = source.TotalUploads;
            this.ConsecutiveRewards = source.ConsecutiveRewards;
            this.ConsecutiveFailures = source.ConsecutiveFailures;
            this.LastExtractOutcome = source.LastExtractOutcome != null ? new ExtractionOutcome(source.LastExtractOutcome) : null;
        }
    }
}