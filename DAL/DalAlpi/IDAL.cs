using System;
using System.Collections.Generic;
using DO;
namespace DalApi
{
    public interface IDal
    { 
            void addCustomer(Customer c);
            void addDrone(Drone d);
            int addParcel(Parcel p);
            void addStation(Station s);
            void attribute(int dID, int pID);
            public double[] ChargeCapacity();
            IEnumerable<droneCharges> chargingGetDroneList();
            IEnumerable<Customer> GetCustomerList();
            public void AddDroneCharge(DO.droneCharges droneCharges);
            void DeliveryPackageCustomer(int cID, int pId, Proirities proirity);
            IEnumerable<Drone> GetDroneList();
            droneCharges GetChargedDrone(int id);
            Customer GetCustomer(int id);
            Drone GetDrone(int id);
            Parcel GetParcel(int id);
            public void RemoveDroneCharge(droneCharges droneCharges);
           public Station GetStation(int id);
            public IEnumerable<Drone> IEDroneList(Func<Drone, bool> predicate = null);
            public IEnumerable<Parcel> getCustomerShippedParcels(int id);
            public IEnumerable<droneCharges> GetDroneIdInStation(int id);
            public IEnumerable<Drone> GetDrones();
            public IEnumerable<Customer> GetCustomers();
            public IEnumerable<Station> getStations();
            public IEnumerable<Parcel> GetParcels();
            public string GetCustomerName(int id);
            IEnumerable<Parcel> GetParcelList();
            void PickUpPackageByDrone(int dID, int pID);
            void releasingDrone(droneCharges dC);
            void SendToCharge(int droneId, int stationId);
            IEnumerable<Station> GetStationList();
            public List<Parcel> UndiliveredParcels();

            /// ////////////////////////////////////////////////////////////
            /// </summary>
            /// <param name="d"></param>
            public void deleteParcel(Parcel p);
           public void updateStation(int stationId, Station s);
            public int AvailableChargingSlots();
            public void updateDrone(int droneId, string droneModel);
            public void updateCustomer(int customerId, Customer c);
          
    }
}