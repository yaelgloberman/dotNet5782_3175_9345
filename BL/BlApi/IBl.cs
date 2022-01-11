using System.Collections.Generic;
using System;
using BO;
using DalApi;
using BlApi;
namespace BlApi
{
    public interface IBl
    {
        #region ADD
        public void addStation(BaseStation stationToAdd);
        public void addDrone(DroneToList droneToAdd, int stationId);
        public void addDrone(int droneId, int stationId, string droneModel, Weight weight);
        public void addCustomer(BO.Customer CustomerToAdd);
        public int addParcel(BO.Parcel parcelToAdd);
        #endregion
        #region GET
        public chargeCapacity GetChargeCapacity();

        public BaseStation GetStation(int id);
        
        public DroneToList GetDrone(int id);
        public DroneInParcel GetDroneInParcel(int id);

        public BO.Parcel GetParcel(int id);
        public IEnumerable<BO.ParcelToList> allParcelsToList(Func<BO.ParcelToList, bool> predicate = null);


        public BO.ParcelToList GetParcelToList(int id);
        public List<BaseStation> GetStations();
        public List<BaseStationToList> GetBaseStationToLists();
        public List<BO.DroneToList> GetDrones();
        public List<BO.CustomerInList> GetCustomersToList();
        public List<BO.Customer> GetCustomers();
        public BO.Customer GetCustomer(int id);
        public List<BO.Parcel> GetParcels();
        public BO.CustomerInParcel GetCustomerParcel(int id);

        public List<BO.ParcelToList> GetParcelToLists();
        public IEnumerable<BaseStationToList> GetBaseStationToList();
        public IEnumerable<BaseStationToList> allStations(Func<BaseStationToList, bool> predicate);
        public List<ParcelCustomer> CustomerSentParcel(int customerID);

        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate);
        public ParcelStatus GetParcelStatus(int id);
        public Priority GetParcelPriorty(int id);
        public BO.CustomerInList GetCustomerToList(int id);
        #endregion
        #region UPDATE
        public void updateDroneName(int droneID, string dModel);

        public void updateStation(int stationID, int AvlblDCharges, string Name = " ");
        public void updateCustomer(int customerID, string Name, string phoneNum);

        public void SendToCharge(int droneID);
        public IEnumerable<BO.Parcel> allParcels(Func<BO.Parcel, bool> predicate);
        public void releasingDrone(int droneID, TimeSpan chargingTime);
        public ParcelCustomer GetParcelToCustomer(int id);
        public void deleteParcel(int parcelId);

        public void deliveryParcelToCustomer(int id);
        public void pickedUpParcelByDrone(int droneID);
        public BO.Drone returnsDrone(int id);
        public void matchingDroneToParcel(int droneID);
        public ParcelStatus parcelStatus(BO.Parcel parcel);
        #endregion
        //public IEnumerable<DO.droneCharges> GetChargegingDrones();
        public bool CheckValidPassword(string name, string Password);///have to ask if i could make it public ??????

        public void startDroneSimulation(int id, Action updateDelegate, Func<bool> stopDelegate);

    }
}