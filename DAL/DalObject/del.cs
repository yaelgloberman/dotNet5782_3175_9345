using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace IBL
{
    public class BL : IBL
    {
        private IDal dal;
        private List<DroneForList> drones;
        private static Random rand = new Random();
        public BL()
        {
            dal = new DalObject.DalObject();
            drones = new List<DroneForList>();
            initializeDrones();
        }

        private void initializeDrones()
        {
            foreach (var drone in dal.GetDrones())
            {
                drones.Add(new DroneForList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                });
            }

            foreach (var drone in drones)
            {
                if (isDroneWhileShipping(drone))
                {
                    drone.Status = DroneStatuses.Delivery;
                    drone.Location = findDroneLocation(drone);
                    // note : drone.DeliveryId getting value inside isDroneWhileShipping
                    int minBattery = calcMinBatteryRequired(drone);
                    drone.Battery = (double)rand.Next(minBattery, 100) / 100;
                }
                else
                {
                    drone.Status = (DroneStatuses)rand.Next(1, 2);
                    if (drone.Status == DroneStatuses.Maintenance)
                    {
                        int stationId = rand.Next(1, dal.GetBaseStations().Count());
                        dal.SendDroneToRecharge(drone.Id, stationId);
                        drone.Location = getBaseStationLocation(stationId);
                        drone.Battery = (double)rand.Next(5, 20) / 100;
                        drone.DeliveryId = 0;
                    }

                    if (drone.Status == DroneStatuses.Available)
                    {
                        drone.Location = findDroneLocation(drone);
                        drone.DeliveryId = 0;
                        int minBattery = calcMinBatteryRequired(drone);
                        drone.Battery = (double)rand.Next(minBattery, 100) / 100;
                    }
                }
            }
        }

        private int calcMinBatteryRequired(DroneForList drone)
        {
            if (drone.Status == DroneStatuses.Available)
            {
                Location location = findClosetBaseStationLocation(drone);
                return (int)(dal.BatteryUsages()[(int)BatteryUsage.Available] * calcDistance(drone.Location, location));
            }

            if (drone.Status == DroneStatuses.Delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.DeliveryId);
                if (parcel.PickedUp is null)
                {
                    int minValue;
                    Customer sender = GetCustomer(parcel.SenderId);
                    Customer target = GetCustomer(parcel.TargetId);
                    double droneToSender = calcDistance(drone.Location, sender.Location);
                    minValue = (int)(dal.BatteryUsages()[(int)BatteryUsage.Available] * droneToSender);
                    double senderToTarget = calcDistance(sender.Location, target.Location);
                    BatteryUsage batteryUsage =
                        (BatteryUsage)Enum.Parse(typeof(BatteryUsage), parcel.Weight.ToString());
                    minValue += (int)(dal.BatteryUsages()[(int)batteryUsage] * senderToTarget);
                    Location baseStationLocation = findClosetBaseStationLocation(target);
                    double targetToCharge = calcDistance(target.Location, baseStationLocation);
                    minValue += (int)(dal.BatteryUsages()[(int)BatteryUsage.Available] * targetToCharge);
                    return minValue;
                }

                if (parcel.Delivered is null)
                {
                    int minValue;
                    Customer sender = GetCustomer(parcel.SenderId);
                    Customer target = GetCustomer(parcel.TargetId);
                    double senderToTarget = calcDistance(sender.Location, target.Location);
                    BatteryUsage batteryUsage =
                        (BatteryUsage)Enum.Parse(typeof(BatteryUsage), parcel.Weight.ToString());
                    minValue = (int)(dal.BatteryUsages()[(int)batteryUsage] * senderToTarget);
                    Location baseStationLocation = findClosetBaseStationLocation(target);
                    double targetToCharge = calcDistance(target.Location, baseStationLocation);
                    minValue += (int)(dal.BatteryUsages()[(int)BatteryUsage.Available] * targetToCharge);
                    return minValue;
                }
            }
            return 90;
        }

        private double calcDistance(Location from, Location to)
        {
            int R = 6371 * 1000; // metres
            double phi1 = from.Latitude * Math.PI / 180; // φ, λ in radians
            double phi2 = to.Latitude * Math.PI / 180;
            double deltaPhi = (to.Latitude - from.Latitude) * Math.PI / 180;
            double deltaLambda = (to.Longitude - from.Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c / 1000; // in kilometres
            return d;
        }
        /// <summary>
        /// note: There is a side effect, if True drone.DeliveryId getting value
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>True if drone while shipping </returns>
        private bool isDroneWhileShipping(DroneForList drone)
        {
            var parcels = dal.GetParcels();
            for (int i = 0; i < parcels.Count(); i++)
            {
                if (parcels.ElementAt(i).DroneId == drone.Id)
                {
                    if (parcels.ElementAt(i).Delivered == null)
                    {
                        drone.DeliveryId = parcels.ElementAt(i).Id;
                        return true;
                    }
                }
            }
            return false;
        }

        private Location getBaseStationLocation(int stationId)
        {
            IDAL.DO.BaseStation baseStation = dal.GetBaseStation(stationId);
            return new Location { Latitude = baseStation.Latitude, Longitude = baseStation.Longitude };

        }

        private Location findDroneLocation(DroneForList drone)
        {
            if (drone.Status == DroneStatuses.Delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.DeliveryId);
                if (parcel.PickedUp == null)
                {
                    Customer customer = GetCustomer(parcel.SenderId);
                    return findClosetBaseStationLocation(customer);
                }
                if (parcel.Delivered == null)
                {
                    return GetCustomer(parcel.SenderId).Location;
                }
            }
            if (drone.Status == DroneStatuses.Available)
            {
                var targetsIds = dal.GetParcels()
                    .Where(parcel => parcel.DroneId == drone.Id)
                    .Select(parcel => parcel.TargetId).ToList();

                if (targetsIds.Count == 0)
                {
                    int stationId = rand.Next(1, dal.GetBaseStations().Count());
                    dal.SendDroneToRecharge(drone.Id, stationId);
                    return getBaseStationLocation(stationId);
                }
                //TODO: get last customer location
                return GetCustomer(targetsIds[rand.Next(targetsIds.Count)]).Location;
            }
            return new Location();
        }

        private Location findClosetBaseStationLocation(Ilocatable fromLocatable)
        {
            List<BaseStation> locations = new List<BaseStation>();
            foreach (var baseStation in dal.GetBaseStations())
            {
                locations.Add(new BaseStation
                {
                    Location = new Location
                    {
                        Latitude = baseStation.Latitude,
                        Longitude = baseStation.Longitude
                    }
                });
            }
            Location location = locations[0].Location;
            double distance = fromLocatable.Distance(locations[0]);
            for (int i = 1; i < locations.Count; i++)
            {
                if (fromLocatable.Distance(locations[i]) < distance)
                {
                    location = locations[i].Location;
                    distance = fromLocatable.Distance(locations[i]);
                }
            }
            return location;
        }
        /// <summary>
        /// add Base exception to dal
        /// </summary>
        /// <param name="AddbaseStation">receive from BL</param>
        public void AddBaseStation(BaseStation AddbaseStation)
        {
            IDAL.DO.BaseStation baseStationDO =
                new IDAL.DO.BaseStation()
                {
                    Id = AddbaseStation.Id,
                    Name = AddbaseStation.Name,
                    ChargingPorts = AddbaseStation.AvailableChargingPorts,
                    Latitude = AddbaseStation.Location.Latitude,
                    Longitude = AddbaseStation.Location.Longitude
                };

            try
            {
                dal.AddBaseStation(baseStationDO);
            }
            catch (Exception ex)
            {
                //sending inner exception for the exception returning from the DAL
                throw new ExistIdException(ex.Message, ex);
            }
        }

        public void AssignmentParcelToDrone(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public void PickedupParcel(int parcelId)
        {
            throw new NotImplementedException();
        }

        public void SendDroneToRecharge(int droneId, int baseStationId)
        {
            throw new NotImplementedException();
        }

        public void ReleaseDroneFromRecharge(int droneId)
        {
            throw new NotImplementedException();
        }

        public BaseStation GetBaseStation(int requestedId)
        {
            BO.BaseStation baseStationBO = new BaseStation();
            IDAL.DO.BaseStation baseStationDO = dal.GetBaseStation(requestedId);
            baseStationBO.Id = baseStationDO.Id;
            baseStationBO.Name = baseStationDO.Name;
            baseStationBO.AvailableChargingPorts = dal.AvailableChargingPorts(baseStationDO.Id);
            baseStationBO.Location = new Location()
            { Latitude = baseStationDO.Latitude, Longitude = baseStationDO.Longitude };
            baseStationBO.DronesInCharging = dal.GetDronesIdInBaseStation(requestedId)
                .Select(droneId => new DroneInCharging()
                {
                    Id = droneId,
                    Battery = getDroneBattery(droneId)
                });
            return baseStationBO;
        }

        private double getDroneBattery(int droneId)
        {
            return drones.Find(drone => drone.Id == droneId).Battery;
        }

        public Drone GetDrone(int requestedId)
        {
            Drone droneBO = new Drone();
            IDAL.DO.Drone droneDO = dal.GetDrone(requestedId);
            DroneForList drone = drones.Find(d => d.Id == requestedId);
            droneBO.Id = droneDO.Id;
            droneBO.Model = droneDO.Model;
            droneBO.MaxWeight = (WeightCategories)droneDO.MaxWeight;
            droneBO.Location = drone.Location;
            droneBO.Battery = drone.Battery;
            droneBO.Status = drone.Status;
            droneBO.Delivery = new Delivery();
            return droneBO;
        }

        private Location findDroneLocation(Drone droneBO)
        {
            return new Location();
        }

        public Customer GetCustomer(int requestedId)
        {
            BO.Customer customerBO = new Customer();
            IDAL.DO.Customer customerDO = dal.GetCustomer(requestedId);
            customerBO.Id = customerDO.Id;
            customerBO.Name = customerDO.Name;
            customerBO.Phone = customerDO.Phone;
            customerBO.Location = new Location() { Latitude = customerDO.Latitude, Longitude = customerDO.Longitude };
            customerBO.ShippedParcels = getCustomerShippedParcels(requestedId);
            customerBO.ReceivedParcels = getCustomerReceivedParcels(requestedId);
            return customerBO;
        }

        private IEnumerable<CustomerDelivery> getCustomerReceivedParcels(int requestedId)
        {
            var deliveries = dal.GetParcels()
                .Where(p => p.TargetId == requestedId)
                .Select(parcel =>
                    new CustomerDelivery
                    {
                        Id = parcel.Id,
                        Sender = parcel.SenderId,
                        Target = parcel.TargetId,
                        Priority = (Priorities)parcel.Priority,
                        Weight = (WeightCategories)parcel.Weight

                    });
            return deliveries;
        }

        private double euclideanDistance(Location from, Location to)
        {
            return Math.Sqrt(Math.Pow(to.Longitude - from.Longitude, 2) + Math.Pow(to.Latitude - from.Latitude, 2));
        }

        private IEnumerable<CustomerDelivery> getCustomerShippedParcels(int requestedId)
        {
            List<CustomerDelivery> deliveries = new List<CustomerDelivery>();
            foreach (var parcel in dal.GetParcels())
            {
                if (parcel.SenderId == requestedId)
                {
                    deliveries.Add(new CustomerDelivery()
                    {
                        Id = parcel.Id,
                        Sender = parcel.SenderId,
                        Target = parcel.TargetId,
                        Priority = (Priorities)parcel.Priority,
                        Weight = (WeightCategories)parcel.Weight
                    }
                    );
                }
            }
            return deliveries;
        }

        public Parcel GetParcel(int requestedId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStationForList> GetBaseStations()
        {
            return dal.GetBaseStations()
                .Select(baseStation =>
                    new BaseStationForList
                    {
                        Id = baseStation.Id,
                        Name = baseStation.Name,
                        UsedChargingPorts = getUsedChargingPorts(baseStation.Id),
                        AvailableChargingPorts = dal.AvailableChargingPorts(baseStation.Id)
                    }
                );
        }

        private int getUsedChargingPorts(int baseStationId)
        {
            return dal.GetBaseStation(baseStationId).ChargingPorts - dal.AvailableChargingPorts(baseStationId);
        }

        public IEnumerable<DroneForList> GetDrones(Func<DroneForList, bool> predicate)
        {
            if (predicate != null)
            {
                return drones.Where(predicate);

            }
            else
            {
                return drones;
            }
        }

        public IEnumerable<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> UnAssignmentParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStation> AvailableChargingStations()
        {
            throw new NotImplementedException();
        }

        public void AddNewCustomer(Customer customer)
        {
            IDAL.DO.Customer customerDO = new IDAL.DO.Customer();
            customerDO.Id = customer.Id;
            customerDO.Name = customer.Name;
            customerDO.Phone = customer.Phone;
            customerDO.Latitude = customer.Location.Latitude;
            customerDO.Longitude = customer.Location.Longitude;

            try
            {
                dal.AddCustomer(customerDO);
            }
            catch (Exception exp)
            {

                throw new ExistIdException("", exp);
            }
        }

        public void AddDrone(DroneForList drone)
        {
            drone.Battery = (double)rand.Next(5, 20) / 100;
            List<int> ids = dal.AvailableChargingStations().Select(bs => bs.Id).ToList();
            int stationId = ids[rand.Next(ids.Count)];
            dal.SendDroneToRecharge(drone.Id, stationId);
            drone.Location = getBaseStationLocation(stationId);
            drone.DeliveryId = 0;
            drones.Add(drone);

            IDAL.DO.Drone droneDO = new IDAL.DO.Drone();
            droneDO.Id = drone.Id;
            try
            {
                dal.AddDrone(droneDO);
            }
            catch (Exception exp)
            {

                throw new ExistIdException(exp);
            }
        }
    }
}