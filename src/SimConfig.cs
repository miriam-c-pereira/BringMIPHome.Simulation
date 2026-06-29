namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;


    [Serializable]
    public class SimConfig
    {
        public float EnergyInit { get; set; } = 1000f;

        public int TimeInit { get; set; } = 1200;

        public float GoToEnergyCost { get; set; } = 20.0f;
        public int GoToTimeCost { get; set; } = 10;

        public float ExtractEnergyCost { get; set; } = 2f;
        public int ExtractTimeCost { get; set; } = 2;

        public float UploadEnergyCost { get; set; } = 4f;
        public int UploadTimeCost { get; set; } = 4;

        public List<ActionType> ExtraActionTypes { get; set; } = new List<ActionType>();

        public List<ChargingStationParams> ChargingStations { get; set; } = new List<ChargingStationParams>
        {
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
            new ChargingStationParams
            {
                Location = LocationType.Station1,
                AccumulatorInitialValue = 0,
                //TODO
                //SetupPosition = new SimVector3(),
                //SetupAngle = ,
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
                //SetupPosition = new SimVector3(),
                //SetupAngle = ,

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
                //SetupPosition = new SimVector3(),
                //SetupAngle = ,
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
        };


        public List<RoleParams> Roles { get; set; } = new List<RoleParams>
        {
            new RoleParams
            {
                Role = RoleType.Jackpot,
                RewardProbability = 0.8f,
                RewardAccumulatorAdd = 10,
                RewardAccumulatorMul = 2,
                DepletionSwitchProbability = 0.1f,
            },
            new RoleParams
            {
                Role = RoleType.BreadCrumb,
                RewardProbability = 0.4f,
                RewardAccumulatorAdd = 10,
                RewardAccumulatorMul = 2,
                DepletionSwitchProbability = 0.1f,
            },
            new RoleParams
            {
                Role = RoleType.Depleted,
                RewardProbability = 0.0f,
                RewardAccumulatorAdd = 0,
                RewardAccumulatorMul = 0,
                DepletionSwitchProbability = 0.0f,
            },
        };

        public RoleReassignmentRule RoleReassignmentRule = RoleReassignmentRule.LocalRotation;

        public float? TargetEnergy = null;

        public int? RandomSeed = null;
    }
}
