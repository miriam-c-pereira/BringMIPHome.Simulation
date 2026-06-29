namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public class ChargingStationParams
    {
        public LocationType Location { get; set; } = LocationType.None;

        public float AccumulatorInitialValue { get; set; } = 0;

        public List<StationAccessSequence> StationAccessSequences { get; set; } = new List<StationAccessSequence>();

        //TODO
        public SimVector3 SetupPosition { get; set; }

        //TODO
        public float SetupAngle { get; set; }
    }
}