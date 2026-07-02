namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public interface IMissionControlController
    {
        void Initialize(ISimulationHost host);
        void ConsoleMessage(string message, IDictionary<string, object> properties);
    }
}