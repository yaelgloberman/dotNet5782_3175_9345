using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalObject
{
    internal class DataSource
    {
        /// <summary>
        /// creating list from each object . drone,pardel,station,Customer,chargingDrones
        /// </summary>
        internal static List<Drone> drones = new List<Drone>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<Station> stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<droneCharges> chargingDrones = new List<droneCharges>();

        internal class Config
        {
            internal static int parcelSerial = 10000000;
            internal static int numberId;
            internal static double available;
            internal static double light;
            internal static double heavy;
            internal static double average;
            internal static int rateLoadingDrone;
        }
        static Random r = new Random();
        int num = r.Next();
        /// <summary>
        /// function that gets cordinates and return the fordinate with floating point
        /// </summary>
        /// <param name="cordinate"></param>
        /// <returns></returns>
        //private static double getRandomCordinates(int num1,int num2) 
        //{
        //    int num3 = r.Next(num1, num2);
        //    return num3 + r.NextDouble()/10;
        //}
        private static double getRandomCordinates(double num1, double num2)
        {
            return (r.NextDouble() * (num2 - num1) + num1);
        }
        public static void Initialize()
        {
            DataSource.createCustomer();
            DataSource.CreateDrone();
            DataSource.CreateParcel();
            DataSource.CreateStation();
        }

        static void CreateStation()
        {
            for (int i = 0; i < 2; i++)
            {
                stations.Add(new Station()
                {
                    id = r.Next(111111111, 999999999),
                    name = "Station" + i,
                    longitude = getRandomCordinates(34.3, 35.5),
                    latitude = getRandomCordinates(31.0, 33.3),
                    chargeSlots = r.Next(5, 100)
                });
            }
        }

        static void CreateDrone()
        {
            for (int i = 0; i < 10; i++)
            {
                drones.Add(new Drone()
                {
                    id = r.Next(111111111, 999999999),
                    model = "Model" + i,
                    maxWeight = (WeightCatigories)r.Next(1, 3),
                });
            }
        }

        static void createCustomer()
        {
            for (int i = 0; i < 10; i++)
            {
                Customers.Add(new Customer()
                {
                    id = r.Next(11111111, 99999999),
                    name = "Name" + i,
                    phoneNumber = r.Next(11111111, 99999999),
                    longitude = getRandomCordinates(34.3, 35.5),
                    latitude = getRandomCordinates(31.0, 33.3),
                }) ;
            }
        }

        static void CreateParcel()
        {
            for (int i = 0; i < 10; i++)
            {
                parcels.Add(new Parcel()
                {
                    id = Config.parcelSerial++,
                    senderId = Customers[(i + 5) % 9].id,
                    targetId = Customers[i].id,
                    priority = (Proirities)r.Next(1, 3),
                    weight = (WeightCatigories)r.Next(1, 3),
                    droneId = drones.ToArray()[i].id,
                    requested = DateTime.Now,
                    scheduled = DateTime.MinValue,
                    pickedUp = DateTime.MinValue,
                    delivered = DateTime.MinValue,
                }); ;
                Config.numberId++;
            }
        }

    }
}



