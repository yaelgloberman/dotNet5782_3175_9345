using IDAL.DO;
using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public static void addDrone(/*int id ,string model,IDAL.DO.WeightCatigories weight, double BateryStatus,IDAL.DO.DroneStatuses status*/)
    {
        DataSource.drones.Add(new Drone());

    }

}

