using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DalApi;
using DO;
namespace Dal
{
    sealed partial class DalObject : IDal
    {
        #region check station
        public bool checkStation(int id)
        {
            return DataSource.stations.Any(s => s.id == id);
        }
        #endregion
        #region add station
        public void addStation(Station s)
        {
            if (DataSource.stations.Exists(item => item.id == s.id))
                throw new AddException("station already exist");
            DataSource.stations.Add(s);
        }
        #endregion
        #region Get station
        public Station GetStation(int id)
        {

            if (!DataSource.stations.Exists(item => item.id == id))
            {
                throw new findException("station");
            }
            var station = GetStationList().ToList().Find(s => s.id == id);
            return station;
        }
        #endregion
        #region delete station
        public void deleteStation(Station s)
        {
            if (!DataSource.stations.Exists(item => item.id == s.id))
                throw new findException("station");
            DataSource.stations.Remove(s);
        }
        #endregion
        #region update station
        public void updateStation(int stationId, Station s)
        {
            for (int i = 0; i < DataSource.stations.Count; i++)
            {
                if (DataSource.stations[i].id == stationId)
                {
                    DataSource.stations[i] = s;
                    return;
                }
            }
            throw new findException("id not found");
        }
        #endregion
        #region Get station
        public IEnumerable<Station> getStations()
        {
            return DataSource.stations;
        }
        #endregion
        #region Available Charging Slots
        public int AvailableChargingSlots()
        {
            Station station = new Station();
            return station.chargeSlots;
        }
        #endregion
    }
}


