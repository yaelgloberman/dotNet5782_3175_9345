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
			//DateTime help = new DateTime();
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
					if (p.pickedUp == DateTime.MinValue)//if the parcel was matched but nit picked up
					{
						Location senderLOcation = new Location();
						IDAL.DO.Customer senderOfParcel= dal.GetCustomer(p.senderId);
                        senderLOcation.longitude = senderOfParcel.longitude;
						senderLOcation.latitude = senderOfParcel.latitude;
						tempD.currentLocation = senderLOcation;
					}
		
				
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
						droneIndex=rand.Next(a)
						// 	מיקומו יוגרל בין לקוחות שיש חבילות שסופקו להם 
						tempD.batteryStatus = rand.Next(40, 100);//not sure if its supposed to be enum or double
					}
				}
				

			}
			//dal = new DalObject.DalObject();

		}





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

        #region ADD STATION
		public void addStation(BaseStation stationToAdd)
        {
			//if (stationToAdd)
        }
		#region ADD Drone
		public void addDrone(IBL.BO.Drone droneToAdd)
		{
			Location l = new Location();
			if (!(droneToAdd.id >= 10000000 && droneToAdd.id <= 1000000000))
				throw new AddException("the number of the drone id in invalid\n");
			if (!(droneToAdd.droneModel <= 100 && droneToAdd.droneModel >= 10000))
				throw new AddException("the number of the drone model invalid\n");
			if (!(droneToAdd.batteryStatus > (BatteryStatus)0 && droneToAdd.batteryStatus < (BatteryStatus)3))
				throw new AddException("the status of the drone is invalid\n");
			if (droneToAdd.currentLocation.latitude < (double)31 || droneToAdd.currentLocation.latitude > (double)33.3
							|| droneToAdd.currentLocation.longitude < (double)34.3 || droneToAdd.currentLocation.longitude > (double)35.5)
				throw new AddException("the given cordinates do not exist in this country/\n");
			if (!(droneToAdd.weight > (Weight)0 && droneToAdd.weight < (Weight)3))
				throw new AddException("the given weight is not valid\n");
            try { dal.addDrone((IDAL.DO.Drone)droneToAdd.CopyPropertiesToNew(typeof(IDAL.DO.Drone))); }
            catch (AlreadyExistException ex)
			{ throw new AddException("this drone already exists\n"); }


		}
		#endregion
		#region DELETE DRONE
		public void deleteDrone(int droneID)
		{
			//IDAL.DO.Parcel parcelInDrone; 
			foreach(var p in dal.parcelList())
            {
				if (p.droneId == droneID)
					throw new deleteException("there is/are parcel that is matched to this drone\n");
            }
            try
            {
				var drone = dal.GetDrone(droneID);
				dal.deleteDrone(drone);

			}
			catch(deleteException ex) { throw new deleteException("cant delete this drone\n",ex); }
			

		}
		//public IEnumerable<IBL.BO.Drone>GetDrones()
  //      {
		//	var listOfDrones = from Drone in IDAL.DO.getDroneList()
		//					   select new IBL.BO.Drone()
		//					   {

		//					   };
		//	retu
  //      }
		public IBL.BO.Drone GetDrone(int id)
        {
			IDAL.DO.Drone dalDrone=dal.GetDrone(id),
			return new IBL.BO.Drone()
            {

            }
            IBL.BO.Drone drone = default;
            try
            {
                dalDrone = dal.GetDrone(id);
            }
            catch (IDAL.DO.findException Fex)
            {
                throw new BLFindException($"Customer id {id}", Fex);
            }
             return (IBL.BO)dalDrone;
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

    }
}
