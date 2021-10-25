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
        public static void updateParcel(Parcel p)
        {
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].Status == (IDAL.DO.DroneStatuses.available))
                {
                    p.DronrId = DataSource.drones[i].Id;  //not sure abt it
                    //return DataSource.drones[i];
                }
                else
                    Console.WriteLine("there are no availble drones");
            }
        }
        public static void updateDrone(Drone d)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].DronrId == )
                {
                    d.DronrId = DataSource.drones[i].Id;  //not sure abt it
                    //return DataSource.drones[i];
                }
                else
                    Console.WriteLine("there are no availble drones");
            }
        }
    }



}
