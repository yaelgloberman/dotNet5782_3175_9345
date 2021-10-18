using System;
using IDAL.DO;

namespace DalObject
{
    internal class DataSource
    {
        Drone[] droneArr = new Drone[10];
        Parcel[] parcelArr = new Parcel[1000];
        Station[] stationArr = new Station[5];
        customer[] customerArr = new customer[100];

        internal class Config
        {
            internal static int droneI = 0;
            internal static int packageI = 0;
            internal static int baseStationI = 0;
            internal static int clientI = 0;
            // internal static int C = 0;
        }

        public static void Initialize(DataSource dataS)
        {
            Random r = new Random();
            int num = r.Next();
            for (int i = Config.droneI; i < 5; i++)
            {
                dataS.droneArr[i].Id = r.Next(1000);

                dataS.droneArr[i].BateryStatus = r.Next(101);

            }
            dataS.droneArr[0].Model = "drone0";
            dataS.droneArr[1].Model = "drone1";
            dataS.droneArr[2].Model = "drone2";
            dataS.droneArr[3].Model = "drone3";
            dataS.droneArr[4].Model = "drone4";
            dataS.droneArr[0].MaxWeight = WeightCatigories.light;
            dataS.droneArr[1].MaxWeight = WeightCatigories.heavy;
            dataS.droneArr[2].MaxWeight = WeightCatigories.avergae;
            dataS.droneArr[3].MaxWeight = WeightCatigories.avergae;
            dataS.droneArr[4].MaxWeight = WeightCatigories.heavy;

            dataS.droneArr[0].Status = DroneStatuses.shipping;
            dataS.droneArr[1].Status = DroneStatuses.available;
            dataS.droneArr[2].Status = DroneStatuses.maintenance;
            dataS.droneArr[3].Status = DroneStatuses.shipping;
            dataS.droneArr[4].Status = DroneStatuses.maintenance;
            for (int i = Config.baseStationI; i < 2; i++)
            {
                dataS.stationArr[i].Id= r.Next(1000);
                dataS.stationArr[i].name = r.Next(6);
                dataS.stationArr[i].Latitude = r.Next(-100, 100);
                dataS.stationArr[i].Longitude = r.Next(-100, 100);
            }
            dataS.baseStationArr[0].NameBaseStation = "Station1";
            dataS.baseStationArr[0].NameBaseStation = "Station2";
            for (int i = Config.clientI; i < 10; i++)
            {
                dataS.customerArr[i].Id = r.Next(99999999, 10000000);
                dataS.customerArr[i].Latitude = r.Next(-100, 100);
                dataS.customerArr[i].Longitude = r.Next(-100, 100);
            }
            dataS.customerArr[0].Name = "jo";
            dataS.customerArr[0].PhoneNumber = "058-2457908";
            dataS.customerArr[0].Name = "keley";
            dataS.customerArr[0].PhoneNumber = "050-2447328";
            dataS.customerArr[0].Name = "nely";
            dataS.customerArr[0].PhoneNumber = "053-3457668";
            dataS.customerArr[0].Name = "pj";
            dataS.customerArr[0].PhoneNumber = "058-2477919";
            dataS.customerArr[0].Name = "pazit";
            dataS.customerArr[0].PhoneNumber = "053-9467738";
            dataS.customerArr[0].Name = "gal";
            dataS.customerArr[0].PhoneNumber = "058-2450008";
            dataS.customerArr[0].Name = "magen";
            dataS.customerArr[0].PhoneNumber = "054-3337908";
            dataS.customerArr[0].Name = "ely";
            dataS.customerArr[0].PhoneNumber = "058-2422208";
            dataS.customerArr[0].Name = "code";
            dataS.customerArr[0].PhoneNumber = 058-2412128;
            dataS.customerArr[0].Name = "jame";
            dataS.customerArr[0].PhoneNumber = "050-3456908";
            for (int i = Config.packageI; i < 10; i++)
            {
                
                dataS.parcelArr[i].SenderId = dataS.clientArr[r.Next(6)].Id;
                dataS.parcelArr[i].TargetId = dataS.clientArr[r.Next(6)].Id;
                dataS.parcelArr[i].IdDrone = dataS.droneArr[r.Next(6)].IdDrone;
                dataS.parcelArr[i].DeliveryTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));
                dataS.parcelArr[i].AssignmentTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));
                dataS.parcelArr[i].PickupTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));
                dataS.parcelArr[i].ArrivalTime = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));

            }
           
            dataS.parcelArr[0].Weight = WeightCatigories.heavy;
            dataS.parcelArr[0].Priority = Proirities.regular;
            dataS.parcelArr[1].Weight = WeightCatigories.light;
            dataS.parcelArr[1].Priority = Proirities.emergency;
            dataS.parcelArr[2].Weight = WeightCatigories.heavy;
            dataS.parcelArr[2].Priority = Proirities.fast;
            dataS.parcelArr[3].Weight = WeightCatigories.avergae;
            dataS.parcelArr[3].Priority = Proirities.regular;
            dataS.parcelArr[4].Weight = WeightCatigories.light;
            dataS.parcelArr[4].Priority = Proirities.emergency;
            dataS.parcelArr[5].Weight = WeightCatigories.heavy;
            dataS.parcelArr[5].Priority = Proirities.regular;
            dataS.parcelArr[6].Weight = WeightCatigories.heavy;
            dataS.parcelArr[6].Priority = Proirities.emergency;
            dataS.parcelArr[7].Weight = WeightCatigories.heavy;
            dataS.parcelArr[7].Priority = Proirities.fast;
            dataS.parcelArr[8].Weight = WeightCatigories.light;
            dataS.parcelArr[8].Priority = Proirities.fast;
            dataS.parcelArr[9].Weight = WeightCatigories.avergae;
            dataS.parcelArr[9].Priority = Proirities.emergency;
            




    }


}

