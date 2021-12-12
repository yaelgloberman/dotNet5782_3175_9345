using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDal
    {

        public droneCharges GetChargedDrone(int id)//finding a drone in the drone charging list
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

                throw new IDAL.DO.findException("drone does not exist");
            }
            return (droneCharges)tmp;
        }
        public void AddDroneCharge(droneCharges droneCharges)
        {
            DataSource.chargingDrones.Add(droneCharges);
        }
        public void RemoveDroneCharge(droneCharges droneCharges)
        {
            DataSource.chargingDrones.Remove(droneCharges);
        }
        public IEnumerable<droneCharges> GetDroneIdInStation(int id)
        {
            List<droneCharges> droneChargesList = new List<droneCharges>();
            DataSource.chargingDrones.ForEach(cd => { if (cd.stationId == id) droneChargesList.Add(cd); });
            return droneChargesList;
        }
        public IEnumerable<droneCharges> GetChargedDrone(Func<droneCharges, bool> predicate = null)
         => predicate == null ? DataSource.chargingDrones : DataSource.chargingDrones.Where(predicate);
    }

}

