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
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBl bL;
        //private BlApi.IBl bl = BlApi.BlFactory.GetBl();
        private static DroneToList drt = new();
        private static Drone dr = new();
        public DroneWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            station.ItemsSource = bl.GetDrones(); //ספציפית פנויות
            weightCategories.ItemsSource = Enum.GetValues(typeof(Weight));
            update.Visibility = Visibility.Hidden;

        }
        public DroneWindow(IBl bl, BO.DroneToList drtl) //update
        {
            InitializeComponent();
            this.bL = bl;
            drt = drtl;
            addDrone.Visibility = Visibility.Hidden;
            btnModelUpdate.Visibility = Visibility.Visible;
            fillTextbox(drt);
            if (drt.droneStatus == DroneStatus.available)
            {
                btnSendToCharge.Visibility = Visibility.Visible;
                btnMatchingDroneToParcel.Visibility = Visibility.Visible;
            }
            if (drt.droneStatus == DroneStatus.charge)
            {
                btnRelesingDrone.Visibility = Visibility.Visible;
            }
            if (drt.droneStatus == DroneStatus.delivery)
            {
                if (bl.GetParcel(drt.parcelId).pickedUp == null)
                {
                    btnPickUpParcelByDrone.Visibility = Visibility.Visible;

                }
                else
                    btnDeliveryToCustomer.Visibility = Visibility.Visible;
            }
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
        private void buttonAddDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateString(model.Text);
                SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
                BaseStationToList s = (BaseStationToList)station.SelectedItem;
                if (model.Text == "" || id.Text == null|| weightCategories.SelectedItem== null ||station.SelectedItem==null||SolidColorBrush.Equals(((SolidColorBrush)txbUpdateModel.BorderBrush).Color,red.Color))
                {
                    MessageBox.Show("Please enter correct input","Error input",MessageBoxButton.OK,MessageBoxImage.Error);
                }
                else
                {
                    bL.addDrone(Convert.ToInt32(id.Text), Convert.ToInt32(s.id), model.Text, (Weight)(weightCategories.SelectedItem));
                    MessageBox.Show("succsesfully added a drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                 
            }
            catch(Exception exp)
            {
                MessageBox.Show($"{exp.Message}","ERROR",MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void fillTextbox(DroneToList d)
        {

            txbDroneId.Text = d.id.ToString();
            txbUpdateModel.Text = d.droneModel.ToString();
            txbWeight.Text = d.weight.ToString();
           // txbBattery.Text= d.batteryStatus.ToString() + "%";
            txbStatus.Text = d.droneStatus.ToString();
            txbParcelID.Text = d.parcelId.ToString();
            txbLongitude.Text = d.location.longitude.ToString();
            txbLatitude.Text = d.location.latitude.ToString();
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
                if (drt.droneModel == txbUpdateModel.Text)
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    bL.updateDroneName(drt.id, txbUpdateModel.Text);
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
                bL.SendToCharge(drt.id);
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
                bL.releasingDrone(drt.id, t);
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
                bL.matchingDroneToParcel(drt.id);
                MessageBox.Show("succsesfully matched drone to parcel!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnPickUpParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.pickedUpParcelByDrone(drt.id);
                MessageBox.Show("succsesfully pick up parcel by drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnDeliveryToCustomer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                bL.deliveryParcelToCustomer(drt.id);
                MessageBox.Show("succsesfully delivery parcel to customer!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        
    }
}
