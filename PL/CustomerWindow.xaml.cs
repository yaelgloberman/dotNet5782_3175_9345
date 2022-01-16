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
        private static CustomerInParcel customerParcel = new();

        IBl bL;
        #region constructor
        /// <summary>
        /// constructor to the customer window opens up when we want to add customer
        /// </summary>
        public CustomerWindow()
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            ComboUser.ItemsSource = Enum.GetValues(typeof(BO.User));
            updateParcels.Visibility = Visibility.Hidden;
            sentParcels.Visibility = Visibility.Hidden;
            customerParcelGrid.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// constructor that we use when we want to update customer 
        /// </summary>
        /// <param name="CO"></param>
        public CustomerWindow(BO.Customer CO) 
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            customer = bL.GetCustomer(CO.id);
            this.DataContext = customer;
            addCustomer.Visibility = Visibility.Hidden;
            sentParcels.Visibility = Visibility.Hidden;
            customerParcelGrid.Visibility = Visibility.Hidden;
            txbID.IsReadOnly = true;
            txbLatitude.IsReadOnly = true;
            txbLongitude.IsReadOnly = true;
        }
        /// <summary>
        /// constructor that gets customer in parcel
        /// </summary>
        /// <param name="CO"></param>
        public CustomerWindow(BO.CustomerInParcel CO) 
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            customerParcel = bL.GetCustomerParcel(CO.id);
            this.DataContext = customerParcel;
            addCustomer.Visibility = Visibility.Hidden;
            sentParcels.Visibility = Visibility.Hidden;
            general.Visibility = Visibility.Hidden;
            updateParcels.Visibility = Visibility.Hidden;
        }
        #endregion
        #region Validate String
        /// <summary>
        /// function that checks string validatios
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
        #region add customer button
        /// <summary>
        /// function that add a customer 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                var user = ComboUser.SelectedItem.ToString();
                bool flag=false;
                if (user == "Customer")
                    flag = true;
                var c = new BO.Customer()
                {
                    id = Convert.ToInt32(txbID.Text),
                    Name = txbName.Text,
                    PassWord = txbPassword.Text,
                        phoneNumber = txbPhoneNumber.Text,
                        isCustomer=flag,
                        location = new Location()
                        {
                            longitude = Convert.ToDouble(txbLongitude.Text),
                            latitude = Convert.ToDouble(txbLatitude.Text)
                        }
                    };
                    bL.addCustomer(c);
                    MessageBox.Show("succsesfully added a customer!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        #endregion
        #region update customer phone number and name
        /// <summary>
        /// functions that let the user to change the customer name and phone number
        /// if the user didnt change the text nut pressed on update the program showw a massage box
        /// that tells the user tio change the inpur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Customer c = new();
                c = bL.GetCustomer(Convert.ToInt32(txbID.Text));
                if(txbName.Text==c.Name && txbPhoneNumber.Text==c.phoneNumber)
                {
                    MessageBox.Show("Please enter diffrent name or phone number", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
                if (txbName.Text == null && txbPhoneNumber.Text == null)
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
                else
                {
                    ValidateString(txbName.Text);
                    bL.updateCustomer(Convert.ToInt32(txbID.Text), txbName.Text, txbPhoneNumber.Text);
                    var a = bL.GetCustomersToList();
                    MessageBox.Show("succsesfully update customer!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            catch (BO.dosntExisetException exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        #endregion
        #region sent parces and recivie parcels list
        /// <summary>
        /// button that showns all the sent parcels of the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSentParcels_Click(object sender, RoutedEventArgs e)
        {
            sentParcels.Visibility = Visibility.Visible;
            ReciveParcelsListView.Visibility = Visibility.Hidden;
            addCustomer.Visibility = Visibility.Hidden;
            updateParcels.Visibility = Visibility.Hidden;
            general.Visibility = Visibility.Hidden;
            sentParcelsListView.ItemsSource = customer.SentParcels;
        }
        /// <summary>
        /// button that showns all the recive parcels of the customer

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceiveParcel_Click(object sender, RoutedEventArgs e)
        {
            sentParcels.Visibility = Visibility.Hidden;
            addCustomer.Visibility = Visibility.Hidden;
            updateParcels.Visibility = Visibility.Hidden;
            general.Visibility = Visibility.Hidden;
            ReciveParcelsListView.ItemsSource = customer.ReceiveParcel;
          
        }
        #endregion
        #region opens full customer
        /// <summary>
        /// opens up the full customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFullCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow wnd = new CustomerWindow(bL.GetCustomer(customerParcel.id));
            wnd.ShowDialog();
        }
        #endregion
        #region recive parcels list view and sent parcels lidt view mouse double click
        private void ReciveParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var parcel = new ParcelCustomer();
            parcel = (ParcelCustomer)ReciveParcelsListView.SelectedItem;
            try { new parcelWindow(bL.GetParcel(parcel.id)).ShowDialog(); }
            catch (dosntExisetException exp) { MessageBox.Show(exp.Message); }
        }
        private void sentParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var parcel = new ParcelCustomer();
            parcel = (ParcelCustomer)sentParcelsListView.SelectedItem;
            try { new parcelWindow(bL.GetParcel(parcel.id)).ShowDialog(); }
            catch (dosntExisetException exp) { MessageBox.Show(exp.Message); }
        }
        #endregion

    }
}



       