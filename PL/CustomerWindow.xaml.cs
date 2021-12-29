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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private static CustomerInList customerInList = new();
        private static Customer customer = new();
        IBl bL;

        public CustomerWindow()//add
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            updateParcels.Visibility = Visibility.Hidden;
            sentParcels.Visibility = Visibility.Hidden;
        }
        public CustomerWindow(BO.Customer CO) //update
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            customer = bL.GetCustomer(CO.id);
            this.DataContext = customer;
            addCustomer.Visibility = Visibility.Hidden;
            sentParcels.Visibility = Visibility.Hidden;
            txbID.IsReadOnly = true;
            txbLatitude.IsReadOnly = true;
            txbLongitude.IsReadOnly = true;
            



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
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbName.Text == "" ||txbID.Text == "" || txbPhoneNumber.Text == "" || txbLongitude.Text == ""|| txbLatitude.Text=="")
                {
                   throw new Exception("Please enter correct input");
                }
                ValidateString(txbName.Text);


                if (!(Convert.ToInt32(txbID.Text) >= 10000000 && Convert.ToInt32(txbID.Text) <= 1000000000))
                        throw new validException("the id number of the drone is invalid\n");
                    if (!(Convert.ToInt32(txbPhoneNumber.Text) >= 500000000 && (Convert.ToInt32(txbPhoneNumber.Text)) <= 0589999999))
                        throw new validException("the phone number of the Customer is invalid\n");
                    if ((Convert.ToInt32(txbLatitude.Text)) < (double)31 || (Convert.ToInt32(txbLatitude.Text)) > 33.3)
                        throw new validException("the given latitude do not exist in this country/\n");
                    if ((Convert.ToInt32(txbLongitude.Text)) < 34.3 || (Convert.ToInt32(txbLongitude.Text)) > 35.5)
                        throw new validException("the given longitude do not exist in this country/\n");
                    var c = new BO.Customer()
                    {
                        id = Convert.ToInt32(txbID.Text),
                        Name = txbName.Text,
                        phoneNumber = Convert.ToInt32(txbPhoneNumber.Text),
                        location = new Location()
                        {
                            longitude = Convert.ToDouble(txbLongitude.Text),
                            latitude = Convert.ToDouble(txbLatitude.Text)
                        }
                    };
                    bL.addCustomer(c);
                    MessageBox.Show("succsesfully added a drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

      

        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //deliverObservableCollection = new ObservableCollection<ParcelCustomer>(customer.parcelsdelivered);
                //sentParcelsListView.DataContext = deliverObservableCollection;
                //receiveObservableCollection = new ObservableCollection<ParcelinCustomer>(customer.parcelsOrdered);
                //receivedParcelsList.DataContext = receiveObservableCollection;
                //if (txbName.Text == c.Name && Convert.ToInt32(txbPhoneNumber.Text) == c.phoneNumber) 
                //{
                //    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                //    this.Close();
                //}
                //else
                //{
                ValidateString(txbName.Text);
                bL.updateCustomer(Convert.ToInt32(txbID.Text), txbName.Text, Convert.ToInt32(txbPhoneNumber.Text));
                var a = bL.GetCustomersToList();
                MessageBox.Show("succsesfully update customer!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
             
            }

            //catch (Exception exp)
            //{
            //    MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            catch(BO.dosntExisetException exp)
            {
                 MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void btnSentParcels_Click(object sender, RoutedEventArgs e)
        {
            sentParcels.Visibility = Visibility.Visible;
            ReciveParcelsListView.Visibility = Visibility.Hidden;
            addCustomer.Visibility = Visibility.Hidden;
            updateParcels.Visibility = Visibility.Hidden;
            general.Visibility = Visibility.Hidden;
            sentParcelsListView.ItemsSource = customer.SentParcels;

            //try
            //{
            //    DataContext = bL.GetCustomer(Convert.ToInt32(txbID.Text)).SentParcels;
            //}
            //catch (BO.dosntExisetException exp)
            //{
            //    MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        private void btnReceiveParcel_Click(object sender, RoutedEventArgs e)
        {
            sentParcels.Visibility = Visibility.Hidden;
            addCustomer.Visibility = Visibility.Hidden;
            updateParcels.Visibility = Visibility.Hidden;
            general.Visibility = Visibility.Hidden;
            ReciveParcelsListView.ItemsSource = customer.ReceiveParcel;
            //try
            //{
            //    ReciveParcelsListView.ItemsSource = (bL.GetCustomer(Convert.ToInt32(txbID.Text))).ReceiveParcel;

            //}
            //catch (BO.dosntExisetException exp)
            //{
            //    MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
    }
}



       