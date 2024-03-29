﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BO
{
    public class Drone
    {
        public int id { set; get; }
        public string droneModel { get; set; }
        public Weight weight { set; get; }
        public double batteryStatus { set; get; }
        public DroneStatus droneStatus { set; get; }
        public ParcelInTransfer parcelInTransfer { get; set; }
        public Location location { get; set; }
        public override string ToString()
        {
            return string.Format($"id: {id}, Model: {droneModel}, the max weight: {weight}, drones battery Status: {Math.Round(batteryStatus)}%, drones  Status: {droneStatus}, Location: {location}, parcel in transfer: {parcelInTransfer}  ");
        }
    }
}
