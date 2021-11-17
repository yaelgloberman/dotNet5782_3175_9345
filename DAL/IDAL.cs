using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using DAL.DalObject;
        using IDAL.DO;
        using DAL.DalObject;
        using DAL.DalObject;
using System.Threading.Tasks;
namespace IDAL
{
    namespace DAL
    {
        namespace DAL
        {
            interface IDal
            {
                void addStation(Station s);
                void addDrone(Station s);
                void addCustomer(Station s);
                void addParcel(Station s);
                #region UPDATE
                public void attribute(int dID, int pID);//the function attribute parcel to drone
                public void PickUpPackageByDrone(int dID, int pID);// the function picking up the parcel bt the drone

                public void SendToCharge(int droneId, int stationId);//update function that updates the station and drone when the drone is sent to chatge

                public void releasingDrone(droneCharges dC);//update function when we release a drone from its charging slot
                public void DeliveryPackageCustomer(int cID, int pId, Proirities proirity);//updating the drone when irt was called from the customer
                public Station Station(int id);//

                public droneCharges findChargedDrone(int id);//finding a drone in the drone charging list

                public Drone GetDrone(int id);//function that gets id and finding the drone in the drones list and returns drone 

                public customer GetCustomer(int id);//function that gets id and finding the customer in the customers list and returns customer

                public Parcel GetParcel(int id);//function that gets id and finding the parcel in the parcels list and returns parcel

                public Drone getDrone(int droneId);
                public IEnumerable<Station> stationList();
                public IEnumerable<Drone> droneList();
                public IEnumerable<customer> customerList();
                public IEnumerable<Parcel> parcelList();
                public IEnumerable<droneCharges> chargingDroneList();
                public double[] ChargeCapacity();

            }
            // }

        }
        #endregion
    }

}
