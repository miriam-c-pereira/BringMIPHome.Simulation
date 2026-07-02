namespace BringMIPHome.Simulation
{
    using System.ComponentModel;

    public interface IStationTelemetry : INotifyPropertyChanged
    {
        LocationType Location { get; }
        
        float Accumulator { get; }
        
        int ConsecutiveFailures { get; }
        
        int ConsecutiveRewards { get; }
        
        int TotalRewards { get; }
        
        int TotalFailures { get; }
        
        int TotalUploads { get; }
    }
}