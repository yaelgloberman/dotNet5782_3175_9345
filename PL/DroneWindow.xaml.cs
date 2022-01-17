using System;
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
        private static DroneToList drt = new();
        private static Drone dr = new();
        private static DroneInParcel droneParcel = new();
        private DroneListWindow dlw;
        public BackgroundWorker bgWorker { set; get; }//set- to enable to add functions to the events of the thread
        bool isRun;
        bool isClose = true;
        IBl bL;
        #region constructor
        /// <summary>
        /// drone window constructor using this when the user wants to add a drone 
        /// </summary>
        public DroneWindow()
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            station.ItemsSource = bL.GetBaseStationToLists().Select(s => s.id);
            weightCategories.ItemsSource = Enum.GetValues(typeof(Weight));
            update.Visibility = Visibility.Hidden;
            droneInParcel.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// the drone constructor using this when the user wants to update the drone by the buttons 
        /// and the buttons visibillity only when the drone can do the diffrent activities
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="droneListWindow"></param>
        public DroneWindow(BO.Drone drone ,DroneListWindow droneListWindow=null)
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
            if(drone.parcelInTransfer!=null)
            {
                btnOpenParcelId.Visibility = Visibility.Visible;
            }
            bgWorker = new()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            dlw = droneListWindow;

        }
        /// <summary>
        /// constructor that gets a drone in parcel
        /// </summary>
        /// <param name="drone"></param>
        public DroneWindow(BO.DroneInParcel drone) 
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
        #endregion
        #region function that cheks the string validation
        /// <summary>
        /// function that cheks the string validation
        /// </summary>
        /// <param name="string1"></param>
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
        #endregion
        #region add drone button
        /// <summary>
        /// function that add a drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddDrone_Click(object sender, RoutedEventArgs e)
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
        #endregion
        #region buttons that updating the drone
        /// <summary>
        /// function that updating the drone name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ValidateString(txbUpdateModel.Text);
                if (dr.droneModel == bL.GetDrone(dr.id).droneModel)
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
        
        /// <summary>
        /// function that sending the drone to charge in a station if function cant it shows massage box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        ///function that realising the drone to charge in a station if function cant it shows massage box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnRelesingDrone_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.releasingDrone(Convert.ToInt32(txbDroneId.Text));
                MessageBox.Show("succsesfully relesing drone charge!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        /// <summary>
        /// function that matching the drone to charge in a station if function cant it shows massage box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #endregion
        #region exit button
        /// <summary>
        /// function that closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
        #region button that opens a drone that mached to a parcel
        /// <summary>
        /// function that opening the drone that mached to a parcel  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDroneInParcelFull_Click(object sender, RoutedEventArgs e)
        {

            DroneWindow wnd = new DroneWindow(bL.returnsDrone(droneParcel.id));  
            wnd.ShowDialog();
        }
        #endregion
        #region function the help the simulator

        /// <summary>
        /// function that updaing the window while the simulator is runing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) //changes what we see
        {
            try
            {
                var d = bL.returnsDrone(dr.id);
                DataContext = d;
            }

            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateDrone() => bgWorker.ReportProgress(0);
        private bool checkStop() => bgWorker.CancellationPending;
        /// <summary>
        /// functions that updating the window
        /// </summary>
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
        /// <summary>
        /// the button that start the simulation 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutomatic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnRelesingDrone.Visibility = Visibility.Hidden;
                btnModelUpdate.Visibility = Visibility.Hidden;
                btnSendToCharge.Visibility = Visibility.Hidden;
                btnMatchingDroneToParcel.Visibility = Visibility.Hidden;
                isRun = true;
                bgWorker.DoWork += (sender, args) => bL.startDroneSimulation((int)args.Argument, updateDrone, checkStop);
                bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
                bgWorker.ProgressChanged += bgWorker_ProgressChanged;
                bgWorker.ProgressChanged += dlw.Worker_ProgressChangedDTL;
                bgWorker.RunWorkerAsync(dr.id);
            }
            catch (Exception exp)
            {
               MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// function that if the sumilation cancalled showing a massage box that it ended
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(!e.Cancelled)
            {
                MessageBox.Show("The simulation has ended", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        /// <summary>
        /// function that when you press the button the simulator is ending
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCanceling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bgWorker.WorkerSupportsCancellation == true)
                {
                    bgWorker.CancelAsync();
                }
            }
            catch (Exception exp) {; }
        }
        #endregion
        #region showing full parcel mached to drone
        /// <summary>
        /// function that opening the parcel window and presenting the parcel that is matched to the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenParcelId_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                parcelWindow wnd = new parcelWindow(bL.GetParcel(dr.parcelInTransfer.id));
                wnd.ShowDialog(); 
            }
            catch (System.NullReferenceException exp) { MessageBox.Show(exp.Message); }
        }
        #endregion
    }
}
