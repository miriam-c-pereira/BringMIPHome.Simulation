namespace BringMIPHome.Simulation
{
    public class RoleParams
    {
        public int Id { get; set; }
        public float RewardProbability { get; set; }
        public float RewardAccumulatorAdd { get; set; }
        public float RewardAccumulatorMul { get; set; }
        public float DepletionSwitchProbability { get; set; }
    }
}