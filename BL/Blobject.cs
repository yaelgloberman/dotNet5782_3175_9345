﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IBL.BO;
using IDAL.DO;
//yaeli
namespace BL
{
    public class Blobject : IBl
    {

        private IDal dal = new DalObject.DalObject();
        private static Random rand = new Random();
        private List<DroneToList> drones = new();
        public Blobject()
        {
            double[] tempArray = dal.ChargeCapacity();
            double pwrAvailable = tempArray[0];
            double pwrLight = tempArray[1];
            double pwrAverge = tempArray[2];
            double pwrHeavy = tempArray[3];
            double pwrRateLoadingDrone = tempArray[4];
            initDrone();
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
        //DateTime help = new DateTime(); 
        #region Init drone
        private void initDrone()
        {
            List<IDAL.DO.Parcel> undeliveredParcels = dal.UndiliveredParcels();
            IBL.BO.DroneToList tempD = new IBL.BO.DroneToList();
            foreach (var p in undeliveredParcels)
            {
                int droneIndex = p.droneId;
                int droneindex = drones.FindIndex(d => d.id == droneIndex);
                var tmpD = drones[droneIndex];
                if (droneIndex >= 0)
                {
                    tmpD.droneStatus = DroneStatus.delivery;
                    if (p.pickedUp == DateTime.MinValue)//if the parcel was matched but nit picked up
                    {
                        Location senderLocation = new Location();
                        IDAL.DO.Customer senderOfParcel = dal.GetCustomer(p.senderId);
                        senderLocation.longitude = senderOfParcel.longitude;
                        senderLocation.latitude = senderOfParcel.latitude;
                        tempD.location = senderLocation;
                    }
                    tmpD.batteryStatus = rand.Next(40, 100);//enough battery left so the parcel could get its destination
                }
                foreach (var d in drones)
                {

                    int maxStations = dal.stationList().Count();
                    if (d.droneStatus != DroneStatus.delivery)
                        d.droneStatus = (DroneStatus)rand.Next(1);
                    if (d.droneStatus == DroneStatus.charge)
                    {
                        IDAL.DO.Station s = new Station();
                        s = dal.GetStation(rand.Next(maxStations));
                        tempD.location.latitude = s.latitude;//not sure why its not letting me and if i should use a temp
                        tempD.droneStatus = (DroneStatus)rand.Next(20);
                    }
                    if (d.droneStatus == DroneStatus.available)
                    {
                        droneIndex = rand.Next();
                        // 	מיקומו יוגרל בין לקוחות שיש חבילות שסופקו להם 
                        tempD.batteryStatus = rand.Next(40, 100);//not sure if its supposed to be enum or double
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
            stationToAdd.DroneInChargeList = new List<DroneInCharge>();  // חושבת שככה מאתחלים ל0 רשימה 
            if (!(stationToAdd.id >= 10000000 && stationToAdd.id <= 1000000000))
                throw new AddException("the number of the base station id in invalid\n");
            if (stationToAdd.location.latitude < (double)31 || stationToAdd.location.latitude > 33.3 || stationToAdd.location.longitude < 34.3 || stationToAdd.location.longitude > 35.5) ;
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

                throw new AlreadyExistException("the station already exist", exp);
            }
        }
        #endregion
        #region ADD Drone
        public void addDrone(DroneToList droneToAdd, int stationId)
        {
            droneToAdd.batteryStatus = (double)rand.Next(20, 40);
            if (!(droneToAdd.id >= 10000000 && droneToAdd.id <= 1000000000))
                throw new AddException("the number of the drone id in invalid\n");
            //if (!(droneToAdd.droneModel <=  && droneToAdd.droneModel >= ))   //איך בודקים עכשיו את המודל כשהוא סטרינג
            //throw new AddException("the number of the drone model invalid\n");
            if (!(droneToAdd.batteryStatus >= (double)0 && droneToAdd.batteryStatus <= (double)100))
                throw new AddException("the status of the drone is invalid\n");
            if (droneToAdd.location.latitude < (double)31 || droneToAdd.location.latitude > 33.3
                            || droneToAdd.location.longitude < 34.3 || droneToAdd.location.longitude > 35.5)
                throw new AddException("the given cordinates do not exist in this country/\n");
            if (!(droneToAdd.weight > (Weight)0 && droneToAdd.weight < (Weight)3))
                throw new AddException("the given weight is not valid\n");
            try   //im not sure that everithing needs to be in the try but wtev
            {
                IDAL.DO.Drone tempDrone = dal.GetDrone(droneToAdd.id);
                IDAL.DO.Station tempStation = dal.GetStation(stationId);
                droneToAdd.droneStatus = DroneStatus.charge;
                droneToAdd.location = getBaseStationLocation(stationId);
                IDAL.DO.Drone drone = new IDAL.DO.Drone();
                drone.id = droneToAdd.id;
                drone.model = droneToAdd.droneModel;
                drone.maxWeight = (WeightCatigories)droneToAdd.weight;
                dal.SendToCharge(droneToAdd.id, stationId);
                drones.Add(droneToAdd);
                dal.addDrone(drone);
            }
            catch (AddException exp)
            {
                throw new AlreadyExistException("this drone already exists\n");
            }

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
                Station station = dal.GetStation(id);
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
        #region GetParcels
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
        public List<IBL.BO.Customer> GetCustomer()
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
                IBL.BO.Customer CustomerBo = new IBL.BO.Customer();
                IDAL.DO.Customer CustomerDo = dal.GetCustomer(id);
                CustomerBo.id = CustomerDo.id;
                CustomerBo.Name = CustomerDo.name;
                CustomerBo.phoneNumber = CustomerDo.phoneNumber;
                CustomerBo.location = new Location() { latitude = CustomerDo.latitude, longitude = CustomerDo.longitude };
                CustomerBo.SentParcels = getCustomerShippedParcels(id).ToList();
                CustomerBo.ReceiveParcel = getCustomerRecivedParcel(id).ToList();
                return CustomerBo;
            }
            catch (IDAL.DO.findException Fex)
            {
                throw new dosntExisetException($"Customer id {id}", Fex);
            }
        }
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
        #endregion
        #endregion
        #region find&delete
        #region find Drone Location
        private Location findDroneLocation(DroneToList drone)
        {
            if (drone.droneStatus == DroneStatus.delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.deliveryId);
                if (parcel.pickedUp == null)
                {
                    IBL.BO.Customer customer = GetCustomer(parcel.senderId);
                    return findClosetBaseStationLocation(customer);
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
        private double Distance(Location x, Location y)
        {
            var distance = Math.Sqrt(Math.Pow(x.longitude - y.longitude, 2) + Math.Pow(x.latitude - y.latitude, 2));
            return distance;
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
        #endregion
        #region HELP FUNCTIONS
        #region Distance Calculation
        private double distanceCalculation(Location from, Location to)
        {
            int R = 6371 * 1000;                         // metres
            double phi1 = from.latitude * Math.PI / 180; // φ, λ in radians
            double phi2 = to.latitude * Math.PI / 180;
            double deltaPhi = (to.latitude - from.latitude) * Math.PI / 180;
            double deltaLambda = (to.longitude - from.longitude) * Math.PI / 180;
            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c / 1000; // in kilometres
            return d;
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
        //#region calculating Minimum Battery Required
        //private int calcMinBatteryRequired(DroneToList drone)
        //{
        //    if (drone.droneStatus == DroneStatus.available)
        //    {
        //        Location location = findClosetBaseStationLocation(drone.location, false);
        //        return (dal.ChargeCapacity[4], Distance(drone.location, location));
        //    }

        //    if (drone.droneStatus == DroneStatus.delivery)
        //    {
        //        IDAL.DO.Parcel parcel = dal.GetParcel(drone.deliveryId);
        //        if (parcel.pickedUp is null)
        //        {
        //            int minValue;
        //            IBL.BO.Customer sender = GetCustomer(parcel.senderId);
        //            IBL.BO.Customer target = GetCustomer(parcel.targetId);
        //            double droneToSender = distanceCalculation(drone.location, sender.location);
        //            double senderToTarget = distanceCalculation(sender.location, target.location);
        //            BatteryUsage batteryUsage =
        //                (dal.)Enum.Parse(typeof(BatteryUsage), parcel.weight.ToString());
        //            minValue += (int)(dal.BatteryUsages()[(int)batteryUsage] * senderToTarget);
        //            Location baseStationLocation = findClosetBaseStationLocation(target.location, false);//not sure if i nee the  target to have available charge slots
        //            double targetToCharge = distanceCalculation(target.location, baseStationLocation);
        //            minValue += (int)(dal.BatteryUsages()[(int)BatteryUsage.Available] * targetToCharge);
        //            return minValue;
        //        }

        //        if (parcel.delivered is null)
        //        {
        //            int minValue;
        //            IBL.BO.Customer sender = GetCustomer(parcel.senderId);
        //            IBL.BO.Customer target = GetCustomer(parcel.targetId);
        //            double senderToTarget = distanceCalculation(sender.location, target.location);
        //            BatteryUsage batteryUsage =
        //                (BatteryUsage)Enum.Parse(typeof(BatteryUsage), parcel.weight.ToString());
        //            minValue = (int)(dal.BatteryUsages()[(int)batteryUsage] * senderToTarget);
        //            Location baseStationLocation = findClosetBaseStationLocation(target.location, false);
        //            double targetToCharge = distanceCalculation(target.location, baseStationLocation);
        //            minValue += (int)(dal.BatteryUsages()[(int)BatteryUsage.Available] * targetToCharge);
        //            return minValue;
        //        }
        //    }
        //    return 90;
        //}
        //#endregion
        //#endregion

        #region UPDATE
        public void updateDroneName(int droneID, string dModel)
        {
            //	if (dModel > 999 && dModel < 10000)//what is the bdika for the model?
            {
                int dIndex = drones.FindIndex(x => x.id == droneID);
                drones[dIndex].droneModel = dModel;
            }
            throw new validException("the name of the model is not valid\n");//remeber to catch in the main

        }
        public void updateStation(int stationID, int AvlblDCharges, string Name = " ")
        {
            IBL.BO.BaseStation s = GetStation(stationID);
            if (AvlblDCharges != null || Name != " ")
            {
                dal.stationList().ToList().Remove(dal.GetStation(stationID));//not sure if this is how i remove the station im updating

                if (Name != " ")
                    s.stationName = Name;
                if (AvlblDCharges != null)
                {
                    if (AvlblDCharges < 0)
                        throw new validException("this amount of drone choging slots is not valid!\n");
                    s.avilableChargeSlots = AvlblDCharges;      //i need al the slots not just the available one - not sure what this variable means
                }
                addStation(s);
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
            IBL.BO.DroneToList drone = GetDrone(droneID);
            Location stationLocation = findClosetBaseStationLocation(drone.location, true);//not sure where and what its from
            IBL.BO.BaseStation station = GetStation(StationID);
            //Location stationLoc = new Location { latitude = station.location, longitude = station.longitude };
            int droneIndex = drones.FindIndex(x => x.id == droneID);
            station.decreasingChargeSlots();
            drones[droneIndex].batteryStatus = calcMinBatteryRequired(drones[droneIndex]);
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.charge;
            dal.deleteDrone(dal.GetDrone(droneID));
            addDrone(drones[droneIndex], StationID);
            IDAL.DO.droneCharges DC = new droneCharges { droneId = droneID, stationId = StationID };
            dal.chargingDroneList().ToList().Add(DC);
        }
        public void releasingDrone(int droneID, TimeSpan chargingTime)
        {
            int index = drones.FindIndex(x => x.id == droneID);
            IDAL.DO.droneCharges DC = dal.chargingDroneList().ToList().Find(X => X.droneId == droneID);
            IBL.BO.BaseStation bstation = GetStation(DC.stationId);
            dal.deleteStation(dal.GetStation(DC.stationId));
            bstation.addingChargeSlots();
            addStation(bstation);
            dal.chargingDroneList().ToList().Remove(DC);
.			if (drones[index].droneStatus != DroneStatus.charge)
                throw new Exception("this drone is not available!\n");
            //drones[index].batteryStatus = has to calculate based on the amount of time it was charginhg but not so sure what that means...
            drones[index].droneStatus = DroneStatus.available;
        }
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
        #endregion
        public void matchingDroneToParcel(int droneID)//didnt finish this function at all 
        {
            IBL.BO.DroneToList drone = GetDrone(droneID);
            //finding the best parcel
            if (drone.droneStatus != DroneStatus.available)
                throw new unavailableException("the drone is unavailable\n");
            bool flag = false;
            List<IBL.BO.Parcel> tempParcels = new List<IBL.BO.Parcel>();
            tempParcels = GetParcels();
            tempParcels = tempParcels.FindAll(x => x.priority == Priority.emergency);///figure out  awayto check if there was no emergencies
            //
            tempParcels.FindAll(x => x.priority == Priority.fast);
            tempParcels = tempParcels.FindAll(x => (int)x.weightCategorie <= (int)drone.weight);
            tempParcels = tempParcels.FindAll(x => x.priority == Priority.emergency);
            tempParcels = tempParcels.FindAll(x => x.priority == Priority.emergency);
            tempParcels = tempParcels.FindAll(x => x.priority == Priority.emergency);
            IBL.BO.Parcel theparcel = tempParcels[0];

            // the actual update
            if (tempParcels.Count() == 0)
            {
                IBL.BO.Parcel ChosenParcel = tempParcels[0];
                drone.droneStatus = DroneStatus.delivery;
                dal.deleteDrone(dal.GetDrone(droneID));
                addDrone(drone, dal.GetStation(ChosenParcel.receive.id).id);
                IBL.BO.DroneInParcel droneInParcel = new DroneInParcel { id = droneID, battery = drone.batteryStatus, location = drone.location };
                ChosenParcel.droneInParcel = droneInParcel;
                ChosenParcel.scheduled = DateTime.Now;//notsure which one is זמן השיוך
                dal.deleteParcel(dal.GetParcel(ChosenParcel.id));
                addParcel(ChosenParcel);
            }
        }
        public void pickedUpParcelByDrone(int droneID)
        {

        }
        //public void ReleaseDroneCharge(int droneId, TimeSpan chargeTime)
        //{
        //    DroneToList droneItem = new();
        //    try
        //    {
        //        droneItem = GetDrones().Find(x => x.id == droneId);
        //    }
        //    catch (IDAL.DO.findException)
        //    {
        //        throw new BO.NotExistException();
        //    }
        //    droneItem = GetDrones().Find(x => x.id == droneId);
        //    if (droneItem.droneStatus != DroneStatus.charge)
        //    {
        //        throw new BO.CannotReleaseFromChargeException(droneId);
        //    }
        //    else
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
    }
}
#endregion