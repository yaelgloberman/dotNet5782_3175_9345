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
        public static void addStation(int id1, int name1, double longi, double lati, int charge)//findout if the sattion name is supposed to be string or int
        {
            Station s = new Station();
            s.id = id1;
            s.name = name1;
            s.latitude = longi;
            DataSource.stations.Add(s);
        }
        public static void addDrone(Drone d)
        {
            DataSource.drones.Add(d);

        }
        public static void addCustomer( customer c)
        {
            DataSource.customers.Add(c);
        }
        public static void addParcel(Parcel p)
        {
            DataSource.parcels.Add(p);
        }
        public static void updatingParcelToDrone(Parcel p,Drone d)
        {
            p.DroneId = d.Id;
            p.Scheduled = DateTime.Today;
            d.Status = (DroneStatuses)2;
        }
        public static void updateParcel(Parcel p)
        {
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].Status == (IDAL.DO.DroneStatuses.available))
                {

                    p.DroneId = DataSource.drones[i].Id;
                    IDAL.DO.Drone tmp = DataSource.drones[i];
                    tmp.Status = DroneStatuses.shipping;
                    DataSource.drones[i] = tmp;
                }
                else
                    Console.WriteLine("there are no availble drones");
            }
        }
        public static void updateDrone(int droneId)
        {
            DataSource.drones.ForEach(d => { if (d.Id == droneId) d.Status = DroneStatuses.shipping; }); // redo i
        }
        /*public static void DeliveryPackageCustomer(int parcelId)
        {
            DataSource.parcels.ForEach(p => { if (p.Id == parcelId) p.Status = DroneStatuses.maintenance; }); // redo it
        }*/
        public static void SendToCharge(int droneId,int sationId)
        {
            DataSource.drones.ForEach(d => { if (d.Id == droneId) d.Status = DroneStatuses.maintenance; });
            IDAL.DO.droneCharges dCharge = new droneCharges();
            dCharge.StationId = sationId;//we have to make sure that we dont need to update the station id in the drone charges
        }
        public static void releaseDroneStation(int droneId)
        {
            DataSource.drones.ForEach(d => { if (d.Id == droneId) d.Status = DroneStatuses.available; });
            //אולי צרך לעדכן גם את התחנת בסיס שאין שם יותר רחפן
        }
        public static Station findStation(int id)//is it static?
        {
            Station empty = new Station();
            for (int i = 0; i < DataSource.stations.Count(); i++)
            {
                if (DataSource.stations[i].id == id)
                    return DataSource.stations[i];
            }
            return empty;///im trying to return null
        }
        public static Drone findDrone(int id)//is it static?
        {
            Drone empty = new Drone();
            for (int i = 0; i < DataSource.stations.Count(); i++)
            {
                if (DataSource.drones[i].Id == id)
                    return DataSource.drones[i];
            }
            return empty;///im trying to return null
        }
        public static customer findCustomer(int id)//is it static?
        {
            customer empty = new customer();
            for (int i = 0; i < DataSource.customers.Count(); i++)
            {
                if (DataSource.customers[i].Id == id)
                    return DataSource.customers[i];
            }
            return empty;
        }
        public static Parcel findParcel(int id)//is it static?
        {
            Parcel empty = new Parcel();
            for (int i = 0; i < DataSource.parcels.Count(); i++)
            {
                if (DataSource.parcels[i].Id == id)
                    return DataSource.parcels[i];
            }
            return empty;
        }
        public static void printStationList()
        {
            DataSource.stations.ForEach(s => { Console.WriteLine(s.ToString()); });
        }
        public static void printDroneList()
        {
            DataSource.drones.ForEach(d => { Console.WriteLine(d.ToString()); });
        }
        public static void printCustomerList()
        {
            DataSource.customers.ForEach(c=> { Console.WriteLine(c.ToString()); });
        }
        public static void printParcelList()
        {
            DataSource.parcels.ForEach(p => { Console.WriteLine(p.ToString()); });
        }
        public static void printParcelWDrone()
        {
            DataSource.parcels.ForEach(p => {if(p.DroneId==0) Console.WriteLine(p.ToString()); });
        }
        public static void printAvailableChrgingStations()
        {
            DataSource.stations.ForEach(s => {if(s.chargeSlots>0)s.ToString(); });
        }
        public static void MenuPrint()
        {
            Console.WriteLine("what would you like to add?");
            Console.WriteLine("enter 1 to add station");
            Console.WriteLine("enter 2 to add drone");
            Console.WriteLine("enter 3 to add customer");
            Console.WriteLine("enter 4 to add parcel");
        }

    }
    

}
