namespace BringMIPHome.Simulation
{
    public class SimEvent
    {
        public SimState Before { get; private set; }
        public ActionType Action { get; private set; }
        public SimState After { get; private set; }

        public SimEvent(SimState before, ActionType action, SimState after)
        {
            this.Before = before;
            this.Action = action;
            this.After = after;
        }
    }
}