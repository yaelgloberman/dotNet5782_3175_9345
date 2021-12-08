using IDAL.DO;
//using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
namespace DalObject
{
    
    public partial class DalObject:IDal
    {
        public bool checkDrone(int id)
        {
            return DataSource.drones.Any(d => d.id == id);

        }
        public void addDrone(Drone d)
        {
            if (DataSource.drones.Exists(item => item.id == d.id))
                throw new AddException("drone already exist");
            DataSource.drones.Add(d);
        }
        public Drone GetDrone(int id)//function that gets id and finding the drone in the drones list and returns drone 
        {
            Drone? tmp = null;
            foreach (Drone d in DataSource.drones)
            {
                if (d.id == id)
                {
                    tmp = d;
                    break;
                }
            }
            if (tmp == null)
            {

                throw new IDAL.DO.findException("drone does not exist");
            }
            return (Drone)tmp;
        }
        public void SendToCharge(int droneId, int stationId)//update function that updates the station and drone when the drone is sent to chatge
        {
            GetDrone(droneId);
            GetStation(stationId);
            droneCharges dCharge = new droneCharges();
            Station tmpS = new Station();
            dCharge.stationId = stationId;//maching the drones id

            DataSource.stations.ForEach(s => { if (s.id == stationId) tmpS = s; });
            DataSource.stations.RemoveAll(s => s.id == dCharge.stationId);
            tmpS.chargeSlots--;
            DataSource.stations.Add(tmpS);
            dCharge.droneId = droneId;
            dCharge.stationId = stationId;
            DataSource.chargingDrones.Add(dCharge);//adding the drone to the drone chargiong list
        }
        public void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        {
            Drone tmpD = GetDrone(dC.droneId);
            Station tmpS = GetStation(dC.stationId);
            DataSource.drones.RemoveAll(m => m.id == dC.droneId);//removing the parcel with the given id
            DataSource.stations.RemoveAll(s => s.id == dC.stationId);
            //tmpD.status = DroneStatuses.available;
            //tmpD.bateryStatus = 100;
            DataSource.drones.Add(tmpD);
            tmpS.chargeSlots++;
            stationList().ToList().Add(tmpS);
            DataSource.chargingDrones.Remove(dC);//removing the drone from the drone charging list
        }
        public void updateDrone(int droneId,string droneModel)
        {
            bool flag = false;
           
            DataSource.drones.ForEach(d => { if (d.id == droneId) { d.model = droneModel; flag = true; } }); 
            if (!flag)
            {
                throw new findException("could not find drone");
            }

         }
        public void deleteDrone(Drone d)
        {
            if (!DataSource.drones.Exists(item => item.id == d.id))
                throw new findException("Customer");
            DataSource.drones.Remove(d);
        }
        public IEnumerable<Drone> GetDrones()
        {
            return DataSource.drones;
        }
    }

}


//public BL()
//{
//    DalObject.DalObject dal = new DalObject.DalObject();
//    //}
//    //private void function()
//    //{
//    bool flag = false;
//    Random rnd = new Random();
//    double minBatery = 0;
//    var DroneArr = new List<DroneToList>();
//    IEnumerable<IDAL.DO.Drone> d = dal.GetDrones();
//    IEnumerable<IDAL.DO.Parcel> p = dal.GetParcels();
//    chargeCapacity chargeCapacity = new();
//    foreach (var item in d)
//    {
//        IBL.BO.DroneToList drt = new DroneToList();
//        drt.id = item.id;
//        drt.droneModel = item.model;
//        foreach (var pr in p)
//        {
//            if (pr.id == item.id && pr.delivered == DateTime.MinValue)
//            {
//                IDAL.DO.Customer sender = dal.GetCustomer(pr.senderId);
//                IDAL.DO.Customer target = dal.GetCustomer(pr.targetId);
//                IBL.BO.Location senderLocation = new Location { latitude = sender.latitude, longitude = sender.longitude };
//                IBL.BO.Location targetLocation = new Location { latitude = target.latitude, longit
//
//                ude = target.longitude };
//                drt.droneStatus = DroneStatus.delivery;
//                if (pr.pickedUp == DateTime.MinValue && pr.scheduled != DateTime.MinValue)//החבילה שויכה אבל עדיין לא נאספה
//                {
//                    drt.location = new Location { latitude = findClosetBaseStationLocation(senderLocation, false).latitude, longitude = findClosetBaseStationLocation(senderLocation, false).longitude };
//                    minBatery = Distance(drt.location, senderLocation) * chargeCapacity.chargeCapacityArr[0];
//                    minBatery += Distance(senderLocation, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.weight];
//                    minBatery += Distance(targetLocation, new Location { latitude = findClosetBaseStationLocation(targetLocation, false).latitude, longitude = findClosetBaseStationLocation(targetLocation, false).longitude }) * chargeCapacity.chargeCapacityArr[0];
//                }
//                if (pr.pickedUp != DateTime.MinValue && pr.delivered == DateTime.MinValue)//החבילה נאספה אבל עדיין לא הגיעה ליעד
//                {
//                    drt.location = new Location();
//                    drt.location = senderLocation;
//                    minBatery = Distance(targetLocation, new Location { latitude = findClosetBaseStationLocation(targetLocation, false).latitude, longitude = findClosetBaseStationLocation(targetLocation, false).longitude }) * chargeCapacity.chargeCapacityArr[0];
//                    minBatery += Distance(drt.location, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.weight];
//                }
//                drt.batteryStatus = rnd.Next((int)minBatery, 101); // 100/;
//                flag = true;
//                break;
//            }
//        }

//        if (!flag)
//        {
//            int temp = rnd.Next(1, 3);
//            if (temp == 1)
//                drt.droneStatus = IBL.BO.DroneStatus.available;
//            else
//                drt.droneStatus = IBL.BO.DroneStatus.charge;
//            if (drt.droneStatus == IBL.BO.DroneStatus.charge)
//            {
//                int r = rnd.Next(0, dal.getStations().Count()), i = 0;
//                IDAL.DO.Station s = new IDAL.DO.Station();
//                foreach (var ite in dal.getStations())
//                {
//                    s = ite;
//                    if (i == r)
//                        break;
//                    i++;
//                }
//                drt.location = new Location { latitude = s.latitude, longitude = s.longitude };
//                drt.batteryStatus = rnd.Next(0, 21); // 100/;
//            }
//            else
//            {
//                List<IDAL.DO.Customer> lst = new List<IDAL.DO.Customer>();
//                foreach (var pr in p)
//                {
//                    if (pr.delivered != DateTime.MinValue)
//                        lst.Add(dal.GetCustomer(pr.targetId));
//                }
//                if (lst.Count == 0)
//                {
//                    foreach (var pr in dal.CustomerList())
//                    {

//                        lst.Add(pr);
//                    }
//                }
//                int l = rnd.Next(0, lst.Count());

//                drt.location = new Location { latitude = lst[l].latitude, longitude = lst[l].longitude };
//                Location Location1 = new Location { latitude = lst[l].latitude, longitude = lst[l].longitude };

//                minBatery += Distance(drt.location, new Location { longitude = findClosetBaseStationLocation(Location1, false).longitude, latitude = findClosetBaseStationLocation(Location1, false).latitude }) * chargeCapacity.chargeCapacityArr[0];

//                drt.batteryStatus = rnd.Next((int)minBatery, 101);/// 100//*/;
//            }

//        }
//        DroneArr.Add(drt);


//    }

//}

