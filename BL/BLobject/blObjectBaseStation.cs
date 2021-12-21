using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DO;
using System.Runtime.Serialization;
namespace BL
{
    public partial class BL : IBl
    {
        #region ADD STATION
        /// <summary>
        /// adding a station to the data source and with basestation regular features and throwing an exception if any of the users info was incorrect
        /// </summary>
        /// <param name="stationToAdd"></param>
        /// <exception cref="AlreadyExistException"></exception>
        /// <exception cref="validException"></exception>
        public void addStation(BaseStation stationToAdd)
        {
            if (dal.getStations().ToList().Exists(item => item.id == stationToAdd.id))
                throw new AlreadyExistException("station already exist");
            List<droneCharges> list = new List<droneCharges>();
            stationToAdd.DroneInChargeList = new List<DroneInCharge>();
            if (!(stationToAdd.id >= 10000000 && stationToAdd.id <= 1000000000))
                throw new validException("the number of the base station id in invalid\n");
            if (stationToAdd.location.longitude < 34.3 || stationToAdd.location.longitude > 35.5)
                throw new validException("the given longitude do not exist in this country\n");
            if (stationToAdd.location.latitude < (double)31 || stationToAdd.location.latitude > 33.3)
                throw new validException("the given latitude do not exist in this country\n");
            if (!(stationToAdd.avilableChargeSlots > 0))
                throw new validException("the given number of available charging slots is negetive\n");
            DO.Station stationDo =
                new DO.Station()
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
            catch (AddException exp)
            {

                throw new AlreadyExistException(exp.Message);
            }
        }
        private int getUnvailableChargeSlots(int stationId)
        {
            int count= dal.chargingGetDroneList().Where(c=>c.stationId== stationId).Count();
            return count;
        }
        #endregion
        #region Get station
        /// <summary>
        /// returns all the basestations in a form of a list form the datasource returns in the bl version of a basestation(regular) fetatures
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        public BaseStationToList GetBaseStationToList(int id)
        {
            BaseStationToList baseStation = new();
            try
            {
                var stationRegular = GetStation(id);
                baseStation.id = stationRegular.id;
                baseStation.stationName = stationRegular.stationName;
                baseStation.avilableChargeSlots = stationRegular.avilableChargeSlots;
                baseStation.unavilableChargeSlots = getUnvailableChargeSlots(id) ;
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
            return baseStation;
        }
        #endregion
        /// <summary>
        /// the programmer recieves  a station in a form of a basestation(reguslar) feateres from  the dal(originally from the datat source
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        public BaseStation GetStation(int id)
        {
            try
            {
                BaseStation baseStation = new BaseStation();
                var station = dal.GetStation(id);
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
                        batteryStatus = getDroneBattery(drone.droneId)
                    });
                return baseStation;
            }
            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
        }
        /// <summary>
        /// returns all the basestations in a form of a list form the datasource returns in the bl version of a basestation( station to list) fetatures
        /// </summary>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        public List<BaseStationToList> GetBaseStationToLists()
        {
            List<BaseStationToList> baseStations = new();
            try
            {
                var stationsDal = dal.GetStationList().ToList();
                foreach (var s in stationsDal)
                { baseStations.Add(GetBaseStationToList(s.id)); }
            }
            catch (ArgumentException) { throw new dosntExisetException(); }
            return baseStations;
        }
        /// <summary>
        /// returns all the basestations in a form of a list form the datasource returns in the bl version of a basestation(regular) fetatures
        /// </summary>
        /// <returns></returns>
        /// <exception cref="dosntExisetException"></exception>
        public List<BaseStation> GetStations()
        {
            List<BaseStation> baseStations = new();
            try
            {
                var stationsDal = dal.GetStationList().ToList();
                foreach (var s in stationsDal)
                { baseStations.Add(GetStation(s.id)); }
            }
            catch (ArgumentException) { throw new dosntExisetException(); }
            return baseStations;
        }
        /// <summary>
        /// an update function that updates a stations name  or available charge slots (or both)
        /// </summary>
        /// <param name="stationID"></param>
        /// <param name="AvlblDCharges"></param>
        /// <param name="Name"></param>
        /// <exception cref="validException"></exception>
        /// <exception cref="dosntExisetException"></exception>
        public void updateStation(int stationID, int AvlblDCharges, string Name = " ")
        {
            try
            {
                DO.Station stationDl = new DO.Station();
                stationDl = dal.GetStation(stationID);
                if (Name != " ")
                    stationDl.name = Name;
                if (AvlblDCharges != 0)
                {
                    if (AvlblDCharges < 0)
                        throw new validException("this amount of drone choging slots is not valid!\n");
                    stationDl.chargeSlots = AvlblDCharges;      //i need al the slots not just the available one - not sure what this variable means
                    dal.updateStation(stationID, stationDl);
                }
            }

            catch (findException exp)
            {
                throw new dosntExisetException(exp.Message);
            }
        }
        public IEnumerable<BaseStationToList> GetBaseStationToList()
        {
            List<BaseStationToList> baseStationToLists = new();
            foreach (var item in dal.GetStationList())
            {
                BaseStationToList station = new BaseStationToList
                {
                    id = item.id,
                    stationName = item.name,
                    avilableChargeSlots = item.chargeSlots,
                    unavilableChargeSlots = getUnvailableChargeSlots(item.id),
                };
                baseStationToLists.Add(station);
            }
            return baseStationToLists.Take(baseStationToLists.Count).ToList();
        }
    }
}

