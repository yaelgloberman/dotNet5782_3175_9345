using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.IO;
using DalApi;
using DO;
using DalApi.DalXml;

namespace Dal
{

    sealed class DLXML : IDal
    {
        static readonly DLXML instance = new DLXML();
        static DLXML() { }
        DLXML() { }
        private static DLXML Instance { get => instance; }

        #region DS XML Files
        string dronesPath = @"DronesXml.xml";
        string dronesChatgePath = @"DronesChargeXml.xml";
        string parcelsPath = @"ParcelXml.xml";
        string customersPath = @"Customer.xml";
        string stationPath = @"StationsXml.xml";

        //public DalObject()
        //{
        //    DataSource.Initialize();///intialize constructor for the data object ib the nmain.
        //}
        //public double[] ChargeCapacity()
        //{

        //    double[] arr = { DataSource.Config.available, DataSource.Config.light, DataSource.Config.average, DataSource.Config.heavy, DataSource.Config.rateLoadingDrone };
        //    return arr;
        //}


        public IEnumerable<droneCharges> chargingGetDroneList()
        {
            return XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesChatgePath);
        }

        public IEnumerable<Customer> GetCustomerList()
        {

            List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            return listOfAllCustomer;
        }

        public IEnumerable<Drone> GetDroneList()
        {
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return listOfAllDrone;
        }
        public IEnumerable<Parcel> GetParcelList()
        {
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            return listOfAllParcel;
        }
        public IEnumerable<Station> GetStationList()
        {
            List<Station> listOfAllStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            return listOfAllStation;
        }
        public bool checkStation(int id)
        {
            List<Station> listOfAllStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            return listOfAllStation.Any(s => s.id == id);
        }
        public void addStation(Station s)
        {

            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElement = (from p in stationRoot.Elements()
                                  where Convert.ToInt32(p.Element("StationCode").Value) == s.id
                                  select p).FirstOrDefault();
            if (stationElement != null)
                throw new findException("The station already exist in the system");

            XElement stationElem = new XElement("Station", new XElement("id", s.id),
                                   new XElement("latitude", s.latitude),
                                   new XElement("longitudr", s.longitude),
                                   new XElement("chargeSlots",s.chargeSlots),
                                   new XElement("name", s.name));
            stationRoot.Add(stationElem);
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
     public Station GetStation(int id)
     {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var station = (from p in stationRoot.Elements()
                           where p.Element("StationCode").Value == id.ToString()
                           select p).FirstOrDefault();

            if (station != null)
                return new Station()
                {
                    id = Convert.ToInt32(station.Element("id").Value),
                    name = station.Element("name").Value,
                    latitude = Convert.ToDouble(station.Element("latitude").Value),
                    longitude = Convert.ToDouble(station.Element("longitude").Value),
                    chargeSlots = Convert.ToInt32(station.Element("charhgeSlots").Value),
                };
            throw new findException("The station doesn't exist in system");
     }
        public void DeleteStation(Station s)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("StationCode").Value) == s.id
                                       select p).FirstOrDefault();
            if (stationElement == null)
                throw new findException("This station doesn't exist in system");
            stationElement.Remove();
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
        
        public void UpdateStation(int stationId, Station s)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("StationCode").Value) == s.id
                                       select p).FirstOrDefault();
            stationElement.Element("name").Value = s.name;
            stationElement.Element("latitude").Value =s.latitude.ToString();
            stationElement.Element("longitude").Value = s.longitude.ToString();
            stationElement.Element("chargeSlots").Value = s.chargeSlots.ToString();
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
    public IEnumerable<Station> getStations()
    {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            return from p in stationRoot.Elements()
                   select new Station()
                   {
                       id = Convert.ToInt32(p.Element("id").Value),
                       name = p.Element("name").Value,
                       latitude = Convert.ToDouble(p.Element("latitude").Value),
                       longitude = Convert.ToDouble(p.Element("longitude").Value),
                       chargeSlots = Convert.ToInt32(p.Element("chargeSlots").Value),
                   };
        }
        public int AvailableChargingSlots()
    {
        Station station = new Station();
        return station.chargeSlots;
    }
    public bool checkParcel(int id)
    {
        List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        return listOfAllParcel.Any(p => p.id == id);
    }
    public int addParcel(Parcel p)
    {
        List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        if (!listOfAllParcel.Exists(item => item.id == p.id))
            throw new AddException("drone already exist");
        if (p.id == 0)
        {
            p.id = listOfAllParcel.parcelSerial++;
        }
        listOfAllParcel.Add(p);
        XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
        return p.id;
    }
    public void attribute(int dID, int pID)//the function attribute parcel to drone
    {
        List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        Drone tmpD = GetDrone(dID);
        Parcel tmpP = GetParcel(pID);
        listOfAllParcel.RemoveAll(m => m.id == tmpP.id);   //removing all the data from the place in the list the equal to tmpP id
        tmpP.droneId = tmpD.id;        //attribute drones id to parcel 
        tmpP.scheduled = DateTime.Now; //changing the time to be right now
        listOfAllParcel.Add(tmpP); //adding to the parcel list tmpP
        XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
    }
    public void PickUpPackageByDrone(int dID, int pID)// the function picking up the parcel by the drone
    {
        List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
        GetDrone(dID);
        GetParcel(pID);
        for (int i = 0; i < listOfAllParcel.Count; i++)  //iterat that goes through all the parcel list
        {
            if (listOfAllParcel[i].id == pID)// if the pId equal to the parcel list 
            {
                Parcel tmpP = listOfAllParcel[i];  //puting into tmpP the parcel that equal to pID
                tmpP.pickedUp = DateTime.Now; //changing the time to be right now
                listOfAllParcel[i] = tmpP;//puting into the parcel list the new tmpP
            }
        }
        for (int i = 0; i < listOfAllDrone.Count; i++) //iterat that goes throe all the drone list
        {
            if (listOfAllDrone[i].id == dID) //iterat that goes through all the parcel list
            {
                Drone tmpD = listOfAllDrone[i];//puting into tmpD the drone that equal to dID
                listOfAllDrone[i] = tmpD; //puting into the drone list the new tmpD
            }
        }
        XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
        XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrone, dronesPath);
    }

        public void DeliveryPackageCustomer(int cID, int pId, DO.Proirities proirity)//updating the drone when irt was called from the Customer
        {
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            Parcel tmpP = GetParcel(pId);
            //tmpP.isDelivered = true;
            Customer tmpC = GetCustomer(cID);
            listOfAllParcel.RemoveAll(m => m.id == tmpP.id);//removing the parcel with the given id
            tmpP.priority = proirity;
            tmpP.targetId = tmpC.id;
            tmpP.delivered = DateTime.Now;
            listOfAllParcel.Add(tmpP);
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
        }
        public IEnumerable<Parcel> GetParcels()
        {
            return XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        }
        public Parcel GetParcel(int id)//function that gets id and finding the parcel in the parcels list and returns parcel
        {
            Parcel? parcel = null;
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            parcel = listOfAllParcel.Find(x => x.droneId == id);
            if (parcel != null)
                return (Parcel)parcel;
            throw new findException("The parcel doesn't exist");
        }
        public void deleteParcel(Parcel p)
        {
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            if (!listOfAllParcel.Exists(item => item.id == p.id))
                throw new findException("parcel");
            listOfAllParcel.Remove(p);
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
        }

        //******************************************* ADDED FUNCTIONS FOR THE BL************************************************
        public List<Parcel> UndiliveredParcels()
        {
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            List<Parcel> unDeliveredP = new List<Parcel>();
            DateTime dateTime_Help = new DateTime(0, 0, 0);
            foreach (Parcel p in listOfAllParcel)
            {
                if (p.delivered == dateTime_Help && p.droneId > 0)
                    unDeliveredP.Add(p);
            }
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
            return unDeliveredP;
        }
        public IEnumerable<Parcel> GetChargedDrone(Func<Parcel, bool> predicate = null)
            => predicate == null ? DataSource.parcels : DataSource.parcels.Where(predicate);

        public droneCharges GetChargedDrone(int id)//finding a drone in the drone charging list
        {
            droneCharges? dronecharges = null;
            List<droneCharges> listOfAllDroneCharge = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesChatgePath);
            dronecharges = listOfAllDroneCharge.Find(x => x.droneId == id);
            if (dronecharges != null)
                return (droneCharges)dronecharges;
            throw new findException("The drone charge in path doesn't exist");
        }
        public void AddDroneCharge(droneCharges droneCharges)
        {
            List<droneCharges> listOfAllDroneCharge = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesChatgePath);
            if (!listOfAllDroneCharge.Exists(x => x.droneId == droneCharges.droneId))
                throw new findException("This drone charge already exist");
            listOfAllDroneCharge.Add(droneCharges);
        }
        //public void RemoveDroneCharge(droneCharges droneCharges)
        //{
        //    DataSource.chargingDrones.Remove(droneCharges);
        //}
        public IEnumerable<droneCharges> GetDroneIdInStation(int id)
        {
            List<droneCharges> listOfAlldroneCharges = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesPath);
            listOfAlldroneCharges.ForEach(cd => { if (cd.stationId == id) listOfAlldroneCharges.Add(cd); });
            XMLTools.SaveListToXMLSerializer<droneCharges>(listOfAlldroneCharges,dronesPath);
            return listOfAlldroneCharges;
        }
        public IEnumerable<droneCharges> GetChargedDrone(Func<droneCharges, bool> predicate = null)
         => predicate == null ? DataSource.chargingDrones : DataSource.chargingDrones.Where(predicate);

        public bool CheckDrone(int id)
        {
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return listOfAllDrone.Any(d => d.id == id);
        }
        public void addDrone(Drone d)
        {
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (listOfAllDrone.Exists(item => item.id == d.id))
                throw new AddException("drone already exist");
            listOfAllDrone.Add(d);
        }
        public Drone GetDrone(int id)//function that gets id and finding the drone in the drones list and returns drone 
        {
            Drone? drone = null;
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            drone = listOfAllDrone.Find(x => x.id == id);
            if (drone != null)
                return (Drone)drone;
            throw new findException("The point in path doesn't exist");
        }
        //public void SendToCharge(int droneId, int stationId)//update function that updates the station and drone when the drone is sent to chatge
        //{
        //    GetDrone(droneId);
        //    GetStation(stationId);
        //    droneCharges dCharge = new droneCharges();
        //    Station tmpS = new Station();
        //    dCharge.stationId = stationId;//maching the drones id
        //    DataSource.stations.ForEach(s => { if (s.id == stationId) tmpS = s; });
        //    DataSource.stations.RemoveAll(s => s.id == dCharge.stationId);
        //    tmpS.chargeSlots--;
        //    DataSource.stations.Add(tmpS);
        //    dCharge.droneId = droneId;
        //    dCharge.stationId = stationId;
        //    DataSource.chargingDrones.Add(dCharge);//adding the drone to the drone chargiong list
        //}
        //public void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        //{
        //    Drone tmpD = GetDrone(dC.droneId);
        //    Station tmpS = GetStation(dC.stationId);
        //    DataSource.drones.RemoveAll(m => m.id == dC.droneId);//removing the parcel with the given id
        //    DataSource.stations.RemoveAll(s => s.id == dC.stationId);
        //    //tmpD.status = DroneStatuses.available;
        //    //tmpD.bateryStatus = 100;
        //    DataSource.drones.Add(tmpD);
        //    tmpS.chargeSlots++;
        //    GetStationList().ToList().Add(tmpS);
        //    DataSource.chargingDrones.Remove(dC);//removing the drone from the drone charging list
        //}
        public void updateDrone(int droneId, string droneModel)
        {
            bool flag = false;
            List<Drone> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            Drone myDrone = listOfAllUsers.Find(x => x.id == droneId);
            myDrone.model = droneModel;
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllUsers, dronesPath);
            if (!flag)
                throw new findException("This user doesn't exist in the system");
        }
        public IEnumerable<Drone> GetDrones()
        {
            return XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
        }
        public IEnumerable<Drone> IEDroneList(Func<Drone, bool> predicate = null)
        {
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            var list = from Drone in listOfAllDrone
                       where predicate(Drone)
                       select Drone;
            return list;
        }
        public bool checkCustomer(int id)
        {
            List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            return listOfAllCustomer.Any(c => c.id == id);
        }
        public void addCustomer(Customer c)
        {
            List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            if (listOfAllCustomer.Exists(item => item.id == c.id))
            {
                if (c.isActive == false)
                {
                    listOfAllCustomer.Remove(c);
                    c.isActive = true;
                    listOfAllCustomer.Add(c);
                    return;
                }
            }
            else
                throw new AddException("customer already exist");
            listOfAllCustomer.Add(c);
        }

        public Customer GetCustomer(int id)//function that gets id and finding the Customer in the Customers list and returns Customer
        {
            Customer? customer = null;
            List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            customer = listOfAllCustomer.Find(x => x.id == id);
            if (customer != null)
                return (Customer)customer;
            throw new findException("The customer doesn't exist");
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
        }
        public IEnumerable<Parcel> GetCustomerReceivedParcels(int customerId) //אני לא בטוחה שזה טוב אבל מה שניסיתי לעשות זה לבדוק ברשיה של כל החבילות אם התז אותו דבר כמו של הלקוח וגם המאפיין הבוליאני אם רבלתי שוו  אז החבילה שייכת לו
        {
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            //IEnumerable<Parcel> parcelTemp = new List<Parcel>();
            listOfAllParcel.ForEach(p => { if (p.targetId == customerId && p.isRecived) listOfAllParcel.ToList().Add(p); });
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
            return listOfAllParcel;
        }
        public IEnumerable<Parcel> getCustomerShippedParcels(int customerId) //אני לא בטוחה שזה טוב אבל מה שניסיתי לעשות זה לבדוק ברשיה של כל החבילות אם התז אותו דבר כמו של הלקוח וגם המאפיין הבוליאני אם רבלתי שוו  אז החבילה שייכת לו
        {
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            listOfAllParcel.ForEach(p => { if (p.targetId == customerId && p.isShipped) listOfAllParcel.ToList().Add(p); });
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcel, parcelsPath);
            return listOfAllParcel;
        }

        //public void deleteCustomer(Customer c)
        //{
        //    if (!DataSource.Customers.Exists(item => item.id == c.id))
        //        throw new findException("Customer");
        //    DataSource.Customers.Remove(c);
        //    c.isActive = false;
        //    DataSource.Customers.Add(c);
        //}
        public string GetCustomerName(int id)
        {
            string name = GetCustomer(id).name;
            return name;
        }

        public void updateCustomer(int customerId, Customer c)
        {
            bool flag = false;
            if (!flag)
                throw new findException("This user doesn't exist in the system");
            try
            {
                List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
                Customer myCustomer = listOfAllCustomer.Find(x => x.id == customerId);
                listOfAllCustomer.Remove(myCustomer);
                myCustomer = c;
                listOfAllCustomer.Add(myCustomer);
                XMLTools.SaveListToXMLSerializer<Customer>(listOfAllCustomer, dronesPath);

            }
            catch (Exception)
            {
                throw new findException("could not find customer");
            }
        }
        public IEnumerable<Customer> GetCustomer(Func<Customer, bool> predicate = null)
        => predicate == null ? DataSource.Customers : DataSource.Customers.Where(predicate);
    }
}

///

    #endregion

