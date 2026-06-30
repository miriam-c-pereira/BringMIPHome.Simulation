namespace BringMIPHome.Simulation
{
    public class ExtractionVisualsArgs
    {
        public LocationType Location { get; }
        public float TotalRewards { get; }
        public float Accumulator { get; }
        public float ExtractionRate { get; }
        public float DurationModifier { get; }

        public ExtractionVisualsArgs(
            LocationType location,
            float totalRewards,
            float accumulator,
            float extractionRate,
            float durationModifier)
        {
            this.Location = location;
            this.TotalRewards = totalRewards;
            this.Accumulator = accumulator;
            this.ExtractionRate = extractionRate;
            this.DurationModifier = durationModifier;
        }
    }

    public class UploadVisualsArgs
    {
        public LocationType Location { get; }
        public float TotalRewards { get; }
        public float TransferSpeed { get; }

        public UploadVisualsArgs(LocationType location, float totalRewards, float transferSpeed)
        {
            this.Location = location;
            this.TotalRewards = totalRewards;
            this.TransferSpeed = transferSpeed;
        }
    }

    public interface IChargingStationController
    {
        public LocationType Location { get; }
        
        void PlayExtractionVisuals(ExtractionVisualsArgs args);
        
        void PlayUploadVisuals(UploadVisualsArgs args);
    }
}