using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DalApi;
using DO;
namespace Dal
{
    sealed partial class DalObject : IDal
    {
        #region Get Charged Drone
        public droneCharges GetChargedDrone(int id)
        {
            droneCharges? tmp = null;
            foreach (droneCharges d in DataSource.chargingDrones)//hi
            {
                if (d.droneId == id)
                {
                    tmp = d;
                    break;
                }
            }
            if (tmp == null)
            {

                throw new DO.findException("drone does not exist");
            }
            return (droneCharges)tmp;
        }
        #endregion
        #region Add Drone Charge
        public void AddDroneCharge(droneCharges droneCharges)
        {
            DataSource.chargingDrones.Add(droneCharges);
        }
        #endregion
        #region Remove Drone Charge
        public void RemoveDroneCharge(droneCharges droneCharges)
        {
            DataSource.chargingDrones.Remove(droneCharges);
        }
        #endregion
        #region Get Drone Id In Station
        public IEnumerable<droneCharges> GetDroneIdInStation(int id)
        {
            List<droneCharges> droneChargesList = new List<droneCharges>();
            DataSource.chargingDrones.ForEach(cd => { if (cd.stationId == id) droneChargesList.Add(cd); });
            return droneChargesList;
        }
        #endregion
        #region Get Charge dDrone
        public IEnumerable<droneCharges> GetChargedDrone(Func<droneCharges, bool> predicate = null)
         => predicate == null ? DataSource.chargingDrones : DataSource.chargingDrones.Where(predicate);
        #endregion
    }

}

