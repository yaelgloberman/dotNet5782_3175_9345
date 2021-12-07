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
    public  class  BL :IBl
    {
        private IDal dal; //= new DalObject.DalObject();
        private static Random rand = new Random();
        private List<DroneToList> drones = new();
        private static double getRandomCordinatesBL(double num1, double num2)
        {
            return (rand.NextDouble() * (num2 - num1) + num1);
        }
        public BL()
        {
            dal = new DalObject.DalObject();
            drones = new List<DroneToList>();
            initializeDrones();
        }

        public chargeCapacity GetChargeCapacity()
        {
            double[] arr=dal.ChargeCapacity();
            chargeCapacity chargingUsage  =new chargeCapacity { pwrAvailable=arr[0],pwrLight=arr[1],pwrAverge=arr[2],pwrHeavy=arr[3],pwrRateLoadingDrone=arr[4],chargeCapacityArr=arr};
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
                    // note : drone.DeliveryId getting value inside isDroneWhileShipping
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
                        drone.deliveryId = 0;
                    }
                    if (drone.droneStatus == DroneStatus.available)
                    {
                        drone.location = findDroneLocation(drone);
                        drone.deliveryId = 0;
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
            if (!(stationToAdd.avilableChargeSlots>0)) 
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
            if (!(droneId >= 10000000 && droneId < 1000000000))
                throw new validException("the number of the drone id in invalid\n");
            if (!(weight > (Weight)0 && weight < (Weight)3))
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
            dtl.droneStatus = DroneStatus.charge;
            dtl.location = new Location();
            dtl.location.latitude= stationDl.latitude;
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
            dal.SendToCharge(droneId, stationId);
        }
        public void addDrone(DroneToList droneToAdd, int stationId)
        {

            IDAL.DO.Station stationDl = dal.GetStation(stationId);
            droneToAdd.location.latitude = stationDl.latitude;
            droneToAdd.location.longitude = stationDl.longitude;
            droneToAdd.batteryStatus = (double)rand.Next(20, 40);
            droneToAdd.droneStatus = DroneStatus.charge;
            if (!(droneToAdd.id >= 10000000 && droneToAdd.id <= 1000000000))
                throw new AddException("the number of the drone id in invalid\n");
            if (!(droneToAdd.batteryStatus >= (double)0 && droneToAdd.batteryStatus <= (double)100))
                throw new AddException("the status of the drone is invalid\n");
            if (droneToAdd.location.latitude < (double)31 || droneToAdd.location.latitude > 33.3)
                throw new AddException("the given latitude do not exist in this country/\n");
            if (droneToAdd.location.longitude < 34.3 || droneToAdd.location.longitude > 35.5)
                throw new AddException("the given longitude do not exist in this country/\n");
            if (!(droneToAdd.weight > (Weight)0 && droneToAdd.weight < (Weight)3))
                throw new AddException("the given weight is not valid\n");
            if (!(stationId >= 10000000 && stationId <= 1000000000))
                throw new AddException("the number of the station id in invalid\n");
            try
            {
                var tempDrone = GetDrone(droneToAdd.id);
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            try
            {
                var tempStation = GetStation(stationId);
            }
            catch (IDAL.DO.findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            droneToAdd.droneStatus = DroneStatus.charge;
            droneToAdd.location = getBaseStationLocation(stationId);
            IDAL.DO.Drone drone = new IDAL.DO.Drone();
            drone.id = droneToAdd.id;
            drone.model = droneToAdd.droneModel;
            drone.maxWeight = (WeightCatigories)droneToAdd.weight;
            drones.Add(droneToAdd);
            dal.addDrone(drone);
            dal.SendToCharge(droneToAdd.id, stationId);
        }
        #endregion
        #region Add Customer
        public void addCustomer(IBL.BO.Customer CustomerToAdd)
        {

            if (!(CustomerToAdd.id >= 10000000 && CustomerToAdd.id <= 1000000000))
                throw new AddException("the id number of the drone is invalid\n");
            if (!(CustomerToAdd.phoneNumber >= 500000000 && CustomerToAdd.phoneNumber <= 0589999999))
                throw new AddException("the phone number of the Customer is invalid\n");
            if (CustomerToAdd.location.latitude < (double)31 || CustomerToAdd.location.latitude > 33.3 || CustomerToAdd.location.longitude < 34.3 || CustomerToAdd.location.longitude > 35.5)
                throw new AddException("the given cordinates do not exist in this country/\n");
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
        public void addParcel(IBL.BO.Parcel parcelToAdd)
        {
            if (!(parcelToAdd.sender.id >= 10000000 && parcelToAdd.sender.id <= 1000000000))
                throw new AddException("the id sender number of the pardel is invalid\n");
            if (!(parcelToAdd.receive.id >= 10000000 && parcelToAdd.receive.id <= 1000000000))
                throw new AddException("the id receive number of the parcel is invalid\n");
            if (!(parcelToAdd.weightCategorie > (Weight)0 && parcelToAdd.weightCategorie < (Weight)3))
                throw new AddException("the given weight is not valid\n");
            if (!(parcelToAdd.priority > (Priority)0 && parcelToAdd.priority < (Priority)3))
                throw new AddException("the given priority is not valid\n");
            IDAL.DO.Parcel parcelDo = new IDAL.DO.Parcel();
            parcelDo.senderId = parcelToAdd.sender.id;
            parcelDo.targetId = parcelToAdd.receive.id;
            parcelDo.weight = (WeightCatigories)parcelToAdd.weightCategorie;
            parcelDo.priority = (Proirities)parcelToAdd.priority;
            parcelDo.requested = DateTime.Now;//נראלי שזה נחשב ליצירה
            parcelDo.scheduled = DateTime.MinValue;
            parcelDo.pickedUp = DateTime.MinValue;
            parcelDo.delivered = DateTime.MinValue;
            parcelDo.droneId = 0;   //אני לא יודעת אם זה נחשב NULL 
            try
            {
                dal.addParcel(parcelDo);
            }
            catch (Exception exp)
            {
                throw new AlreadyExistException("the parccel already exist");
            }
        }
        #endregion
        #endregion
        #region GET
        #region Get station
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
                baseStation.DroneInChargeList= dal.GetDroneIdInStation(id)
                    .Select(drone => new DroneInCharge()
                    {
                        id = drone.droneId,
                        batteryStatus = getDroneBattery(drone.droneId)
                    });
                return baseStation;
            }
            catch (findException exp)
            {
                throw new dosntExisetException("station");
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
        #region Get Drone
        public DroneToList GetDrone(int id)
        {
            try
            {
                DroneToList dl = drones.Find(x => x.id == id);
                if (dl.id != 0)
                    return dl;
                DroneToList droneBo = new DroneToList();
                IDAL.DO.Drone droneDo = dal.GetDrone(id);
                DroneToList drone = drones.Find(d => d.id == id);
                droneBo.id = droneDo.id;
                droneBo.droneModel = drone.droneModel;
                droneBo.weight = drone.weight;
                droneBo.location = drone.location;
                droneBo.batteryStatus = drone.batteryStatus;
                droneBo.droneStatus = drone.droneStatus;
                droneBo.numOfDeliverdParcels = drone.numOfDeliverdParcels;
                return droneBo;
            }
            catch (findException ex)
            {
                throw new dosntExisetException("drone");
            }
        }
        public IBL.BO.Parcel GetParcel(int id)
        {
            try
            {
                IBL.BO.Parcel parcel = new IBL.BO.Parcel();
                IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel();
                dalParcel = dal.GetParcel(id);
                parcel.id = dalParcel.id;
                parcel.priority = (IBL.BO.Priority)dalParcel.priority;
                parcel.receive = new CustomerInParcel { id = dalParcel.id, name = dal.GetCustomerName(dalParcel.targetId) };
                parcel.requested = dalParcel.requested;
                parcel.scheduled = dalParcel.scheduled;
                parcel.sender = new CustomerInParcel { id = dalParcel.id, name = dal.GetCustomerName(dalParcel.senderId) };
                return parcel;
            }
            catch (findException exp)
            {
                throw new dosntExisetException($"Parcel id: {id}", exp);
            }
        }
        #endregion
        public List<IBL.BO.BaseStation> GetStations()
        {
            List<IBL.BO.BaseStation> baseStations = new List<IBL.BO.BaseStation>();
            foreach (var s in dal.stationList())
            { baseStations.Add(GetStation(s.id)); }
            return baseStations;
        }
        public List<IBL.BO.DroneToList> GetDrones()//all the drones to list i hope thats ok
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
        public List<IBL.BO.Parcel> GetParcels()
        {
            List<IBL.BO.Parcel> parcels = new List<IBL.BO.Parcel>();
            foreach (var p in dal.parcelList())
            { parcels.Add(GetParcel(p.id)); }
            return parcels;
        }
        #region Get Customer
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
                throw new dosntExisetException($"Customer id {id}", Fex);
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
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.deliveryId);
                if (parcel.pickedUp == null)
                {
                    IBL.BO.Customer customer = GetCustomer(parcel.senderId);
                    return findClosetBaseStationLocation(customer.location, false);
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
        private static double deg2rad( double val)
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
        private Location findClosetBaseStationLocation(Location currentlocation, bool withChargeSlots)//the function could also be used to check in addtion if the charge slots are more then 0
        {
            List<BaseStation> locations = new List<BaseStation>();
            foreach (var baseStation in dal.getStations())
            {
                locations.Add(new BaseStation
                {
                    location = new Location
                    {
                        latitude = baseStation.latitude,
                        longitude = baseStation.longitude
                    }
                });
            }
            Location location = locations[0].location;
            double distance = Distance(locations[0].location, currentlocation);
            for (int i = 1; i < locations.Count; i++)
            {
                if (withChargeSlots)
                {
                    if (Distance(locations[i].location, currentlocation) < distance && locations[i].avilableChargeSlots > 0)
                    {
                        location = locations[i].location;
                        distance = Distance(locations[i].location, currentlocation);
                    }
                }
                else
                {
                    if (Distance(locations[i].location, currentlocation) < distance)
                    {
                        location = locations[i].location;
                        distance = Distance(locations[i].location, currentlocation);
                    }
                }

            }
            return location;
        }
        #endregion
        #region DELETE DRONE
        public void deleteDrone(int droneID)
        {
            foreach (var p in dal.parcelList())
            {
                if (p.droneId == droneID)
                    throw new deleteException("there is/are parcel that is matched to this drone\n");
            }
            try
            {
                var drone = dal.GetDrone(droneID);
                dal.deleteDrone(drone);

            }
            catch (findException exp) { throw new deleteException("cant delete this drone\n"); }
        }
        #endregion
        #region Euclidean Distance
        private double euclideanDistance(Location from, Location to)
        {
            return Math.Sqrt(Math.Pow(to.longitude - from.longitude, 2) + Math.Pow(to.latitude - from.latitude, 2));
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
                        drone.deliveryId = parcels.ElementAt(i).id;
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
            if (dIndex==0)//לדעת מה הוא מחזיר אם הוא לא מוצא ולשים בתנאי
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
            catch(findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
        }
        public void updateCustomer(int customerID, string Name = " ", int phoneNum = 0)
        {
            IBL.BO.Customer c = GetCustomer(customerID);
            if (phoneNum != 0 || Name != " ")
            {
                dal.stationList().ToList().Remove(dal.GetStation(customerID));//not sure if this is how i remove the station im updating

                if (Name != " ")
                    c.Name = Name;
                if (phoneNum != 0)
                {
                    if (phoneNum < 0)
                        throw new validException("this amount of drone choging slots is not valid!\n");
                    c.phoneNumber = phoneNum;      //i need al the slots not just the available one - not sure what this variable means

                }
                addCustomer(c);

            }
        }
        public void SendToCharge(int droneID, int StationID)//have to send the closest sation that has available sattions
        {
            IBL.BO.DroneToList drone = new();
            IBL.BO.BaseStation station = new();
            try
            {
                drone= GetDrone(droneID);
            }
            catch (IDAL.DO.findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            if (drone.droneStatus != DroneStatus.available)
                throw new unavailableException("not available");
            try
            {
                station = GetStation(StationID);
            }
            catch (IDAL.DO.findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            Location stationLocation = findClosetBaseStationLocation(drone.location, true);//not sure where and what its from
            int droneIndex = drones.FindIndex(x => x.id == droneID);
            station.decreasingChargeSlots();
            drones[droneIndex].batteryStatus = calcMinBatteryRequired(drones[droneIndex]);
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.charge;
            try { dal.deleteDrone(dal.GetDrone(droneID)); }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
                addDrone(drones[droneIndex], StationID);
            IDAL.DO.droneCharges DC = new droneCharges { droneId = droneID, stationId = StationID };
            dal.chargingDroneList().ToList().Add(DC);
        }
        public void releasingDrone(int droneID, TimeSpan chargingTime)
        {
            DroneToList droneItem = new ();
            try { droneItem = GetDrones().Find(x => x.id == droneID); }
            catch (IDAL.DO.findException) { throw new findException(); }
            if (droneItem.droneStatus != DroneStatus.charge)
                throw new CannotReleaseFromChargeException();
            else
            { int index = drones.FindIndex(x => x.id == droneID);
                IDAL.DO.droneCharges DC = new();
                    try { DC = dal.chargingDroneList().ToList().Find(X => X.droneId == droneID); }
                catch (IDAL.DO.findException) { throw new findException(); }
                IBL.BO.BaseStation bstation = new();
                    try { bstation = GetStation(DC.stationId); }
                catch (IDAL.DO.findException) { throw new findException(); }
               
                double timeInMinutes = chargingTime.TotalMinutes;//converting the format to number of minutes, for instance, 1:30 to 90 minutes
                timeInMinutes /= 60; //getting the time in hours 
                drones[index].batteryStatus = timeInMinutes * GetChargeCapacity().pwrRateLoadingDrone; // the battery calculation
                if (droneItem.batteryStatus > 100) //battery can't has more than a 100 percent
                    droneItem.batteryStatus = 100;

                dal.deleteStation(dal.GetStation(DC.stationId));
                bstation.addingChargeSlots();
                addStation(bstation);
                dal.chargingDroneList().ToList().Remove(DC);
                deleteDrone(drones[index].id);
                drones[index].droneStatus = DroneStatus.available;
                addDrone(drones[index],DC.stationId);
            }
        }
        //public void ReleaseDroneCharge(int droneId, TimeSpan chargeTime)
        //{
        //    {
        //        double timeInMinutes = chargeTime.TotalMinutes;//converting the format to number of minutes, for instance, 1:30 to 90 minutes
        //        timeInMinutes /= 60; //getting the time in hours 
        //        droneItem.batteryStatus = timeInMinutes * chargeRate; // the battery calculation
        //        if (droneItem.batteryStatus > 100) //battery can't has more than a 100 percent
        //            droneItem.batteryStatus = 100;

        //        droneItem.droneStatus = DroneStatus.available;
        //        var droneChargeItem = dal.chargingDroneList().ToList().Find(x => x.droneId == droneId);
        //        var stationItem = dal.getStations().ToList().Find(x => x.id == droneChargeItem.stationId);
        //        stationItem.addingChargeSlot();
        //        dal.chargingDroneList().ToList().Remove(droneChargeItem);
        //    }
        //}
                    
                
       public void deliveryParcelToCustomer(int id)
        {
            try
            {
                IBL.BO.DroneToList drone = GetDrone(id);
                IDAL.DO.Parcel parcel = dal.parcelList().ToList().Find(p => p.droneId == id);
                IDAL.DO.Station station = dal.stationList().ToList().Find(s => s.id == parcel.targetId);
                if (!(drone.droneStatus == DroneStatus.delivery)) ;   //the drone pickedup but didnt delivert yet
                    throw new ExecutionTheDroneIsntAvilablle();
                drone.batteryStatus = calcMinBatteryRequired(drone);
                drone.location.latitude = station.latitude;
                drone.location.longitude = station.longitude;
                drone.droneStatus = DroneStatus.available;
                dal.deleteDrone(dal.GetDrone(id));
                addDrone(drone, station.id);
                IBL.BO.Parcel parcelTemp = new IBL.BO.Parcel();
                parcelTemp.requested = DateTime.Now;   //מה זה זמן אספקה
                dal.deleteParcel(dal.GetParcel(parcelTemp.id));
                addParcel(parcelTemp);
            }
            catch (findException f)
            {
                throw new ExecutionEngineException("rcfci");
            }

        }
        private IBL.BO.Parcel findTheParcel(IBL.BO.Location givenLocation, double battery, IBL.BO.Priority pri)
        {
            double distance;
            IBL.BO.Parcel theParcel=new IBL.BO.Parcel();
            IBL.BO.Location SenderLocation = new IBL.BO.Location();
            double mindistance = 1000000;
            bool flag = false;
            var parcels =GetParcels();
            var prioritizedParcels = from item in parcels
                    where item.priority==pri
                    select item;    
            foreach(var p in prioritizedParcels)
            {
                var sender = GetCustomer(p.sender.id);
                SenderLocation = sender.location;
                distance = Distance(givenLocation, SenderLocation);
                double fromCusToSta = Distance(SenderLocation, findClosetBaseStationLocation(SenderLocation, false));
                double batteryUse = distance * GetChargeCapacity().chargeCapacityArr[(int)(p.weightCategorie)] + fromCusToSta * GetChargeCapacity().pwrAvailable;
                if (distance < mindistance && (battery - batteryUse) > 0 && p.delivered == DateTime.MinValue)
                {
                    mindistance = distance;
                    theParcel = p;
                }
            }
            if (prioritizedParcels.Count() > 0)//if there is a parcel.priority. ....
                flag = true;

            if (!flag && pri == IBL.BO.Priority.emergency)//אם לא מצא בעדיפות הכי גבוהה מחפש בעדיפות מתחתיה
                theParcel = findTheParcel(givenLocation, battery, IBL.BO.Priority.fast);

            if (pri == IBL.BO.Priority.fast && !flag)
                theParcel = findTheParcel(givenLocation, battery, IBL.BO.Priority.regular);

            return theParcel;

        }


        #endregion
        public void matchingDroneToParcel(int droneID)//didnt finish this function at all 
        {
            IBL.BO.DroneToList drone = new();
            try { drone = GetDrone(droneID); } catch (IDAL.DO.findException) { throw new findException(); }

            //finding the best parcel
            if (drone.droneStatus != DroneStatus.available)
                throw new unavailableException("the drone is unavailable\n");
            bool flag = false;
            IBL.BO.Parcel ChosenParcel = findTheParcel(drone.location, drone.batteryStatus, IBL.BO.Priority.emergency);
            // the actual update
                dal.deleteDrone(dal.GetDrone(droneID));
                drone.droneStatus = DroneStatus.delivery;          
                addDrone(drone, dal.GetStation(ChosenParcel.receive.id).id);
                dal.deleteParcel(dal.GetParcel(ChosenParcel.id));
                IBL.BO.DroneInParcel droneInParcel = new DroneInParcel { id = droneID, battery = drone.batteryStatus, location = drone.location };
                ChosenParcel.droneInParcel = droneInParcel;
                ChosenParcel.scheduled = DateTime.Now;//notsure which one is זמן השיוך
                addParcel(ChosenParcel);
            
        }
        public void pickedUpParcelByDrone(int droneID)
        {
            var d = GetDrones().Find(x => x.id == droneID);
            if (d == null)
                throw new findException("Error! the drone not found");
            foreach (var item in dal.GetParcels())
            {
                if (item.droneId == droneID)
                {
                    if (item.pickedUp != DateTime.MinValue && item.delivered == DateTime.MinValue)
                    {
                        GetDrones().Remove(d);
                        d.batteryStatus = d.batteryStatus - Distance(d.location, new IBL.BO.Location { latitude = dal.GetCustomer(item.targetId).latitude, longitude =dal.GetCustomer(item.targetId).longitude }) * GetChargeCapacity().chargeCapacityArr[(int)(item.weight)];
                        d.location.longitude = dal.GetCustomer(item.targetId).longitude;
                        d.location.latitude = dal.GetCustomer(item.targetId).latitude;
                        d.droneStatus = IBL.BO.DroneStatus.available;
                        var par = item;
                        dal.deleteParcel(item);
                        par.delivered = DateTime.Now;
                        dal.addParcel(par);
                        addDrone(d,GetStation(item.targetId).id);
                        return;
                    }
                }
            }
            throw new unavailableException();
        }
        //  #region calculating Minimum Battery Required

        private int calcMinBatteryRequired(DroneToList drone)
        {
            if (drone.droneStatus == DroneStatus.available)
            {
                Location location = findClosetBaseStationLocation(drone.location,false);
                return (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * Distance(drone.location, location));
            }

            if (drone.droneStatus == DroneStatus.delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.deliveryId);
                if (parcel.pickedUp == null)
                {
                    int minValue;
                    IBL.BO.Customer sender = GetCustomer(parcel.senderId);
                    var target = GetCustomer(parcel.targetId);
                    double droneToSender = Distance(drone.location, sender.location);
                    minValue = (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * droneToSender);
                    double senderToTarget = Distance(sender.location, target.location);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)parcel.weight] * senderToTarget);
                    Location baseStationLocation = findClosetBaseStationLocation(target.location,false);
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
                    Location baseStationLocation = findClosetBaseStationLocation(target.location,false);
                    double targetToCharge = Distance(target.location, baseStationLocation);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * targetToCharge);
                    return minValue;
                }
            }
            return 90;
        }
        //#endregion
        //#endregion

        #endregion
        //#region Get Customer Recived Parcel
        //private IEnumerable<ParcelToList> getCustomerRecivedParcel(int id)
        //{
        //    var deliveries = dal.GetParcels()
        //        .Where(p => p.targetId == id)
        //        .Select(parcel =>
        //            new ParcelToList
        //            {
        //                id = parcel.id,
        //                senderName = parcel.senderId,
        //                receiveName = parcel.targetId,
        //                weight = (Weight)parcel.weight,
        //                priority = (Priority)parcel.priority,
        //            });
        //    return deliveries;
        //}
    }


}
