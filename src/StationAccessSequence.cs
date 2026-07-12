namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public class SimUiConfig
    {
        public List<LocationUiConfig> Locations { get; set; } = new List<LocationUiConfig>();
    }

    public class LocationUiConfig
    {
        public LocationType Location { get; set; } = LocationType.None;

        public string DisplayName { get; set; } = "Display name";

        public SimVector3 Position { get; set; }
        public SimVector3 Rotation { get; set; }

        public SimVector3 RoverPosition { get; set; }
        public float RoverHeading { get; set; }
        

        /// <summary>
        /// Ordered sequences of access points (Unity) used to navigate to different station areas.
        /// </summary>
        public List<NavigationRoute> NavigationRoutes { get; set; } = new List<NavigationRoute>();
    }


    public class NavigationRoute
    {
        public LocationType TargetLocation { get; set; } = LocationType.None;

        public List<NavigationStep> Steps { get; set; } = new List<NavigationStep>();
    }
}