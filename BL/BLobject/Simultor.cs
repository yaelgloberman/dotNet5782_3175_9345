using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Formats;
using System.ComponentModel;
using System.Threading;
using DalApi;
using BlApi;
using BO;

using System.Windows;

using System.Windows.Input;


using System.Runtime.Serialization;
namespace BL
{

    internal class Simulator
    {
        private const double VELOCITY = 1.0;
        private const int DELAY = 500;
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


        public Simulator(BL bl, int droneId, Action updateDrone, Func<bool> func)
        {
            BO.Drone d = new();
            bL = bl;
            drone = bL.GetDrone(droneId);
            while (isRun)
            {
                if (drone.droneStatus == BO.DroneStatus.available)
                {
                    if (!(bL.GetParcelToLists().Count(x => bL.GetParcel(x.id).delivered == null) == 0))
                    {
                        try
                        {
                            bl.matchingDroneToParcel(droneId);
                            updateDrone();
                            Thread.Sleep(DELAY);
                            bl.pickedUpParcelByDrone(droneId);
                            updateDrone();
                            Thread.Sleep(DELAY);
                            bl.deliveryParcelToCustomer(droneId);
                            updateDrone();
                            Thread.Sleep(DELAY);
                        }
                        catch (validException)
                        {
                            isRun = false;
                        }
                        catch
                        {
                            bl.SendToCharge(droneId);
                            while (drone.batteryStatus < 100)
                            {
                                Thread.Sleep(DELAY);
                                bl.releasingDrone(droneId);
                                drone = bl.GetDrone(droneId);
                                bl.SendToCharge(droneId);
                                updateDrone();
                                drone = bl.GetDrone(droneId);
                            }
                            bl.releasingDrone(droneId);
                            drone = bl.GetDrone(droneId);
                            updateDrone();
                            Thread.Sleep(DELAY);
                            drone = bl.GetDrone(droneId);
                        }
                       
                    }
                }
                if (drone.droneStatus == BO.DroneStatus.charge)
                {
                    while (drone.batteryStatus < 100)
                    {
                        Thread.Sleep(DELAY);
                        bl.releasingDrone(droneId);
                        drone = bl.GetDrone(droneId);
                        bl.SendToCharge(droneId);
                        updateDrone();
                        drone = bl.GetDrone(droneId);
                    }
                    bl.releasingDrone(droneId);
                    drone = bl.GetDrone(droneId);
                    updateDrone();
                    Thread.Sleep(DELAY);
                }
                if (drone.droneStatus == BO.DroneStatus.delivery)
                {
                    d = bL.returnsDrone(drone.id);
                    if (d.parcelInTransfer.parcelStatus == false)
                    {
                        bl.pickedUpParcelByDrone(droneId);
                        updateDrone();
                        Thread.Sleep(DELAY);
                    }
                    bl.deliveryParcelToCustomer(droneId);
                    updateDrone();
                    Thread.Sleep(DELAY);
                }
                drone = bl.GetDrone(droneId);
                if ((bL.GetDrones().Count(x => x.parcelId == 0) == 0))
                    isRun = false;
            }
        }
    }
}