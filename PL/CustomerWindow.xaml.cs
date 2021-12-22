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
        public CustomerWindow()
        {
            InitializeComponent();
        }
        BlApi.IBl bL;
        private static CustomerInList customerInList = new();
        private static Customer customer = new();
        public CustomerWindow(IBl bl, BO.CustomerInList cust) //update
        {
            InitializeComponent();
            this.bL = bl;
            customerInList = cust;
            Customer c = bl.GetCustomer(cust.id);
            fillTextbox(c);
        }
        private void fillTextbox(Customer c)
        {
            txbID.Text = c.id.ToString();
            txbName.Text = c.Name.ToString();
            txbPhoneNumper.Text = c.phoneNumber.ToString();
            txbLongitude.Text = c.location.longitude.ToString();
            txtLatitude.Text = c.location.latitude.ToString();
        }

        private void btnSentParcels_Click(object sender, RoutedEventArgs e)
        {
            //לפתוח חלון רשימת sentparcels
        }
    }
}
