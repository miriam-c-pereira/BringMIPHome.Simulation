namespace BringMIPHome.Simulation
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SimConfig
    {
        public string Id { get; set; }
        
        public string DisplayName { get; set; }
        
        public float EnergyInit { get; set; }
        
        public int TimeInit { get; set; }

        //public float GoToEnergyCost { get; set; }
        //public int GoToTimeCost { get; set; }

        public float NavigationEnergyCost { get; set; }
        
        public float ExtractEnergyCost { get; set; }
        //public int ExtractTimeCost { get; set; }

        public float UploadEnergyCost { get; set; }
        //public int UploadTimeCost { get; set; }

        public float? TargetEnergy { get; set; }

        public int? RandomSeed { get; set; }

        public bool AllowFixAction { get; set; }

        public bool AllowDetachBattery { get; set; }

        public bool EnsureFirstStationIsNotDepleted { get; set; }

        public List<RoleParams> Roles { get; set; } = new List<RoleParams>();

        public List<ChargingStationParams> ChargingStations { get; set; } = new List<ChargingStationParams>();

        public float RoverMoveSpeed { get; set; }

        public float RoverRotationSpeed { get; set; }
    }
}