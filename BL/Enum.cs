namespace BO
{
    public enum Weight
    {
        light=1, average, heavy
    }
    public enum Priority
    {
        regular, fast, emergency
    }

    public enum ParcelStatus
    {
        Created, Assigned, PickedUp, Delivered
    }
    public enum DroneStatus
    {
        available=1,charge,delivery
    }
    public enum BoolParcelStatus
    {
        Awaitingcollection, OnTheWayToDestination
    }
    public enum User
    {
        Worker, Customer
    }
}