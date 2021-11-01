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
        //*********************** add station functions******************************************//
        public  void addStation(int id1, int name1, double longi, double lati, int charge)//findout if the sattion name is supposed to be string or int
        {
            // adding all the objects parts 
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
        //**************** update functions ***************************
        public void attribute(int dID, int pID)
        {
            IDAL.DO.Parcel tmpP = new Parcel();
            IDAL.DO.Drone tmpD = new Drone();
            tmpP = findParcel(pID);
            tmpD= findDrone(dID);
            DataSource.parcels.RemoveAll(m => m.Id == tmpP.Id);
            tmpP.DroneId = tmpD.Id;
            tmpP.Scheduled = DateTime.Now;
            DataSource.parcels.Add(tmpP);
        }
        public void PickUpPackageByDrone(int dID, int pID)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)
            {
                if (DataSource.parcels[i].Id == pID)
                {
                    IDAL.DO.Parcel tmpP = DataSource.parcels[i];
                    tmpP.PickedUp= DateTime.Now;
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
        public  void SendToCharge(int droneId,int stationId)//update function that updates the station and drone when the drone is sent to chatge
        {
            IDAL.DO.droneCharges dCharge = new droneCharges();  //creates a new drone object in the drone charges        
            DataSource.drones.ForEach(d => { if (d.Id == droneId) d.Status = DroneStatuses.maintenance; });// changing the drones status
            dCharge.StationId = stationId;//maching the drones id
            stationList().ForEach(s => { if (s.id == stationId) s.chargeSlots--; });//less drone slots in the station
            dCharge.droneId = droneId;
            dCharge.StationId = stationId;
            chargingDroneList().Add(dCharge);//adding the drone to the drone chargiong list
        }
        public  void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        {
            DataSource.drones.ForEach(d => { if (d.Id == dC.droneId) { d.Status = DroneStatuses.available; d.BateryStatus = 100; } });//changing the drone status
            DataSource.stations.ForEach(s => { if (s.id == dC.StationId) s.chargeSlots++; });//increaseing the drone slots since the drone is finished charging
            DataSource.chargingDrones.Remove(dC);//removing the drone from the drone charging list
        }
        public void DeliveryPackageCustomer(int cID,int pId,IDAL.DO.Proirities proirity)//updating the drone when irt was called from the customer
        {
            IDAL.DO.Parcel tmpP = new Parcel();
            IDAL.DO.customer tmpC = new customer();
            tmpP = findParcel(pId);
            tmpC = findCustomer(cID);
            DataSource.parcels.RemoveAll(m => m.Id == tmpP.Id);//removing the parcel with the given id
            tmpP.Priority = proirity;
            tmpP.TargetId = tmpC.Id;
            tmpP.Delivered = DateTime.Now;
            DataSource.parcels.Add(tmpP);
           // parcelList().ForEach(p => { if (p.Id == pId) { p.TargetId = cID;p.Priority = proirity;p.Delivered = DateTime.Now; }; });//its proiority is updated - going fast regular or emergency
        }
        public droneCharges findChargedDrone(int id)//finding a drone in the drone charging list
        {
            droneCharges empty = new droneCharges();
            for (int i = 0; i < DataSource.chargingDrones.Count(); i++)
            {
                if (DataSource.chargingDrones[i].droneId == id)
                    return DataSource.chargingDrones[i];// returnong the drone
            }
            return empty;
        }


        //********************** dispaly function *********************************
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


    //*********************** printing functions ****************************

        //all the list functions below return the lists of the station drone parcel or customer that was created in the dtata source 
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
        public List<droneCharges> chargingDroneList()
        {
            return DataSource.chargingDrones;
        }
 
        public void MenuPrint(string action)//th menue that helps specify the main action 
        {
            Console.WriteLine($"what would you like to {action}?");
            Console.WriteLine($"enter 1 to {action} station");
            Console.WriteLine($"enter 2 to {action} drones");
            Console.WriteLine($"enter 3 to {action} customers");
            Console.WriteLine($"enter 4 to {action} parcel");
        }
        
    }
    

}
