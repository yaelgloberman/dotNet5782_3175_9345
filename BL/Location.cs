﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
{
    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public override string ToString()
        {
         return string.Format($" latitude: {latitude}, longitude: {longitude}\n  ");/// didnt include the last 2 lists in the 2 string

        }

    }
}