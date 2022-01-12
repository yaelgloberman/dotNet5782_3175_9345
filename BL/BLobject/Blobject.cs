using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using DalApi;
using BlApi;
using Dal;
using BO;
using DO;
namespace BL
{
    sealed partial class BL : IBl
    {
        static readonly IBl instance = new BL();
        public static IBl Instance { get => instance; }
        internal IDal dal = DalFactory.GetDal();
        private static Random rand = new Random();
        private List<DroneToList> drones;
        #region constractor
        /// <summary>
        /// the bl constructor that intializes the dron list in the bl and has access to the DAL
        /// </summary>
        public BL()
        {
            {
                drones = new List<BO.DroneToList>();
                bool flag = false;
                Random rnd = new Random();
                double minBatery = 0;
                IEnumerable<DO.Drone> d = dal.IEDroneList(x => x.id != 0);
                IEnumerable<DO.Parcel> p = dal.GetParcels();
                chargeCapacity chargeCapacity = GetChargeCapacity();
                foreach (var item in d)
                {
                    BO.DroneToList drt = new DroneToList();
                    drt.id = item.id;
                    drt.droneModel = item.model;
                    drt.weight = (BO.Weight)(int)item.maxWeight;
                    int parcelID = dal.GetParcelList().ToList().Find(x => x.droneId == drt.id).id;
                    drt.parcelId = parcelID;
                    if (parcelID != 0)
                    {
                        drt.droneStatus = DroneStatus.delivery;
                    }
                    var baseStationLocations = BaseStationLocationslist();
                    foreach (var pr in p)
                    {
                        if (pr.id == item.id && pr.delivered == null)
                        {
                            DO.Customer sender = dal.GetCustomer(pr.senderId);
                            DO.Customer target = dal.GetCustomer(pr.targetId);
                            BO.Location senderLocation = new Location { latitude = sender.latitude, longitude = sender.longitude };
                            BO.Location targetLocation = new Location { latitude = target.latitude, longitude = target.longitude };
                            drt.droneStatus = DroneStatus.delivery;
                            if (pr.pickedUp == null && pr.scheduled != null)//החבילה שויכה אבל עדיין לא נאספה
                            {
                                drt.location = new Location { latitude = findClosestStationLocation(senderLocation, false, baseStationLocations).latitude, longitude = findClosestStationLocation(senderLocation, false, baseStationLocations).longitude };
                                minBatery = Distance(drt.location, senderLocation) * chargeCapacity.chargeCapacityArr[0];
                                minBatery += Distance(senderLocation, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.weight];
                                minBatery += Distance(targetLocation, new Location { latitude = findClosestStationLocation(targetLocation, false, baseStationLocations).latitude, longitude = findClosestStationLocation(targetLocation, false, baseStationLocations).longitude }) * chargeCapacity.chargeCapacityArr[0];
                            }
                            if (pr.pickedUp != null && pr.delivered == null)//החבילה נאספה אבל עדיין לא הגיעה ליעד
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
                            drt.droneStatus = BO.DroneStatus.available;
                        else
                            drt.droneStatus = BO.DroneStatus.charge;
                        if (drt.droneStatus == BO.DroneStatus.charge)
                        {
                            int r = rnd.Next(0, dal.getStations().Count()), i = 0;
                            DO.Station s = new DO.Station();
                            foreach (var ite in dal.getStations())
                            {

                                s = ite;
                                if (i == r)
                                    break;
                                i++;
                            }
                            DO.droneCharges DC = new DO.droneCharges { droneId = drt.id, stationId = s.id };
                            var b = GetStation(DC.stationId);
                            Console.WriteLine(b.unavailableChargeSlots);
                            GetStation(DC.stationId).decreasingChargeSlots();
                            Console.WriteLine(b.unavailableChargeSlots);
                            dal.AddDroneCharge(DC);
                            drt.location = new Location { latitude = s.latitude, longitude = s.longitude };
                            drt.batteryStatus = rnd.Next(0, 21); // 100/;
                        }
                        else
                        {
                            List<DO.Customer> lst = new List<DO.Customer>();
                            foreach (var pr in p)
                            {
                                if (pr.delivered != null)
                                    lst.Add(dal.GetCustomer(pr.targetId));
                            }
                            if (lst.Count == 0)
                            {
                                foreach (var pr in dal.GetCustomerList())
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
        #endregion
        #region help private function 
        private static double getRandomCordinatesBL(double num1, double num2)
        {
            return (rand.NextDouble() * (num2 - num1) + num1);
        }
        /// <summary>
        /// recieving the dals version of the charge capacity - the battery usage of the parcels weight from the drone 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public chargeCapacity GetChargeCapacity()
        {
            double[] arr = dal.ChargeCapacity();
            chargeCapacity chargingUsage = new chargeCapacity { pwrAvailable = arr[0], pwrLight = arr[1], pwrAverge = arr[2], pwrHeavy = arr[3], pwrRateLoadingDrone = arr[4], chargeCapacityArr = arr };
            return chargingUsage;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckValidPassword(string name, string Password)///have to ask if i could make it public ??????
        {
            try { var Customer1 = GetCustomersToList().ToList().Where(x => x.Name == name && x.Password == Password );
                if (Customer1 != null)
                    return true;
                return false;
            }
            catch(dosntExisetException exp)
            { throw new  dosntExisetException(exp.Message); }

        }
        #endregion
        #region Get Used ChargingSlots
        /// <summary>
        /// a function that returns the unavailable chatrged slots
        /// </summary>
        /// <param name="baseStationId"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        private int getUsedChargingSlots(int baseStationId)
        {
            try
            {
                return dal.GetStation(baseStationId).chargeSlots - dal.AvailableChargingSlots(); 
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message+" station");
            }

        }
        #endregion
        #region Get Drone Battery
        /// <summary>
        /// the function recieves the current battery ststaus of teh drone in the bl
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        private double getDroneBattery(int droneId)
        {
            return drones.Find(drone => drone.id == droneId).batteryStatus;
        }
        #endregion
        #region get Customer In Parcel
        /// <summary>
        /// the progrmmer recievs the customer taht ordered or that sent the parcel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private BO.CustomerInParcel getCustomerInParcel(int id)
        {
            BO.CustomerInParcel c = new BO.CustomerInParcel();
            c.id = id;
            DO.Customer cs = dal.GetCustomer(id);
            c.name = cs.name;
            return c;
        }
        #endregion
        #region get Customer Shipped Parcels
        /// <summary>
        /// recieves the sender of the parcel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                        CustomerInParcel = new CustomerInParcel()   
                        {
                            id = parcel.id,   
                            name = dal.GetCustomerName(id),
                        }
                    });
            return tempParcel;
        }
        #endregion
        #region Get base station location
        /// <summary>
        /// recieves the station location by getting the station id
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        private Location getBaseStationLocation(int stationId)
        {
            DO.Station station = dal.GetStation(stationId);
            return new Location { latitude = station.latitude, longitude = station.longitude };
        }
        #endregion
        #region find Drone Location
        /// <summary>
        /// finding the drone location 
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private Location findDroneLocation(DroneToList drone)
        {
            if (drone.droneStatus == DroneStatus.delivery)
            {
                DO.Parcel parcel = dal.GetParcel(drone.parcelId);
                if (parcel.pickedUp == null)
                {
                    BO.Customer customer = GetCustomer(parcel.senderId);
                    return findClosestStationLocation(customer.location, false, BaseStationLocationslist());
                }
                if (parcel.delivered == null)
                {
                    return GetCustomer(parcel.senderId).location;
                }
            }
            if (drone.droneStatus == DroneStatus.available)////
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
                return GetCustomer(targetsIds[rand.Next(targetsIds.Count)]).location;
            }
            return new Location();
        }
        #endregion
        #region find Closet Base Station Location
        /// <summary>
        /// converting degrees format to radians foramt
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static double deg2rad(double val)
        {
            return (Math.PI / 180) * val;
        }
        /// <summary>
        /// calculating the ditance between 2 longitued and 2 lattitudes => 2 locations
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private double Distance(BO.Location l1, BO.Location l2)
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
        #endregion
        #region function that helps get locations
        /// <summary>
        /// reterns a list of all the station locations
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// returns the closest basestation location to a given location-> this has an option to choose if you want the closest location wit available charge slots or not
        /// </summary>
        /// <param name="currentlocation"></param>
        /// <param name="withChargeSlots"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
        #region Is Drone While Shipping
        /// <summary>
        /// returns if the drone is in delivery or not
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
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
        #region find the parcel
        /// <summary>
        /// finding the best parcel for the drone - based on the weight and battery that is left....
        /// </summary>
        /// <param name="we"></param>
        /// <param name="a"></param>
        /// <param name="buttery"></param>
        /// <param name="pri"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        /// 

        private DO.Parcel findTheParcel(BO.Weight we, BO.Location a, double buttery, DO.Proirities pri)
        {
            double d, x;
            DO.Parcel theParcel = new DO.Parcel();

            Location b = new Location();
            DO.Customer c = new DO.Customer();
            double far = 1000000;

                //The link is responsible for finding all packages in the requested priority
                var p = dal.UndiliveredParcels();
                IEnumerable<DO.Parcel> pr = new List<DO.Parcel>();
                pr = p.Where(item => item.priority == pri);

                foreach (var item in pr)
                {
                    c = dal.GetCustomer(item.senderId);
                    b.latitude = c.latitude;
                    b.longitude = c.longitude;
                    d = Distance(a, b);//המרחק בין מיקום נוכחי למיקום השולח
                    x = Distance(b, new Location { longitude = dal.GetCustomer(item.targetId).longitude, latitude = dal.GetCustomer(item.targetId).latitude });//המרחק בין מיקום שולח למיקום יעד
                    double fromCusToSta = Distance(new Location { longitude = dal.GetCustomer(item.targetId).longitude, latitude = dal.GetCustomer(item.targetId).latitude }, findClosestStationLocation(new Location { longitude = dal.GetCustomer(item.targetId).longitude, latitude = dal.GetCustomer(item.targetId).latitude }, false, BaseStationLocationslist()));
                    double butteryUse = x * GetChargeCapacity().chargeCapacityArr[indexOfChargeCapacity(item.weight)] + fromCusToSta * GetChargeCapacity().chargeCapacityArr[0] + d * GetChargeCapacity().chargeCapacityArr[0];
                    if (d < far && (buttery - butteryUse) > 0 && item.scheduled == null)
                    {
                        if (weight(we, (Weight)item.weight) == true)
                        {
                            far = d;
                            theParcel = item;
                            return theParcel;
                        }
                    }
                }
            

            if (pri == DO.Proirities.emergency)//If not found the highest priority looking for the priority below it
                theParcel = findTheParcel(we, a, buttery, DO.Proirities.fast);

            if (pri == DO.Proirities.fast)
                theParcel = findTheParcel(we, a, buttery, DO.Proirities.regular);
            if (theParcel.id == 0)
            {
                if (dal.UndiliveredParcels().Count(x => (Weight)x.weight == we) == 0) 
                {
                    throw new validException("there is no parcel that can be matched bcs weight");
                }
                throw new findException("ERROR! there is not a parcel that match to the drone ");

            }
            return theParcel;

        }
        public void startDroneSimulation(int id, Action updateDelegate, Func<bool> stopDelegate)
        {
            new Simulator(this, id, updateDelegate, stopDelegate);
        }
        /// <summary>
        /// retruning if the drone is matching the parcels whieght
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="pa"></param>
        /// <returns></returns>
        private bool weight(BO.Weight dr, BO.Weight pa)
        {
            if (dr == BO.Weight.heavy)
                return true;
            if (dr == BO.Weight.average && (pa == BO.Weight.average || pa == BO.Weight.light))
                return true;
            if (dr == BO.Weight.light && pa == BO.Weight.light)
                return true;
            return false;
        }
        #region index Of Charge Capacity
        /// <summary>
        /// returning the index of charge capacity based on the weight
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private int indexOfChargeCapacity(DO.WeightCatigories w)
        {
            if (w == DO.WeightCatigories.light)
                return 1;
            if (w == DO.WeightCatigories.heavy)
                return 2;
            if (w == DO.WeightCatigories.average)
                return 3;

            return 0;

        }
        #endregion
        #region Get Station By Drone
        /// <summary>
        /// returning a station by recieving a drone
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private BaseStation GetStationByDrone(DroneToList d)
        {
            var stationLoc = findClosestStationLocation(d.location, false, BaseStationLocationslist());
            var station = GetStations().ToList().Find(x => x.location.longitude == stationLoc.longitude && x.location.latitude == stationLoc.latitude);
                return station;
        }
        #endregion
        #region calculate the minumum battery required
        /// <summary>
        /// returns the minumin of battery required from the current location to destination 
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private int calcMinBatteryRequired(DroneToList drone)
        {
            if (drone.droneStatus == DroneStatus.available)
            {
                Location location = findClosestStationLocation(drone.location, false, BaseStationLocationslist());
                return (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * Distance(drone.location, location));
            }

            if (drone.droneStatus == DroneStatus.delivery)
            {
                DO.Parcel parcel = dal.GetParcel(drone.parcelId);
                if (parcel.pickedUp == null)
                {
                    int minValue;
                    BO.Customer sender = GetCustomer(parcel.senderId);
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
        #endregion
    }
}
#endregion