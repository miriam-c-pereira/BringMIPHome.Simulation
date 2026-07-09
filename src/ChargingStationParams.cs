namespace BringMIPHome.Simulation
{
    public class ChargingStationParams
    {
        public LocationType Location { get; set; } = LocationType.None;

        public float AccumulatorInitialValue { get; set; } = 0;
    }
}