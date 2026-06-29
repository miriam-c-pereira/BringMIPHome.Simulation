namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SimState
    {
        public float Energy => this.Batteries.Where(b => !b.IsDetached).Sum(b => b.Energy);

        public float TimeLeft { get; set; }

        public List<RoverBattery> Batteries { get; set; } = new List<RoverBattery>();

        public LocationType CurrentLocation { get; set; }

        public StationState CurrentChargingStation => this.ChargingStations.FirstOrDefault(l => l.Location == this.CurrentLocation);

        public DoneReasonType Done { get; set; }

        public List<StationState> ChargingStations { get; set; } = new List<StationState>();

        /// <summary>
        /// Default constructor for standard initialization.
        /// </summary>
        public SimState()
        {
        }

        /// <summary>
        /// Deep copy constructor to duplicate states without reference bleeding.
        /// </summary>
        public SimState(SimState source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.TimeLeft = source.TimeLeft;
            this.CurrentLocation = source.CurrentLocation;
            this.Done = source.Done;

            // Deep copy primitive collections explicitly
            this.Batteries = source.Batteries.Select(b => new RoverBattery
                {
                    Id = b.Id,
                    IsDetached = b.IsDetached,
                    Energy = b.Energy
                }).
                ToList();

            // Assumes StationState also implements a corresponding copy constructor
            this.ChargingStations = source.ChargingStations.Select(s => new StationState(s)).ToList();
        }

        /// <summary>
        /// Generates a pure snapshot of the current state for event histories or rewind steps.
        /// </summary>
        public SimState GetSnapshot()
        {
            return new SimState(this);
        }
    }
}