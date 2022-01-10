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

namespace Dal
{

    sealed class DalXml : IDal
    {
        internal static string[] CapatalLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L" };
        internal static string[] stationName = { "Jerusalem", "Eilat" };
        internal static string[] droneName = { "Reaper", "Shadow", "Grey Eagle", "Global Hawk", "Pioneer", "Fire Scout", "Snowgoose", "Hunter", "Stalker", "GNAT", "Wing Loong II", "AVENGER", "Apollo Earthly", "AirHaven", "indRazer", "Godspeed", "Phantom", "Novotek", "Tri-Propeller", "WikiDrone" };
        internal static string[] customerName = { "Eliana", "Adina", "Shifra", "Yaeli", "Shirel", "Yisca", "Ruchama", "Noa", "Dan", "Eliezer" };
        internal static List<Drone> drones = new List<Drone>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<Station> stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<droneCharges> chargingDrones = new List<droneCharges>();

        public static Random random = new();
        static readonly IDal instance = new DalXml();
       private DalXml() { }
        public static IDal Instance { get => instance; }
     // private DalXml() { Initialize(); } // default constructer calls on initialize func
        #region DS XML Files
        string configPath = @"ConfigXml.xml";
        string dronesPath = @"DroneXml.xml";
        string dronesChatgePath = @"DroneChargeXml.xml";
        string parcelsPath = @"ParcelXml.xml";
        string customersPath = @"CustomerXml.xml";
        string stationPath = @"StationXml.xml";
        private static double getRandomCordinates(double num1, double num2)
        {
            return (random.NextDouble() * (num2 - num1) + num1);
        }

        public void Initialize()
        {
            createCustomer();
            CreateDrone();
            CreateParcel();
            CreateStation();

        }


        void CreateStation()
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);


            for (int i = 0; i < 2; i++)
            {
                int id = random.Next(111111111, 999999999);
                string name = stationName[i];
                double longitude = getRandomCordinates(34.3, 35.5);
                double latitude = getRandomCordinates(31.0, 33.3);
                int chargeSlots = random.Next(5, 100);
                XElement stationElem = new XElement("Station",
                                 new XElement("id", id),
                                 new XElement("latitude", latitude),
                                 new XElement("longitude", longitude),
                                 new XElement("chargeSlots", chargeSlots),
                                 new XElement("name", name));
                stationRoot.Add(stationElem);

            }
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }

        void CreateDrone()
        {
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            for (int i = 0; i < 10; i++)
            {
                listOfAllDrone.Add(new Drone()
                {
                    id = random.Next(111111111, 999999999),
                    model = droneName[i],
                    maxWeight = (WeightCatigories)random.Next(1, 3),
                });
            }
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrone, dronesPath);
        }
        void createCustomer()
        {
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            string p = CapatalLetters[10];
            for (int i = 0; i < 10; i++)
            {
                listOfAllCustomer.Add(new Customer()
                {
                    id = random.Next(11111111, 99999999),
                    name = customerName[i],
                    phoneNumber = "05" + random.Next(11111111, 99999999),
                    longitude = getRandomCordinates(34.3, 35.5),
                    latitude = getRandomCordinates(31.0, 33.3),
                    Password = CapatalLetters[i] + p,
                    isCustomer = true,
                });

            }
            for (int i = 8; i < 10; i++) //creates 2 workers
                listOfAllCustomer.Add(new Customer()
                {
                    id = random.Next(100000000, 999999999),
                    name = customerName[i],
                    phoneNumber = "05" + random.Next(00000000, 99999999),
                    longitude = getRandomCordinates(34.3, 35.5),
                    latitude = getRandomCordinates(31.0, 33.3),
                    Password = CapatalLetters[i] + p,
                    isCustomer = false,
                });
            XMLTools.SaveListToXMLSerializer(listOfAllCustomer, customersPath);
            XMLTools.SaveListToXMLSerializer(listOfAllDrone, dronesPath);
        }



        void CreateParcel()
        {
            List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            XElement dalConfig = XElement.Load(@"xml\dal-config.xml");
            int parcelId = Convert.ToInt32(dalConfig.Element("parcelSerial").Value);
            dalConfig.Element("parcelSerial").Value = (parcelId + 10).ToString();
            dalConfig.Save(@"xml\dal-config.xml");
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            for (int i = 0; i < 10; i++)
            {
                var parcel = new Parcel();
                parcel.id = parcelId++;
                parcel.senderId = listOfAllCustomer[(i + 5) % 9].id;
                parcel.targetId = listOfAllCustomer[i].id;
                parcel.priority = (Proirities)random.Next(1, 3);
                parcel.weight = (WeightCatigories)random.Next(1, 3);

                if ((listOfAllDrone.ToArray()[i].id) % 2 == 0)
                {
                    parcel.droneId = listOfAllDrone.ToArray()[i].id;
                    parcel.requested = DateTime.Now.AddDays(-1);
                    parcel.scheduled = DateTime.Now;

                    if ((listOfAllDrone.ToArray()[i].id) % 4 == 0)
                    {
                        parcel.pickedUp = DateTime.Now.AddDays(2);
                    }
                }
                else
                {
                    parcel.droneId = 0;
                    parcel.requested = DateTime.Now;
                    parcel.scheduled = null;
                    parcel.pickedUp = null;
                    parcel.delivered = null;
                }
                listOfAllParcel.Add(parcel);
            }
            XMLTools.SaveListToXMLSerializer(listOfAllParcel, parcelsPath);
            XMLTools.SaveListToXMLSerializer(listOfAllDrone, dronesPath);
        }

        public IEnumerable<droneCharges> chargingGetDroneList()
        {
            IEnumerable<droneCharges> chargingList = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesChatgePath);
            return from droneCharge in chargingList
                   select droneCharge;//לעשות קלון 
        }

        public IEnumerable<Customer> GetCustomerList()
        {

            List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            return listOfAllCustomer;
        }

        public IEnumerable<Drone> GetDroneList()
        {
            IEnumerable<Drone> listOfDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return from drone in listOfDrone
                   select drone;
        }

        public IEnumerable<Parcel> GetParcelList()
        {
            IEnumerable<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            return from parcel in listOfAllParcel
                   select parcel;
        }
        public IEnumerable<Station> GetStationList()
        {

            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationL = from s in stationRoot.Elements()
                           select new Station()
                           {
                               id = int.Parse(s.Element("id").Value),
                               name = s.Element("name").Value,
                               longitude = double.Parse(s.Element("longitude").Value),
                               latitude = double.Parse(s.Element("latitude").Value),
                               chargeSlots = int.Parse(s.Element("chargeSlots").Value)
                           };
            return stationL;
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
                                  where int.Parse(p.Element("id").Value) == s.id
                                  select p).FirstOrDefault();
            if (stationElement != null)
                throw new findException("The station already exist in the system");

            XElement stationElem = new XElement("Station", new XElement("id", s.id),
                                   new XElement("latitude", s.latitude),
                                   new XElement("longitude", s.longitude),
                                   new XElement("chargeSlots", s.chargeSlots),
                                   new XElement("name", s.name));
            stationRoot.Add(stationElem);
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
        public Station GetStation(int id)
        {

       
        XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
        var station = (from p in stationRoot.Elements()
                       where int.Parse( p.Element("id").Value) == id
                       select p).FirstOrDefault();

            if (station != null)
            {
                var s = new Station()
                {
                    id = Convert.ToInt32(station.Element("id").Value),
                    name = station.Element("name").Value,
                    latitude = Convert.ToDouble(station.Element("latitude").Value),
                    longitude = Convert.ToDouble(station.Element("longitude").Value),
                    chargeSlots = Convert.ToInt32(station.Element("chargeSlots").Value),
                };
                return s;
            }
            throw new findException("The station doesn't exist in system");
        }


    public void deleteStation(Station s)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("id").Value) == s.id
                                       select p).FirstOrDefault();
            if (stationElement == null)
                throw new findException("This station doesn't exist in system");
            stationElement.Remove();
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }

        public void updateStation(int stationId, Station s)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("id").Value) == s.id
                                       select p).FirstOrDefault();
            stationElement.Element("name").Value = s.name;
            stationElement.Element("latitude").Value = s.latitude.ToString();
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
            XElement dalConfig = XElement.Load(@"xml\dal-config.xml");
            int parcelId = Convert.ToInt32(dalConfig.Element("parcelSerial").Value);
            dalConfig.Element("parcelSerial").Value = (parcelId + 1).ToString();
            dalConfig.Save(@"xml\dal-config.xml");
            if (listOfAllParcel.Exists(item => item.id == p.id))
                throw new AddException("drone already exist");
            if (p.id==0)
            {
                p.id = parcelId;
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
            IEnumerable<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            return from parcel in listOfAllParcel
                   select parcel;
        }
        public Parcel GetParcel(int id)//function that gets id and finding the parcel in the parcels list and returns parcel
        {
            Parcel? parcel = null;
            List<Parcel> listOfAllParcel = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            parcel = listOfAllParcel.Find(x => x.id == id);
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
        //public IEnumerable<Parcel> GetChargedDrone(Func<Parcel, bool> predicate = null)          
        //=> predicate == null ? DataSource.parcels : DataSource.parcels.Where(predicate);

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
            var listOfAllDroneCharge = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesChatgePath);
            if(!(listOfAllDroneCharge.Exists(x=>x.droneId==droneCharges.droneId)))
            {

                listOfAllDroneCharge.Add(droneCharges);
                XMLTools.SaveListToXMLSerializer<droneCharges>(listOfAllDroneCharge, dronesChatgePath);
            }
        }
        public void RemoveDroneCharge(droneCharges droneCharges)
        {
            List<droneCharges> listOfAllDroneCharge = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesChatgePath);
            listOfAllDroneCharge.Remove(droneCharges);
            XMLTools.SaveListToXMLSerializer<droneCharges>(listOfAllDroneCharge, dronesChatgePath);

        }
        public IEnumerable<droneCharges> GetDroneIdInStation(int id)
        {
            var listOfAlldroneCharges = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesChatgePath);
            var list = from droneCharges in listOfAlldroneCharges
                       where (droneCharges.stationId==id)
                       select droneCharges;
            return list;
        }
        //public IEnumerable<droneCharges> GetChargedDrone(Func<droneCharges, bool> predicate = null)
        // => predicate == null ? DataSource.chargingDrones : DataSource.chargingDrones.Where(predicate);

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
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrone, dronesPath);
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
        public void SendToCharge(int droneId, int stationId)//update function that updates the station and drone when the drone is sent to chatge

        {
            List<droneCharges> listOfAlldroneCharges = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesPath);
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            //var s = GetDrone(droneId);
            GetStation(stationId);
            droneCharges dCharge = new ();
            Station tmpS = new();
            dCharge.stationId = stationId;//maching the drones id
            object p = stationRoot.Elements().Where(s => int.Parse(s.Element("id").Value) == stationId);
            stationRoot.Elements().Where(s => Convert.ToInt32( s.Elements("id")) == dCharge.stationId).Remove();
            tmpS.chargeSlots--;
            stationRoot.Add(tmpS);
            dCharge.droneId = droneId;
            dCharge.stationId = stationId;
            listOfAlldroneCharges.Add(dCharge);//adding the drone to the drone chargiong list
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
            XMLTools.SaveListToXMLSerializer<droneCharges>(listOfAlldroneCharges,dronesChatgePath);

        }
        public void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        {
            List<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            List<droneCharges> listOfAlldroneCharges = XMLTools.LoadListFromXMLSerializer<droneCharges>(dronesPath);
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            Drone tmpD = GetDrone(dC.droneId);
            Station tmpS = GetStation(dC.stationId);
            listOfAllDrone.RemoveAll(m => m.id == dC.droneId);//removing the drone with the given id
            stationRoot.Elements().Where(s => Convert.ToInt32(s.Elements("id")) == dC.stationId).Remove();
            //tmpD.status = DroneStatuses.available;
            //tmpD.bateryStatus = 100;
            listOfAllDrone.Add(tmpD);
            tmpS.chargeSlots++;
            GetStationList().ToList().Add(tmpS);
            listOfAlldroneCharges.Remove(dC);//removing the drone from the drone charging list
            XMLTools.SaveListToXMLSerializer<droneCharges>(listOfAlldroneCharges, dronesChatgePath);
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrone, dronesPath);
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
        public void updateDrone(int droneId, string droneModel)
        {
            List<Drone> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            Drone myDrone = listOfAllUsers.Find(x => x.id == droneId);
            listOfAllUsers.Remove(myDrone);
            myDrone.model = droneModel;
            listOfAllUsers.Add(myDrone);
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllUsers, dronesPath);
            if (myDrone.id == 0)
                throw new findException("This user doesn't exist in the system");
        }
        public IEnumerable<Drone> GetDrones()
        {
            IEnumerable<Drone> listOfAllDrone = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return from drone in listOfAllDrone
                   select drone;
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
            if (!(listOfAllCustomer.Exists(item => item.id == c.id)))
            {
                if (c.isActive == true)
                {
                    listOfAllCustomer.Remove(c);
                    c.isActive = true;
                    listOfAllCustomer.Add(c);
                }
            }
            else
                throw new AddException("customer already exist");
            listOfAllCustomer.Add(c);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfAllCustomer, customersPath);

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
            List<Customer> listCustomer= XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
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
            try
            {
                List<Customer> listOfAllCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
                Customer myCustomer = listOfAllCustomer.Find(x => x.id == customerId);
                listOfAllCustomer.Remove(myCustomer);
                myCustomer = c;
                listOfAllCustomer.Add(myCustomer);
                XMLTools.SaveListToXMLSerializer<Customer>(listOfAllCustomer,customersPath);

            }
            catch (Exception)
            {
                throw new findException("could not find customer");
            }
        }
        //public IEnumerable<Customer> GetCustomer(Func<Customer, bool> predicate = null)
        //=> predicate == null ? DataSource.Customers : DataSource.Customers.Where(predicate);

        public void deleteDrone(Drone d)
        {
            List<Drone> DroneRoot = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (!DroneRoot.Exists(item => item.id == d.id))
                throw new findException("drone");
            DroneRoot.Remove(d);
            XMLTools.SaveListToXMLSerializer<Drone>(DroneRoot, dronesPath);

        }

        //public void deleteStation(Station s)
        //{
        //    throw new NotImplementedException();
        //}

        
        public double[] ChargeCapacity()
        {
            List<double> list = XMLTools.LoadListFromXMLSerializer<double>(configPath);

            double[] arr = new double[100];
            for (int i = 0; i < list.Count(); i++)
            { arr[i] = list[i]; }
            return arr;
        }
    }
}

///

#endregion

