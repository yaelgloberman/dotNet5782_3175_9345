using System.Collections.Generic;
using System;
using BO;
using DalApi;
using BlApi;
namespace BlApi
{
    public interface IBl
    {
        #region DRONE
        public void addDrone(DroneToList droneToAdd, int stationId);
        public void addDrone(int droneId, int stationId, string droneModel, Weight weight);
        public void addCustomer(BO.Customer CustomerToAdd);
        public int addParcel(BO.Parcel parcelToAdd);
        public BaseStation GetStation(int id);
        
        public DroneToList GetDrone(int id);
        public DroneInParcel GetDroneInParcel(int id);
        public BO.Drone returnsDrone(int id);
        public void matchingDroneToParcel(int droneID);
        public List<BO.DroneToList> GetDrones();
        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate);
        public void updateDroneName(int droneID, string dModel);
        public void SendToCharge(int droneID);
        public void releasingDrone(int droneID);

        public void pickedUpParcelByDrone(int droneID);
        #endregion
        #region STATION
        public void addStation(BaseStation stationToAdd);
        public void updateStation(int stationID, int AvlblDCharges, string Name = " ");
        public List<BaseStation> GetStations();
        public List<BaseStationToList> GetBaseStationToLists();
        public IEnumerable<BaseStationToList> allStations(Func<BaseStationToList, bool> predicate);

        #endregion
        #region CUSTOMER
        public BO.CustomerInList GetCustomerToList(int id);
        public void updateCustomer(int customerID, string Name, string phoneNum);

        public List<BO.CustomerInList> GetCustomersToList();
        public List<BO.Customer> GetCustomers();
        public BO.Customer GetCustomer(int id);
        public BO.CustomerInParcel GetCustomerParcel(int id);
        #endregion
        #region PARCEL

        public BO.Parcel GetParcel(int id);

        public IEnumerable<BO.ParcelToList> allParcelsToList(Func<BO.ParcelToList, bool> predicate = null);

        public BO.ParcelToList GetParcelToList(int id);
       
        public List<BO.Parcel> GetParcels();
   
        public List<BO.ParcelToList> GetParcelToLists();
        public List<ParcelCustomer> CustomerSentParcel(int customerID);
        public ParcelStatus GetParcelStatus(int id);
        public Priority GetParcelPriorty(int id);
     
        public IEnumerable<BO.Parcel> allParcels(Func<BO.Parcel, bool> predicate);
        public void deleteParcel(int parcelId);
        public ParcelCustomer GetParcelToCustomer(int id);

        public void deliveryParcelToCustomer(int id);
   
        public ParcelStatus parcelStatus(BO.Parcel parcel);
        //public IEnumerable<DO.droneCharges> GetChargegingDrones();
        public bool CheckValidPassword(string name, string Password);///have to ask if i could make it public ??????

        #endregion
        #region help functoions
        public void startDroneSimulation(int id, Action updateDelegate, Func<bool> stopDelegate);
        public chargeCapacity GetChargeCapacity();
        #endregion
    }
}