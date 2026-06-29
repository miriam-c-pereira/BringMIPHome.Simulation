namespace BringMIPHome.Simulation
{
    public class RoleParams
    {
        public RoleType Role { get; set; }
        public float RewardProbability { get; set; }
        public float RewardAccumulatorAdd { get; set; }
        public float RewardAccumulatorMul { get; set; }
        public float DepletionSwitchProbability { get; set; }
    }
}