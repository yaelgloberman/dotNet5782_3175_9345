using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL.BO
{
    public interface IBl
    {
        Customer GetCustomer(int id);
        IEnumerable<ParcelCustomer> GetParcelCustomers();
        //IEnumerable<ParcelCustomer> get();
        void addStation(BaseStation stationToAdd);
        public void addStation(BaseStation stationToAdd);
        public void deleteDrone(int droneID);
        public DroneToList GetDrone(int id);
        public void addDrone(DroneToList d, int stationId);
        public IBL.BO.Customer GetCustomer(int id);
    }
}
