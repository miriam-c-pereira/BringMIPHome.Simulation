namespace BringMIPHome.Simulation
{
    using System;

    public class StateResult
    {
        public SimVector2 Position { get; set; }

        public float Heading { get; set; }

        public ActionType RunningAction { get; set; }
    }

    public abstract class RoverActionArgs
    {
    }

    public abstract class RoverEvent : SimEventArgs
    {
    }

    public enum NavigationStopReason { None, ObstacleDetected, }

    public class NavigationStartedEvent : RoverEvent
    {
        public RoverGotoArgs Args { get; set; }
    }

    public class NavigationStoppedEvent : RoverEvent
    {
        public RoverGotoArgs Args { get; set; }
        public NavigationStopReason StopReason { get; set; }
    }

    public class PositionUpdatedEvent : RoverEvent
    {
        public SimVector3 Position { get; set; }
        public float Velocity { get; set; }
    }

    public class RotationUpdatedEvent : RoverEvent
    {
        public SimVector3 Rotation { get; set; }
        public float Heading { get; set; }
    }

    public class RoverGotoArgs : RoverActionArgs
    {
        public NavigationRoute NavigationRoute { get; set; }
    }


    public interface IRoverController : IController
    {
        event EventHandler<RoverEvent> RoverEvent;

        void Initialize(ISimulationHost host);
        bool StartAction(ActionType actionType, RoverActionArgs args);
        StateResult GetState();
    }
}