namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public interface IMissionControlController
    {
        void ConsoleMessage(string message, IDictionary<string, object> properties);
        void UpdateTelemetryUI(SimulationHost simHost);
    }
}