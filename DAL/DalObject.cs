using IDAL.DO;
using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        public  void addStation(int id1, int name1, double longi, double lati, int charge)//findout if the sattion name is supposed to be string or int
        {
            Station s = new Station();
            s.id = id1;
            s.name = name1;
            s.latitude = longi;
            DataSource.stations.Add(s);
        }
        public  void addDrone(Drone d)
        {
            DataSource.drones.Add(d);

        }
        public  void addCustomer( customer c)
        {
            DataSource.customers.Add(c);
        }
        public  void addParcel(Parcel p)
        {
            DataSource.parcels.Add(p);
        }
        public void attribute(int dID, int pID)
        {
            IDAL.DO.Parcel tmpP = new Parcel();
            IDAL.DO.Drone tmpD = new Drone();
            int i = 0;
            int j = 0;
            for (; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].Id == pID)
                {
                    tmpP = DataSource.parcels[i];
                }
            }
            for (; j < DataSource.drones.Count; j++)
            {
                if (DataSource.drones[j].Id == dID)
                {
                    tmpD = DataSource.drones[j];
                }
            }
            tmpP.Id = tmpD.Id;
            tmpP.Scheduled=DateTime.Today;
            tmpD.Status = (DroneStatuses)2;
            DataSource.parcels[i] = tmpP;
            DataSource.drones[j] = tmpD;
            
        }
        public void PickUpPackageByDrone(int dID, int pID)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].Id == pID)
                {
                    IDAL.DO.Parcel tmpP = DataSource.parcels[i];
                    tmpP.PickedUp = DateTime.Today;
                    DataSource.parcels[i] = tmpP;
                }
            }
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].Id == dID)
                {
                    IDAL.DO.Drone tmpD = DataSource.drones[i];
                    tmpD.Status = IDAL.DO.DroneStatuses.shipping;
                    DataSource.drones[i] = tmpD;
                }
            }
        }
        public  void SendToCharge(int droneId,int sationId)
        {
            DataSource.drones.ForEach(d => { if (d.Id == droneId) d.Status = DroneStatuses.maintenance; });
            IDAL.DO.droneCharges dCharge = new droneCharges();
            dCharge.StationId = sationId;//we have to make sure that we dont need to update the station id in the drone charges
        }
        public  void releaseDroneStation(int droneId)
        {
            DataSource.drones.ForEach(d => { if (d.Id == droneId) d.Status = DroneStatuses.available; });
            //אולי צרך לעדכן גם את התחנת בסיס שאין שם יותר רחפן
        }
        public  Station findStation(int id)//is it static?
        {
            Station empty = new Station();
            for (int i = 0; i < DataSource.stations.Count(); i++)
            {
                if (DataSource.stations[i].id == id)
                    return DataSource.stations[i];
            }
            return empty;///im trying to return null
        }
        public Drone findDrone(int id)//is it static?
        {
            Drone empty = new Drone();
            for (int i = 0; i < DataSource.stations.Count(); i++)
            {
                if (DataSource.drones[i].Id == id)
                    return DataSource.drones[i];
            }
            return empty;///im trying to return null
        }
        public customer findCustomer(int id)//is it static?
        {
            customer empty = new customer();
            for (int i = 0; i < DataSource.customers.Count(); i++)
            {
                if (DataSource.customers[i].Id == id)
                    return DataSource.customers[i];
            }
            return empty;
        }
        public Parcel findParcel(int id)//is it static?
        {
            Parcel empty = new Parcel();
            for (int i = 0; i < DataSource.parcels.Count(); i++)
            {
                if (DataSource.parcels[i].Id == id)
                    return DataSource.parcels[i];
            }
            return empty;
        }
        public Drone getDrone(int droneId)
        {
            Drone drone = new Drone();
            drone.Id = droneId;
            return drone;
        }
        public List<Station>stationList()
        {
            return DataSource.stations;
        }
        public List<Drone> droneList()
        {
            return DataSource.drones;
        }
        public List<customer> customerList()
        {
            return DataSource.customers;
        }
        public List<Parcel> parcelList()
        {
            return DataSource.parcels;
        }

        public void MenuPrint(string action)
        {
            Console.WriteLine($"what would you like to {action}?");
            Console.WriteLine($"enter 1 to {action} station");
            Console.WriteLine($"enter 2 to {action} drone");
            Console.WriteLine($"enter 3 to {action} customer");
            Console.WriteLine($"enter 4 to {action} parcel");
        }
        
    }
    

}
