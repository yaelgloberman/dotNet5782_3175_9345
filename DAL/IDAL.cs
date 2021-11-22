using System.Collections.Generic;
using IDAL.DO;
 namespace IDAL
 {
    namespace DO
    {
        public interface IDal
        {
            void addCustomer(Customer c);
            void addDrone(Drone d);
            void addParcel(Parcel p);
            void addStation(Station s);
            void attribute(int dID, int pID);
            double[] ChargeCapacity();
            IEnumerable<droneCharges> chargingDroneList();
            IEnumerable<Customer> CustomerList();
            void DeliveryPackageCustomer(int cID, int pId, Proirities proirity);
            IEnumerable<Drone> droneList();
            droneCharges GetChargedDrone(int id);
            Customer GetCustomer(int id);
            Drone GetDrone(int id);
            Parcel GetParcel(int id);
            Station GetStation(int id);
            void MenuPrint(string action);
            IEnumerable<Parcel> parcelList();
            void PickUpPackageByDrone(int dID, int pID);
            void releasingDrone(droneCharges dC);
            void SendToCharge(int droneId, int stationId);
            IEnumerable<Station> stationList();
            public List<Parcel> UndiliveredParcels();

        }
    }
}
