using IDAL.DO;
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
            //  Drone[] droneArr = new Drone[10];
            // Parcel[] parcelArr = new Parcel[1000];
            // Station[] stationArr = new Station[10];//we first wrote 5 but we dont want to be limmited therefor we expanded altho we have to check if its ok
            // customer[] customerArr = new customer[100];
            internal static List<Drone> drones = new List<Drone>();
            internal static List<Parcel> parcels = new List<Parcel>();
            internal static List<Station> stations = new List<Station>();
            internal static List<customer> customers = new List<customer>();
            // internal static list<Drone> drones =new list<Drone>;// maybe drone charges

            internal class Config
            {
                internal static int parcelSerial = 0;
                internal static int numberId;
            }
            static Random r = new Random();
            int num = r.Next();
            public static void Initialize(DataSource dataS)
            {
                static void CreateStation()//why private?
                {
                    for (int i = 0; i < 2; i++)
                    {
                        stations.Add(new Station()//added ()
                        {
                            id = r.Next(111111, 8888888),
                            name = r.Next(1, 1000),
                            longitude = getRandomCordinates(34, 8),
                            latitude = getRandomCordinates(29, 6),
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
                            Id = r.Next(111111, 8888888),
                            Model = "Model" + i,
                            MaxWeight = (IDAL.DO.WeightCatigories)r.Next(1, 3),
                            //BateryStatus =(IDAL.DO.r.Next(IDAL.DO.)
                            Status = (IDAL.DO.DroneStatuses)r.Next(1, 3),
                        });
                    }

                }

            }
           
        }
    }

