namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public interface ISimulationTelemetry
    {
        float TotalEnergy { get; }
        float TimeLeft { get; }
        LocationType CurrentLocation { get; }
        DoneReasonType DoneReason { get; }
        IReadOnlyList<RoverBattery> Batteries { get; }
    }
}