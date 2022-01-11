using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Formats;
using System.ComponentModel;
using System.Threading;
using DalApi;
using BlApi;
using BO;
using System.Runtime.Serialization;
namespace BL
{

    internal class Simulator
    {
        private const double VELOCITY = 1.0;
        private const int DELAY = 500;
        private const double TIME_STEP = DELAY / 1000.0;
        private const double STEP = VELOCITY / TIME_STEP;
        bool isRun=true;
        bool isClose;
        public BackgroundWorker bgWorker { set; get; }//set- to enable to add functions to the events of the thread
        IBl bL;
        private static BO.DroneToList drone = new();
        private void updateDrone() => bgWorker.ReportProgress(0);
        private bool checkStop() => bgWorker.CancellationPending;
        public Simulator(BL bl, int droneId, Action updateDrone, Func<bool> func)
        {
            bL = bl;
            drone = bL.GetDrone(droneId);
            //while (checkStop() == true)
            //{
            if (bL.GetParcelToLists().Count(x => x.parcelStatus == ParcelStatus.Created) == 0)
                this.isRun = false;
            while (this.isRun)
            {
                if (bL.GetParcelToLists().Count(x => x.parcelStatus == ParcelStatus.Created) == 0)
                    this.isRun = false;
                if (drone.droneStatus == DroneStatus.available)
                {
                    try
                    {
                        bL.matchingDroneToParcel(drone.id);
                        bL.pickedUpParcelByDrone(drone.id);
                        bL.deliveryParcelToCustomer(drone.id);
                    }
                    catch
                    {
                        bL.SendToCharge(drone.id);
                        TimeSpan t = new();
                        bL.releasingDrone(drone.id, t);
                        Thread.Sleep(500);
                        bL.matchingDroneToParcel(drone.id);
                        Thread.Sleep(500);
                        bL.pickedUpParcelByDrone(drone.id);
                        Thread.Sleep(500);
                        bL.deliveryParcelToCustomer(drone.id);
                        Thread.Sleep(500);
                    }
                }
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e) //when it runs, 
        {
            bL.startDroneSimulation(drone.id, updateDrone, checkStop);
        }
    }
    
}


