﻿using IDAL.DO;
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
            DataSource.Initialize();///intialize constructor for the data object ib the nmain.
        }
        //*********************** add station functions******************************************//
        /// <summary>
        /// adding station functions 
     ////  the functions recieve an object and add it to the list
     //// or the variables and create a matching aobject and add it
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="name1"></param>
        /// <param name="longi"></param>
        /// <param name="lati"></param>
        /// <param name="charge"></param>
        public  void addStation(int id1, int name1, double longi, double lati, int charge)
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
        public void attribute(int dID, int pID)//the function attribute parcel to drone
        {
            IDAL.DO.Parcel tmpP = new Parcel();//creates a new parcel object
            IDAL.DO.Drone tmpD = new Drone();// creates a new drone object 
            tmpP = findParcel(pID); //finding the parcel and puting the parcel into tmpP   
            tmpD = findDrone(dID);//finding the drone and puting the drone into tmpD
            DataSource.parcels.RemoveAll(m => m.Id == tmpP.Id);   //removing all the data from the place in the list the equal to tmpP id
            tmpP.DroneId = tmpD.Id;  //attribute drones id to parcel 
            tmpP.Scheduled = DateTime.Now; //changing the time to be right now
            DataSource.parcels.Add(tmpP); //adding to the parcel list tmpP
        }
        public void PickUpPackageByDrone(int dID, int pID)// the function picking up the parcel bt the drone
        {
            for (int i = 0; i < DataSource.parcels.Count; i++)  //iterat that goes through all the parcel list
            {
                if (DataSource.parcels[i].Id == pID)// if the pId equal to the parcel list 
                {
                    IDAL.DO.Parcel tmpP = DataSource.parcels[i];  //puting into tmpP the parcel that equal to pID
                    tmpP.PickedUp= DateTime.Now; //changing the time to be right now
                    DataSource.parcels[i] = tmpP;//puting into the parcel list the new tmpP
                }
            }
            for (int i = 0; i < DataSource.drones.Count; i++) //iterat that goes throe all the drone list
            {
                if (DataSource.drones[i].Id == dID) //iterat that goes through all the parcel list
                {
                    IDAL.DO.Drone tmpD = DataSource.drones[i];//puting into tmpD the drone that equal to dID
                    tmpD.Status = IDAL.DO.DroneStatuses.shipping; //changing the status to shipping
                    DataSource.drones[i] = tmpD; //puting into the drone list the new tmpD
                }
            }
        }
        public  void SendToCharge(int droneId,int stationId)//update function that updates the station and drone when the drone is sent to chatge
        {
            IDAL.DO.droneCharges dCharge = new droneCharges();
            IDAL.DO.Station tmpS = new Station();//creates a new drone object in the drone charges        
            DataSource.drones.ForEach(d => { if (d.Id == droneId) d.Status = DroneStatuses.maintenance; });// changing the drones status
            dCharge.StationId = stationId;//maching the drones id
            stationList().ForEach(s => { if (s.id == stationId) tmpS = s; });//less drone slots in the station
            stationList().RemoveAll(s => s.id == dCharge.StationId) ;
            tmpS.chargeSlots--;
            stationList().Add(tmpS);
            dCharge.droneId = droneId;
            dCharge.StationId = stationId;
            chargingDroneList().Add(dCharge);//adding the drone to the drone chargiong list
        }
        /// <summary>
        /// updating the system when releasing a drone from its charging slot - 
        // by changing the drones status to availble increasing the chargins available slots, updatating the battery status and removing the drone from the charging drones
        /// </summary>
        /// <param name="dC"></param>
        public  void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        {
            IDAL.DO.Drone tmpD= new Drone();
            IDAL.DO.Station tmpS = new Station();
            tmpD = findDrone(dC.droneId);
            tmpS = findStation(dC.StationId);
            DataSource.drones.RemoveAll(m => m.Id == dC.droneId);//removing the parcel with the given id
            DataSource.stations.RemoveAll(s => s.id == dC.StationId);
            tmpD.Status = DroneStatuses.available;
            tmpD.BateryStatus = 100;
            droneList().Add(tmpD);
            tmpS.chargeSlots++;
            stationList().Add(tmpS);
            DataSource.chargingDrones.Remove(dC);//removing the drone from the drone charging list
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cID"></param>
        /// <param name="pId"></param>
        /// <param name="proirity"></param>
        public void DeliveryPackageCustomer(int cID, int pId, IDAL.DO.Proirities proirity)//updating the drone when irt was called from the customer
        {
            IDAL.DO.Parcel tmpP = new Parcel();
            IDAL.DO.customer tmpC = new customer();
            tmpP = findParcel(pId);//parcel id
            tmpC = findCustomer(cID);//customer id
            DataSource.parcels.RemoveAll(m => m.Id == tmpP.Id);//removing the parcel with the given id
            tmpP.Priority = proirity;
            tmpP.TargetId = tmpC.Id;
            tmpP.Delivered = DateTime.Now;
            DataSource.parcels.Add(tmpP);
        }
        public droneCharges findChargedDrone(int id)//finding a drone in the drone charging list
        {
            droneCharges empty = new droneCharges();
            for (int i = 0; i < DataSource.chargingDrones.Count(); i++)
            {
                if (DataSource.chargingDrones[i].droneId == id)
                    return DataSource.chargingDrones[i];// returnong the drone
            }
            return empty;//if he didnd found return empty
        }


        //********************** dispaly function *********************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">//recieves an id of an object in the main  based on the users choice of object and returns the object and in the main it porints the infio about the obj</param>
        /// <returns></returns>
        public  Station findStation(int id)//
        {
            Station empty = new Station(); //creating a new station called empty
            for (int i = 0; i < DataSource.stations.Count(); i++) //iterates that goes throught the stations list
            {
                if (DataSource.stations[i].id == id)  //if the id of the stattion is the same as the list station
                    return DataSource.stations[i];//return the station
            }
            return empty;
        }
        public Drone findDrone(int id)//function that gets id and finding the drone in the drones list and returns drone 
        {
            Drone empty = new Drone(); //creating a new drone called empty
            for (int i = 0; i < DataSource.drones.Count(); i++)//iterates that goes throught the drones list
            {
                if (DataSource.drones[i].Id == id)//if the id of the drones is the same as the list drones
                    return DataSource.drones[i];//return the drone
            }
            return empty;//if he didnd found return empty
        }
        public customer findCustomer(int id)//function that gets id and finding the customer in the customers list and returns customer
        {
            customer empty = new customer();//creating a new customer called empty
            for (int i = 0; i < DataSource.customers.Count(); i++)//iterates that goes throught the customer list
            {
                if (DataSource.customers[i].Id == id)//if the id of the customer is the same as the list customers
                    return DataSource.customers[i];//return customer
            }
            return empty; //if he didnd found return empty
        }
        public Parcel findParcel(int id)//function that gets id and finding the parcel in the parcels list and returns parcel
        {
            Parcel empty = new Parcel();
            for (int i = 0; i < DataSource.parcels.Count(); i++)//iterates that goes throught the parcel list
            {
                if (DataSource.parcels[i].Id == id)//if the id of the parcel is the same as the list parcels
                    return DataSource.parcels[i];//return parcel
            }
            return empty;//if he didnd found return empty
        }
        /// <summary>
        /// retuens the drone 
        /// </summary>
        /// <param name="droneId"> recives the drones id from the user in the mian</param>
        /// <returns></returns>
        public Drone getDrone(int droneId)
        {
            Drone drone = new Drone();
            drone.Id = droneId;
            return drone;
        }


    //*********************** printing functions ****************************
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
 /// <summary>
 /// a menue to print in the main to navagte the switch to the correct object
 /// </summary>
 /// <param name="action"></param>
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
