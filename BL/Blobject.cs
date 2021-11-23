using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
namespace BL
{
	public class Blobject : IBl
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
						IDAL.DO.Customer senderOfParcel = dal.GetCustomer(p.senderId);
						senderLOcation.longitude = senderOfParcel.longitude;
						senderLOcation.latitude = senderOfParcel.latitude;
						tempD.currentLocation = senderLOcation;
					}


					tmpD.batteryStatus = rand.Next(40, 100);//enough battery left so the parcel could get its destination
				}
				foreach (var d in drones)
				{

					int maxStations = dal.stationList().Count();
					if (d.droneStatus != DroneStatus.delivery)
						d.droneStatus = (DroneStatus)rand.Next(1);
					if (d.droneStatus == DroneStatus.charge)
					{
						IDAL.DO.Station s = new Station();
						s = dal.GetStation(rand.Next(maxStations));
						tempD.currentLocation.latitude = s.latitude;//not sure why its not letting me and if i should use a temp
						tempD.droneStatus = (DroneStatus)rand.Next(20);
					}
					if (d.droneStatus == DroneStatus.available)
					{
						droneIndex = rand.Next(a);
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
		public void addDrone(DroneToList droneToAdd)
		{
			Location l = new Location();
			if (!(droneToAdd.id >= 10000000 && droneToAdd.id <= 1000000000))
				throw new AddException("the number of the drone id in invalid\n");
			if (!(droneToAdd.droneModel <= 100 && droneToAdd.droneModel >= 10000))
				throw new AddException("the number of the drone model invalid\n");
			if (!(droneToAdd.batteryStatus >= (double)0 && droneToAdd.batteryStatus <= (double)100))
				throw new AddException("the status of the drone is invalid\n");
			if (droneToAdd.currentLocation.latitude < (double)31 || droneToAdd.currentLocation.latitude > 33.3
							|| droneToAdd.currentLocation.longitude < 34.3 || droneToAdd.currentLocation.longitude > 35.5)
				throw new AddException("the given cordinates do not exist in this country/\n");
			if (!(droneToAdd.weight > (Weight)0 && droneToAdd.weight < (Weight)3))
				throw new AddException("the given weight is not valid\n");
			try
			{
				IDAL.DO.Drone drone = new IDAL.DO.Drone()
				{

					id = droneToAdd.id,

				};
				dal.addDrone(drone);
			}
			catch (AddException ex)
			{ 
				throw new AddException("this drone already exists\n");
			}

		}
		#endregion
		#region DELETE DRONE
		public void deleteDrone(int droneID)
		{
			//IDAL.DO.Parcel parcelInDrone; 
			foreach (var p in dal.parcelList())
			{
				if (p.droneId == droneID)
					throw new deleteException("there is/are parcel that is matched to this drone\n");
			}
			try
			{
				var drone = dal.GetDrone(droneID);
				dal.deleteDrone(drone);

			}
			catch (deleteException ex) { throw new deleteException("cant delete this drone\n", ex); }
		}
		public DroneToList GetDrone(int id)
		{
			try
			{
				DroneToList dl = drones.Find(x => x.id == id);
				if (dl.id != 0)
					return dl;
				DroneToList droneBo = new DroneToList();
				IDAL.DO.Drone droneDo = dal.GetDrone(id);
				DroneToList drone = drones.Find(d => d.id == id);
				droneBo.id = droneDo.id;
				droneBo.droneModel = drone.droneModel;
				droneBo.weight = drone.weight;
				droneBo.currentLocation = drone.currentLocation;
				droneBo.batteryStatus = drone.batteryStatus;
				droneBo.droneStatus = drone.droneStatus;
				droneBo.numOfDeliverdParcels = drone.numOfDeliverdParcels;
				return droneBo;
			}
			catch (DoesntExistException ex)
			{
				throw new GetDetailsException("Can't get this drone", ex);
			}
		}
		public void addDrone(DroneToList d, int stationId)
		{
			d.batteryStatus = (double)rand.Next(20, 40);
			IDAL.DO.Drone tmpDrone = dal.GetDrone(d.id);
			IDAL.DO.Station tmpStation = dal.GetStation(stationId);
			d.droneStatus = DroneStatus.charge;
		}
		public IBL.BO.Customer GetCustomer(int id)
		{
			IDAL.DO.Customer dalCustomer;
			IBL.BO.Customer Customer = default;
			try
			{
				dalCustomer = dal.GetCustomer(id);
			}
			catch (IDAL.DO.findException Fex)
			{
				throw new BLFindException($"Customer id {id}", Fex);
			}
			return Customer;

		}
		public void addCustomer(IBL.BO.Customer customerToAdd)
		{
			if (!(customerToAdd.id >= 10000000 && customerToAdd.id <= 1000000000))
				throw new AddException("the number of the drone id in invalid\n");
			if (customerToAdd.currentLocation.latitude < (double)31 || drocustomerToAddneToAdd.currentLocation.latitude > 33.3
							|| droneToAdd.currentLocation.longitude < 34.3 || droneToAdd.currentLocation.longitude > 35.5)
				throw new AddException("the given cordinates do not exist in this country/\n");
			if (!(droneToAdd.weight > (Weight)0 && droneToAdd.weight < (Weight)3))
				throw new AddException("the given weight is not valid\n");
			if (DataSource.Customers.Exists(item => item.id == c.id))
				throw new AddException("Customer already exist");
			DataSource.Customers.Add(c);
		}
	}
    
