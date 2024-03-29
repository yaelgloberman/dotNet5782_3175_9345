﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DO
    {
        public struct Drone
        {
            public int id { set; get; }
            public string model { set; get; }
            public WeightCatigories maxWeight { set; get; }
            public bool deleted { set; get; }
            public override string ToString()
            {
                return string.Format($"id: {id}, Model: {model}, the max weight: {maxWeight}");
            }
        }
    }

