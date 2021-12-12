using IDAL.DO;
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
        public IEnumerable<droneCharges> chargingGetDroneList()
        {
            if (DataSource.chargingDrones.ToList() == null)
                throw new System.ArgumentException("charging drone" + " list =null");
            return DataSource.chargingDrones.ToList();
        }
        public IEnumerable<Customer> GetCustomerList()
        {
            if (DataSource.Customers.ToList() == null)
                throw new System.ArgumentException("drons list =null");
            return DataSource.Customers.ToList();
        }
        public IEnumerable<Drone> GetDroneList()
        {
            if (DataSource.drones.ToList() == null)
                throw new System.ArgumentException("drons list =null");
            return DataSource.drones.ToList();
        }
        public IEnumerable<Parcel> GetParcelList()
        {
            if (DataSource.parcels.ToList() == null)
                throw new System.ArgumentException("station list =null");
            return DataSource.parcels.ToList();
        }
        public IEnumerable<Station> GetStationList()
        {
            if (DataSource.stations.ToList().Count==0 )
                throw new System.ArgumentException("station list =null");
            return DataSource.stations.ToList();
        }
    }
}