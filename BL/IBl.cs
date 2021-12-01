using System.Collections.Generic;
using System;
using IBL.BO;
namespace IBL
{
    public interface IBl
    {
        // private void initDrone();

        public void addStation(BaseStation stationToAdd);

        public void addDrone(DroneToList droneToAdd, int stationId);

        public void addCustomer(IBL.BO.Customer CustomerToAdd);

        public void addParcel(IBL.BO.Parcel parcelToAdd);

        public BaseStation GetStation(int id);

        //private double getDroneBattery(int droneId);
        public DroneToList GetDrone(int id);

        public IBL.BO.Parcel GetParcel(int id);

        public List<IBL.BO.BaseStation> GetStations();

        public List<IBL.BO.DroneToList> GetDrones();//all the drones to list i hope thats ok

        public List<IBL.BO.Customer> GetCustomer();

        public List<IBL.BO.Parcel> GetParcels();


        public IBL.BO.Customer GetCustomer(int id);

        //private IEnumerable<ParcelCustomer> getCustomerShippedParcels(int id);

        //private Location findDroneLocation(DroneToList drone);

        //private Location findClosetBaseStationLocation(Ilocatable fromLocatable);

        public void deleteDrone(int droneID);

        //private double distanceCalculation(Location from, Location to);

        //private double euclideanDistance(Location from, Location to);

        //private bool isDroneWhileShipping(DroneToList drone);


        //private int calcMinBatteryRequired(DroneToList drone);
        public void updateDroneName(int droneID, string dModel);

        public void updateStation(int stationID, int AvlblDCharges, string Name = " ");

        public void updateCustomer(int customerID, string Name = " ", int phoneNum = 0);

        public void SendToCharge(int droneID, int StationID);//have to send the closest sation that has available sattions

        public void releasingDrone(int droneID, TimeSpan chargingTime);
        public void deliveryParcelToCustomer(int id);

        public void matchingDroneToParcel(int droneID);//didnt finish this function at all 
        public void pickedUpParcelByDrone(int droneID);

    }



}  