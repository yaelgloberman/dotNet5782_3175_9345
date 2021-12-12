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
        public IEnumerable<DroneToList> droneFilterStatus(DroneStatus s)
        {
            List<DroneToList> lst = new List<DroneToList>();

            foreach (var item in drones)
            {
                if (item.droneStatus == s)
                    lst.Add(item);
            }
            return lst;
        }
        public IEnumerable<DroneToList> droneFilterWeight(Weight w)
        {
            List<DroneToList> lst = new List<DroneToList>();

            foreach (var item in drones)
            {
                if (item.weight == w)
                    lst.Add(item);
            }
            return lst;
        }


        #region ADD Drone
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
        }
        public void addDrone(DroneToList droneToAdd, int stationId)
        {
            if (dal.GetDrones().ToList().Exists(item => item.id == droneToAdd.id))
                throw new AlreadyExistException("Drone already exist");
            IDAL.DO.Station stationDl = dal.GetStation(stationId);
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
            IDAL.DO.Drone drone = new IDAL.DO.Drone();
            drone.id = droneToAdd.id;
            drone.model = droneToAdd.droneModel;
            drone.maxWeight = (WeightCatigories)droneToAdd.weight;
            dal.addDrone(drone);
            drones.Add(droneToAdd);
        }
        #endregion
        #region DELETE DRONE
        public void deleteDrone(int droneID)
        {
            try
            {
                if (GetDrone(droneID).droneStatus == DroneStatus.charge)
                { var DC = dal.chargingDroneList().ToList().Find(x => x.droneId == droneID); dal.RemoveDroneCharge(DC); }
                dal.deleteDrone(dal.GetDrone(droneID));

            }
            catch (findException exp) { throw new deleteException("cant delete this drone\n"); }
        }
        #endregion
        #region returns drone
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
                if (p.pickedUp == null)
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
        #endregion
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
        public List<IBL.BO.DroneToList> GetDrones()
        {
            List<IBL.BO.DroneToList> drones = new List<IBL.BO.DroneToList>();
            foreach (var d in dal.droneList())
            { drones.Add(GetDrone(d.id)); }
            return drones;
        }
        public List<IDAL.DO.Drone> GetDronesFake()
        {
            return dal.GetDrones().ToList();
        }
        public void updateDroneName(int droneID, string dModel)
        {
            int dIndex = drones.FindIndex(x => x.id == droneID);
            if (dIndex == 0)//לדעת מה הוא מחזיר אם הוא לא מוצא ולשים בתנאי
            {
                throw new dosntExisetException("drone do not exist");
            }
            try { dal.updateDrone(droneID, dModel); }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
            IBL.BO.DroneToList dr = drones.Find(p => p.id == droneID);
            drones.Remove(dr);
            dr.droneModel = dModel;
            drones.Add(dr);
        }
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
                station.decreasingChargeSlots();
            drones[droneIndex].batteryStatus = calcMinBatteryRequired(drones[droneIndex]);//not sure that if it needs to be 100%
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.charge;
            try { deleteDrone(droneID); }
            catch (deleteException exp) { throw new deleteException(exp.Message); }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
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
                throw new unavailableException("Cannot relese the drone because he isnt charge");
            else
            {
                int index = drones.FindIndex(x => x.id == droneID);
                IDAL.DO.droneCharges DC = new();
                try { DC = dal.chargingDroneList().ToList().Find(X => X.droneId == droneID); }
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
    }
}
#endregion
