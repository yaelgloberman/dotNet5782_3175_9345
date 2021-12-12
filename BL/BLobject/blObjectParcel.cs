﻿using System;
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
        #region Add Parcel
        public int addParcel(IBL.BO.Parcel parcelToAdd)
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
            IDAL.DO.Parcel parcelDo = new IDAL.DO.Parcel();
            parcelDo.senderId = parcelToAdd.sender.id;
            parcelDo.targetId = parcelToAdd.receive.id;
            parcelDo.weight = (WeightCatigories)parcelToAdd.weightCategorie;
            parcelDo.priority = (Proirities)parcelToAdd.priority;
            parcelDo.requested = DateTime.Now;
            parcelDo.scheduled = null;
            parcelDo.pickedUp = null;
            parcelDo.delivered = null;
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
                parcel.receiveName = dal.GetCustomer(dalParcel.targetId).name;
                parcel.weight = (Weight)dalParcel.weight;
                parcel.senderName = dal.GetCustomer(dalParcel.senderId).name;
                if (dalParcel.pickedUp != null)
                    parcel.parcelStatus = IBL.BO.ParcelStatus.PickedUp;
                if (dalParcel.requested != null)
                    parcel.parcelStatus = IBL.BO.ParcelStatus.Assigned;
                if (dalParcel.scheduled != null)
                    parcel.parcelStatus = IBL.BO.ParcelStatus.Created;
                if (dalParcel.delivered != null)
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
                drone.batteryStatus = previoseBatteryStatus - (Distance(drone.location, Location) * GetChargeCapacity().chargeCapacityArr[(int)drone.weight]);
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
            catch (findException exp) { throw new dosntExisetException(exp.Message); }

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
                    if (item.pickedUp == null && item.delivered == null)
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
                        int parcelNewID = dal.addParcel(par);
                        d.parcelId = parcelNewID;
                        var station = GetStationByDrone(d);
                        addDrone(d, station.id);
                        return;
                    }
                }
            }
            throw new validException("the drone cant picked up the parcel because the parcel is not matching to him");
        }


    }
}