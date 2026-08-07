namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface ISimulationTelemetry : INotifyPropertyChanged
    {
        SimulationStatus Status { get; }
        
        float TimeLeft { get; }

        SimulationCompletionReason CompletionReason { get; }

        IRoverTelemetry Rover { get; }

        ActionType CurrentAction { get; }

        IStationTelemetry CurrentStation { get; }

        IReadOnlyList<ActionType> ValidActions { get; }
    }
}