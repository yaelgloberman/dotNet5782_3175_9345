using System;
using System.Collections.Generic;
namespace IDAL
 {
    namespace DO
    {
        public interface IDal
        {
            void addCustomer(Customer c);
            void addDrone(Drone d);
            int addParcel(Parcel p);
            void addStation(Station s);
            void attribute(int dID, int pID);
            public double[] ChargeCapacity();
            IEnumerable<droneCharges> chargingDroneList();
            IEnumerable<Customer> CustomerList();

            void DeliveryPackageCustomer(int cID, int pId, Proirities proirity);
            IEnumerable<Drone> droneList();
            droneCharges GetChargedDrone(int id);
            Customer GetCustomer(int id);
            Drone GetDrone(int id);
            Parcel GetParcel(int id);
           public Station GetStation(int id);
            //public IEnumerable<Parcel> getCustomerReceivedParcels(int id);
            public IEnumerable<Parcel> getCustomerShippedParcels(int id);
            public IEnumerable<droneCharges> GetDroneIdInStation(int id);
            public IEnumerable<Drone> GetDrones();
            public IEnumerable<Customer> GetCustomers();
            public IEnumerable<Station> getStations();
            public IEnumerable<Parcel> GetParcels();
            public string GetCustomerName(int id);
            IEnumerable<Parcel> parcelList();
            void PickUpPackageByDrone(int dID, int pID);
            void releasingDrone(droneCharges dC);
            void SendToCharge(int droneId, int stationId);
            IEnumerable<Station> stationList();
            public List<Parcel> UndiliveredParcels();
            public void deleteDrone(Drone d);
            public void deleteParcel(Parcel p);
            public void deleteStation(Station s);
            public void updateStation(int stationId, Station s);
            public int AvailableChargingSlots();
            public void updateDrone(int droneId, string droneModel);
          }
    }
}
