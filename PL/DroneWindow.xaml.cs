﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        //private BlApi.IBl bl = BlApi.BlFactory.GetBl();
        private static DroneToList drt = new();
        private static Drone dr = new();
        private static DroneInParcel droneParcel = new();
        public BackgroundWorker bgWorker { set; get; }//set- to enable to add functions to the events of the thread
        bool isRun;
        bool isClose = true;
        IBl bL;
        public DroneWindow()//add
        {

            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            station.ItemsSource = bL.GetBaseStationToLists().Select(s => s.id); //ספציפית פנויות
            weightCategories.ItemsSource = Enum.GetValues(typeof(Weight));
            update.Visibility = Visibility.Hidden;
            droneInParcel.Visibility = Visibility.Hidden;

        }
        public DroneWindow(BO.Drone drone) //update
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            this.DataContext = drone;
            dr = drone;
            addDrone.Visibility = Visibility.Hidden;
            droneInParcel.Visibility = Visibility.Hidden;
            btnModelUpdate.Visibility = Visibility.Visible;
            if (drone.droneStatus == DroneStatus.available)
            {
                btnSendToCharge.Visibility = Visibility.Visible;
                btnMatchingDroneToParcel.Visibility = Visibility.Visible;
            }
            if (drone.droneStatus == DroneStatus.charge)
            {
                btnRelesingDrone.Visibility = Visibility.Visible;
            }
           

        }
        public DroneWindow(BO.DroneInParcel drone) //update
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            this.DataContext = drone;
            droneParcel = bL.GetDroneInParcel(drone.id);
            addDrone.Visibility = Visibility.Hidden;
            btnModelUpdate.Visibility = Visibility.Hidden;
            btnSendToCharge.Visibility = Visibility.Hidden;
            btnMatchingDroneToParcel.Visibility = Visibility.Hidden;
            btnRelesingDrone.Visibility = Visibility.Hidden;
            update.Visibility = Visibility.Hidden;
          
        }
      

        public static void ValidateString(string string1)
        {
            List<string> invalidChars = new List<string>() { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-" };
            if (string1.Length > 100)
            {
                throw new validException("String too Long");
            }
            else if (!(!string1.Equals(string1.ToLower())))
            {
                throw new validException("Requres at least one uppercase");
            }
            else
            {
                foreach (string s in invalidChars)
                {
                    if (string1.Contains(s))
                    {
                        throw new validException("String contains invalid character: " + s);
                    }
                }
            }
        }
        private void buttonAddDrone_Click(object sender, RoutedEventArgs e)//have to change this to binding
        {

            try
            {
                ValidateString(model.Text);
                SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
                var s = bL.GetStation(Convert.ToInt32(station.SelectedItem));
                if (model.Text == "" || id.Text == null || weightCategories.SelectedIndex == -1 || station.SelectedIndex == -1 || SolidColorBrush.Equals(((SolidColorBrush)txbUpdateModel.BorderBrush).Color, red.Color))
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    BO.DroneToList d = new() { id = Convert.ToInt32(id.Text), droneModel = model.Text, weight = (Weight)weightCategories.SelectedItem, location = s.location };
                    bL.addDrone(d, s.id);
                    MessageBox.Show("succsesfully added a drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ValidateString(txbUpdateModel.Text);
                if (dr.droneModel == txbUpdateModel.Text)
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
                else
                {
                    bL.updateDroneName(Convert.ToInt32(txbDroneId.Text), txbUpdateModel.Text);
                    MessageBox.Show("succsesfully update the drone name!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.SendToCharge(Convert.ToInt32(txbDroneId.Text));
                MessageBox.Show("succsesfully drone sent to charge!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRelesingDrone_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                TimeSpan t = DateTime.Now.TimeOfDay;
                bL.releasingDrone(Convert.ToInt32(txbDroneId.Text), t);
                MessageBox.Show("succsesfully relesing drone charge!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void btnMatchingDroneToParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.matchingDroneToParcel(Convert.ToInt32(txbDroneId.Text));
                MessageBox.Show("succsesfully matched drone to parcel!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void btnDroneInParcelFull_Click(object sender, RoutedEventArgs e)
        {

            DroneWindow wnd = new DroneWindow(bL.returnsDrone(droneParcel.id));  //צריך לחשוב איך אני שמה את הרחפן 
            wnd.ShowDialog();
        }
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) //changes what we see
        {
            int progress = e.ProgressPercentage;
        }
        private void updateDrone() => bgWorker.ReportProgress(0);
        private bool checkStop() => bgWorker.CancellationPending;
        public void updateView()
        {
            var d = bL.returnsDrone(dr.id);
            if (d.droneStatus == DroneStatus.delivery)
            {
                var p = new Parcel();
                p = bL.GetParcel(d.parcelInTransfer.id);
                weightCategories.ItemsSource = Enum.GetValues(typeof(Weight));
                station.ItemsSource = Enum.GetValues(typeof(BaseStation));
            }
        }

        private void btnAutomatic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnRelesingDrone.Visibility = Visibility.Hidden;
                btnModelUpdate.Visibility = Visibility.Hidden;
                btnSendToCharge.Visibility = Visibility.Hidden;
                isRun = true;
                bgWorker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                bgWorker.DoWork += (sender, args) => bL.startDroneSimulation((int)args.Argument,updateDrone,checkStop);
                bgWorker.RunWorkerCompleted += (sender, args) => isRun = false;
                bgWorker.ProgressChanged += (sender, args) => updateDrone();
                bgWorker.RunWorkerAsync(dr.id);
            }
                catch (Exception exp)
                {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                 }

        }
    }
    }
