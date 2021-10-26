﻿using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL
{
    namespace DalObject
    {
        internal class DataSource
        {

            internal static List<Drone> drones = new List<Drone>();
            internal static List<Parcel> parcels = new List<Parcel>();
            internal static List<Station> stations = new List<Station>();
            internal static List<customer> customers = new List<customer>();

            internal class Config
            {
                internal static int parcelSerial = 0;
                internal static int numberId;
            }
            static Random r = new Random();
            int num = r.Next();
            //DataSource dataS
            public static void Initialize()
            {
                createCustomer();
                CreateDrone();
                CreateParcel();
                CreateStation();
            }
            static void CreateStation()//why private?
            {
                for (int i = 0; i < 2; i++)
                {
                    stations.Add(new Station()//added ()
                    {
                        id = r.Next(111111111, 999999999),
                        name = r.Next(1, 1000),
                        longitude = r.Next(34,8),/*getRandomCordinates(34, 8)*/
                        latitude = r.Next(29,6),//getRandomCordinates(29, 6),
                        chargeSlots = r.Next(5, 100)
                    });
                }
            }
            static void CreateDrone()
            {
                for (int i = 0; i < 5; i++)
                {
                    drones.Add(new Drone()
                    {
                        Id = r.Next(111111111, 999999999),
                        Model = "Model" + i,
                        MaxWeight = (IDAL.DO.WeightCatigories)r.Next(1, 3),
                        //BateryStatus =(IDAL.DO.r.Next(IDAL.DO.)
                        Status = (IDAL.DO.DroneStatuses)r.Next(1, 3),
                    });
                }

            }
            static void createCustomer()
            {
                for (int i = 0; i < 100; i++)
                {
                    customers.Add(new customer()
                    {
                        Id = r.Next(111111111, 999999999),
                        Name = "Name" + i,
                        PhoneNumber = 05 + r.Next(0, 9) + -+r.Next(111111111, 999999999),
                        Longitude = r.Next(34, 8),//getRandomCordinates(34, 8),
                        Latitude = r.Next(29, 6)// getRandomCordinates(29, 6),

                    });
                }
            }

            static void CreateParcel()
            {
                for (int i = 0; i < 1000; i++)
                {
                    parcels.Add(new Parcel()
                    {
                        Id = r.Next(111111111, 999999999),
                        SenderId = r.Next(111111111, 999999999),
                        TargetId = r.Next(111111111, 999999999),
                        Priority = (IDAL.DO.Proirities)r.Next(1, 3),
                        Weight = (IDAL.DO.WeightCatigories)r.Next(1, 3),
                        DroneId = r.Next(111111111, 999999999),
                        Requested = DateTime.Now,
                        Scheduled = DateTime.Now,
                        PickedUp = DateTime.Now,
                        Datetime = DateTime.Now,
                    });
                    Config.parcelSerial++;
                    Config.numberId++;
                }
            }
        }
    }
}


