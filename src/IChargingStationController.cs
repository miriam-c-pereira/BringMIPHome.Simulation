using System;

namespace BringMIPHome.Simulation
{
    public abstract class ChargingStationActionArgs
    {
    }


    public abstract class ChargingStationEvent : SimEventArgs
    {
    }

    public class ExtractionArgs : ChargingStationActionArgs
    {
        public int ConsecutiveRewards { get; set; }
        public int ConsecutiveFailures { get; set; }
        public float Accumulator { get; set; }
    }

    public class UploadingArgs : ChargingStationActionArgs
    {
        public int ConsecutiveRewards { get; set; }
    }

    public class ExtractionStartedEvent : ChargingStationEvent
    {
        public ExtractionArgs Args { get; set; }
    }

    public class ExtractionStoppedEvent : ChargingStationEvent
    {
        public ExtractionArgs Args { get; set; }
    }

    public class UploadingStartedEvent : ChargingStationEvent
    {
        public UploadingArgs Args { get; set; }
    }

    public class UploadingStoppedEvent : ChargingStationEvent
    {
        public UploadingArgs Args { get; set; }
    }

    public interface IChargingStationController : IController
    {
        event EventHandler<ChargingStationEvent> ChargingStationEvent;

        public LocationType Location { get; }

        public void Initialize(ISimulationHost host, LocationUiConfig locationUiConfig, ChargingStationAnimationConfig chargingStationAnimationConfig);

        void StartAction(ActionType actionType, ChargingStationActionArgs args);
    }
}