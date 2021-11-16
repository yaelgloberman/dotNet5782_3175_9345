using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    public interface IBl
    {
        Customer GetCustomer(int id);
    }
}
