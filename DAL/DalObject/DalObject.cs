using IDAL.DO;
//using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalObject
{
    public partial class DalObject : IDal
    {
        public DalObject()
        {
            DataSource.Initialize();///intialize constructor for the data object ib the nmain.
        }
        public double[] ChargeCapacity()
        {

            double[] arr = { DataSource.Config.available, DataSource.Config.light, DataSource.Config.average, DataSource.Config.heavy, DataSource.Config.rateLoadingDrone };
            return arr;
        }
        public IEnumerable<droneCharges> chargingDroneList()
        {
            if (DataSource.chargingDrones.ToList() == null)
                throw new System.ArgumentException("charging drone" + " list =null");
            return DataSource.chargingDrones.ToList();
        }
        public IEnumerable<Customer> CustomerList()
        {
            if (DataSource.Customers.ToList() == null)
                throw new System.ArgumentException("drons list =null");
            return DataSource.Customers.ToList();
        }
        public IEnumerable<Drone> droneList()
        {
            if (DataSource.drones.ToList() == null)
                throw new System.ArgumentException("drons list =null");
            return DataSource.drones.ToList();
        }
        public IEnumerable<Parcel> parcelList()
        {
            if (DataSource.parcels.ToList() == null)
                throw new System.ArgumentException("station list =null");
            return DataSource.parcels.ToList();
        }
        public IEnumerable<Station> stationList()
        {
            if (DataSource.stations.ToList().Count==0 )
                throw new System.ArgumentException("station list =null");
            return DataSource.stations.ToList();
        }


        /// <summary>
        /// a menue to print in the main to navagte the switch to the correct object
        /// </summary>
        /// <param name="action"></param
        /// }
        /// 
    }
}