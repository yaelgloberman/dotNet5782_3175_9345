using IDAL.DO;
using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
namespace DalObject
{
    public partial class DalObject
    {
        public bool checkDrone(int id)
        {
            return DataSource.drones.Any(d => d.id == id);

        }
        public void addDrone(Drone d)
        {
            if (DataSource.Customers.Exists(item => item.id == d.id))
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
            stationList().ToList().ForEach(s => { if (s.id == stationId) tmpS = s; });
            stationList().ToList().RemoveAll(s => s.id == dCharge.stationId);
            tmpS.chargeSlots--;
            stationList().ToList().Add(tmpS);
            dCharge.droneId = droneId;
            dCharge.stationId = stationId;
            chargingDroneList().ToList().Add(dCharge);//adding the drone to the drone chargiong list
        }
        public void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        {
            Drone tmpD = GetDrone(dC.droneId);
            Station tmpS = GetStation(dC.stationId);
            DataSource.drones.RemoveAll(m => m.id == dC.droneId);//removing the parcel with the given id
            DataSource.stations.RemoveAll(s => s.id == dC.stationId);
            //tmpD.status = DroneStatuses.available;
            //tmpD.bateryStatus = 100;
            droneList().ToList().Add(tmpD);
            tmpS.chargeSlots++;
            stationList().ToList().Add(tmpS);
            DataSource.chargingDrones.Remove(dC);//removing the drone from the drone charging list
        }
        public void deleteDrone(Drone d)
        {
            if (!DataSource.drones.Exists(item => item.id == d.id))
                throw new findException("Customer");
            DataSource.drones.Remove(d);
        }
    }

}


