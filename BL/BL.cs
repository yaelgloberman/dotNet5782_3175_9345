using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace IBL
{
    class BL:IBl
    {
        private IDaL dal;
        private List<CustomerInParcel> parcels;
        private static Random rand = new Random();
        public BL()
        {
            dal=new DalObject DalObject();
        }

    
    }
}
