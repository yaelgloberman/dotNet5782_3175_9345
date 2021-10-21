using System;
using IDAL.DO;

namespace DalObject
{
    internal class DataSource
    {
      //  Drone[] droneArr = new Drone[10];
       // Parcel[] parcelArr = new Parcel[1000];
       // Station[] stationArr = new Station[10];//we first wrote 5 but we dont want to be limmited therefor we expanded altho we have to check if its ok
       // customer[] customerArr = new customer[100];
         internal static list<Drone> drones =new list<Drone>;
         internal static list<Parcel> drones =new list<Drone>;
         internal static list<Station> drones =new list<Drone>;
         internal static list<customer> drones =new list<Drone>;
        // internal static list<Drone> drones =new list<Drone>;// maybe drone charges

        internal class Config
        {
            //eliezer only did parcel and number id- y??
            internal static int droneI = 0;
            internal static int parcelI = 0;
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
                dataS.stationArr[i].Id = r.Next(1000);
                dataS.stationArr[i].name = r.Next(6);
                dataS.stationArr[i].Latitude = r.Next(-100, 100);
                dataS.stationArr[i].Longitude = r.Next(-100, 100);
            }
            dataS.stationArr[0].name = 1;
            dataS.stationArr[0].name = 2;
            for (int i = Config.clientI; i < 10; i++)
            {
                dataS.customerArr[i].Id = r.Next(99999999, 10000000);
                dataS.customerArr[i].Latitude = r.Next(-100, 100);
                dataS.customerArr[i].Longitude = r.Next(-100, 100);
            }
            dataS.customerArr[0].Name = "Shifra";
            dataS.customerArr[0].PhoneNumber = 0586503993;
            dataS.customerArr[0].Name = "Yael";
            dataS.customerArr[0].PhoneNumber = 0525140409;
            dataS.customerArr[0].Name = "Alex";
            dataS.customerArr[0].PhoneNumber = 0502894536;
            dataS.customerArr[0].Name = "Yaeli";
            dataS.customerArr[0].PhoneNumber = 0527703139;
            dataS.customerArr[0].Name = "Chani";
            dataS.customerArr[0].PhoneNumber = 0526673322;
            dataS.customerArr[0].Name = "Bezalel";
            dataS.customerArr[0].PhoneNumber = 0524070888;
            dataS.customerArr[0].Name = "Shimi";
            dataS.customerArr[0].PhoneNumber = 0587703153;
            dataS.customerArr[0].Name = "Adina";
            dataS.customerArr[0].PhoneNumber = 0533493551;
            dataS.customerArr[0].Name = "Rina";
            dataS.customerArr[0].PhoneNumber = 0548670887;
            dataS.customerArr[0].Name = "Kineret";
            dataS.customerArr[0].PhoneNumber = 0586275356;
            for (int i = Config.parcelI; i < 10; i++)
            {

                dataS.parcelArr[i].SenderId = dataS.customerArr[r.Next(6)].Id;
                dataS.parcelArr[i].TargetId = dataS.customerArr[r.Next(6)].Id;
                dataS.parcelArr[i].Id = dataS.droneArr[r.Next(6)].Id;
                dataS.parcelArr[i].Requested = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));
                dataS.parcelArr[i].Scheduled = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));
                dataS.parcelArr[i].PickedUp = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));
                dataS.parcelArr[i].Datetime = new DateTime(2021, r.Next(1, 13), r.Next(1, 32), r.Next(24), r.Next(61), r.Next(61));
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

        public DalObject()
        {
            DataSource.Initialize();
        }
        public static void addStation(int id1,int name1, double longi,double lati,int charge)//findout if the sattion name is supposed to be string or int
        {
            DataSource s=new Station();
            s.id=id1;
            s.name=name1;
            s.latitude=longi;


        }
    }

}
