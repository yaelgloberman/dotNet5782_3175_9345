using System.Collections.Generic;
using System;
using IBL.BO;
namespace IBL
{
    public interface IBl
    {
        public chargeCapacity GetChargeCapacity();
        public void addStation(BaseStation stationToAdd);
        public void addDrone(DroneToList droneToAdd, int stationId);
        public void addDrone(int droneId, int stationId, string droneModel, Weight weight);
        public void addCustomer(IBL.BO.Customer CustomerToAdd);
        public int addParcel(IBL.BO.Parcel parcelToAdd);
        public BaseStation GetStation(int id);
        
        public DroneToList GetDrone(int id);

        public IBL.BO.Parcel GetParcel(int id);

        public IBL.BO.ParcelToList GetParcelToList(int id);
        public List<BaseStation> GetStations();
        public List<BaseStationToList> GetBaseStationToLists();
        public List<IBL.BO.DroneToList> GetDrones();
        public List<IBL.BO.CustomerInList> GetCustomersToList();
        public List<IBL.BO.Customer> GetCustomers();
        public IBL.BO.Customer GetCustomer(int id);
        public List<IBL.BO.Parcel> GetParcels();

        public List<IBL.BO.ParcelToList> GetParcelToLists();

        public IBL.BO.CustomerInList GetCustomerToList(int id);
        public List<ParcelCustomer> CustomerReceiveParcel(int customerID);

        public void deleteDrone(int droneID);

        public void updateDroneName(int droneID, string dModel);

        public void updateStation(int stationID, int AvlblDCharges, string Name = " ");
        public void updateCustomer(int customerID, string Name = " ", int phoneNum = 0);

        public void SendToCharge(int droneID);//have to send the closest sation that has available sattions

        public void releasingDrone(int droneID, TimeSpan chargingTime);

        public void deliveryParcelToCustomer(int id);
        public void pickedUpParcelByDrone(int droneID);
        public IBL.BO.Drone returnsDrone(int id);
        public void matchingDroneToParcel(int droneID);
      
    }
}