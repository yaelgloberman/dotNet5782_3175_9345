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
    public partial class BL : IBl
    {
        public static IDal dal; 
        private static Random rand = new Random();
        private List<DroneToList> drones;
        public static int unavailableChargeSlots;
        private static double getRandomCordinatesBL(double num1, double num2)
        {
            return (rand.NextDouble() * (num2 - num1) + num1);
        }
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
                        if (pr.id == item.id && pr.delivered == null)
                        {
                            IDAL.DO.Customer sender = dal.GetCustomer(pr.senderId);
                            IDAL.DO.Customer target = dal.GetCustomer(pr.targetId);
                            IBL.BO.Location senderLocation = new Location { latitude = sender.latitude, longitude = sender.longitude };
                            IBL.BO.Location targetLocation = new Location { latitude = target.latitude, longitude = target.longitude };
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
                                if (pr.delivered != null)
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
        #region Get Used ChargingSlots
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

        private double getDroneBattery(int droneId)
        {
            return drones.Find(drone => drone.id == droneId).batteryStatus;
        }
        #endregion
        #region get Customer In Parcel
        private IBL.BO.CustomerInParcel getCustomerInParcel(int id)
        {
            IBL.BO.CustomerInParcel c = new IBL.BO.CustomerInParcel();
            c.id = id;
            IDAL.DO.Customer cs = dal.GetCustomer(id);
            c.name = cs.name;
            return c;
        }
        #endregion
        #region get Customer Shipped Parcels
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
                return GetCustomer(targetsIds[rand.Next(targetsIds.Count)]).location;
            }
            return new Location();
        }
        #endregion
        #region find Closet Base Station Location
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
        #endregion
        #region dunction that helps get locations
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
        #region find the parcel
        private IDAL.DO.Parcel findTheParcel(IBL.BO.Weight we, IBL.BO.Location a, double buttery, IDAL.DO.Proirities pri)
        {


            double d, x;
            IDAL.DO.Parcel theParcel = new IDAL.DO.Parcel();

            IBL.BO.Location loc = new IBL.BO.Location();
            IDAL.DO.Customer customer = new IDAL.DO.Customer();
            double far = 1000000;
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
                if (d < far && (buttery - butteryUse) > 0 && item.scheduled == null && weight(we, (IBL.BO.Weight)item.weight) == true)
                {
                    far = d;
                    theParcel = item;
                    return theParcel;
                }
            }
            if (pri == IDAL.DO.Proirities.emergency)//אם לא מצא בעדיפות הכי גבוהה מחפש בעדיפות מתחתיה
                theParcel = findTheParcel(we, a, buttery, IDAL.DO.Proirities.fast);

            if (pri == IDAL.DO.Proirities.fast)
                theParcel = findTheParcel(we, a, buttery, IDAL.DO.Proirities.regular);
            if (theParcel.id == 0)
                throw new dosntExisetException("ERROR! there is not a parcel that match to the drone ");
            return theParcel;
        }
        #endregion
        #region weight
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
        #endregion
        #region index Of Charge Capacity
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
        #endregion
        #region Get Station By Drone
        private BaseStation GetStationByDrone(DroneToList d)
        {
            var stationLoc = findClosestStationLocation(d.location, false, BaseStationLocationslist());
            var station = GetStations().ToList().Find(x => x.location.longitude == stationLoc.longitude && x.location.latitude == stationLoc.latitude);
                return station;
        }
        #endregion
        #region calculate the minumum battery required
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
        #endregion


    }
}