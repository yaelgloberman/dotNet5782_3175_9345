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

    }
}
