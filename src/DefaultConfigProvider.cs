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
                ExtraActionTypes = new List<ActionType>(),
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
                        //TODO
                        //Position = new SimVector3(),
                        //Heading = ,
                        StationAccessSequences = new List<StationAccessSequence>
                        {
                            new StationAccessSequence
                            {
                                Location = LocationType.Station1,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(17f, 0.5f, 0f) },
                                    new OrientationStep { Angle = 90 },
                                },
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station2,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 210f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, -14.72f) },
                                    new OrientationStep { Angle = 210f },
                                }
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station3,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 330f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, 14.72f) },
                                    new OrientationStep { Angle = 330f },
                                }
                            }
                        }
                    },
                    new ChargingStationParams
                    {
                        Location = LocationType.Station2,
                        AccumulatorInitialValue = 0,
                        //TODO
                        //Position = new SimVector3(),
                        //Heading = ,

                        StationAccessSequences = new List<StationAccessSequence>
                        {
                            new StationAccessSequence
                            {
                                Location = LocationType.Station1,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(17f, 0.5f, 0f) },
                                    new OrientationStep { Angle = 90 },
                                }
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station2,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 210f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, -14.72f) },
                                    new OrientationStep { Angle = 210f },
                                }
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station3,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 330f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, 14.72f) },
                                    new OrientationStep { Angle = 330f },
                                }
                            }
                        }
                    },
                    new ChargingStationParams
                    {
                        Location = LocationType.Station3,
                        AccumulatorInitialValue = 0,
                        //TODO
                        //Position = new SimVector3(),
                        //Heading = ,
                        StationAccessSequences = new List<StationAccessSequence>
                        {
                            new StationAccessSequence
                            {
                                Location = LocationType.Station1,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(17f, 0.5f, 0f) },
                                    new OrientationStep { Angle = 90 },
                                }
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station2,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 210f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, -14.72f) },
                                    new OrientationStep { Angle = 210f },
                                }
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station3,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 330f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, 14.72f) },
                                    new OrientationStep { Angle = 330f },
                                }
                            },
                        }
                    },

                    new ChargingStationParams
                    {
                        Location = LocationType.Start,
                        AccumulatorInitialValue = 0,
                        StationAccessSequences = new List<StationAccessSequence>
                        {
                            new StationAccessSequence
                            {
                                Location = LocationType.Station1,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 90 },
                                    new WaypointStep { Destination = new SimVector3(17f, 0.5f, 0f) },
                                    new OrientationStep { Angle = 90 },
                                }
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station2,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 210f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, -14.72f) },
                                    new OrientationStep { Angle = 210f },
                                }
                            },
                            new StationAccessSequence
                            {
                                Location = LocationType.Station3,
                                Steps = new List<NavigationStep>
                                {
                                    new OrientationStep { Angle = 330f },
                                    new WaypointStep { Destination = new SimVector3(-8.5f, 0.5f, 14.72f) },
                                    new OrientationStep { Angle = 330f },
                                }
                            }
                        }
                    },
                },
            };
        }
    }
}