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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private static CustomerInList customerInList = new();
        private static Customer customer = new();
        BlApi.IBl bL;
        BO.Customer c;
        public CustomerWindow(CustomerInList CO) //update
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            c = bL.GetCustomer(CO.id);
            this.DataContext = c;
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

        private void btnSentParcels_Click(object sender, RoutedEventArgs e)
        {
            //לפתוח חלון רשימת sentparcels
        }

        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateString(txbName.Text);
                bL.updateCustomer(Convert.ToInt32(txbID.Text), txbName.Text, Convert.ToInt32(txbPhoneNumper.Text));
                MessageBox.Show("succsesfully update customer!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}



       