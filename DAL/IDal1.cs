using System.Collections.Generic;
using IDAL.DO;

namespace IDAL.DO
{
    public interface IDal
    {
        void addCustomer(customer c);
        void addDrone(Drone d);
        void addParcel(Parcel p);
        void addStation(Station s);
        void attribute(int dID, int pID);
        double[] ChargeCapacity();
        IEnumerable<droneCharges> chargingDroneList();
        IEnumerable<customer> customerList();
        void DeliveryPackageCustomer(int cID, int pId, Proirities proirity);
        IEnumerable<Drone> droneList();
        droneCharges findChargedDrone(int id);
        customer findCustomer(int id);
        Drone findDrone(int id);
        Parcel findParcel(int id);
        Station findStation(int id);
        Drone getDrone(int droneId);
        void MenuPrint(string action);
        IEnumerable<Parcel> parcelList();
        void PickUpPackageByDrone(int dID, int pID);
        void releasingDrone(droneCharges dC);
        void SendToCharge(int droneId, int stationId);
        IEnumerable<Station> stationList();
    }
}