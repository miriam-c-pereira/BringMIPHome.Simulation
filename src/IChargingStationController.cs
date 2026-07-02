namespace BringMIPHome.Simulation
{
    public class StationExtractionArgs
    {
        public int ConsecutiveRewards { get; set; }
        public int ConsecutiveFailures { get; set; }
        public float Accumulator { get; set; }
    }

    public class StationUploadArgs
    {
        public int ConsecutiveRewards { get; set; }
    }

    public interface IChargingStationController
    {
        public LocationType Location { get; }

        public void Initialize(ISimulationHost host, ChargingStationAnimationConfig config);

        void StartExtraction(StationExtractionArgs args);

        void StartUpload(StationUploadArgs args);
    }
}