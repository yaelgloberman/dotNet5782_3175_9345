using IDAL.DO;
using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalObject
{
    public class DalObject :IDal
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
        /// 
        #region ADD
        public void addStation(Station s)
        { 
          if (!DataSource.Customers.Exists(item => item.id == s.id))
              throw new AddException("station already exist");
            DataSource.stations.Add(s);
        }
        public void addDrone(Drone d)
        { 
            if (!DataSource.Customers.Exists(item => item.id == d.id))
                throw new AddException("drone already exist");
            DataSource.drones.Add(d);
        }
        public void addCustomer(Customer c)
        {
            if (!DataSource.Customers.Exists(item => item.id == c.id))
                throw new AddException("Customer already exist");
            DataSource.Customers.Add(c);
        }
        public void addParcel(Parcel p)
        {
            if (!DataSource.Customers.Exists(item => item.id == p.id))
                throw new AddException("drone already exist");
            DataSource.parcels.Add(p);
        }
        #endregion
        #region UPDATE
        //**************** update functions ***************************
        public void attribute(int dID, int pID)//the function attribute parcel to drone
        {
            Drone tmpD = GetDrone(dID);
            Parcel tmpP = GetParcel(pID);
            DataSource.parcels.RemoveAll(m => m.id == tmpP.id);   //removing all the data from the place in the list the equal to tmpP id
            tmpP.droneId = tmpD.id;        //attribute drones id to parcel 
            tmpP.scheduled = DateTime.Now; //changing the time to be right now
            DataSource.parcels.Add(tmpP); //adding to the parcel list tmpP
        }
        public void PickUpPackageByDrone(int dID, int pID)// the function picking up the parcel by the drone
        {
            GetDrone(dID);
            GetParcel(pID);
            for (int i = 0; i < DataSource.parcels.Count; i++)  //iterat that goes through all the parcel list
            {
                if (DataSource.parcels[i].id == pID)// if the pId equal to the parcel list 
                {
                    Parcel tmpP = DataSource.parcels[i];  //puting into tmpP the parcel that equal to pID
                    tmpP.pickedUp = DateTime.Now; //changing the time to be right now
                    DataSource.parcels[i] = tmpP;//puting into the parcel list the new tmpP
                }
            }
            for (int i = 0; i < DataSource.drones.Count; i++) //iterat that goes throe all the drone list
            {
                if (DataSource.drones[i].id == dID) //iterat that goes through all the parcel list
                {
                    Drone tmpD = DataSource.drones[i];//puting into tmpD the drone that equal to dID
                    DataSource.drones[i] = tmpD; //puting into the drone list the new tmpD
                }
            }
        }
        //int index = DataSource.parcels.FindIndex(item => item.id == pID);
        //if (index == -1)
        //{
        //    throw new findException("id");
        //}
        //Parcel parcel = DataSource.parcels[index];
        //parcel.droneId = dID;
        //parcel.pickedUp = DateTime.Today;
        //DataSource.parcels[index] = parcel;
        //int indexDrone = DataSource.drones.FindIndex(item => item.id == dID);
        //if (indexDrone == -1)
        //{
        //    throw new findException("drone");
        //}
        public void SendToCharge(int droneId, int stationId)//update function that updates the station and drone when the drone is sent to chatge
        {
            GetDrone(droneId); 
            GetStation(stationId); 
            droneCharges dCharge = new droneCharges();
            Station tmpS = new Station();       
            dCharge.stationId = stationId;//maching the drones id
            stationList().ToList().ForEach(s => { if (s.id == stationId) tmpS = s; });
            stationList().ToList().RemoveAll(s => s.id == dCharge.stationId);
            tmpS.chargeSlots--;
            stationList().ToList().Add(tmpS);
            dCharge.droneId = droneId;
            dCharge.stationId = stationId;
            chargingDroneList().ToList().Add(dCharge);//adding the drone to the drone chargiong list
        }
        /// <summary>
        /// updating the system when releasing a drone from its charging slot - 
        // by changing the drones status to availble increasing the chargins available slots, updatating the battery status and removing the drone from the charging drones
        /// </summary>
        /// <param name="dC"></param>
        public void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        {
            Drone tmpD = GetDrone(dC.droneId);
            Station tmpS = GetStation(dC.stationId);
            DataSource.drones.RemoveAll(m => m.id == dC.droneId);//removing the parcel with the given id
            DataSource.stations.RemoveAll(s => s.id == dC.stationId);
            //tmpD.status = DroneStatuses.available;
            //tmpD.bateryStatus = 100;
            droneList().ToList().Add(tmpD);
            tmpS.chargeSlots++;
            stationList().ToList().Add(tmpS);
            DataSource.chargingDrones.Remove(dC);//removing the drone from the drone charging list
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cID"></param>
        /// <param name="pId"></param>
        /// <param name="proirity"></param>
        public void DeliveryPackageCustomer(int cID, int pId, IDAL.DO.Proirities proirity)//updating the drone when irt was called from the Customer
        {
            Parcel tmpP = GetParcel(pId);
            Customer tmpC = GetCustomer(cID);
            DataSource.parcels.RemoveAll(m => m.id == tmpP.id);//removing the parcel with the given id
            tmpP.priority = proirity;
            tmpP.targetId = tmpC.id;
            tmpP.delivered = DateTime.Now;
            DataSource.parcels.Add(tmpP);
        }
        #endregion
        #region GET
        //********************** get function *********************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">//recieves an id of an object in the main  based on the users choice of object and returns the object and in the main it porints the infio about the obj</param>
        /// <returns></returns>
        public Station GetStation(int id)
        {
            Station? tmp = null;
            foreach (Station s in DataSource.stations)
            {
                if (s.id == id)
                {
                    tmp = s;
                    break;
                }
            }
            if (tmp == null)
            {

                throw new IDAL.DO.findException("station does not exist");
            }
            return (Station)tmp;
        }
        public droneCharges GetChargedDrone(int id)//finding a drone in the drone charging list
        {
            droneCharges? tmp = null;
            foreach (droneCharges d in DataSource.chargingDrones)//hi
            {
                if (d.droneId == id)
                {
                    tmp = d;
                    break;
                }
            }
            if (tmp == null)
            {

                throw new IDAL.DO.findException("drone does not exist");
            }
            return (droneCharges)tmp;
        }
        public Drone GetDrone(int id)//function that gets id and finding the drone in the drones list and returns drone 
        {
            Drone? tmp = null;
            foreach (Drone d in DataSource.drones)
            {
                if (d.id == id)
                {
                    tmp = d;
                    break;
                }
            }
            if (tmp == null)
            {

                throw new IDAL.DO.findException("drone does not exist");
            }
            return (Drone)tmp;
        }
        public Customer GetCustomer(int id)//function that gets id and finding the Customer in the Customers list and returns Customer
        {
            Customer? tmp = null;
            foreach (Customer c in DataSource.Customers)
            {
                if (c.id == id)
                {
                    tmp = c;
                    break;
                }
            }
            if (tmp == null)
            {
                throw new findException("Customer does not exist");
            }
            return (Customer)tmp;
        }
        public Parcel GetParcel(int id)//function that gets id and finding the parcel in the parcels list and returns parcel
        {
            Parcel? tmp = null;
            foreach (Parcel p in DataSource.parcels)
            {
                if (p.id == id)
                {
                    tmp = p;
                    break;
                }
            }
            if (tmp == null)
            {
                throw new UpdateException("parcel does not exist");
            }
            return (Parcel)tmp;
        }
        #endregion
        #region ReturnList
        //*********************** printing functions ****************************
        public IEnumerable<Station> stationList()
        {
            return DataSource.stations.ToList();
        }
        public IEnumerable<Drone> droneList()
        {
            return DataSource.drones.ToList();
        }
        public IEnumerable<Customer> CustomerList()
        {
            return DataSource.Customers.ToList();
        }
        public IEnumerable<Parcel> parcelList()
        {
            return DataSource.parcels.ToList();
        }
        public IEnumerable<droneCharges> chargingDroneList()
        {
            return DataSource.chargingDrones.ToList();
        }
        #endregion
        #region delete
        public void deleteCustomer(Customer c)
        {
            if(!DataSource.Customers.Exists(item=>item.id==c.id))
                throw new findException("Customer");
            DataSource.Customers.Remove(c);

        }
        public void deleteDrone(Drone d)
        {
            if (!DataSource.drones.Exists(item => item.id == d.id))
                throw new findException("Customer");
            DataSource.drones.Remove(d);

        }
        public void deleteParcel(Parcel p)
        {
            if (!DataSource.parcels.Exists(item => item.id == p.id))
                throw new findException("parcel");
            DataSource.parcels.Remove(p);

        }
        public void deleteStation(Station s)
        {
            if (!DataSource.stations.Exists(item => item.id == s.id))
                throw new findException("station");
            DataSource.stations.Remove(s);
        }

        #endregion

        public double[] ChargeCapacity()
        {
            double[] arr = new double[] { DataSource.Config.available, DataSource.Config.light, DataSource.Config.average, DataSource.Config.heavy, DataSource.Config.rateLoadingDrone };
            return arr;
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
            Console.WriteLine($"enter 3 to {action} Customers");
            Console.WriteLine($"enter 4 to {action} parcel");
        }

    }


};
