namespace BringMIPHome.Simulation
{
    //public enum RoleReassignmentRule
    //{
    //    LocalRotation,
    //    Random,
    //}

    public static class RoleType
    {
        public const int Depleted = 0;
        public const int BreadCrumb = 1;
        public const int Jackpot = 2;
        public const int Super = 3;
    }

    public enum LocationType
    {
        None = -1,

        Station1 = 0,
        Station2 = 1,
        Station3 = 2,
        Station4 = 3,

        Start = 100,
    }

    public enum ActionType
    {
        None,
        Extract,
        Upload,
        GoToStation1,
        GoToStation2,
        GoToStation3,
        GoToStation4,
        Fix,
        DetachBattery,
        AttachBattery
    }

    public enum DoneReasonType
    {
        NotDone,
        EnergyDepleted,
        TimeExpired,
        TargetEnergyReached
    }




}
