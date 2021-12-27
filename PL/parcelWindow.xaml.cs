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
            if (p.delivered!=null)
            {
                if (p.pickedUp == null)
                {
                    btnPickUpParcelByDrone.Visibility = Visibility.Visible;

                }
                else
                    btnDeliveryToCustomer.Visibility = Visibility.Visible;
            }
        }
        private void btnDeliveryToCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.deliveryParcelToCustomer(p.id);
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
                bL.pickedUpParcelByDrone(p.id);
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
