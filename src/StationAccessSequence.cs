namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public class StationAccessSequence
    {
        public LocationType Location { get; set; } = LocationType.None;

        public List<NavigationStep> Steps { get; set; } = new List<NavigationStep>();
    }
}