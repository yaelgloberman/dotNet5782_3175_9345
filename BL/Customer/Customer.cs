using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int phoneNumber { get; set; }
        public Location location { get; set; }
        public List<ParcelCustomer> SentParcels { get; set; }
        public List<ParcelCustomer> ReceiveParcel { get; set; }
        

    }
}
