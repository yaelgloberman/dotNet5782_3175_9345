using IBL.BO;

namespace BL
{
    public interface IBl
    {
        void addCustomer(Customer CustomerToAdd);
        void addDrone(DroneToList droneToAdd, int stationId);
        void addParcel(Parcel parcelToAdd);
        void addStation(BaseStation stationToAdd);
        void deleteDrone(int droneID);
        Customer GetCustomer(int id);
        DroneToList GetDrone(int id);
        BaseStation GetStation(int id);
    }
}