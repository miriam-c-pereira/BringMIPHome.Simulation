namespace BringMIPHome.Simulation
{
    using System;

    public abstract class SimulationEvent : SimEventArgs
    {

    }

    public class DoneEvent : SimulationEvent
    {
        public DoneReasonType Done { get; set; }

        public float TotalEnergy { get; set; }
        
        public float TimeLeft { get; set; }
    }

    public class ActionEvent : SimulationEvent
    {
        public ActionType Action { get; private set; }
        public SimState Before { get; private set; }
        public SimState After { get; private set; }

        public ActionEvent(SimState before, ActionType action, SimState after)
        {
            this.Before = before;
            this.Action = action;
            this.After = after;
        }
    }
}