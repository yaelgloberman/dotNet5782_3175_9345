using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
namespace BL
{
    public class Blobject
    {
        IDAL.DO.IDal dal;
        public Blobject()
        {
           dal = new DalObject.DalObject();
        }
        public IBL.BO.Customer GetCustomer(int id)
        {
            IBL.BO.Customer Customer = default; 
            try
            {
                IDAL.DO.Customer dalCustomer = dal.GetCustomer(id);
            }
            catch (IDAL.DO.findException Fex)
            {
                throw new BLFindException($"Customer id {id}", Fex);
            }
            return IDAL.DO.dalCustomer;

        }
        public CustomerInParcel (int id)
        {

        }
    }
}
