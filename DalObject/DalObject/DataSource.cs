using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace Dal
{
    internal class DataSource
    {
        /// <summary>
        /// creating list from each object . drone,pardel,station,Customer,chargingDrones
        /// </summary>
          
        internal static string[] CapatalLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L" };
        internal static string[] stationName = { "Jerusalem", "Eilat" };
        internal static string[] droneName = { "Reaper", "Shadow", "Grey Eagle", "Global Hawk", "Pioneer", "Fire Scout", "Snowgoose", "Hunter", "Stalker", "GNAT", "Wing Loong II", "AVENGER", "Apollo Earthly", "AirHaven", "indRazer", "Godspeed", "Phantom", "Novotek", "Tri-Propeller", "WikiDrone" };
        internal static string[] customerName = { "Eliana", "Adina", "Shifra", "Yaeli", "Shirel", "Yisca", "Ruchama", "Noa", "Dan", "Eliezer" };
        internal static List<Drone> drones = new List<Drone>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<Station> stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<droneCharges> chargingDrones = new List<droneCharges>();

        internal class Config
        {
            internal static int parcelSerial = 10000000;
            internal static int numberId = 1000;
            internal static double available = 0.05;
            internal static double light = 0.005;
            internal static double heavy = 0.05;
            internal static double average = 0.3;
            internal static double rateLoadingDrone = 0.5;
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
                    name = stationName[i],
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
                    model = droneName[i],
                    maxWeight = (WeightCatigories)r.Next(1, 3),
                }) ;
            }
        }
        static void createCustomer()
        {
            string p = CapatalLetters[10];
            for (int i = 0; i < 10; i++)
            {
                Customers.Add(new Customer()
                {
                    id = r.Next(11111111, 99999999),
                    name = customerName[i],
                    phoneNumber = "05"+r.Next(11111111, 99999999),
                    longitude = getRandomCordinates(34.3, 35.5),
                    latitude = getRandomCordinates(31.0, 33.3),
                    Password = CapatalLetters[i] + p,
                    isCustomer = true,
                });

            }
            for (int i = 8; i < 10; i++) //creates 2 workers
                Customers.Add(new Customer()
                {
                    id = r.Next(100000000, 999999999),
                    name = customerName[i],
                    phoneNumber ="05"+ r.Next(00000000, 99999999),
                    longitude = getRandomCordinates(34.3, 35.5),
                    latitude = getRandomCordinates(31.0, 33.3),
                    Password = CapatalLetters[i] + p,
                    isCustomer = false,
                });
        }

        static void CreateParcel()
        {
            for (int i = 0; i < 10; i++)
            {
                var parcel = new Parcel();


                parcel.id = Config.parcelSerial++;
                parcel.senderId = Customers[(i + 5) % 9].id;
                parcel.targetId = Customers[i].id;
                parcel.priority = (Proirities)r.Next(1, 3);
                parcel.weight = (WeightCatigories)r.Next(1, 3);
                if ((drones.ToArray()[i].id) % 2 == 0)
                {
                    parcel.droneId = drones.ToArray()[i].id;
                    parcel.requested = DateTime.Now;

                }
                else
                    parcel.droneId = 0;
                parcel.requested = DateTime.Now;
                parcel.scheduled = DateTime.Now;
                parcel.pickedUp = null;
                parcel.delivered = null;
                parcels.Add(parcel);
            }
            Config.numberId++;
        }
    }

}




