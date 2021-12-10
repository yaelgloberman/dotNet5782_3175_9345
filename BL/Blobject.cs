using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
using IBL;
using System.Runtime.Serialization;
namespace BL
{
    public class BL : IBl
    {
        
        public static IDal dal; //= new DalObject.DalObject();
        private static Random rand = new Random();
        private List<DroneToList> drones;
        public static int unavailableChargeSlots;
        private static double getRandomCordinatesBL(double num1, double num2)
        {
            return (rand.NextDouble() * (num2 - num1) + num1);
        }
        //have to fix the num of delivered parcels and the delivery
        public BL()
        {
            {
                unavailableChargeSlots = 0;
                dal = new DalObject.DalObject();
                drones = new List<IBL.BO.DroneToList>();
                bool flag = false;
                Random rnd = new Random();
                double minBatery = 0;
                IEnumerable<IDAL.DO.Drone> d = dal.GetDrones();
                IEnumerable<IDAL.DO.Parcel> p = dal.GetParcels();
                chargeCapacity chargeCapacity = GetChargeCapacity();
                foreach (var item in d)
                {
                    IBL.BO.DroneToList drt = new DroneToList();
                    drt.id = item.id;
                    drt.droneModel = item.model;
                    drt.weight = (IBL.BO.Weight)(int)item.maxWeight;
                    drt.numOfDeliverdParcels = dal.parcelList().Count(x => x.droneId == drt.id);
                    int parcelID = dal.parcelList().ToList().Find(x => x.droneId == drt.id).id;
                    drt.parcelId = parcelID;
                    var baseStationLocations = BaseStationLocationslist();
                    foreach (var pr in p)
                    {
                        if (pr.id == item.id && pr.delivered == DateTime.MinValue)
                        {
                            IDAL.DO.Customer sender = dal.GetCustomer(pr.senderId);
                            IDAL.DO.Customer target = dal.GetCustomer(pr.targetId);
                            IBL.BO.Location senderLocation = new Location { latitude = sender.latitude, longitude = sender.longitude };
                            IBL.BO.Location targetLocation = new Location { latitude = target.latitude, longitude = target.longitude };
                            drt.droneStatus = DroneStatus.delivery;
                            if (pr.pickedUp == DateTime.MinValue && pr.scheduled != DateTime.MinValue)//החבילה שויכה אבל עדיין לא נאספה
                            {
                                drt.location = new Location { latitude = findClosestStationLocation(senderLocation, false, baseStationLocations).latitude, longitude = findClosestStationLocation(senderLocation, false, baseStationLocations).longitude };
                                minBatery = Distance(drt.location, senderLocation) * chargeCapacity.chargeCapacityArr[0];
                                minBatery += Distance(senderLocation, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.weight];
                                minBatery += Distance(targetLocation, new Location { latitude = findClosestStationLocation(targetLocation, false, baseStationLocations).latitude, longitude = findClosestStationLocation(targetLocation, false, baseStationLocations).longitude }) * chargeCapacity.chargeCapacityArr[0];
                            }
                            if (pr.pickedUp != DateTime.MinValue && pr.delivered == DateTime.MinValue)//החבילה נאספה אבל עדיין לא הגיעה ליעד
                            {
                                drt.location = new Location();
                                drt.location = senderLocation;
                                minBatery = Distance(targetLocation, new Location { latitude = findClosestStationLocation(targetLocation, false, baseStationLocations).latitude, longitude = findClosestStationLocation(targetLocation, false, baseStationLocations).longitude }) * chargeCapacity.chargeCapacityArr[0];
                                minBatery += Distance(drt.location, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.weight];
                            }
                            if (minBatery > 100) { minBatery = 100; }
                            drt.batteryStatus = rnd.Next((int)minBatery, 101); // 100/;
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        int temp = rnd.Next(1, 3);
                        if (temp == 1)
                            drt.droneStatus = IBL.BO.DroneStatus.available;
                        else
                            drt.droneStatus = IBL.BO.DroneStatus.charge;
                        if (drt.droneStatus == IBL.BO.DroneStatus.charge)
                        {
                            int r = rnd.Next(0, dal.getStations().Count()), i = 0;
                            IDAL.DO.Station s = new IDAL.DO.Station();
                            foreach (var ite in dal.getStations())
                            {
                                s = ite;
                                if (i == r)
                                    break;
                                i++;
                            }
                            IDAL.DO.droneCharges DC = new droneCharges { droneId = drt.id, stationId = s.id };
                            dal.AddDroneCharge(DC);
                            drt.location = new Location { latitude = s.latitude, longitude = s.longitude };
                            drt.batteryStatus = rnd.Next(0, 21); // 100/;
                        }
                        else
                        {
                            List<IDAL.DO.Customer> lst = new List<IDAL.DO.Customer>();
                            foreach (var pr in p)
                            {
                                if (pr.delivered != DateTime.MinValue)
                                    lst.Add(dal.GetCustomer(pr.targetId));
                            }
                            if (lst.Count == 0)
                            {
                                foreach (var pr in dal.CustomerList())
                                {

                                    lst.Add(pr);
                                }
                            }
                            int l = rnd.Next(0, lst.Count());

                            drt.location = new Location { latitude = lst[l].latitude, longitude = lst[l].longitude };
                            Location Location1 = new Location { latitude = lst[l].latitude, longitude = lst[l].longitude };

                            minBatery += Distance(drt.location, new Location { longitude = findClosestStationLocation(Location1, false, baseStationLocations).longitude, latitude = findClosestStationLocation(Location1, false, baseStationLocations).latitude }) * chargeCapacity.chargeCapacityArr[0];

                            if (minBatery > 100) { minBatery = 100; }

                            drt.batteryStatus = rnd.Next((int)minBatery, 101);/// 100//*/;
                        }

                    }
                    drones.Add(drt);
                }

            }

        }
        public chargeCapacity GetChargeCapacity()
        {
            double[] arr = dal.ChargeCapacity();
            chargeCapacity chargingUsage = new chargeCapacity { pwrAvailable = arr[0], pwrLight = arr[1], pwrAverge = arr[2], pwrHeavy = arr[3], pwrRateLoadingDrone = arr[4], chargeCapacityArr = arr };
            return chargingUsage;
        }


        //public void deliveryParcelToCustomer(int id)
        //{
        //    try
        //    {
        //        IBL.BO.DroneToList drone = GetDrone(id);
        //        IDAL.DO.Parcel parcel = dal.parcelList().ToList().Find(p => p.droneId == id);
        //        IDAL.DO.Station station = dal.stationList().ToList().Find(s => s.id == parcel.targetId);
        //        if (!(drone.droneStatus == DroneStatus.delivery)) ;   //the drone pickedup but didnt delivert yet
        //        throw new ExecutionTheDroneIsntAvilablle();
        //        drone.batteryStatus = calcMinBatteryRequired(drone);
        //        drone.location.latitude = station.latitude;
        //        drone.location.longitude = station.longitude;
        //        drone.droneStatus = DroneStatus.available;
        //        dal.deleteDrone(dal.GetDrone(id));
        //        addDrone(drone, station.id);
        //        IBL.BO.Parcel parcelTemp = new IBL.BO.Parcel();
        //        parcelTemp.requested = DateTime.Now;   //מה זה זמן אספקה
        //        dal.deleteParcel(dal.GetParcel(parcelTemp.id));
        //        addParcel(parcelTemp);

        //    }
        //    catch(findException f)
        //    {
        //        throw new ExecutionEngineException("rcfci");
        //    }

        //}


        /// im supposed to intialize the new drones in a bl format - but not sure how***********
        /// 














        #region Init drone
        private void initializeDrones()
        {
            try
            {
                foreach (var drone in dal.GetDrones())
                {

                    drones.Add(new DroneToList
                    {
                        id = drone.id,
                        droneModel = drone.model,
                        weight = (Weight)drone.maxWeight,
                        droneStatus = DroneStatus.available,
                        batteryStatus = 100,
                        location = new Location
                        {
                            longitude = getRandomCordinatesBL(34.3, 35.5),
                            latitude = getRandomCordinatesBL(31.0, 33.3),
                        }
                    }
                    );
                }
            }

            catch (ArgumentException exp)
            {
                throw new ArgumentException(exp.Message);
            }

            foreach (var drone in drones.ToList())
            {
                if (isDroneWhileShipping(drone))
                {
                    drone.droneStatus = DroneStatus.delivery;
                    drone.location = findDroneLocation(drone);
                    // note : drone.parcelId getting value inside isDroneWhileShipping
                    int minBattery = calcMinBatteryRequired(drone);
                    drone.batteryStatus = (double)rand.Next(minBattery, 100) / 100;
                }
                else
                {
                    drone.droneStatus = (DroneStatus)rand.Next(1, 2);
                    if (drone.droneStatus == DroneStatus.charge)
                    {
                        DroneToList d = GetDrone(drone.id);
                        deleteDrone(d.id);
                        d.droneStatus = DroneStatus.available;
                        int num = rand.Next(1, dal.stationList().Count());
                        var stationId = dal.stationList().ToArray()[num].id;
                        addDrone(d, stationId);
                        drone.location = getBaseStationLocation(stationId);
                        drone.batteryStatus = (double)rand.Next(5, 20);
                        drone.parcelId = 0;
                    }
                    if (drone.droneStatus == DroneStatus.available)
                    {
                        drone.location = findDroneLocation(drone);
                        drone.parcelId = 0;
                        int minBattery = calcMinBatteryRequired(drone);
                        drone.batteryStatus = (double)rand.Next(minBattery, 100) / 100;
                    }
                }
            }
        }

        #endregion
        #region ADD
        #region ADD STATION
        public void addStation(BaseStation stationToAdd)
        {
            List<droneCharges> list = new List<droneCharges>();
            stationToAdd.DroneInChargeList = new List<DroneInCharge>();
            if (!(stationToAdd.id >= 10000000 && stationToAdd.id <= 1000000000))
                throw new validException("the number of the base station id in invalid\n");
            if (stationToAdd.location.longitude < 34.3 || stationToAdd.location.longitude > 35.5)
                throw new validException("the given longitude do not exist in this country\n");
            if (stationToAdd.location.latitude < (double)31 || stationToAdd.location.latitude > 33.3)
                throw new validException("the given latitude do not exist in this country\n");
            if (!(stationToAdd.avilableChargeSlots > 0))
                throw new validException("the given number of available charging slots is negetive\n");
            IDAL.DO.Station stationDo =
                new IDAL.DO.Station()
                {
                    id = stationToAdd.id,
                    name = stationToAdd.stationName,
                    longitude = stationToAdd.location.longitude,
                    latitude = stationToAdd.location.latitude,
                    chargeSlots = stationToAdd.avilableChargeSlots
                };
            try
            {
                dal.addStation(stationDo);
            }
            catch (AddException exp)
            {

                throw new AlreadyExistException(exp.Message);
            }
        }
        #endregion
        #region ADD Drone
        public void addDrone(int droneId, int stationId, string droneModel, Weight weight)
        {
            try
            {
                if (!(droneId >= 10000000 && droneId < 1000000000))
                    throw new validException("the number of the drone id in invalid\n");
                if (!(weight >= (Weight)1 && weight <= (Weight)3))
                    throw new validException("the given weight is not valid\n");
                if (!(stationId >= 10000000 && stationId <= 1000000000))
                    throw new validException("the number of the drone id in invalid\n");
                IDAL.DO.Station stationDl = dal.GetStation(stationId);
                if (stationDl.latitude < (double)31 || stationDl.latitude > 33.3)
                    throw new validException("the given latitude do not exist in this country/\n");
                if (stationDl.longitude < 34.3 || stationDl.longitude > 35.5)
                    throw new validException("the given longitude do not exist in this country/\n");
                DroneToList dtl = new DroneToList();
                dtl.id = droneId;
                dtl.droneModel = droneModel;
                dtl.weight = weight;
                dtl.batteryStatus = (double)rand.Next(20, 40);
                dtl.droneStatus = DroneStatus.available; ///עכשיו שיניתי את זה שזה יהיה פנוי ולא CHARGE
                dtl.location = new Location();
                dtl.location.latitude = stationDl.latitude;
                dtl.location.longitude = stationDl.longitude;
                IDAL.DO.Drone dr = new IDAL.DO.Drone();
                {
                    dr.id = droneId;
                    dr.model = droneModel;
                    dr.maxWeight = (WeightCatigories)weight;
                }
                dal.addDrone(dr);
                drones.Add(dtl);
                IDAL.DO.droneCharges dc = new IDAL.DO.droneCharges { droneId = droneId, stationId = stationId };
            }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
            ///dal.SendToCharge(droneId)
        }

        public void addDrone(DroneToList droneToAdd, int stationId)
        {

            IDAL.DO.Station stationDl = dal.GetStation(stationId);
            droneToAdd.location.latitude = stationDl.latitude;
            droneToAdd.location.longitude = stationDl.longitude;
            if (droneToAdd.batteryStatus == 0){ droneToAdd.batteryStatus = (double)rand.Next(20, 40); }
            if (droneToAdd.droneStatus == 0) { droneToAdd.droneStatus = DroneStatus.charge; }
            if (!(droneToAdd.id >= 10000000 && droneToAdd.id <= 1000000000))
                throw new validException("the number of the drone id in invalid\n");
            if (!(droneToAdd.batteryStatus >= (double)0 && droneToAdd.batteryStatus <= (double)100))
                throw new validException("the status of the drone is invalid\n");
            if (droneToAdd.location.latitude < (double)31 || droneToAdd.location.latitude > 33.3)
                throw new validException("the given latitude do not exist in this country/\n");
            if (droneToAdd.location.longitude < 34.3 || droneToAdd.location.longitude > 35.5)
                throw new validException("the given longitude do not exist in this country/\n");
            if (!(droneToAdd.weight >= (Weight)1 && droneToAdd.weight <= (Weight)3))
                throw new validException("the given weight is not valid\n");
            if (!(stationId >= 10000000 && stationId <= 1000000000))
                throw new validException("the number of the station id in invalid\n");
            droneToAdd.location = getBaseStationLocation(stationId);
            IDAL.DO.Drone drone = new IDAL.DO.Drone();
            drone.id = droneToAdd.id;
            drone.model = droneToAdd.droneModel;
            drone.maxWeight = (WeightCatigories)droneToAdd.weight;
            dal.addDrone(drone);
            drones.Add(droneToAdd);
        }
        #endregion
        #region Add Customer
        public void addCustomer(IBL.BO.Customer CustomerToAdd)
        {

            if (!(CustomerToAdd.id >= 10000000 && CustomerToAdd.id <= 1000000000))
                throw new validException("the id number of the drone is invalid\n");
            if (!(CustomerToAdd.phoneNumber >= 500000000 && CustomerToAdd.phoneNumber <= 0589999999))
                throw new validException("the phone number of the Customer is invalid\n");
            if (CustomerToAdd.location.latitude < (double)31 || CustomerToAdd.location.latitude > 33.3)
                throw new AddException("the given latitude do not exist in this country/\n");
            if (CustomerToAdd.location.longitude < 34.3 || CustomerToAdd.location.longitude > 35.5)
                throw new AddException("the given longitude do not exist in this country/\n");
            if (dal.GetCustomers().ToList().Exists(item => item.id == CustomerToAdd.id))
                throw new AddException("Customer already exist");
            IDAL.DO.Customer CustomerDo = new IDAL.DO.Customer();
            CustomerDo.id = CustomerToAdd.id;
            CustomerDo.name = CustomerToAdd.Name;
            CustomerDo.phoneNumber = CustomerToAdd.phoneNumber;
            CustomerDo.longitude = CustomerToAdd.location.longitude;
            CustomerDo.latitude = CustomerToAdd.location.latitude;
            try
            {
                dal.addCustomer(CustomerDo);
            }
            catch (AddException exp)
            {

                throw new AlreadyExistException("the customer already exist", exp);
            }
            // Nה צריך לבדוק עם SentParcels ReceiveParcel
        }
        #endregion
        #region Add Parcel
        public int addParcel(IBL.BO.Parcel parcelToAdd)
        {
            if (!(parcelToAdd.sender.id >= 10000000 && parcelToAdd.sender.id <= 1000000000))
                throw new validException("the id sender number of the pardel is invalid\n");
            if (!(parcelToAdd.receive.id >= 10000000 && parcelToAdd.receive.id <= 1000000000))
                throw new validException("the id receive number of the parcel is invalid\n");
            if (!(parcelToAdd.weightCategorie >= (Weight)1 && parcelToAdd.weightCategorie <= (Weight)3))
                throw new validException("the given weight is not valid\n");
            if (!(parcelToAdd.priority >= (Priority)0 && parcelToAdd.priority <= (Priority)3))
                throw new validException("the given priority is not valid\n");
            IDAL.DO.Parcel parcelDo = new IDAL.DO.Parcel();
            parcelDo.senderId = parcelToAdd.sender.id;
            parcelDo.targetId = parcelToAdd.receive.id;
            parcelDo.weight = (WeightCatigories)parcelToAdd.weightCategorie;
            parcelDo.priority = (Proirities)parcelToAdd.priority;
            parcelDo.requested = DateTime.Now;
            parcelDo.scheduled = DateTime.MinValue;
            parcelDo.pickedUp = DateTime.MinValue;
            parcelDo.delivered = DateTime.MinValue;
            parcelDo.droneId = 0;
            try
            {
                return dal.addParcel(parcelDo);
            }
            catch (AddException exp)
            {
                throw new AlreadyExistException(exp.Message);
            }
        }
        #endregion
        #endregion
        #region GET
        #region Get station
        public BaseStationToList GetBaseStationToList(int id)
        {
            BaseStationToList baseStation = new();
            try
            {
                var station = dal.GetStation(id);
                baseStation.id = station.id;
                baseStation.stationName = station.name;
                baseStation.avilableChargeSlots = station.chargeSlots;
                baseStation.unavilableChargeSlots = unavailableChargeSlots;
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            return baseStation;
        }
        public BaseStation GetStation(int id)
        {
            try
            {
                BaseStation baseStation = new BaseStation();
                var station = dal.GetStation(id);
                baseStation.id = station.id;
                baseStation.stationName = station.name;
                baseStation.avilableChargeSlots = station.chargeSlots;
                baseStation.location = new Location()
                {
                    latitude = station.latitude,
                    longitude = station.longitude
                };
                baseStation.DroneInChargeList = dal.GetDroneIdInStation(id)
                    .Select(drone => new DroneInCharge()
                    {
                        id = drone.droneId,
                        batteryStatus = getDroneBattery(drone.droneId)
                    });
                return baseStation;
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
        }
        #endregion
        #region Get Used ChargingSlots
        private int getUsedChargingPorts(int baseStationId)
        {
            try
            {
                return dal.GetStation(baseStationId).chargeSlots - dal.AvailableChargingSlots();  //they did it AvailableChargingSlots (id)
            }
            catch (findException exp)
            {
                throw BLFindException("station");
            }

        }
        private Exception BLFindException(string v)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Get Drone Battery

        private double getDroneBattery(int droneId)
        {
            return drones.Find(drone => drone.id == droneId).batteryStatus;
        }
        #endregion
        private IBL.BO.CustomerInParcel getCustomerInParcel(int id)
        {
            IBL.BO.CustomerInParcel c = new IBL.BO.CustomerInParcel();
            c.id = id;
            IDAL.DO.Customer cs = dal.GetCustomer(id);
            c.name = cs.name;
            return c;
        }
        public IBL.BO.Drone returnsDrone(int id)
        {

            var drn = drones.Find(x => x.id == id);
            if (drn == null)
                throw new dosntExisetException("Error! the drone doesn't found");
            IBL.BO.Drone d = new IBL.BO.Drone();
            d.id = drn.id;
            d.droneModel = drn.droneModel;
            d.weight = drn.weight;
            d.droneStatus = drn.droneStatus;
            d.batteryStatus = drn.batteryStatus;
            d.location = new IBL.BO.Location();
            d.location = drn.location;
            IBL.BO.ParcelInTransfer pt = new IBL.BO.ParcelInTransfer();
            if (drn.droneStatus == IBL.BO.DroneStatus.delivery)
            {
                pt.id = drn.parcelId;
                IDAL.DO.Parcel p = new IDAL.DO.Parcel();
                try
                {
                    p = dal.GetParcel(drn.parcelId);//get the parcel from the dal
                }
                catch (Exception)
                {
                    throw new dosntExisetException("Error! the parcel not found");
                }
                if (p.pickedUp == DateTime.MinValue)
                    pt.parcelStatus = false;
                else
                    pt.parcelStatus = true;
                pt.priority = (IBL.BO.Priority)p.priority;
                pt.weight = (IBL.BO.Weight)p.weight;
                pt.sender = new IBL.BO.CustomerInParcel();
                pt.sender = getCustomerInParcel(p.senderId);
                pt.receive = new IBL.BO.CustomerInParcel();
                pt.receive = getCustomerInParcel(p.targetId);
                IDAL.DO.Customer sender = dal.GetCustomer(p.senderId);
                IDAL.DO.Customer target = dal.GetCustomer(p.targetId);
                pt.collection = new IBL.BO.Location();
                pt.collection.longitude = sender.longitude;
                pt.collection.latitude = sender.latitude;
                pt.DeliveryDestination = new IBL.BO.Location();
                pt.DeliveryDestination.longitude = target.longitude;
                pt.DeliveryDestination.latitude = target.latitude;
                pt.TransportDistance = Distance(pt.collection, pt.DeliveryDestination);
                d.parcelInTransfer = new IBL.BO.ParcelInTransfer();
                d.parcelInTransfer = pt;
            }
            return d;
        }
        #region Get Drone
        public DroneToList GetDrone(int id)
        {
            try
            {
                DroneToList droneBo = new DroneToList();
                IDAL.DO.Drone droneDo = dal.GetDrone(id);
                DroneToList drone = drones.ToList().Find(d => d.id == id);
                droneBo.id = droneDo.id;
                droneBo.droneModel = drone.droneModel;
                droneBo.weight = drone.weight;
                droneBo.location = drone.location;
                droneBo.batteryStatus = drone.batteryStatus;
                droneBo.droneStatus = drone.droneStatus;
                droneBo.numOfDeliverdParcels = drone.numOfDeliverdParcels;
                droneBo.numOfDeliverdParcels = dal.parcelList().Count(x => x.droneId == droneBo.id);
                int parcelID = dal.parcelList().ToList().Find(x => x.droneId == droneBo.id).id;
                droneBo.parcelId = parcelID;
                return droneBo;
            }
            catch (ArgumentNullException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
        }
        public IBL.BO.ParcelToList GetParcelToList(int id)
        {
            IBL.BO.ParcelToList parcel = new();
            IDAL.DO.Parcel dalParcel = new();
            try
            { 
                dalParcel = dal.GetParcel(id);
                var droneIP = GetDrone(dalParcel.droneId);
                var droneInParcel = new DroneInParcel { id = dalParcel.droneId, battery = droneIP.batteryStatus, location = droneIP.location };
                parcel.id = dalParcel.id;
                parcel.priority = (IBL.BO.Priority)dalParcel.priority;
                parcel.receiveName =dal.GetCustomer(dalParcel.targetId).name;
                parcel.weight = (Weight)dalParcel.weight;
                parcel.senderName =dal.GetCustomer(dalParcel.senderId).name;
                if (dalParcel.pickedUp != DateTime.MinValue)
                    parcel.parcelStatus = IBL.BO.ParcelStatus.PickedUp;
                if(dalParcel.requested !=DateTime.MinValue)
                    parcel.parcelStatus = IBL.BO.ParcelStatus.Assigned;
                if (dalParcel.scheduled != DateTime.MinValue)
                    parcel.parcelStatus = IBL.BO.ParcelStatus.Created;
                if (dalParcel.delivered != DateTime.MinValue)
                    parcel.parcelStatus = IBL.BO.ParcelStatus.Delivered;
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            return parcel;
        }
        public IBL.BO.Parcel GetParcel(int id)
        {
            try
            {
                IBL.BO.Parcel parcel = new IBL.BO.Parcel();
                IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel();
                dalParcel = dal.GetParcel(id);
                var droneIP = GetDrone(dalParcel.droneId);
                var droneInParcel = new DroneInParcel { id = dalParcel.droneId, battery = droneIP.batteryStatus, location = droneIP.location };
                parcel.id = dalParcel.id;
                parcel.priority = (IBL.BO.Priority)dalParcel.priority;
                parcel.receive = new CustomerInParcel { id = dal.GetCustomer(dalParcel.targetId).id, name = dal.GetCustomer(dalParcel.targetId).name };
                parcel.weightCategorie = (Weight)dalParcel.weight;
                parcel.droneInParcel = droneInParcel;
                parcel.requested = dalParcel.requested;
                parcel.scheduled = dalParcel.scheduled;
                parcel.pickedUp = dalParcel.pickedUp;
                parcel.delivered = dalParcel.delivered;
                parcel.sender = new CustomerInParcel { id = dal.GetCustomer(dalParcel.senderId).id, name = dal.GetCustomer(dalParcel.senderId).name };
                return parcel;
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
        }
        #endregion
     
        public List<IBL.BO.BaseStationToList> GetBaseStationToLists()
        {
            List<IBL.BO.BaseStationToList> baseStations = new();
            try
            {
                var stationsDal = dal.stationList().ToList();
                foreach (var s in stationsDal)
                { baseStations.Add(GetBaseStationToList(s.id)); }
            }
            catch (ArgumentException) { throw new dosntExisetException(); }
            return baseStations;
        }
        public List<IBL.BO.BaseStation> GetStations()
        {
            List<IBL.BO.BaseStation> baseStations = new();
            try
            {
                var stationsDal = dal.stationList().ToList();
                foreach (var s in stationsDal)
                { baseStations.Add(GetStation(s.id)); }
            }
            catch (ArgumentException) { throw new dosntExisetException(); }
            return baseStations;
        }
        public List<IBL.BO.DroneToList> GetDrones()
        {
            List<IBL.BO.DroneToList> drones = new List<IBL.BO.DroneToList>();
            foreach (var d in dal.droneList())
            { drones.Add(GetDrone(d.id)); }
            return drones;
        }
        public List<IBL.BO.Customer> GetCustomers()
        {
            List<IBL.BO.Customer> customers = new List<IBL.BO.Customer>();
            foreach (var c in dal.CustomerList())
            { customers.Add(GetCustomer(c.id)); }
            return customers;
        }
        public List<IBL.BO.CustomerInList> GetCustomersToList()
        {
            List<IBL.BO.CustomerInList> customers = new();
            foreach (var c in dal.CustomerList())
            { customers.Add(GetCustomerToList(c.id)); }
            return customers;
        }
        public List<IBL.BO.Parcel> GetParcels()
        {
            List<IBL.BO.Parcel> parcels = new List<IBL.BO.Parcel>();
            foreach (var p in dal.parcelList())
            { parcels.Add(GetParcel(p.id)); }
            return parcels;
        }

        public List<IBL.BO.ParcelToList> GetParcelToLists()
        {
            List<ParcelToList> lst = new List<ParcelToList>();//create the list
            foreach (var item in dal.parcelList())//pass on the list of the parcels and copy them to the new list
            {
                lst.Add(GetParcelToList(item.id));
            }
            return lst;
        }
        #region Get Customer


        public IBL.BO.CustomerInList GetCustomerToList(int id)
        {
            try
            {
                List<IBL.BO.Parcel> tempParcels = GetParcels();
                IBL.BO.CustomerInList CustomerBo = new();
                IDAL.DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.Name = CustomerDo.name;
                CustomerBo.PhoneNumber = CustomerDo.phoneNumber;
                CustomerBo.Parcles_Delivered_Recieved = GetParcels().Count(x => x.delivered != DateTime.MinValue&& x.pickedUp == DateTime.MinValue);
                CustomerBo.Parcels_unrecieved =GetParcels().Count(x => x.pickedUp ==DateTime.MinValue);   //נראלי שזה שלא קבלו אומר שהם לא אספו
                CustomerBo.Recieved_Parcels = CustomerReceiveParcel(id).Count;////לא בטוחה צריך לבדוק את זה
                CustomerBo.ParcelsInDeliver = GetParcels().Count(x => x.delivered !=DateTime.MinValue);
                    return CustomerBo;
            }
            catch (IDAL.DO.findException Fex)
            {
                throw new validException($"Customer id {id}", Fex);
            }
        }
        public IBL.BO.Customer GetCustomer(int id)
        {
            try
            {
                List<IBL.BO.Parcel> tempParcels = GetParcels();
                IBL.BO.Customer CustomerBo = new IBL.BO.Customer();
                IDAL.DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.Name = CustomerDo.name;
                CustomerBo.phoneNumber = CustomerDo.phoneNumber;
                CustomerBo.location = new Location() { latitude = CustomerDo.latitude, longitude = CustomerDo.longitude };
                CustomerBo.SentParcels = getCustomerShippedParcels(id).ToList();
                CustomerBo.ReceiveParcel = CustomerReceiveParcel(id).ToList();
                return CustomerBo;
            }
            catch (IDAL.DO.findException Fex)
            {
                throw new validException(Fex.Message);
            }
        }
        public List<ParcelCustomer> CustomerReceiveParcel(int customerID)
        {
            List<IBL.BO.ParcelCustomer> recievedParcels = new List<ParcelCustomer>();
            foreach (var parcel in GetParcels())
            {
                if (parcel.receive.id == customerID)
                {
                    IBL.BO.ParcelCustomer customersPrevioseParcel = new ParcelCustomer { CustomerInParcel = parcel.receive, id = parcel.id, parcelStatus = ParcelStatus.Delivered, priority = parcel.priority, weight = parcel.weightCategorie };
                    recievedParcels.Add(customersPrevioseParcel);
                }
            }
            return recievedParcels;

        }

        #region Get Customer Shipped Parcels
        private IEnumerable<ParcelCustomer> getCustomerShippedParcels(int id)
        {
            var tempParcel = dal.getCustomerShippedParcels(id)
                .Select(parcel =>
                    new ParcelCustomer
                    {
                        id = parcel.id,
                        weight = (Weight)parcel.weight,
                        priority = (Priority)parcel.priority,
                        parcelStatus = (ParcelStatus)2,
                        CustomerInParcel = new CustomerInParcel()    //לא יודעת בדיוק מה להציב במשתנים האלו
                        {
                            id = parcel.id,   //זה
                            name = dal.GetCustomerName(id),  //זה
                        }
                    });
            return tempParcel;
        }
        #endregion
        #region Get base station location
        private Location getBaseStationLocation(int stationId)
        {
            IDAL.DO.Station station = dal.GetStation(stationId);
            return new Location { latitude = station.latitude, longitude = station.longitude };
        }
        #endregion
        #region GetParcels

        #endregion
        #endregion
        #region find&delete

        #endregion
        #region HELP FUNCTIONS
        #region find Drone Location
        private Location findDroneLocation(DroneToList drone)
        {
            if (drone.droneStatus == DroneStatus.delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.parcelId);
                if (parcel.pickedUp == null)
                {
                    IBL.BO.Customer customer = GetCustomer(parcel.senderId);
                    return findClosestStationLocation(customer.location, false, BaseStationLocationslist());
                }
                if (parcel.delivered == null)
                {
                    return GetCustomer(parcel.senderId).location;
                }
            }
            if (drone.droneStatus == DroneStatus.available)
            {
                var targetsIds = dal.GetParcels()
                    .Where(parcel => parcel.droneId == drone.id)
                    .Select(parcel => parcel.targetId).ToList();

                if (targetsIds.Count == 0)
                {
                    int stationId = rand.Next(1, dal.getStations().Count());
                    dal.SendToCharge(drone.id, stationId);
                    return getBaseStationLocation(stationId);
                }
                //TODO: get last Customer location
                return GetCustomer(targetsIds[rand.Next(targetsIds.Count)]).location;
            }
            return new Location();
        }
        #endregion
        #region find Closet Base Station Location
        //private double Distance(Location x, Location y)
        //{
        //    var distance = Math.Sqrt(Math.Pow(x.longitude - y.longitude, 2) + Math.Pow(x.latitude - y.latitude, 2));
        //    return distance;
        //}
        private static double deg2rad(double val)
        {
            return (Math.PI / 180) * val;
        }
        private double Distance(IBL.BO.Location l1, IBL.BO.Location l2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(l2.latitude - l1.latitude);  // deg2rad below
            var dLon = deg2rad(l2.longitude - l1.longitude);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(l1.latitude)) * Math.Cos(deg2rad(l2.latitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        private List<Location> BaseStationLocationslist()
        {
            List<Location> locations = new List<Location>();
            foreach (var baseStation in GetStations())
            {
                locations.Add(new Location
                {
                    latitude = baseStation.location.latitude,
                    longitude = baseStation.location.longitude
                });
            }
            return locations;
        }
        private Location findClosestStationLocation(Location currentlocation, bool withChargeSlots, List<Location> l)//the function could also be used to check in addtion if the charge slots are more then 0
        {

            var locations = l;
            Location location = locations[0];
            double distance = Distance(locations[0], currentlocation);
            for (int i = 1; i < locations.Count; i++)
            {
                if (withChargeSlots)
                {
                    var station = GetStations().ToList().Find(x => x.location.latitude == locations[i].latitude && x.location.longitude == locations[i].longitude);
                    if (Distance(locations[i], currentlocation) < distance && station.avilableChargeSlots > 0)
                    {
                        distance = Distance(locations[i], currentlocation);
                    }
                    else
                    {
                        if (locations.Count() == 0)
                            throw new Exception("there are no stations with available charge slots");
                        locations.RemoveAt(i);
                        findClosestStationLocation(currentlocation, withChargeSlots, locations);
                    }

                }
                else
                {
                    if (Distance(locations[i], currentlocation) < distance)
                    {
                        location = locations[i];
                        distance = Distance(locations[i], currentlocation);
                    }

                }

            }
            return location;
        }
        #endregion
        #region DELETE DRONE
        public void deleteDrone(int droneID)
        {
        //    foreach (var p in dal.parcelList())
        //    {
        //        if (p.droneId == droneID)
        //            throw new deleteException("there is/are parcel that is matched to this drone\n");
        //    }
            try
            {
                if (GetDrone(droneID).droneStatus == DroneStatus.charge)
                { var DC = dal.chargingDroneList().ToList().Find(x => x.droneId == droneID); dal.RemoveDroneCharge(DC); }
                dal.deleteDrone(dal.GetDrone(droneID));

            }
            catch (findException exp) { throw new deleteException("cant delete this drone\n"); }
        }
        #endregion
        #region Is Drone While Shipping
        private bool isDroneWhileShipping(DroneToList drone)
        {
            var parcels = dal.GetParcels();
            for (int i = 0; i < parcels.Count(); i++)
            {
                if (parcels.ElementAt(i).droneId == drone.id)
                {
                    if (parcels.ElementAt(i).delivered == null)
                    {
                        drone.parcelId = parcels.ElementAt(i).id;
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        #endregion


        #region UPDATE
        public void updateDroneName(int droneID, string dModel)
        {
            int dIndex = drones.FindIndex(x => x.id == droneID);
            if (dIndex == 0)//לדעת מה הוא מחזיר אם הוא לא מוצא ולשים בתנאי
            {
                throw new dosntExisetException("drone do not exist");
            }

            dal.updateDrone(droneID, dModel);
            IBL.BO.DroneToList dr = drones.Find(p => p.id == droneID);
            drones.Remove(dr);
            dr.droneModel = dModel;
            drones.Add(dr);
        }
        public void updateStation(int stationID, int AvlblDCharges, string Name = " ")
        {
            try
            {
                IDAL.DO.Station stationDl = new IDAL.DO.Station();
                stationDl = dal.GetStation(stationID);
                if (Name != " ")
                    stationDl.name = Name;
                if (AvlblDCharges != 0)
                {
                    if (AvlblDCharges < 0)
                        throw new validException("this amount of drone choging slots is not valid!\n");
                    stationDl.chargeSlots = AvlblDCharges;      //i need al the slots not just the available one - not sure what this variable means
                    dal.updateStation(stationID, stationDl);
                }
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
        }
        public void updateCustomer(int customerID, string Name = " ", int phoneNum = 0)
        {
            try
            {
                IDAL.DO.Customer customerDl = new IDAL.DO.Customer();
                customerDl = dal.GetCustomer(customerID);
                customerDl.name = Name;
                customerDl.phoneNumber = phoneNum;
                dal.updateCustomer(customerID, customerDl);
            }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
        }
        public void SendToCharge(int droneID) //int StationID)//have to send the closest sation that has available sattions
        {
            
                IBL.BO.DroneToList drone = new();
                IBL.BO.BaseStation station = new();
                try
                {
                    drone = GetDrone(droneID);
                }
                catch (IDAL.DO.findException exp)
                {
                    throw new dosntExisetException(exp.Message);
                }
                if (drone.droneStatus != DroneStatus.available)
                    throw new unavailableException("not available");
                Location stationLocation = findClosestStationLocation(drone.location, false, BaseStationLocationslist());//not sure where and what its from
                station = GetStations().Find(x => x.location.longitude == stationLocation.longitude && x.location.latitude == stationLocation.latitude);
                int droneIndex = drones.ToList().FindIndex(x => x.id == droneID);
                //var droneBL=GetDrones().ToList().Find(x => x.id == droneID);
                if (station.avilableChargeSlots > 0)
                    station.decreasingChargeSlots();
                drones[droneIndex].batteryStatus = calcMinBatteryRequired(drones[droneIndex]);//not sure that if it needs to be 100%
                drones[droneIndex].location = station.location;
                drones[droneIndex].droneStatus = DroneStatus.charge;

                try { deleteDrone(droneID); }
                catch (deleteException exp) { throw new deleteException(exp.Message); }
                catch (findException exp)
                {
                    throw new dosntExisetException(exp.Message);
                }
                addDrone(drones[droneIndex], station.id);
            IDAL.DO.droneCharges DC = new droneCharges { droneId = droneID, stationId = station.id };
            dal.AddDroneCharge(DC);
        }
        
        public void releasingDrone(int droneID, TimeSpan chargingTime)
        {
                DroneToList droneItem = new();
                try { droneItem = GetDrones().Find(x => x.id == droneID); }
                catch (IDAL.DO.findException) { throw new findException(); }
                if (droneItem.droneStatus != DroneStatus.charge)
                    throw new CannotReleaseFromChargeException();
                else
                {
                    int index = drones.FindIndex(x => x.id == droneID);
                    IDAL.DO.droneCharges DC = new();
                    try { DC = dal.chargingDroneList().ToList().Find(X => X.droneId == droneID); }
                    catch (IDAL.DO.findException) { throw new findException(); }
                    IBL.BO.BaseStation bstation = new();
                    try { bstation = GetStation(DC.stationId); }
                    catch (IDAL.DO.findException) { throw new findException(); }

                    double timeInMinutes = chargingTime.TotalMinutes;//converting the format to number of minutes, for instance, 1:30 to 90 minutes
                    timeInMinutes /= 60; //getting the time in hours 
                    drones[index].batteryStatus = timeInMinutes * GetChargeCapacity().pwrRateLoadingDrone + droneItem.batteryStatus; // the battery calculation
                    if (droneItem.batteryStatus > 100) //battery can't has more than a 100 percent
                        droneItem.batteryStatus = 100;

                    dal.deleteStation(dal.GetStation(DC.stationId));
                    bstation.addingChargeSlots();
                    addStation(bstation);
                    dal.RemoveDroneCharge(DC);
                    dal.deleteDrone(dal.GetDrone(droneID));
                    drones[index].droneStatus = DroneStatus.available;
                    Console.WriteLine(drones[index].ToString());
                    addDrone(drones[index], DC.stationId);
                }
            
        }
        public void deliveryParcelToCustomer(int id)
        {
            var drone = new IBL.BO.DroneToList();
            try
            {
                drone = GetDrone(id);
                IDAL.DO.Parcel parcel = dal.parcelList().ToList().Find(p => p.droneId == id);
                var customer = GetCustomer(parcel.targetId);
                var stationLoc = findClosestStationLocation(customer.location, false, BaseStationLocationslist());
                IDAL.DO.Station station = dal.stationList().ToList().Find(s => s.longitude == stationLoc.longitude && s.latitude == stationLoc.latitude);
                double previoseBatteryStatus = drone.batteryStatus;
                if (!(drone.droneStatus == DroneStatus.delivery))   //the drone pickedup but didnt delivert yet
                    throw new ExecutionTheDroneIsntAvilablle(" this drone is not in delivery");
                var Location = new Location { longitude = station.longitude, latitude = station.latitude };
                drone.batteryStatus =previoseBatteryStatus - (Distance(drone.location,Location) * GetChargeCapacity().chargeCapacityArr[(int)drone.weight]);
                drone.location.latitude = station.latitude;
                drone.location.longitude = station.longitude;
                drone.droneStatus = DroneStatus.available;
                dal.deleteDrone(dal.GetDrone(id));
                addDrone(drone, station.id);
                parcel.requested = DateTime.Now;   //מה זה זמן אספקה
                var parcelBL = GetParcel(parcel.id);
                dal.deleteParcel(dal.GetParcel(parcel.id));
                addParcel(parcelBL);
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            catch (ExecutionTheDroneIsntAvilablle exp) { throw new ExecutionTheDroneIsntAvilablle(exp.Message); }

        }

        private IDAL.DO.Parcel findTheParcel(IBL.BO.Weight we, IBL.BO.Location a, double buttery, IDAL.DO.Proirities pri)
        {


            double d, x;
            IDAL.DO.Parcel theParcel = new IDAL.DO.Parcel();

            IBL.BO.Location loc = new IBL.BO.Location();
            IDAL.DO.Customer customer = new IDAL.DO.Customer();
            double far = 1000000;
            // bool flug = false;

            //השאילתא אחראית למצוא את כל החבילות בעדיפות המבוקשת
            var parcels = dal.GetParcels();
            var tempParcel = from item in parcels
                             where item.priority == pri
                             select item;

            foreach (var item in tempParcel)
            {
                customer = dal.GetCustomer(item.senderId);
                loc.latitude = customer.latitude;
                loc.longitude = customer.longitude;
                chargeCapacity chargeCapacity = GetChargeCapacity();
                d = Distance(a, loc);//המרחק בין מיקום נוכחי למיקום השולח
                x = Distance(loc, new IBL.BO.Location { longitude = dal.GetCustomer(item.targetId).longitude, latitude = dal.GetCustomer(item.targetId).latitude });//המרחק בין מיקום שולח למיקום יעד
                double fromCusToSta = Distance(new IBL.BO.Location { longitude = dal.GetCustomer(item.targetId).longitude, latitude = dal.GetCustomer(item.targetId).latitude }, findClosestStationLocation(new IBL.BO.Location { longitude = dal.GetCustomer(item.targetId).longitude, latitude = dal.GetCustomer(item.targetId).latitude }, false, BaseStationLocationslist()));
                double butteryUse = x * chargeCapacity.chargeCapacityArr[(int)item.weight] + fromCusToSta * chargeCapacity.chargeCapacityArr[0] + d * chargeCapacity.chargeCapacityArr[0];
                if (d < far && (buttery - butteryUse) > 0 && item.scheduled == DateTime.MinValue && weight(we, (IBL.BO.Weight)item.weight) == true)
                {
                    far = d;
                    theParcel = item;
                    return theParcel;
                }
            }
            //if (v.Count() > 0)//if there is a parcel.priority. ....
            //flug = true;

            if (pri == IDAL.DO.Proirities.emergency)//אם לא מצא בעדיפות הכי גבוהה מחפש בעדיפות מתחתיה
                theParcel = findTheParcel(we, a, buttery, IDAL.DO.Proirities.fast);

            if (pri == IDAL.DO.Proirities.fast)
                theParcel = findTheParcel(we, a, buttery, IDAL.DO.Proirities.regular);
            if (theParcel.id == 0)
                throw new dosntExisetException("ERROR! there is not a parcel that match to the drone ");
            return theParcel;
        }
        private bool weight(IBL.BO.Weight dr, IBL.BO.Weight pa)
        {
            if (dr == IBL.BO.Weight.heavy)
                return true;
            if (dr == IBL.BO.Weight.avergae && (pa == IBL.BO.Weight.avergae || pa == IBL.BO.Weight.light))
                return true;
            if (dr == IBL.BO.Weight.light && pa == IBL.BO.Weight.light)
                return true;
            return false;
        }
        private int indexOfChargeCapacity(IDAL.DO.WeightCatigories w)
        {
            if (w == IDAL.DO.WeightCatigories.light)
                return 1;
            if (w == IDAL.DO.WeightCatigories.heavy)
                return 2;
            if (w == IDAL.DO.WeightCatigories.avergae)
                return 3;

            return 0;

        }
        private BaseStation GetStationByDrone(DroneToList d)
        {
            var stationLoc = findClosestStationLocation(d.location, false, BaseStationLocationslist());
            var station = GetStations().ToList().Find(x => x.location.longitude == stationLoc.longitude && x.location.latitude == stationLoc.latitude);
                return station;
        }
        #endregion
        public void matchingDroneToParcel(int droneId)
        {

            try
            {
                var myDrone = GetDrone(droneId);
                var droneLoc = findClosestStationLocation(myDrone.location, false, BaseStationLocationslist());
                var station = GetStations().ToList().Find(x => x.location.longitude == droneLoc.longitude && x.location.latitude == droneLoc.latitude);
                if (myDrone.droneStatus != DroneStatus.available)
                    throw new unavailableException("the drone is unavailable\n");
                IDAL.DO.Parcel myParcel = findTheParcel(myDrone.weight, myDrone.location, myDrone.batteryStatus, IDAL.DO.Proirities.emergency);
                dal.attribute(myDrone.id, myParcel.id);
                int index = drones.FindIndex(x => x.id == droneId);
                deleteDrone(myDrone.id);
                drones.RemoveAt(index);
                myDrone.droneStatus = DroneStatus.delivery;
                myDrone.parcelId = myParcel.id;
                addDrone(myDrone, station.id);
            }
            catch(findException exp) { throw new dosntExisetException(exp.Message); }
           
        }
        public void pickedUpParcelByDrone(int droneID)
        {
            var d = GetDrones().Find(x => x.id == droneID);
            if (d == null)
                throw new dosntExisetException("the drone dosnt exist");
            foreach (var item in dal.GetParcels())
            {
                if (item.droneId == droneID)
                {
                    if (item.pickedUp == DateTime.MinValue && item.delivered == DateTime.MinValue)
                        {
                            int index = drones.FindIndex(x => x.id == d.id);
                            deleteDrone(droneID);
                            drones.RemoveAt(index);
                            d.batteryStatus = d.batteryStatus - Distance(d.location, new IBL.BO.Location { latitude = dal.GetCustomer(item.targetId).latitude, longitude = dal.GetCustomer(item.targetId).longitude }) * GetChargeCapacity().chargeCapacityArr[(int)(item.weight)];
                            d.location.longitude = dal.GetCustomer(item.senderId).longitude;
                            d.location.latitude = dal.GetCustomer(item.senderId).latitude;
                            var par = item;
                            dal.deleteParcel(item);
                            par.pickedUp = DateTime.Now;
                            int parcelNewID=dal.addParcel(par);
                            d.parcelId = parcelNewID;
                            var station = GetStationByDrone(d);
                            addDrone(d, station.id);
                            return;
                        }
                }
            }
            throw new validException("the drone cant picked up the parcel because the parcel is not matching to him");
        }
     
        private int calcMinBatteryRequired(DroneToList drone)
        {
            if (drone.droneStatus == DroneStatus.available)
            {
                Location location = findClosestStationLocation(drone.location, false, BaseStationLocationslist());
                return (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * Distance(drone.location, location));
            }

            if (drone.droneStatus == DroneStatus.delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.parcelId);
                if (parcel.pickedUp == null)
                {
                    int minValue;
                    IBL.BO.Customer sender = GetCustomer(parcel.senderId);
                    var target = GetCustomer(parcel.targetId);
                    double droneToSender = Distance(drone.location, sender.location);
                    minValue = (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * droneToSender);
                    double senderToTarget = Distance(sender.location, target.location);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)parcel.weight] * senderToTarget);
                    Location baseStationLocation = findClosestStationLocation(target.location, false, BaseStationLocationslist());
                    double targetToCharge = Distance(target.location, baseStationLocation);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * targetToCharge);
                    return minValue;
                }

                if (parcel.delivered == null)
                {
                    int minValue;
                    var sender = GetCustomer(parcel.senderId);
                    var target = GetCustomer(parcel.targetId);
                    double senderToTarget = Distance(sender.location, target.location);
                    int batteryUsage = (int)parcel.weight;
                    // (chargeCapacity)Enum.Parse(typeof(chargeCapacity), parcel.weight.ToString());
                    minValue = (int)(GetChargeCapacity().chargeCapacityArr[batteryUsage] * senderToTarget);
                    Location baseStationLocation = findClosestStationLocation(target.location, false, BaseStationLocationslist());
                    double targetToCharge = Distance(target.location, baseStationLocation);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * targetToCharge);
                    return minValue;
                }
            }
            return 90;
        }
       
     
    }
}
#endregion