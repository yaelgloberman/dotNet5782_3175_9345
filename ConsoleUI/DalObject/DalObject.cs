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
            double[] arr = new double[] { DataSource.Config.available, DataSource.Config.light, DataSource.Config.average, DataSource.Config.heavy, DataSource.Config.rateLoadingDrone };
            return arr;
        }
        public IEnumerable<droneCharges> chargingDroneList() { throw new Exception(); }
        public IEnumerable<Customer> CustomerList() { throw new Exception(); }
        // void DeliveryPackageCustomer(int cID, int pId, Proirities proirity) { throw new Exception(); }
        public IEnumerable<Drone> droneList() { throw new Exception(); }

        public void MenuPrint(string action) { throw new Exception(); }

        public IEnumerable<Parcel> parcelList() { throw new Exception(); }
        public IEnumerable<Station> stationList() { throw new Exception(); }



        /// <summary>
        /// a menue to print in the main to navagte the switch to the correct object
        /// </summary>
        /// <param name="action"></param
        /// }
        /// 
    }
}