using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Customer
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int phoneNumber { get; set; }
        public Location Location { get; set; }
        public List<ParcelCustomer> SentParvels { get; set; }
        public List<ParcelCustomer> ReceiveParcel { get; set; }

    }
}
