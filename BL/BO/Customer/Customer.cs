using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string phoneNumber { get; set; }
        public Location location { get; set; }
        public List<ParcelCustomer> SentParcels { get; set; }
        public List<ParcelCustomer> ReceiveParcel { get; set; }
        public string PassWord { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, Name: {Name}, Phone Number: 05{phoneNumber}, Location: {location}");/// didnt include the last 2 lists in the 2 string
        }
    }
}
