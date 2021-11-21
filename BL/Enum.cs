namespace IBL.BO
{
    public enum Weight
    {
        light, avergae, heavy
    }
    public enum Priority
    {
        regular, fast, emergency
    }

    public enum ParcelStatus
    {
        Created, Assigned, Picke, Delivered
    }
    public enum DroneStatus
    {
        available,charge,delivery
    }

    public enum BatteryStatus     //not sure i need this
    {
        empty,average, full       //not sure how to describe average
    }
    public enum BoolParcelStatus
    {
        Awaitingcollection, OnTheWayToDestination
    }
}