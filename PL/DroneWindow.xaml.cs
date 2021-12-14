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
using IBL.BO;
using BL;


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBl bL;
        private static DroneToList drt=new();
        public DroneWindow(IBL.IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            station.ItemsSource = bl.GetBaseStationToLists(); //ספציפית פנויות
            weightCategories.ItemsSource = Enum.GetValues(typeof(Weight));
            update.Visibility = Visibility.Hidden;

        }
        public DroneWindow(IBL.IBl bl, IBL.BO.DroneToList drtl) //update
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
                btnRelesingDrone.Visibility = Visibility.Visible;
            if (drt.droneStatus == DroneStatus.delivery)
            { 
               //btnsen.Visibility = Visibility.Visible;
                btnDeliveryToCustomer.Visibility = Visibility.Visible;
                btnPickUpParcelByDrone.Visibility = Visibility.Visible;

            }
        }

        private void buttonAddDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BaseStationToList s = (BaseStationToList)station.SelectedItem;
                bL.addDrone(Convert.ToInt32(id.Text), Convert.ToInt32(s.id), model.Text, (Weight)(weightCategories.SelectedItem));
                MessageBox.Show("succsesfully added a drone!");
                this.Close();
            }
            catch(AlreadyExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (validException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        private void fillTextbox(DroneToList d)
        {

            txbDroneId.Text = d.id.ToString();
            txbUpdateModel.Text = d.droneModel.ToString();
            txbWeight.Text = d.weight.ToString();
            txbBattery.Text= d.batteryStatus.ToString() + "%";
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
                if (drt.droneModel == txbUpdateModel.Text)
                {
                    throw new Exception("enter a new drone model");
                }
                bL.updateDroneName(drt.id, txbUpdateModel.Text);
                MessageBox.Show("succsesfully update the drone name!");

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            

        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.SendToCharge(drt.id);
                MessageBox.Show("succsesfully drone sent to charge!");
                this.Close();
            }
            catch(Exception exp)
                { MessageBox.Show(exp.Message); }
        }
       

        private void btnRelesingDrone_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                TimeSpan t = DateTime.Now.TimeOfDay;
                bL.releasingDrone(drt.id, t);
                MessageBox.Show("succsesfully relesing drone charge!");
                this.Close();
            }
            catch (Exception exp)
            {

                MessageBox.Show(exp.Message);
            }
        }

        private void btnMatchingDroneToParcel_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                bL.matchingDroneToParcel(drt.id);
                MessageBox.Show("succsesfully matched drone to parcel!");
                this.Close();
            }
            catch (Exception exp)
            { MessageBox.Show(exp.Message); }
        }

        private void btnPickUpParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.pickedUpParcelByDrone(drt.id);
                MessageBox.Show("succsesfully pick up parcel by drone!");
                this.Close();
            }
            catch (Exception exp)
            { MessageBox.Show(exp.Message); }
        }

        private void btnDeliveryToCustomer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                bL.CustomerReceiveParcel(drt.id);
                MessageBox.Show("succsesfully delivery parcel to customer!");
                this.Close();
            }
            catch (Exception exp)
            { MessageBox.Show(exp.Message); }
        }

        private void txbWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            ;
        }
    }
}
