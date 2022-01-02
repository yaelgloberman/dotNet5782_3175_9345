//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DalApi;
//using BlApi;
//using BO;
//using DO;
//using System.Runtime.Serialization;
//namespace BL
//{
//    public partial class BL : IBl
//    {
//        #region ADD Drone
//        /// <summary>
//        /// adding a drone to the datasource by recieving features of a drone and throwing an exception of any of the given info was incorrects
//        /// </summary>
//        /// <param name="droneId"></param>
//        /// <param name="stationId"></param>
//        /// <param name="droneModel"></param>
//        /// <param name="weight"></param>
//        /// <exception cref="AlreadyExistException"></exception>
//        /// <exception cref="validException"></exception>
//        /// <exception cref="dosntExisetException"></exception>
//        public void addDrone(int droneId, int stationId, string droneModel, Weight weight)
//        {
//            try
//            {
//                if (dal.GetDrones().ToList().Exists(item => item.id == droneId))
//                    throw new AlreadyExistException("Drone already exist");
//                if (!(droneId >= 10000000 && droneId < 1000000000))
//                    throw new validException("the number of the drone id in invalid\n");
//                if (!(weight >= (Weight)1 && weight <= (Weight)3))
//                    throw new validException("the given weight is not valid\n");
//                if (!(stationId >= 10000000 && stationId <= 1000000000))
//                    throw new validException("the number of the drone id in invalid\n");
//                DO.Station stationDl = dal.GetStation(stationId);
//                if (stationDl.latitude < (double)31 || stationDl.latitude > 33.3)
//                    throw new validException("the given latitude do not exist in this country/\n");
//                if (stationDl.longitude < 34.3 || stationDl.longitude > 35.5)
//                    throw new validException("the given longitude do not exist in this country/\n");
//                DroneToList dtl = new DroneToList();
//                dtl.id = droneId;
//                dtl.droneModel = droneModel;
//                dtl.weight = weight;
//                dtl.batteryStatus = (double)rand.Next(20, 40);
//                dtl.droneStatus = DroneStatus.available; ///עכשיו שיניתי את זה שזה יהיה פנוי ולא CHARGE
//                dtl.location = new Location();
//                dtl.location.latitude = stationDl.latitude;
//                dtl.location.longitude = stationDl.longitude;
//                DO.Drone dr = new DO.Drone();
//                {
//                    dr.id = droneId;
//                    dr.model = droneModel;
//                    dr.maxWeight = (WeightCatigories)weight;
//                }
//                dal.addDrone(dr);
//                drones.Add(dtl);
//                DO.droneCharges dc = new DO.droneCharges { droneId = droneId, stationId = stationId };
//                SendToCharge(droneId);
//            }
//            catch (findException exp) { throw new dosntExisetException(exp.Message); }
//        }
//        /// <summary>
//        /// adding a drone to the datasource by recieving features of a drone and throwing an exception of any of the given info was incorrects
//        /// </summary>
//        /// <param name="droneToAdd"></param>
//        /// <param name="stationId"></param>
//        /// <exception cref="AlreadyExistException"></exception>
//        /// <exception cref="validException"></exception>
//        public void addDrone(DroneToList droneToAdd, int stationId)
//        {
//            if (dal.GetDrones().ToList().Exists(item => item.id == droneToAdd.id))
//                throw new AlreadyExistException("Drone already exist");
//            DO.Station stationDl = dal.GetStation(stationId);
//            droneToAdd.location.latitude = stationDl.latitude;
//            droneToAdd.location.longitude = stationDl.longitude;
//            if (droneToAdd.batteryStatus == 0) { droneToAdd.batteryStatus = (double)rand.Next(20, 40); }
//            if (droneToAdd.droneStatus == 0) { droneToAdd.droneStatus = DroneStatus.charge; }
//            if (!(droneToAdd.id >= 10000000 && droneToAdd.id <= 1000000000))
//                throw new validException("the number of the drone id in invalid\n");
//            if (!(droneToAdd.batteryStatus >= (double)0 && droneToAdd.batteryStatus <= (double)100))
//                throw new validException("the status of the drone is invalid\n");
//            if (droneToAdd.location.latitude < (double)31 || droneToAdd.location.latitude > 33.3)
//                throw new validException("the given latitude do not exist in this country/\n");
//            if (droneToAdd.location.longitude < 34.3 || droneToAdd.location.longitude > 35.5)
//                throw new validException("the given longitude do not exist in this country/\n");
//            if (!(droneToAdd.weight >= (Weight)1 && droneToAdd.weight <= (Weight)3))
//                throw new validException("the given weight is not valid\n");
//            if (!(stationId >= 10000000 && stationId <= 1000000000))
//                throw new validException("the number of the station id in invalid\n");
//            droneToAdd.location = getBaseStationLocation(stationId);
//            DO.Drone drone = new DO.Drone();
//            drone.id = droneToAdd.id;
//            drone.model = droneToAdd.droneModel;
//            drone.maxWeight = (WeightCatigories)droneToAdd.weight;
//            dal.addDrone(drone);
//            drones.Add(droneToAdd);
//        }
//        #endregion


//        /// <summary>
//        /// recieving a drone from the data source and returning ot to the progrmmer with the bl Drone (regular) features
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        /// <exception cref="dosntExisetException"></exception>
//        public BO.Drone returnsDrone(int id)
//        {
//            var drtl = GetDrones().Find(x => x.id == id);
//            if (drtl == null)
//                throw new dosntExisetException("Error! the drone doesn't found");
//            BO.Drone d = new();
//            d.id = drtl.id;
//            d.droneModel = drtl.droneModel;
//            d.weight = drtl.weight;
//            d.droneStatus = drtl.droneStatus;
//            d.batteryStatus = drtl.batteryStatus;
//            d.location = new BO.Location();
//            d.location = drtl.location;
//            BO.ParcelInTransfer pt = new BO.ParcelInTransfer();
//            if (drtl.droneStatus == BO.DroneStatus.delivery)
//            {
//                pt.id = drtl.parcelId;
//                DO.Parcel p = new DO.Parcel();
//                try
//                {
//                    p = dal.GetParcel(drtl.parcelId);//get the parcel from the dal
//                }
//                catch (Exception)
//                {
//                    throw new dosntExisetException("Error! the parcel not found");
//                }
//                if (p.pickedUp == null)
//                    pt.parcelStatus = false;
//                else
//                    pt.parcelStatus = true;
//                pt.priority = (BO.Priority)p.priority;
//                pt.weight = (BO.Weight)p.weight;
//                pt.sender = new BO.CustomerInParcel();
//                pt.sender = getCustomerInParcel(p.senderId);
//                pt.receive = new BO.CustomerInParcel();
//                pt.receive = getCustomerInParcel(p.targetId);
//                DO.Customer sender = dal.GetCustomer(p.senderId);
//                DO.Customer target = dal.GetCustomer(p.targetId);
//                pt.collection = new BO.Location();
//                pt.collection.longitude = sender.longitude;
//                pt.collection.latitude = sender.latitude;
//                pt.DeliveryDestination = new BO.Location();
//                pt.DeliveryDestination.longitude = target.longitude;
//                pt.DeliveryDestination.latitude = target.latitude;
//                pt.TransportDistance = Distance(pt.collection, pt.DeliveryDestination);
//                d.parcelInTransfer = new BO.ParcelInTransfer();
//                d.parcelInTransfer = pt;
//            }
//            return d;
//        }
//        //public BO.Drone returnsDrone(int id)
//        //{
//        //    try
//        //    {
//        //        var drn = drones.Find(x => x.id == id);
//        //        if (drn == null)
//        //            throw new Exception("Error! the drone doesn't found");
//        //        BO.Drone d = new();
//        //        d.id = drn.id;
//        //        d.droneModel = drn.droneModel;
//        //        d.weight = drn.weight;
//        //        d.droneStatus = drn.droneStatus;
//        //        d.batteryStatus = drn.batteryStatus;
//        //        d.location = new Location();
//        //        d.location = drn.location;
//        //        ParcelInTransfer pt = new ParcelInTransfer();
//        //        if (drn.droneStatus == DroneStatus.delivery)
//        //        {
//        //            pt.id = drn.parcelId;
//        //            DO.Parcel p = new DO.Parcel();
//        //            try
//        //            {
//        //                p = dal.findParcel(drn.parcelId);//get the parcel from the dal
//        //            }
//        //            catch (Exception)
//        //            {
//        //                throw new Exception("Error! the parcel not found");
//        //            }
//        //            if (p.pickedUp == DateTime.MinValue)
//        //                pt.parcelStatus = false;
//        //            else
//        //                pt.parcelStatus = true;
//        //            pt.priority = (Priority)p.priority;
//        //            pt.weight = (Weight)p.weight;
//        //            pt.sender = new CustomerInParcel();
//        //            pt.sender = getCustomerInParcel(p.senderId);
//        //            pt.receive = new CustomerInParcel();
//        //            pt.receive = getCustomerInParcel(p.targetId);
//        //            DO.Customer sender = dal.findCustomer(p.senderId);
//        //            DO.Customer target = dal.findCustomer(p.targetId);
//        //            pt.collection = new Location();
//        //            pt.collection.longitude = sender.longitude;
//        //            pt.collectionLocation.latitude = sender.lattitude;
//        //            pt.targetLocation = new Location();
//        //            pt.targetLocation.longitude = target.longitude;
//        //            pt.targetLocation.latitude = target.lattitude;
//        //            pt.distance = distance(pt.collectionLocation, pt.targetLocation);

//        //            d.parcel = new ParcelInTransfer();
//        //            d.parcel = pt;
//        //        }
//        //        return d;
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        throw new BLgeneralException(e.Message /, e /);
//        //    }
//        //}


//        //#endregion
//        #region Get Drone
//        /// <summary>
//        /// recieving a drone from the data source and returning ot to the progrmmer with the bl Drone to list features

//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        /// <exception cref="dosntExisetException"></exception>
//        public DroneToList GetDrone(int id)
//        {
//            try
//            {
//                DroneToList droneBo = new DroneToList();
//                DO.Drone droneDo = dal.GetDrone(id);
//                DroneToList drone = drones.ToList().Find(d => d.id == id);
//                droneBo.id = droneDo.id;
//                droneBo.droneModel = drone.droneModel;
//                droneBo.weight = drone.weight;
//                droneBo.location = drone.location;
//                droneBo.batteryStatus = drone.batteryStatus;
//                int parcelID = dal.GetParcelList().ToList().Find(x => x.droneId == droneBo.id).id;
//                //if (parcelID != 0)
//                //{
//                //    droneBo.parcelId = parcelID;
//                //    droneBo.droneStatus = DroneStatus.delivery;
//                //}
//                //else
//                droneBo.droneStatus = drone.droneStatus;
//                return droneBo;
//            }
//            catch (ArgumentNullException exp)
//            {
//                throw new dosntExisetException(exp.Message);
//            }
//            catch (findException exp)
//            {
//                throw new dosntExisetException(exp.Message);
//            }
//        }
//        /// <summary>
//        ///recieving the list of all the drones  from the data source and returning ot to the progrmmer with the bl Drone to list features

//        /// </summary>
//        /// <returns></returns>
//        public List<BO.DroneToList> GetDrones()
//        {
//            List<BO.DroneToList> drones = new List<BO.DroneToList>();
//            foreach (var d in dal.GetDroneList())
//            { drones.Add(GetDrone(d.id)); }
//            return drones;
//        }
//        /// <summary>
//        /// an update function  that updates teh drone name  and model with recieving the drones id
//        /// </summary>
//        /// <param name="droneID"></param>
//        /// <param name="dModel"></param>
//        /// <exception cref="dosntExisetException"></exception>
//        public void updateDroneName(int droneID, string dModel)
//        {
//            int dIndex = drones.FindIndex(x => x.id == droneID);
//            if (dIndex == -1)//לדעת מה הוא מחזיר אם הוא לא מוצא ולשים בתנאי
//            {
//                throw new dosntExisetException("drone do not exist");
//            }
//            try { dal.updateDrone(droneID, dModel); }
//            catch (findException exp) { throw new dosntExisetException(exp.Message); }
//            BO.DroneToList dr = drones.Find(p => p.id == droneID);
//            drones.Remove(dr);
//            dr.droneModel = dModel;
//            drones.Add(dr);
//        }
//        /// <summary>
//        /// an update function that updates the drone  and the station slots when the  drone is sent to be charged
//        /// </summary>
//        /// <param name="droneID"></param>
//        /// <exception cref="dosntExisetException"></exception>
//        /// <exception cref="unavailableException"></exception>
//        /// <exception cref="deleteException"></exception>
//        public void SendToCharge(int droneID)
//        {

//            DroneToList drone = new();
//            BaseStation station = new();
//            try { drone = GetDrone(droneID); }
//            catch (findException exp) { throw new dosntExisetException(exp.Message); }
//            if (drone.droneStatus != DroneStatus.available)
//                throw new unavailableException("the drone is unavailable");
//            Location stationLocation = findClosestStationLocation(drone.location, false, BaseStationLocationslist());//not sure where and what its from
//            station = GetStations().Find(x => x.location.longitude == stationLocation.longitude && x.location.latitude == stationLocation.latitude);
//            int droneIndex = drones.ToList().FindIndex(x => x.id == droneID);
//            dal.deleteDrone(dal.GetDrone(drones[droneIndex].id));
//            if (station.avilableChargeSlots > 0)
//            {
//                dal.deleteStation(dal.GetStation(station.id));
//                station.decreasingChargeSlots();
//                addStation(station);
//            }
//            drones[droneIndex].batteryStatus = calcMinBatteryRequired(drones[droneIndex]);//not sure that if it needs to be 100%
//            drones[droneIndex].location = station.location;
//            drones[droneIndex].droneStatus = DroneStatus.charge;
//            addDrone(drones[droneIndex], station.id);
//            drones.RemoveAt(droneIndex);
//            DO.droneCharges DC = new droneCharges { droneId = droneID, stationId = station.id };
//            dal.AddDroneCharge(DC);
//        }
//        /// <summary>
//        /// an update function that upadets the drone an d the station charging slots when the drone is being released from its chraging
//        /// </summary>
//        /// <param name="droneID"></param>
//        /// <param name="chargingTime"></param>
//        /// <exception cref="findException"></exception>
//        /// <exception cref="unavailableException"></exception>
//        /// <exception cref="dosntExisetException"></exception>
//        public void releasingDrone(int droneID, TimeSpan chargingTime)
//        {
//            //DroneToList droneItem = new();
//            //try { droneItem = GetDrones().Find(x => x.id == droneID); }
//            //catch (DO.findException) { throw new findException(); }
//            //if (droneItem.droneStatus != DroneStatus.charge)
//            //    throw new unavailableException("Cannot relese the drone because he isnt charge");
//            //else
//            //{
//            //    int index = drones.FindIndex(x => x.id == droneID);
//            //    DO.droneCharges DC = new();
//            //    try { DC = dal.chargingGetDroneList().ToList().Find(X => X.droneId == droneID); }
//            //    catch (findException exp) { throw new dosntExisetException(exp.Message); }
//            //    BaseStation bstation = new();
//            //    try { bstation = GetStation(DC.stationId); }
//            //    catch (findException exp) { throw new dosntExisetException(exp.Message); }
//            //    double timeInMinutes = chargingTime.TotalMinutes;//converting the format to number of minutes, for instance, 1:30 to 90 minutes
//            //    timeInMinutes /= 60; //getting the time in hours 
//            //    drones[index].batteryStatus = timeInMinutes * GetChargeCapacity().pwrRateLoadingDrone + droneItem.batteryStatus; // the battery calculation
//            //    if (droneItem.batteryStatus > 100) //battery can't has more than a 100 percent
//            //        droneItem.batteryStatus = 100;
//            //    bstation.addingChargeSlots();
//            //    addStation(bstation);
//            //    dal.RemoveDroneCharge(DC);
//            //    drones[index].droneStatus = DroneStatus.available;
//            //    Console.WriteLine(drones[index].ToString());
//            //    addDrone(drones[index], DC.stationId);
//            //}
//            DroneToList droneItem = new();
//            try { droneItem = GetDrones().Find(x => x.id == droneID); }
//            catch (DO.findException) { throw new findException(); }
//            if (droneItem.droneStatus != DroneStatus.charge)
//                throw new unavailableException("Cannot relese the drone because he isnt charge");
//            else
//            {
//                int index = drones.FindIndex(x => x.id == droneID);
//                DO.droneCharges DC = new();
//                try { DC = dal.chargingGetDroneList().ToList().Find(X => X.droneId == droneID); }
//                catch (findException exp) { throw new dosntExisetException(exp.Message); }
//                BaseStation bstation = new();
//                try { bstation = GetStation(DC.stationId); }
//                catch (findException exp) { throw new dosntExisetException(exp.Message); }
//                double timeInMinutes = chargingTime.TotalMinutes;//converting the format to number of minutes, for instance, 1:30 to 90 minutes
//                timeInMinutes /= 60; //getting the time in hours 
//                drones[index].batteryStatus = timeInMinutes * GetChargeCapacity().pwrRateLoadingDrone + droneItem.batteryStatus; // the battery calculation
//                if (droneItem.batteryStatus > 100) //battery can't has more than a 100 percent
//                    droneItem.batteryStatus = 100;
//                dal.deleteStation(dal.GetStation(DC.stationId));
//                bstation.addingChargeSlots();
//                addStation(bstation);
//                dal.RemoveDroneCharge(DC);
//                dal.deleteDrone(dal.GetDrone(droneID));
//                drones[index].droneStatus = DroneStatus.available;
//                Console.WriteLine(drones[index].ToString());
//                addDrone(drones[index], DC.stationId);
//            }
//        }
//        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate = null)
//        {
//            if (predicate == null)
//            {
//                return drones.Take(drones.Count).ToList();
//            }
//            return drones.Where(predicate).ToList();
//        }
//    }
//}
//#endregion
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
        public void addDrone(int droneId, int stationId, string droneModel, Weight weight)
        {
            try
            {
                if (dal.GetDrones().ToList().Exists(item => item.id == droneId))
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
                dtl.droneStatus = DroneStatus.available; ///עכשיו שיניתי את זה שזה יהיה פנוי ולא CHARGE
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
        public void addDrone(DroneToList droneToAdd, int stationId)
        {
            if (dal.GetDrones().ToList().Exists(item => item.id == droneToAdd.id))
                throw new AlreadyExistException("Drone already exist");
            DO.Station stationDl = dal.GetStation(stationId);
            droneToAdd.location.latitude = stationDl.latitude;
            droneToAdd.location.longitude = stationDl.longitude;
            if (droneToAdd.batteryStatus == 0) { droneToAdd.batteryStatus = (double)rand.Next(20, 40); }
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
            DO.Drone drone = new DO.Drone();
            drone.id = droneToAdd.id;
            drone.model = droneToAdd.droneModel;
            drone.maxWeight = (WeightCatigories)droneToAdd.weight;
            dal.addDrone(drone);
            drones.Add(droneToAdd);
        }
        #endregion
        /// <summary>
        /// deleting a drone for the datasource  and throwing an exception of the drone was not found
        /// </summary>
        /// <param name="droneID"></param>
        /// <exception cref="deleteException"></exception>
        #region DELETE DRONE
        public void deleteDrone(int droneID)
        {
            try
            {
                if (GetDrone(droneID).droneStatus == DroneStatus.charge)
                { var DC = dal.chargingGetDroneList().ToList().Find(x => x.droneId == droneID); dal.RemoveDroneCharge(DC); }
                dal.deleteDrone(dal.GetDrone(droneID));

            }
            catch (findException exp) { throw new deleteException("cant delete this drone\n"); }
        }
        #endregion
        /// <summary>
        /// recieving a drone from the data source and returning ot to the progrmmer with the bl Drone (regular) features
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
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
                if (p.pickedUp == null)
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
        //#endregion
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
        }
        /// <summary>
        ///recieving the list of all the drones  from the data source and returning ot to the progrmmer with the bl Drone to list features

        /// </summary>
        /// <returns></returns>
        public List<BO.DroneToList> GetDrones()
        {
            List<BO.DroneToList> drones = new List<BO.DroneToList>();
            foreach (var d in dal.GetDroneList())
            { drones.Add(GetDrone(d.id)); }
            return drones;
        }
        /// <summary>
        /// an update function  that updates teh drone name  and model with recieving the drones id
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="dModel"></param>
        /// <exception cref="dosntExisetException"></exception>
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
            if (station.avilableChargeSlots > 0)
            {
                dal.deleteStation(dal.GetStation(station.id));
                station.decreasingChargeSlots();
                addStation(station);
            }
            drones[droneIndex].batteryStatus = calcMinBatteryRequired(drones[droneIndex]);//not sure that if it needs to be 100%
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.charge;
            try { deleteDrone(droneID); }
            catch (deleteException exp) { throw new deleteException(exp.Message); }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
            addDrone(drones[droneIndex], station.id);
            DO.droneCharges DC = new droneCharges { droneId = droneID, stationId = station.id };
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
        public void releasingDrone(int droneID, TimeSpan chargingTime)
        {
            DroneToList droneItem = new();
            try { droneItem = GetDrones().Find(x => x.id == droneID); }
            catch (DO.findException) { throw new findException(); }
            if (droneItem.droneStatus != DroneStatus.charge)
                throw new unavailableException("Cannot relese the drone because he isnt charge");
            else
            {
                int index = drones.FindIndex(x => x.id == droneID);
                DO.droneCharges DC = new();
                try { DC = dal.chargingGetDroneList().ToList().Find(X => X.droneId == droneID); }
                catch (findException exp) { throw new dosntExisetException(exp.Message); }
                BaseStation bstation = new();
                try { bstation = GetStation(DC.stationId); }
                catch (findException exp) { throw new dosntExisetException(exp.Message); }
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
        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate = null)
        {
            if (predicate == null)
            {
                return drones.Take(drones.Count).ToList();
            }
            return drones.Where(predicate).ToList();
        }
    }
}
#endregion







