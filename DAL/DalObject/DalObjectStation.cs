using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject : IDal
    {

        public bool checkStation(int id)
        {
            return DataSource.stations.Any(s => s.id == id);
        }
        public void addStation(Station s)
        {
            if (DataSource.stations.Exists(item => item.id == s.id))
                throw new AddException("station already exist");
            DataSource.stations.Add(s);
        }
        public Station GetStation(int id)
        {

            if (!DataSource.stations.Exists(item => item.id == id))
            {
                throw new findException("station");
            }
            var station = stationList().ToList().Find(s => s.id == id);
            return station;
        }
        public void deleteStation(Station s)
        {
            if (!DataSource.stations.Exists(item => item.id == s.id))
                throw new findException("station");
            DataSource.stations.Remove(s);
        }
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
        public IEnumerable<Station> getStations()
        {
            return DataSource.stations;
        }
        public int AvailableChargingSlots()
        {
            Station station = new Station();
            return station.chargeSlots;
        }
    }
}


