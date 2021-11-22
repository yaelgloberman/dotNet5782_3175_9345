using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
namespace BL
{
    public class Blobject
    {

        IDAL.DO.IDal dal = new DalObject.DalObject();
        internal static Random rand = new Random();
        internal List<IBL.BO.DroneToList> drones = new();

        public Blobject()
        {
            double[] tempArray = dal.ChargeCapacity();
            double pwrAvailable = tempArray[0];
            double pwrLight = tempArray[1];
            double pwrAverge = tempArray[2];
            double pwrHeavy = tempArray[3];
            double pwrRateLoadingDrone = tempArray[4];
            initDrone();

            /// im supposed to intialize the new drones in a bl format - but not sure how***********
            DateTime help = new DateTime(0, 0, 0);
            List<IDAL.DO.Parcel> undeliveredParcels = dal.UndiliveredParcels();
            IBL.BO.DroneToList tempD = new IBL.BO.DroneToList();
            foreach (var p in undeliveredParcels)
            {
                int droneIndex = p.droneId;
                int droneindex = drones.FindIndex(d => d.id == droneIndex);
                var tmpD = drones[droneIndex];
                if (droneIndex >= 0)
                {                 
                    tmpD.droneStatus = DroneStatus.delivery;
                    tmpD.batteryStatus = rand.Next(40, 100);//enough battery left so the parcel could get its destination
                }
                foreach(var d  in drones)
                {
                   
                    int maxStations = dal.stationList().Count();
                    if (d.droneStatus != DroneStatus.delivery)
                        d.droneStatus = (DroneStatus)rand.Next(1);
                    if (d.droneStatus == DroneStatus.charge)
                    { 
                        IDAL.DO.Station s = new Station();
                        s= dal.GetStation(rand.Next(maxStations));
                        tempD.currentLocation.latitude=s.latitude;//not sure why its not letting me and if i should use a temp
                        tempD.droneStatus =  (DroneStatus)rand.Next(20);
                    }
                    if(d.droneStatus==DroneStatus.available)
                    {
                        // 	מיקומו יוגרל בין לקוחות שיש חבילות שסופקו להם 
                        tempD.batteryStatus = rand.Next(40, 100);//not sure if its supposed to be enum or double
                    }
                }
                

            }
            //dal = new DalObject.DalObject();

        }





        //public IBL.BO.Customer GetCustomer(int id)
        //{
        //    IDAL.DO.Customer dalCustomer;
        //    IBL.BO.Customer Customer = default; 
        //    try
        //    {
        //        dalCustomer = dal.GetCustomer(id);
        //    }
        //    catch (IDAL.DO.findException Fex)
        //    {
        //        throw new BLFindException($"Customer id {id}", Fex);
        //    }
        //   // return dalCustomer;


        //public CustomerInParcel (int id)
        //{

        //}
        private void initDrone()
        {
            foreach (var drone in dal.droneList())
            {
                drones.Add(new DroneToList
                {
                    id = drone.id,
                    droneModel = int.Parse(drone.model),
                    weight = (Weight)drone.maxWeight,

                });
            }
        }


    }
}
