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
                ValidateString(txbName.Text);
                if (txbName.Text == "" || txbID.Text == null || txbPhoneNumber.Text == null || txbLongitude.Text == null)
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {

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
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnSentParcels_Click(object sender, RoutedEventArgs e)
        {
            sentParcels.Visibility = Visibility.Visible;
            addCustomer.Visibility = Visibility.Hidden;
            updateParcels.Visibility = Visibility.Hidden;
            general.Visibility = Visibility.Hidden;
            sentParcelsListView.ItemsSource = bL.GetCustomers().ToList().Select(x=> x.id== Convert.ToInt32(txbID.Text));   
        }

        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
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

            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


       
    }
}



       