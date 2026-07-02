namespace BringMIPHome.Simulation
{
    public class ExtractionOutcome
    {
        public bool BecameDepleted { get; set; }
        public float ExtractedAmount { get; set; }
        public int ConsecutiveRewards { get; set; }
        public int ConsecutiveFailures { get; set; }

        public ExtractionOutcome()
        {
        }

        public ExtractionOutcome(ExtractionOutcome source)
        {
            this.BecameDepleted = source.BecameDepleted;
            this.ExtractedAmount = source.ExtractedAmount;
            this.ConsecutiveRewards = source.ConsecutiveRewards;
            this.ConsecutiveFailures = source.ConsecutiveFailures;
        }
    }
}