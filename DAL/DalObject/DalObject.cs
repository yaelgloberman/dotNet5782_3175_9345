using IDAL.DO;
using DAL.DalObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalObject
{
    public partial class DalObject//:IDal
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

        /// <summary>
        /// a menue to print in the main to navagte the switch to the correct object
        /// </summary>
        /// <param name="action"></param>
        public void MenuPrint(string action)//th menue that helps specify the main action 
        {
            Console.WriteLine($"what would you like to {action}?");
            Console.WriteLine($"enter 1 to {action} station");
            Console.WriteLine($"enter 2 to {action} drones");
            Console.WriteLine($"enter 3 to {action} Customers");
            Console.WriteLine($"enter 4 to {action} parcel");
        }

    }
}
