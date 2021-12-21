using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//namespace BlApi thats what dan has
namespace BlApi
{
    public static class BlFactory
    {
        public static IBl GetBl() => BL.BL.Instance;
    }
}


