using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BO
{
    public class CustomerInParcel
    {
        public int id { get; set; }
        public string name { get; set; }
        public double distance { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, Name: {name}\n");
        }
    }

}
