namespace BringMIPHome.Simulation
{
    using System;

    public abstract class SimulationEvent : SimEventArgs
    {
        public long TimestampMilliseconds { get; private set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }


    public sealed class SimulationStartedEvent : SimulationEvent
    {
    }

    public enum MissionTimeThreshold
    {
        None= 0,
        Percent75 = 75,
        Percent50 = 50,
        Percent25 = 25,
        Percent5 = 5
    }


    public sealed class MissionTimeThresholdReachedEvent : SimulationEvent
    {
        public MissionTimeThreshold Threshold { get; set; }
    }


    public class SimulationFinishedEvent : SimulationEvent
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