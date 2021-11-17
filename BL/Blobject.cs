using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    class Blobject
    {
        IDAL.DO.IDal dal;
        public Blobject()
        {
           dal = new DalObject.DalObject();
        }
        public Customer GetCustomer(int id)
        {
            IDAL.DO.customer Someone;
            try
            {
                Someone = dal.GetCustomer(id);
            }
            catch (IDAL.DO.findException Fex)
            {
                throw;
            }
            return new Customer
            {
                id = Someone.id,
                Name = Someone.name,
                phoneNumber = Someone.phoneNumber,
                Location = new Location { latitude = Someone.latitude, longitude = Someone.longitude }

            };

        }
        public CustomerInParcel (int id)
        {

        }
    }
}
