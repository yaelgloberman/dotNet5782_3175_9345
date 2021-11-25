using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO; //yael
namespace BL
{
	public class Blobject : IBl
	{

		private IDal dal = new DalObject.DalObject();
		private static Random rand = new Random();
		private List<DroneToList> drones = new();
		public Blobject()
		{
			double[] tempArray = dal.ChargeCapacity();
			double pwrAvailable = tempArray[0];
			double pwrLight = tempArray[1];
			double pwrAverge = tempArray[2];
			double pwrHeavy = tempArray[3];
			double pwrRateLoadingDrone = tempArray[4];
			initDrone();
		}

		/// im supposed to intialize the new drones in a bl format - but not sure how***********
		//DateTime help = new DateTime();
		#region Init drone
		private void initDrone()
		{
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
						Location senderLocation = new Location();
						IDAL.DO.Customer senderOfParcel = dal.GetCustomer(p.senderId);
						senderLocation.longitude = senderOfParcel.longitude;
						senderLocation.latitude = senderOfParcel.latitude;
						tempD.location = senderLocation;
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
						tempD.location.latitude = s.latitude;//not sure why its not letting me and if i should use a temp
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
		}
		#endregion
		#region A STATION
		public void addStation(BaseStation stationToAdd)
		{
			List<droneCharges> list = new List<droneCharges>();

			stationToAdd.DroneInChargeList = new List<DroneInCharge>();  // חושבת שככה מאתחלים ל0 רשימה 

			IDAL.DO.Station stationDo =
				new IDAL.DO.Station()
				{
					id = stationToAdd.id,
					name = stationToAdd.stationName,
					longitude = stationToAdd.location.longitude,
					latitude = stationToAdd.location.latitude,
					chargeSlots = stationToAdd.avilableChargeSlots 
				};
			try
			{
				dal.addStation(stationDo);
			}
			catch (Exception exp)
			{

				throw new ExistException("", exp);
			}


		}
		#endregion
		#region ADD Drone
		public void addDrone(DroneToList droneToAdd,int stationId)
		{
			droneToAdd.batteryStatus = (double)rand.Next(20, 40);
			if (!(droneToAdd.id >= 10000000 && droneToAdd.id <= 1000000000))
				throw new AddException("the number of the drone id in invalid\n");
			//if (!(droneToAdd.droneModel <=  && droneToAdd.droneModel >= ))   //איך בודקים עכשיו את המודל כשהוא סטרינג
				//throw new AddException("the number of the drone model invalid\n");
			if (!(droneToAdd.batteryStatus >= (double)0 && droneToAdd.batteryStatus <= (double)100))
				throw new AddException("the status of the drone is invalid\n");
			if (droneToAdd.location.latitude < (double)31 || droneToAdd.location.latitude > 33.3
							|| droneToAdd.location.longitude < 34.3 || droneToAdd.location.longitude > 35.5)
				throw new AddException("the given cordinates do not exist in this country/\n");
			if (!(droneToAdd.weight > (Weight)0 && droneToAdd.weight < (Weight)3))
				throw new AddException("the given weight is not valid\n");
			try   //im not sure that everithing needs to be in the try but wtev
			{
				IDAL.DO.Drone tempDrone = dal.GetDrone(droneToAdd.id);
				IDAL.DO.Station tempStation = dal.GetStation(stationId);
				droneToAdd.droneStatus = DroneStatus.charge;
				droneToAdd.location = getBaseStationLocation(stationId);
				IDAL.DO.Drone drone = new IDAL.DO.Drone();
				drone.id = droneToAdd.id;
				drone.model = droneToAdd.droneModel;
				drone.maxWeight = (WeightCatigories)droneToAdd.weight;
				dal.SendToCharge(droneToAdd.id, stationId);
				drones.Add(droneToAdd);
				dal.addDrone(drone);
			}
			catch (AddException ex)
			{
				throw new AddException("this drone already exists\n");
			}

		}
		#endregion
		#region Add Customer
		public void addCustomer(IBL.BO.Customer customerToAdd)
		{

			if (!(customerToAdd.id >= 10000000 && customerToAdd.id <= 1000000000))
				throw new AddException("the id number of the drone is invalid\n");
			if (!(customerToAdd.phoneNumber >= 500000000 && customerToAdd.phoneNumber <= 0589999999))
				throw new AddException("the phone number of the customer is invalid\n");
			if (customerToAdd.location.latitude < (double)31 || customerToAdd.location.latitude > 33.3 || customerToAdd.location.longitude < 34.3 || customerToAdd.location.longitude > 35.5)
				throw new AddException("the given cordinates do not exist in this country/\n");
			if (dal.GetCustomers().ToList().Exists(item => item.id == customerToAdd.id))
				throw new AddException("Customer already exist");
			IDAL.DO.Customer customerDo = new IDAL.DO.Customer();
			customerDo.id = customerToAdd.id;
			customerDo.name = customerToAdd.Name;
			customerDo.phoneNumber = customerToAdd.phoneNumber;
			customerDo.longitude = customerToAdd.location.longitude;
			customerDo.latitude = customerToAdd.location.latitude;
			try
			{
				dal.addCustomer(customerDo);
			}
			catch (Exception exp)
			{

				throw new ExistException("", exp);
			}
			// Nה צריך לבדוק עם SentParcels ReceiveParcel
		}
		#endregion
		#region Add Parcel
		public void addParcel(IBL.BO.Parcel parcelToAdd)
		{
			if (!(parcelToAdd.sender.Id >= 10000000 && parcelToAdd.sender.Id <= 1000000000))
				throw new AddException("the id sender number of the pardel is invalid\n");
			if (!(parcelToAdd.recive.Id >= 10000000 && parcelToAdd.recive.Id <= 1000000000))
				throw new AddException("the id recive number of the parcel is invalid\n");
			if (!(parcelToAdd.weightCategorie > (Weight)0 && parcelToAdd.weightCategorie < (Weight)3))
				throw new AddException("the given weight is not valid\n");
			if (!(parcelToAdd.priority > (Priority)0 && parcelToAdd.priority < (Priority)3))
				throw new AddException("the given priority is not valid\n");
			IDAL.DO.Parcel parcelDo = new IDAL.DO.Parcel();
			parcelDo.senderId = parcelToAdd.sender.Id;
			parcelDo.targetId = parcelToAdd.recive.Id;
			parcelDo.weight = (WeightCatigories)parcelToAdd.weightCategorie;
			parcelDo.priority = (Proirities)parcelToAdd.priority;
			parcelDo.requested = DateTime.Now;//נראלי שזה נחשב ליצירה
			parcelDo.scheduled = DateTime.MinValue;
			parcelDo.pickedUp = DateTime.MinValue;
			parcelDo.delivered = DateTime.MinValue;
			parcelDo.droneId= 0;   //אני לא יודעת אם זה נחשב NULL 
            try
            {
				dal.addParcel(parcelDo);
            }
			catch (Exception exp)
			{
				throw new ExistException(",exp");
            }
		}
        #endregion
        #region Get Customer Recived Parcel
        private IEnumerable<ParcelToList> getCustomerRecivedParcel(int id)
		{
			var deliveries = dal.GetParcels()
				.Where(p => p.targetId == id)
				.Select(parcel =>
					new ParcelToList
					{
						id = parcel.id,
						senderName = parcel.senderId,
						reciveName = parcel.targetId,
						weight=(Weight)parcel.weight,
						priority=(Priority)parcel.priority,
					});
			return deliveries;
		}
        #endregion
        #region Get station
        public BaseStation GetStation(int id)
		{
			try
			{
				BaseStation baseStation = new BaseStation();
				Station station = dal.GetStation(id);
				baseStation.id = station.id;
				baseStation.stationName = station.name;
				baseStation.avilableChargeSlots = station.chargeSlots;
				baseStation.location = new Location()
				{
					latitude = station.latitude,
					longitude = station.longitude
				};
				baseStation.DroneInChargeList = dal.GetDroneIdInStation(id)  
					.Select(drone => new DroneInCharge()
					{
						id = drone.droneId,
						batteryStatus =getDroneBattery(drone.droneId)
					}); 
      				return baseStation;
			}
			catch (DoesntExistException ex)
			{
				throw new GetDetailsException("Can't get this drone", ex);
			}
		}
		#endregion
		#region Get Drone Battery

		private double getDroneBattery(int droneId)
		{
			return drones.Find(drone => drone.id == droneId).batteryStatus;
		}
        #endregion
        #region Get Drone
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
				droneBo.location = drone.location;
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
		#endregion
        #region Get Customer
        public IBL.BO.Customer GetCustomer(int id)
		{
			try
			{
				IBL.BO.Customer customerBo = new IBL.BO.Customer();
				IDAL.DO.Customer customerDo = dal.GetCustomer(id);
				customerBo.id = customerDo.id;
				customerBo.Name = customerDo.name;
				customerBo.phoneNumber = customerDo.phoneNumber;
				customerBo.location = new Location() { latitude = customerDo.latitude, longitude = customerDo.longitude };
				customerBo.SentParcels = getCustomerShippedParcels(id).ToList();
				customerBo.ReceiveParcel = getCustomerReceivedParcels(id).ToList();
				return customerBo;
			}
			catch (IDAL.DO.findException Fex)
			{
				throw new BLFindException($"Customer id {id}", Fex);
			}
		}
        #endregion
        #region Get Customer Shipped Parcels
        private IEnumerable<ParcelCustomer> getCustomerShippedParcels(int id)
		{
			var tempParcel = dal.getCustomerShippedParcels(id)
				.Select(parcel =>
					new ParcelCustomer
					{
						id = parcel.id,
						weight = (Weight)parcel.weight,
						priority = (Priority)parcel.priority,
						parcelStatus = (ParcelStatus)2,
						customerInParcel = new CustomerInParcel()    //לא יודעת בדיוק מה להציב במשתנים האלו
						{
							id = parcel.id,   //זה
							name = dal.GetCustomerName(id),  //זה
						}
					});
			return tempParcel;
		}
		#endregion
		#region get Customer Received Parcels
		private IEnumerable<ParcelCustomer> getCustomerReceivedParcels(int id)
		{
			var tempParcel = dal.getCustomerReceivedParcels(id)
				.Select(parcel =>
					new ParcelCustomer
					{
						id = parcel.id,
						weight = (Weight)parcel.weight,
						priority = (Priority)parcel.priority,
						parcelStatus = (ParcelStatus)2,
						customerInParcel = new CustomerInParcel()    //לא יודעת בדיוק מה להציב במשתנים האלו
						{
							id = parcel.id,   //זה
							name = dal.GetCustomerName(id),  //זה
						}
					}); 
			return tempParcel;
		}
		#endregion
		#region base station location
		private Location getBaseStationLocation(int stationId)
		{
			IDAL.DO.Station station = dal.GetStation(stationId);
			return new Location { latitude = station.latitude, longitude = station.longitude };
		}
#endregion
        #region find Drone Location
        private Location findDroneLocation(IBL.BO.Drone droneBO)
		{
			return new Location();
		}
		#endregion
		#region DELETE DRONE
		public void deleteDrone(int droneID)
		{
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
		#endregion
		public void updateDroneName(int droneID, string dModel)
		{
		//	if (dModel > 999 && dModel < 10000)//what is the bdika for the model?
			{
				int dIndex = drones.FindIndex(x => x.id == droneID);
				drones[dIndex].droneModel = dModel;
			}
			throw new Exception("the name of the model is not valid\n");//remeber to catch in the main

		}
		public void updateStation(int stationID, int AvlblDCharges, string Name = " ")
		{
			IBL.BO.BaseStation s = GetStation(stationID);
			if (AvlblDCharges != null || Name != " ")
			{
				dal.stationList().ToList().Remove(dal.GetStation(stationID));//not sure if this is how i remove the station im updating

				if (Name != " ")
					s.stationName = Name;
				if (AvlblDCharges != null)
				{
					if (AvlblDCharges < 0)
						throw new Exception("this amount of drone choging slots is not valid!\n");
					s.avilableChargeSlots = AvlblDCharges;      //i need al the slots not just the available one - not sure what this variable means
				}
				addStation(s);
			}

		}
		public void updateCustomer(int customerID, string Name = " ", int phoneNum)
		{
			IBL.BO.Customer c = GetCustomer(customerID);
			if (phoneNum != null || Name != " ")
			{
				dal.stationList().ToList().Remove(dal.GetStation(customerID));//not sure if this is how i remove the station im updating

				if (Name != " ")
					c.Name = Name;
				if (phoneNum != null)
				{
					if (phoneNum < 0)
						throw new Exception("this amount of drone choging slots is not valid!\n");
					c.phoneNumber = phoneNum;      //i need al the slots not just the available one - not sure what this variable means

				}
				addCustomer(c);

			}
		}

		public void SendToCharge(int droneID, int StationID)//have to send the closest sation that has available sattions
		{
			//Location stationLocation= new Location();
			//stationLocation = findClosetBaseStationLocation(fromLocatable);//not sure where and what its from
			IBL.BO.BaseStation station = GetStation(StationID);
			Location stationLoc = new Location { latitude = station.latitude, longitude = station.longitude };
			int droneIndex = drones.FindIndex(x => x.id == droneID);
			station.chargeSlots--;
			drones[droneIndex].batteryStatus = calcMinBatteryRequired(drones[droneIndex]);
			drones[droneIndex].location = stationLoc;
			drones[droneIndex].droneStatus = DroneStatus.charge;
			dal.deleteDrone( dal.GetDrone(droneID);
			addDrone(drones[droneIndex], StationID);
			IDAL.DO.droneCharges DC = new droneCharges { droneId = droneID, stationId = StationID };
			dal.chargingDroneList().ToList().Add(DC);


		}
		public void releasingDrone(int droneID, TimeSpan chargingTime)
		{
			int index = drones.FindIndex(x => x.id == droneID);
			IDAL.DO.droneCharges DC = dal.chargingDroneList().ToList().Find(X => X.droneId == droneID);
			foreach (var s in dal.stationList())
			{
				if (s.id == DC.stationId)
					s.addingChargeSlot();
			}
			dal.chargingDroneList().ToList().Remove(DC);
.			if (drones[index].droneStatus != DroneStatus.charge)
				throw new Exception("this drone is not available!\n");
			//drones[index].batteryStatus = has to calculate based on the amount of time it was charginhg but not so sure what that means...
			drones[index].droneStatus = DroneStatus.available;


		}
	}
}
