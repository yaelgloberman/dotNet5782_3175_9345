using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BlApi;
using BO;
using DO;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
namespace BL
{
    public partial class BL : IBl
    {
        #region ADD Drone
        /// <summary>
        /// adding a drone to the datasource by recieving features of a drone and throwing an exception of any of the given info was incorrects
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        /// <param name="droneModel"></param>
        /// <param name="weight"></param>
        /// <exception cref="AlreadyExistException"></exception>
        /// <exception cref="validException"></exception>
        /// <exception cref="dosntExisetException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void addDrone(int droneId, int stationId, string droneModel, Weight weight)
        {
            try
            {
                if (dal.IEDroneList(x=>x.id!=0).ToList().Exists(item => item.id == droneId))
                    throw new AlreadyExistException("Drone already exist");
                if (!(droneId >= 10000000 && droneId < 1000000000))
                    throw new validException("the number of the drone id in invalid\n");
                if (!(weight >= (Weight)1 && weight <= (Weight)3))
                    throw new validException("the given weight is not valid\n");
                if (!(stationId >= 10000000 && stationId <= 1000000000))
                    throw new validException("the number of the drone id in invalid\n");
                DO.Station stationDl = dal.GetStation(stationId);
                if (stationDl.latitude < (double)31 || stationDl.latitude > 33.3)
                    throw new validException("the given latitude do not exist in this country/\n");
                if (stationDl.longitude < 34.3 || stationDl.longitude > 35.5)
                    throw new validException("the given longitude do not exist in this country/\n");
                DroneToList dtl = new DroneToList();
                dtl.id = droneId;
                dtl.droneModel = droneModel;
                dtl.weight = weight;
                dtl.batteryStatus = (double)rand.Next(20, 40);
                dtl.droneStatus = DroneStatus.available; 
                dtl.location = new Location();
                dtl.location.latitude = stationDl.latitude;
                dtl.location.longitude = stationDl.longitude;
                DO.Drone dr = new DO.Drone();
                {
                    dr.id = droneId;
                    dr.model = droneModel;
                    dr.maxWeight = (WeightCatigories)weight;
                }
                dal.addDrone(dr);
                drones.Add(dtl);
                DO.droneCharges dc = new DO.droneCharges { droneId = droneId, stationId = stationId };
                SendToCharge(droneId);
            }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
        }
        /// <summary>
        /// adding a drone to the datasource by recieving features of a drone and throwing an exception of any of the given info was incorrects
        /// </summary>
        /// <param name="droneToAdd"></param>
        /// <param name="stationId"></param>
        /// <exception cref="AlreadyExistException"></exception>
        /// <exception cref="validException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void addDrone(DroneToList droneToAdd, int stationId)
        {
            if (dal.IEDroneList(x => x.id != 0).ToList().Exists(item => item.id == droneToAdd.id))
                throw new AlreadyExistException("Drone already exist");
            DO.Station stationDl = dal.GetStation(stationId);
            droneToAdd.location.latitude = stationDl.latitude;
            droneToAdd.location.longitude = stationDl.longitude;
            if (droneToAdd.batteryStatus == 0) { droneToAdd.batteryStatus = (double)rand.Next(20, 40); }
            if (droneToAdd.droneStatus == 0)
            {
                droneToAdd.droneStatus = DroneStatus.charge; DO.droneCharges DC = new droneCharges { droneId = droneToAdd.id, stationId = stationId }; dal.AddDroneCharge(DC);
            }
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
            DO.Drone drone = new DO.Drone();
            drone.id = droneToAdd.id;
            drone.model = droneToAdd.droneModel;
            drone.maxWeight = (WeightCatigories)droneToAdd.weight;
            dal.addDrone(drone);
            drones.Add(droneToAdd);
        }
        #endregion
        #region DELETE DRONE

        /// <summary>
        /// deleting a drone for the datasource  and throwing an exception of the drone was not found
        /// </summary>
        /// <param name="droneID"></param>
        /// <exception cref="deleteException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void deleteDrone(int droneID)
        {
            try
            {
                if (GetDrone(droneID).droneStatus == DroneStatus.charge)
                { var DC = dal.chargingGetDroneList().ToList().Find(x => x.droneId == droneID); dal.RemoveDroneCharge(DC); }
                dal.deleteDrone(dal.GetDrone(droneID));

            }
            catch (findException exp) { throw new deleteException (exp.Message); }
        }
        #endregion
        #region returns drone
        /// <summary>
        /// recieving a drone from the data source and returning ot to the progrmmer with the bl Drone (regular) features
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Drone returnsDrone(int id)
        {

            var drn = GetDrones().Find(x => x.id == id);
            if (drn == null)
                throw new dosntExisetException("Error! the drone doesn't found");
            BO.Drone d = new BO.Drone();
            d.id = drn.id;
            d.droneModel = drn.droneModel;
            d.weight = drn.weight;
            d.droneStatus = drn.droneStatus;
            d.batteryStatus = drn.batteryStatus;
            d.location = new BO.Location();
            d.location = drn.location;
            BO.ParcelInTransfer pt = new BO.ParcelInTransfer();
            if (drn.droneStatus == BO.DroneStatus.delivery)
            {
                pt.id = drn.parcelId;
                DO.Parcel p = new DO.Parcel();
                try
                {
                    p = dal.GetParcel(drn.parcelId);//get the parcel from the dal
                }
                catch (Exception)
                {
                    throw new dosntExisetException("Error! the parcel not found");
                }
                if (p.pickedUp == null )
                    pt.parcelStatus = false;
                else
                    pt.parcelStatus = true;
                pt.priority = (BO.Priority)p.priority;
                pt.weight = (BO.Weight)p.weight;
                pt.sender = new BO.CustomerInParcel();
                pt.sender = getCustomerInParcel(p.senderId);
                pt.receive = new BO.CustomerInParcel();
                pt.receive = getCustomerInParcel(p.targetId);
                DO.Customer sender = dal.GetCustomer(p.senderId);
                DO.Customer target = dal.GetCustomer(p.targetId);
                pt.collection = new BO.Location();
                pt.collection.longitude = sender.longitude;
                pt.collection.latitude = sender.latitude;
                pt.DeliveryDestination = new BO.Location();
                pt.DeliveryDestination.longitude = target.longitude;
                pt.DeliveryDestination.latitude = target.latitude;
                pt.TransportDistance = Distance(pt.collection, pt.DeliveryDestination);
                d.parcelInTransfer = new BO.ParcelInTransfer();
                d.parcelInTransfer = pt;
            }
            return d;
        }
        #endregion
        #region Get Drone
        /// <summary>
        /// recieving a drone from the data source and returning ot to the progrmmer with the bl Drone to list features

        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        public DroneToList GetDrone(int id)
        {
            try
            {
                DroneToList droneBo = new DroneToList();
                DO.Drone droneDo = dal.GetDrone(id);
                DroneToList drone = drones.ToList().Find(d => d.id == id);
                droneBo.id = droneDo.id;
                droneBo.droneModel = drone.droneModel;
                droneBo.weight = drone.weight;
                droneBo.location = drone.location;
                droneBo.batteryStatus = drone.batteryStatus;
                droneBo.droneStatus = drone.droneStatus;
                int parcelID = dal.GetParcelList().ToList().Find(x => x.droneId == droneBo.id).id;
                if (parcelID != 0)
                {
                    droneBo.parcelId = parcelID;
                    droneBo.droneStatus = DroneStatus.delivery;
                }
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
            catch(Exception e) { throw new dosntExisetException(e.Message); }
        }
        public DroneInParcel GetDroneInParcel(int id)
        {
            try
            {
                DroneInParcel droneBo = new ();
                DO.Drone droneDo = dal.GetDrone(id);
                DroneToList drone = drones.ToList().Find(d => d.id == id);
                droneBo.id = droneDo.id;
                droneBo.location = drone.location;
                droneBo.battery = drone.batteryStatus; 
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
        #endregion
        #region returns the list
        /// <summary>
        ///recieving the list of all the drones  from the data source and returning ot to the progrmmer with the bl Drone to list features
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<BO.DroneToList> GetDrones()
        {
            List<BO.DroneToList> drones = new List<BO.DroneToList>();
            foreach (var d in dal.GetDroneList())
            { drones.Add(GetDrone(d.id)); }
            return drones;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate = null)
        {
            if (predicate == null)
            {
                return drones.Take(drones.Count).ToList();
            }
            return GetDrones().Where(predicate).ToList();
        }
        #endregion
        #region update functions
        /// <summary>
        /// an update function  that updates teh drone name  and model with recieving the drones id
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="dModel"></param>
        /// <exception cref="dosntExisetException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void updateDroneName(int droneID, string dModel)
        {
            //int dIndex = GetDrones().FindIndex(x => x.id == droneID);
            //if (dIndex == -1)//לדעת מה הוא מחזיר אם הוא לא מוצא ולשים בתנאי
            //{
            //    throw new dosntExisetException("drone do not exist");
            //}
            try { dal.updateDrone(droneID, dModel); }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
            BO.DroneToList dr = drones.Find(p => p.id == droneID);
            drones.Remove(dr);
            dr.droneModel = dModel;
            drones.Add(dr);
        }
        /// <summary>
        /// an update function that updates the drone  and the station slots when the  drone is sent to be charged
        /// </summary>
        /// <param name="droneID"></param>
        /// <exception cref="dosntExisetException"></exception>
        /// <exception cref="unavailableException"></exception>
        /// <exception cref="deleteException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendToCharge(int droneID)
        {
            DroneToList drone = new();
            BaseStation station = new();
            try { drone = GetDrone(droneID); }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
            if (drone.droneStatus != DroneStatus.available)
                throw new unavailableException("the drone is unavailable");
            Location stationLocation = findClosestStationLocation(drone.location, false, BaseStationLocationslist());//not sure where and what its from
            station = GetStations().Find(x => x.location.longitude == stationLocation.longitude && x.location.latitude == stationLocation.latitude);
            int droneIndex = drones.ToList().FindIndex(x => x.id == droneID);
            drones[droneIndex].batteryStatus -= chargeCapacity[0] * Distance(drone.location, stationLocation);
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.charge;
            dal.SendToCharge(droneID, station.id);
            DO.droneCharges DC = new droneCharges { droneId = droneID, stationId = station.id ,enterToCharge=DateTime.Now};
            dal.AddDroneCharge(DC);
        }
        /// <summary>
        /// an update function that upadets the drone an d the station charging slots when the drone is being released from its chraging
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="chargingTime"></param>
        /// <exception cref="findException"></exception>
        /// <exception cref="unavailableException"></exception>
        /// <exception cref="dosntExisetException"></exception>
        public void releasingDrone(int id)
        {
            try
            {
                DroneToList d = drones.Find(p => p.id == id);
                DO.droneCharges DC = new();
                    if (d.droneStatus == DroneStatus.charge)
                    {
                    try {  DC = dal.chargingGetDroneList().ToList().Find(X => X.droneId == id); }
                    catch (findException exp) { throw new dosntExisetException(exp.Message); }
                    double t = DateTime.Now.TimeOfDay.TotalSeconds;
                    double total = t - DC.enterToCharge.TimeOfDay.TotalSeconds;
                    d.batteryStatus += total * chargeCapacity[4];
                    if (d.batteryStatus > 100)
                        d.batteryStatus = 100;
                    d.droneStatus = DroneStatus.available;
                    dal.releasingDrone(DC);
                    }
                    else throw new validException("Error! the drone dont was in charge");
                
            }
            catch (Exception e)
            {
                throw new validException(e.Message, e);
            }
        }
    }
        #endregion

    }








