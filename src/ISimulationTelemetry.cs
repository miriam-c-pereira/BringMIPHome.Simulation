namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface ISimulationTelemetry : INotifyPropertyChanged
    {
        float TimeLeft { get; }

        DoneReasonType DoneReason { get; }

        IRoverTelemetry Rover { get; }

        ActionType CurrentAction { get; }

        IStationTelemetry CurrentStation { get; }

        IReadOnlyList<ActionType> ValidActions { get; }
    }
}