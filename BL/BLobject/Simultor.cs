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
        bool isRun = true;
        TimeSpan t = DateTime.Now.TimeOfDay;
        public BackgroundWorker bgWorker { set; get; }//set- to enable to add functions to the events of the thread
        IBl bL;
        private static BO.DroneToList drone = new();
        private void updateDrone() => bgWorker.ReportProgress(0);
        private bool checkStop() => bgWorker.CancellationPending;
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e) //when it runs, 
        {
            bL.startDroneSimulation(drone.id, updateDrone, checkStop);
        }

        //    public Simulator(BL bl, int droneId, Action updateDrone, Func<bool> func)
        //    {
        //        bL = bl;
        //        drone = bL.GetDrone(droneId);
        //        BO.Drone d = new();
        //        d = bL.returnsDrone(drone.id);
        //        if (drone.droneStatus==BO.DroneStatus.delivery)
        //        {
        //            if(bl.parcelStatus(bl.GetParcel(d.parcelInTransfer.id))==BO.ParcelStatus.Assigned)
        //            {
        //                bl.pickedUpParcelByDrone(droneId);
        ////     // updateView();
        //                Thread.Sleep(DELAY);
        //            }
        //            bl.deliveryParcelToCustomer(droneId);
        //                // updateView();
        //            Thread.Sleep(DELAY);
        //        }
        //        if (drone.droneStatus == BO.DroneStatus.charge)
        //        {
        //            while (drone.batteryStatus!=90)
        //            {
        //                Thread.Sleep(DELAY);
        //                bl.releasingDrone(droneId, t);
        //                bl.SendToCharge(droneId);
        //                drone = bL.GetDrone(droneId);
        //            }
        //            bl.releasingDrone(droneId, t);
        //            Thread.Sleep(DELAY);
        //            bl.matchingDroneToParcel(droneId);
        //            Thread.Sleep(DELAY);
        //            bl.pickedUpParcelByDrone(droneId);
        //            Thread.Sleep(DELAY);
        //            bl.deliveryParcelToCustomer(droneId);
        //            Thread.Sleep(DELAY);
        //            drone = bL.GetDrone(droneId);

        //        }
        //        if (!bl.GetParcelToLists().Any(x=>x.parcelStatus==BO.ParcelStatus.Created))
        //        {
        //            isRun = false;
        //        }
        //        while(isRun)
        //        {
        //            if(drone.droneStatus==BO.DroneStatus.available)
        //            {
        //                try
        //                {
        //                    bl.matchingDroneToParcel(droneId);
        //                    Thread.Sleep(DELAY);
        //                    drone = bL.GetDrone(droneId);
        //                }
        //                catch
        //                {
        //                    bl.SendToCharge(droneId);
        //                    while(d.batteryStatus!=90.0)
        //                    {
        //                        Thread.Sleep(DELAY);
        //                        bl.SendToCharge(droneId);
        //                        drone = bL.GetDrone(droneId);
        //                    }
        //                    bl.releasingDrone(droneId, t);
        //                    Thread.Sleep(DELAY);
        //                    bl.matchingDroneToParcel(droneId);
        //                    Thread.Sleep(DELAY);
        //                    drone = bL.GetDrone(droneId);

        //                }
        //                bl.pickedUpParcelByDrone(droneId);
        //                Thread.Sleep(DELAY);
        //                bl.deliveryParcelToCustomer(droneId);
        //                Thread.Sleep(DELAY);
        //                drone = bL.GetDrone(droneId);
        //            }
        //            if (!bl.GetParcelToLists().Any(x => x.parcelStatus == BO.ParcelStatus.Created))
        //                isRun = false;
        //        }
        //        if (bL.GetParcelToLists().Count(x => x.parcelStatus == ParcelStatus.Created) == 0)
        //            this.isRun = false;
        //        drone = bL.GetDrone(droneId);

        //    }



        public Simulator(BL bl, int droneId, Action updateDrone, Func<bool> func)
        {
            bL = bl;
            drone = bL.GetDrone(droneId);
            BO.Drone d = new();
            d = bL.returnsDrone(drone.id);
            if (isRun)
            {
                //if (!bl.getParcelsList().Any(x => x.parcelStatus == BO.ParcelStatus.created))
                //    checkRun = false;
                if (drone.droneStatus == BO.DroneStatus.delivery)
                {
                    if (d.parcelInTransfer.parcelStatus == false)
                    {
                        bl.pickedUpParcelByDrone(droneId);
                        Thread.Sleep(DELAY);
                        d = bL.returnsDrone(droneId);

                    }
                    bl.deliveryParcelToCustomer(droneId);
                    Thread.Sleep(DELAY);
                }
                if (drone.droneStatus == BO.DroneStatus.charge)
                {
                    while (drone.batteryStatus != 90.0)
                    {
                        Thread.Sleep(DELAY);
                        bl.releasingDrone(droneId, t);        //fix this!!!!!
                        bl.SendToCharge(droneId);
                        drone = bL.GetDrone(droneId);

                    }
                    bl.releasingDrone(droneId, t);
                    Thread.Sleep(DELAY);
                        bl.matchingDroneToParcel(droneId);
                    
                    Thread.Sleep(DELAY);
                    bl.pickedUpParcelByDrone(droneId);
                    Thread.Sleep(DELAY);
                    bl.deliveryParcelToCustomer(droneId);
                    Thread.Sleep(DELAY);
                    drone = bL.GetDrone(droneId);

                }
            }

            while (isRun)
            {
                drone = bl.GetDrone(droneId);
                if (drone.droneStatus == BO.DroneStatus.available)
                {
                    if (!(bL.GetParcelToLists().Count(x => x.parcelStatus == ParcelStatus.Created) == 0))
                    {
                        try
                        {
                            bl.matchingDroneToParcel(droneId);
                            Thread.Sleep(DELAY);
                        }
                        catch
                        {
                            bl.SendToCharge(droneId);
                            while (drone.batteryStatus != 90.0)
                            {
                                Thread.Sleep(DELAY);
                                bl.releasingDrone(droneId, t);
                                bl.SendToCharge(droneId);
                                drone = bL.GetDrone(droneId);
                            }
                            bl.releasingDrone(droneId, t);
                            Thread.Sleep(DELAY);
                            bl.matchingDroneToParcel(droneId);
                            drone = bL.GetDrone(droneId);
                            Thread.Sleep(DELAY);
                        }
                        bl.pickedUpParcelByDrone(droneId);
                        Thread.Sleep(DELAY);
                        bl.deliveryParcelToCustomer(droneId);
                        Thread.Sleep(DELAY);
                        drone = bL.GetDrone(droneId);

                    }
                    else
                    {
                        bl.SendToCharge(droneId);
                        while (drone.batteryStatus != 90.0)
                        {
                            Thread.Sleep(DELAY);
                            bl.releasingDrone(droneId, t);
                            bl.SendToCharge(droneId);
                            drone = bL.GetDrone(droneId);

                        }
                        bl.releasingDrone(droneId, t);
                        Thread.Sleep(DELAY);
                    }
                }
            }
        }
    }

    
}




