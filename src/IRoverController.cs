namespace BringMIPHome.Simulation
{
    public interface IRoverController
    {
        void Initialize(ISimulationHost host);
        void StartNavigation(StationAccessSequence sequence);
    }
}