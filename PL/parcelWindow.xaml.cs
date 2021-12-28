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
using System.Collections.ObjectModel;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for parcelWindow.xaml
    /// </summary>
    public partial class parcelWindow : Window
    {
       
        private static ParcelToList parcelToList = new();
        private static Parcel parcel = new();
        BlApi.IBl bL;
        BO.Parcel p;

        public parcelWindow()//add
        {
            bL = BlApi.BlFactory.GetBl();
            InitializeComponent();
        }
        public parcelWindow(BO.Parcel ptl) //update
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
             p= bL.GetParcel(ptl.id);
            this.DataContext = p;
            txbID.IsReadOnly = true;
            if(p.droneInParcel!=null)
            {
                btnDeliveryToCustomer.Visibility = Visibility.Visible;
                lblDroneInParcel.Visibility = Visibility.Visible;
                txbDroneInParcelBattery.Visibility = Visibility.Visible;
                txbDroneInParcelId.Visibility = Visibility.Visible;
                txbDroneInParcelLocationLatitude.Visibility = Visibility.Visible;
                txbDroneInParcelLocationLongitude.Visibility = Visibility.Visible;
                if (p.delivered != null)
                {

                    lblDelivered.Visibility = Visibility.Visible;
                    txbDeliverd.Visibility = Visibility.Visible;
                }
            }
            else
                btnPickUpParcelByDrone.Visibility = Visibility.Visible;
            if (p.requested!=null)
            {
                lblRequested.Visibility = Visibility.Visible;
                txbRequested.Visibility = Visibility.Visible;
            }
            if(p.scheduled!=null)
            {
                lblScheduled.Visibility = Visibility.Visible;
                txbScheduled.Visibility = Visibility.Visible;
            }
            if (p.pickedUp != null)
            {
                lblPickedUp.Visibility = Visibility.Visible;
                txbPickedUp.Visibility = Visibility.Visible;
            }
        }
        private void btnDeliveryToCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.deliveryParcelToCustomer(p.droneInParcel.id);
                MessageBox.Show("succsesfully delivery parcel to customer!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
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
                bL.pickedUpParcelByDrone(p.droneInParcel.id);
                MessageBox.Show("succsesfully pick up parcel by drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
