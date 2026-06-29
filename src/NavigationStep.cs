namespace BringMIPHome.Simulation
{
    public abstract class NavigationStep
    {
    }

    public class WaypointStep : NavigationStep
    {
        public SimVector3 Destination { get; set; }
    }

    public class OrientationStep : NavigationStep
    {
        public float Angle { get; set; }
    }
}