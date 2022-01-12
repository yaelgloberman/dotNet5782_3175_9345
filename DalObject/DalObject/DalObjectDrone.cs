using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
namespace Dal
{
    sealed partial class DalObject : IDal
    {
        #region check drone
        public bool checkDrone(int id)
        {
            return DataSource.drones.Any(d => d.id == id);
        }
        #endregion
        #region add drone
        public void addDrone(Drone d)
        {
            if (DataSource.drones.Exists(item => item.id == d.id))
                throw new AddException("drone already exist");
            DataSource.drones.Add(d);
        }
        #endregion
        #region get drone
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

                throw new DO.findException("drone does not exist");
            }
            return (Drone)tmp;
        }
        #endregion
        #region send to charge
        public void SendToCharge(int droneId, int stationId)//update function that updates the station and drone when the drone is sent to chatge
        {

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
        #endregion
        #region releasing drone
        public void releasingDrone(droneCharges dC)//update function when we release a drone from its charging slot
        {
            Drone tmpD = GetDrone(dC.droneId);
            Station tmpS = GetStation(dC.stationId);
            DataSource.drones.RemoveAll(m => m.id == dC.droneId);//removing the parcel with the given id
            DataSource.stations.RemoveAll(s => s.id == dC.stationId);
            DataSource.drones.Add(tmpD);
            tmpS.chargeSlots++;
            GetStationList().ToList().Add(tmpS);
            DataSource.chargingDrones.Remove(dC);//removing the drone from the drone charging list
        }
        #endregion
        #region update drone
        public void updateDrone(int droneId, string droneModel)
        {
            bool flag = false;
            DataSource.drones.ForEach(d => { if (d.id == droneId) { d.model = droneModel; flag = true; } });
            if (!flag)
            {
                throw new findException("could not find drone");
            }
        }
        #endregion
        #region deleteDrone
        public void deleteDrone(Drone d)
        {
            if (!DataSource.drones.Exists(item => item.id == d.id))
                throw new findException("drone");
            DataSource.drones.Remove(d);
        }
        #endregion
        #region IEnumerable Drone List
        public IEnumerable<Drone> IEDroneList(Func<Drone, bool> predicate = null)
        {
            List<Drone> droneList = new List<Drone>();
            if (predicate == null)
            {
                droneList = DataSource.drones;
                return droneList;
            }
            return DataSource.drones.Where(predicate).ToList();
        }
        #endregion
    }
}


