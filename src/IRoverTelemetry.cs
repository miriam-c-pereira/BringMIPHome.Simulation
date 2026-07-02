namespace BringMIPHome.Simulation
{
    using System.ComponentModel;

    public interface IRoverTelemetry : INotifyPropertyChanged
    {
        bool IsMoving { get; set; }
        LocationType TargetLocation { get; }
        float DistanceToTarget { get; }
        float EstimatedTimeToArrival { get; }
        
        float TotalBatteryEnergy { get; }
        bool LeftBatteryIsDetached { get; }
        float LeftBatteryEnergy { get; }
        bool RightBatteryIsDetached { get; }
        float RightBatteryEnergy { get; }

        SimVector3 Position { get; }
        float Heading { get; set; }
        float AngularVelocity { get; }
        float LinearVelocity { get; }
    }
}