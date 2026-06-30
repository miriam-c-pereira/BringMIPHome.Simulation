namespace BringMIPHome.Simulation
{
    public enum RoleReassignmentRule
    {
        LocalRotation,
        Random,
    }

    public enum RoleType
    {
        Jackpot,
        BreadCrumb,
        Depleted
    }

    public enum LocationType
    {
        None,
        Start,
        Station1,
        Station2,
        Station3,
        Station4,
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
