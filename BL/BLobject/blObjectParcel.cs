﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;
using BlApi;
using DalApi;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
namespace BL
{  
    public partial class BL : IBl
    {
        #region Add Parcel
        /// <summary>
        /// adding a parcel to the datasource with all the bl fetures and throwing an exception if any of the users input was incorrect
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int addParcel(BO.Parcel parcelToAdd)
        {
            if (dal.GetParcels().ToList().Exists(item => item.id == parcelToAdd.id))
                throw new AlreadyExistException("Parcel already exist");
            if (!(parcelToAdd.sender.id >= 10000000 && parcelToAdd.sender.id <= 1000000000))
                throw new validException("the id sender number of the pardel is invalid\n");
            if (!(parcelToAdd.receive.id >= 10000000 && parcelToAdd.receive.id <= 1000000000))
                throw new validException("the id receive number of the parcel is invalid\n");
            if (!(parcelToAdd.weightCategorie >= (Weight)1 && parcelToAdd.weightCategorie <= (Weight)3))
                throw new validException("the given weight is not valid\n");
            if (!(parcelToAdd.priority >= (Priority)0 && parcelToAdd.priority <= (Priority)3))
                throw new validException("the given priority is not valid\n");
            DO.Parcel parcelDo = new()
            {
                id=parcelToAdd.id,
                senderId = parcelToAdd.sender.id,
                targetId = parcelToAdd.receive.id,
                weight = (WeightCatigories)parcelToAdd.weightCategorie,
                priority = (Proirities)parcelToAdd.priority,
                requested =DateTime.Now,
                scheduled = null,
                pickedUp = null,
                delivered = null,
                droneId =0
            };
           
            try
            {
                return dal.addParcel(parcelDo);
            }
            catch (AddException exp)
            {
                throw new AlreadyExistException(exp.Message);
            }
        }
        /// <summary>
        /// recieving a parcel from the data source and returning ot to the progrmmer with the bl parcel to list features
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        #endregion
        #region Get parcels
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.ParcelToList GetParcelToList(int id)
        {
            BO.ParcelToList parcel = new();
            DO.Parcel dalParcel = new();
            try
            {
                dalParcel = dal.GetParcel(id);
                var droneIP = GetDrones().Find(x=>x.id==dalParcel.droneId);
                if (droneIP != null)
                {  var droneInParcel = new DroneInParcel { id = dalParcel.droneId, battery = droneIP.batteryStatus, location = droneIP.location };}
                parcel.id = dalParcel.id;
                parcel.priority = (BO.Priority)dalParcel.priority;
                parcel.receiveName = dal.GetCustomer(dalParcel.targetId).name;
                parcel.weight = (Weight)dalParcel.weight;
                parcel.senderName = dal.GetCustomer(dalParcel.senderId).name;
                parcel.parcelStatus= parcelStatus(GetParcel(parcel.id));
               
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            return parcel;
        }
        /// <summary>
        ///         /// recieving a parcel from the data source and returning ot to the progrmmer with the bl parcel (regular) features

        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Parcel GetParcel(int id)
        {
            try
            {
                BO.Parcel parcel = new BO.Parcel();
                DO.Parcel dalParcel = new DO.Parcel();
                dalParcel = dal.GetParcel(id);
                var droneIP = GetDrones().ToList().Find(x => x.id == dalParcel.droneId);
                if (droneIP != null)
                {
                    var droneInParcel = new DroneInParcel { id = dalParcel.droneId, battery = droneIP.batteryStatus, location = droneIP.location };
                    parcel.droneInParcel = droneInParcel;
                }
                parcel.id = dalParcel.id;
                parcel.priority = (BO.Priority)dalParcel.priority;
                parcel.receive = new CustomerInParcel { id = dal.GetCustomer(dalParcel.targetId).id, name = dal.GetCustomer(dalParcel.targetId).name };
                parcel.weightCategorie = (Weight)dalParcel.weight;
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
        #region returns list of parcels 
        /// <summary>
        // recieving a list of all the parcels from the data source and returning ot to the progrmmer with the bl parcel (regular) features

        /// </summary>
        /// <returns></returns>
        public List<BO.Parcel> GetParcels()
        {
            List<BO.Parcel> parcels = new List<BO.Parcel>();
            try
            {
                foreach (var p in dal.GetParcelList())
                { parcels.Add(GetParcel(p.id)); }
            }
            catch (dosntExisetException exp)
            { throw new dosntExisetException(exp.Message); }
            return parcels;
        }
        public List<BO.ParcelToList> GetParcelToLists()
        {
            List<ParcelToList> lst = new List<ParcelToList>();//create the list
            foreach (var item in dal.GetParcelList())//pass on the list of the parcels and copy them to the new list
            {
                lst.Add(GetParcelToList(item.id));
            }
            return lst;
        }

        #endregion
        #region delivery Parcel To Customer
        /// <summary>
        /// upadte function that updates the drone nd the parcel when the package was delivvered to the customer
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="dosntExisetException"></exception>
        /// <exception cref="ExecutionTheDroneIsntAvilablle"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void deliveryParcelToCustomer(int id)
        {
    
            var drone = new BO.DroneToList();
            try
            {
                drone = GetDrone(id);
                DO.Parcel parcel = dal.GetParcelList().ToList().Find(p => p.droneId == id);
                var customer = GetCustomer(parcel.targetId);
                var stationLoc = findClosestStationLocation(customer.location, false, BaseStationLocationslist());
                DO.Station station = dal.GetStationList().ToList().Find(s => s.longitude == stationLoc.longitude && s.latitude == stationLoc.latitude);
                double previoseBatteryStatus = drone.batteryStatus;
                if (!(drone.droneStatus == DroneStatus.delivery))   //the drone pickedup but didnt delivert yet
                    throw new ExecutionTheDroneIsntAvilablle(" this drone is not in delivery");
                var Location = new Location { longitude = station.longitude, latitude = station.latitude };
                drone.parcelId = 0;
                drone.batteryStatus = previoseBatteryStatus - (Distance(drone.location, Location) * GetChargeCapacity().chargeCapacityArr[(int)drone.weight]);
                drone.location.latitude = station.latitude;
                drone.location.longitude = station.longitude;
                drone.droneStatus = DroneStatus.available;
                dal.deleteDrone(dal.GetDrone(id));
                int droneIndex = drones.FindIndex(x => x.id == id);
                drones.RemoveAt(droneIndex);
                addDrone(drone, station.id);
                var parcelBL = GetParcel(parcel.id);
                dal.DeliveryPackageCustomer(parcelBL.receive.id, parcelBL.id, (Proirities)parcelBL.priority);
                //parcelBL.droneInParcel.id = 0;
                //parcelBL.delivered=DateTime.Now;
                //dal.deleteParcel(dal.GetParcel(parcel.id));
                //parcel.droneId = 0;
                //parcel.delivered = DateTime.Now;
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            catch (ExecutionTheDroneIsntAvilablle exp) { throw new ExecutionTheDroneIsntAvilablle(exp.Message); }
        }
        #endregion
        #region matching Drone To Parcel
        /// <summary>
        /// an update function that matches a drone to its parcel - updates the drone and the parcel while finding the perfect parcel to match the drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <exception cref="unavailableException"></exception>
        /// <exception cref="dosntExisetException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void matchingDroneToParcel(int droneId)
        {
           
            try
            {
                var myDrone = GetDrone(droneId);
                var droneLoc = findClosestStationLocation(myDrone.location, false, BaseStationLocationslist());
                var station = GetStations().ToList().Find(x => x.location.longitude == droneLoc.longitude && x.location.latitude == droneLoc.latitude);
                if (myDrone.droneStatus != DroneStatus.available)
                    throw new unavailableException("the drone is unavailable\n");
                DO.Parcel myParcel = findTheParcel(myDrone.weight, myDrone.location, myDrone.batteryStatus, DO.Proirities.emergency);
                dal.attribute(myDrone.id, myParcel.id);
                int index = drones.FindIndex(x => x.id == droneId);
                drones.RemoveAt(index);
                DO.Drone drone = dal.GetDrone(myDrone.id);
                dal.deleteDrone(drone);
                myDrone.droneStatus = DroneStatus.delivery;
                myDrone.parcelId = myParcel.id;
                addDrone(myDrone, station.id);
            }
            catch (findException exp) { throw new dosntExisetException(exp.Message); }
            catch(dosntExisetException ex) { throw new Exception(ex.Message); }
            catch(validException ex) { throw new validException(ex.Message); }
        }
        #endregion
        #region returns parcel status
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelStatus parcelStatus(BO.Parcel parcel)
        {
            if (parcel.delivered != null)
                return ParcelStatus.Delivered;
            if (parcel.pickedUp != null)
                return ParcelStatus.PickedUp;
            if (parcel.scheduled == null)
                return ParcelStatus.Created;
            if (parcel.scheduled != null)
                return ParcelStatus.Assigned;

            else
                return ParcelStatus.Created;
        }
        #endregion
        #region pick up parcel by drone
        /// <summary>
        /// an update function that upadtes the parcel an dthe drone when the parcel was picked up by the drone but was still not delivered to its customer 
        /// </summary>
        /// <param name="droneID"></param>
        /// <exception cref="dosntExisetException"></exception>
        /// <exception cref="validException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void pickedUpParcelByDrone(int droneID)
        {
            var d = GetDrones().Find(x => x.id == droneID);
            if (d == null)
                throw new dosntExisetException("the drone dosnt exist");
            foreach (var item in dal.GetParcels())
            {
                if (item.droneId == droneID)
                {
                    if (item.pickedUp == null && item.delivered == null)
                    {
                        int index = GetDrones().FindIndex(x => x.id == d.id);
                        drones.RemoveAt(index);
                        dal.deleteDrone(dal.GetDrone(droneID));
                        d.batteryStatus = d.batteryStatus - Distance(d.location, new BO.Location { latitude = dal.GetCustomer(item.targetId).latitude, longitude = dal.GetCustomer(item.targetId).longitude }) * chargeCapacity[(int)(item.weight)];
                        d.location.longitude = dal.GetCustomer(item.senderId).longitude;
                        d.location.latitude = dal.GetCustomer(item.senderId).latitude;
                        var par = item;
                        dal.deleteParcel(item);
                        par.scheduled = DateTime.Now.AddHours(-10);
                        par.pickedUp = DateTime.Now;
                        int parcelNewID = dal.addParcel(par);
                        d.parcelId = parcelNewID;
                        d.droneStatus = DroneStatus.delivery;
                        var station = GetStationByDrone(d);
                        addDrone(d, station.id);
                        return;
                    }
                }
            }
            throw new validException("the drone cant picked up the parcel because the parcel is not matching to him");
        }
        #endregion
        #region returns list of parcels
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BO.Parcel> allParcels(Func<BO.Parcel, bool> predicate)
        {
            if (predicate == null)
            {
                return GetParcels().ToList();
            }
            return GetParcels().Where(predicate).ToList();
        }
        public IEnumerable<ParcelToList> allParcelsToList(Func<ParcelToList, bool> predicate = null)
        {
            var parcel = GetParcelToLists();
            if (predicate == null)
            {
                return parcel.Take(parcel.Count).ToList();
            }
            return parcel.Where(predicate).ToList();
        }
        #endregion
        #region delete parcel
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void deleteParcel(int parcelId)
        {
            try
            {
                dal.deleteParcel(dal.GetParcel(parcelId));

            }
            catch (findException) { throw new deleteException("cant delete this parcel\n"); }
        }
        #endregion
    }
}
