namespace BringMIPHome.Simulation
{
    using System.Collections.Generic;

    public class DefaultConfigProvider
    {
        public ChargingStationAnimationConfig GetChargingStationAnimationConfig()
        {
            return new ChargingStationAnimationConfig()
            {
                ExtractionDuration = 2,
                UploadDuration = 4,
                BatteryUnitAnimationStepTime = 100/ 1000f, // 100ms
            };
        }
        
        
        public SimUiConfig GetSimUiConfig()
        {
            return new SimUiConfig()
            {
                Locations = new List<LocationUiConfig>
                {
                    new LocationUiConfig
                    {
                        Location = LocationType.Station1,
                        DisplayName = "1",
                        Position = new SimVector3(0, 0, 0),
                        Rotation = new SimVector3(0, 0, 0),
                        NavigationRoutes = new List<NavigationRoute>
                        {
                           
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station2,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 0) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(10, 0, 0) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(10, 0, 10) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 10) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 20) },
                                    new OrientationStep { Angle = 45 },
                                }
                            },
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station3,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 0) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, 0) },
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, -20) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, -20) },
                                    new OrientationStep { Angle = 135 },
                                }
                            }
                        }
                    },
                    
                    
                    new LocationUiConfig
                    {
                        Location = LocationType.Station2,
                        DisplayName = "2",
                        Position = new SimVector3(0, 0, 0),
                        Rotation = new SimVector3(0, 0, 0),


                        NavigationRoutes = new List<NavigationRoute>
                        {
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station1,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 10) },
                                    new OrientationStep { Angle = 270 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, 10) },
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, 0) },
                                    new OrientationStep { Angle = 270 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 0) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 20) },
                                    new OrientationStep { Angle = 315 },
                                }
                            },
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station3,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 10) },
                                    new OrientationStep { Angle = 270 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, 10) },
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, -10) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, -10) },
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, -20) },
                                    new OrientationStep { Angle = 135 },
                                }
                            }

                        }


                    },
                    
                    
                    
                    
                    new LocationUiConfig
                    {
                        Location = LocationType.Station3,
                        DisplayName = "3",
                        Position = new SimVector3(0, 0, 0),
                        Rotation = new SimVector3(0, 0, 0),

                        NavigationRoutes = new List<NavigationRoute>
                        {
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station1,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 270 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, -20) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, 10) },
                                    new OrientationStep { Angle = 270 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 10) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 20) },
                                    new OrientationStep { Angle = 315 },
                                }
                            },
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station2,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 270 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, -20) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, 10) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 10) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 20) },
                                    new OrientationStep { Angle = 45 },
                                }
                            },
                        }


                    },

                    new LocationUiConfig
                    {
                        Location = LocationType.Start,
                        DisplayName = "Start",
                        Position = new SimVector3(0, 0, 0),
                        Rotation = new SimVector3(0, 0, 0),

                        NavigationRoutes = new List<NavigationRoute>
                        {
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station1,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = -90 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 0) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(-20, 0, 20) },
                                    new OrientationStep { Angle = 315 },
                                }
                            },
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station2,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(0, 0, 10) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 10) },
                                    new OrientationStep { Angle = 0 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, 20) },
                                    new OrientationStep { Angle = 45 },
                                }
                            },
                            new NavigationRoute
                            {
                                TargetLocation = LocationType.Station3,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(10, 0, 0) },
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(10, 0, -10) },
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, -10) },
                                    new OrientationStep { Angle = 180 },
                                    new WaypointStep { Destination = new SimVector3(20, 0, -20) },
                                    new OrientationStep { Angle = 135 },
                                }
                            }
                        }

                    },

                }
            };
        }

        public SimConfig GetSimConfig()
        {
            return new SimConfig
            {
                EnergyInit = 1000f,
                TimeInit = 1200,
                GoToEnergyCost = 20.0f,
                GoToTimeCost = 10,
                ExtractEnergyCost = 2f,
                ExtractTimeCost = 2,
                UploadEnergyCost = 4f,
                UploadTimeCost = 4,
                TargetEnergy = null,
                RandomSeed = null,
                Roles = new List<RoleParams>
                {
                    new RoleParams
                    {
                        Id = RoleType.Depleted,
                        RewardProbability = 0.0f,
                        RewardAccumulatorAdd = 0,
                        RewardAccumulatorMul = 0,
                        DepletionSwitchProbability = 0.0f,
                    },
                    new RoleParams
                    {
                        Id = RoleType.BreadCrumb,
                        RewardProbability = 0.4f,
                        RewardAccumulatorAdd = 10,
                        RewardAccumulatorMul = 2,
                        DepletionSwitchProbability = 0.1f,
                    },
                    new RoleParams
                    {
                        Id = RoleType.Jackpot,
                        RewardProbability = 0.8f,
                        RewardAccumulatorAdd = 10,
                        RewardAccumulatorMul = 2,
                        DepletionSwitchProbability = 0.1f,
                    },
                },
                ChargingStations = new List<ChargingStationParams>()
                {
                    new ChargingStationParams
                    {
                        Location = LocationType.Station1,
                        AccumulatorInitialValue = 0,
                        
                    },
                    new ChargingStationParams
                    {
                        Location = LocationType.Station2,
                        AccumulatorInitialValue = 0,
                    },
                    new ChargingStationParams
                    {
                        Location = LocationType.Station3,
                        AccumulatorInitialValue = 0,
                    },

                    new ChargingStationParams
                    {
                        Location = LocationType.Start,
                        AccumulatorInitialValue = 0,
                    },
                },
            };
        }
    }
}