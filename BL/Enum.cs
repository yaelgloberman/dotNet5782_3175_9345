﻿namespace IBL.BO
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
        Created, Assigned, PickedUp, Delivered
    }
    public enum DroneStatus
    {
        available,charge,delivery
    }
    public enum BoolParcelStatus
    {
        Awaitingcollection, OnTheWayToDestination
    }
}