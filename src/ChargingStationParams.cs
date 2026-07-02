namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public class ChargingStationParams
    {
        public LocationType Location { get; set; } = LocationType.None;

        public float AccumulatorInitialValue { get; set; } = 0;

        /// <summary>
        /// Ordered sequences of access points (Unity) used to navigate to different station areas.
        /// </summary>
        public List<StationAccessSequence> StationAccessSequences { get; set; } = new List<StationAccessSequence>();

        /// <summary>
        /// World position of the charging station in Unity space.
        /// </summary>
        public SimVector3 Position { get; set; }

        /// <summary>
        /// Y-axis rotation (heading) of the station in degrees.
        /// </summary>
        public float Heading { get; set; }
    }
}