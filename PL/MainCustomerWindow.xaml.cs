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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainCustomerWindow.xaml
    /// </summary>
    public partial class MainCustomerWindow : Window
    {
        BlApi.IBl myBl;
        BO.Customer C1;
        BO.Parcel P1;
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="c"></param>
        public MainCustomerWindow(BO.CustomerInList c)
        {
            InitializeComponent();
            myBl = BL.BL.Instance;
            C1 = myBl.GetCustomer(c.id);
        }
        private void Button_Click_Customer(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(C1).ShowDialog();
        }

        private void Button_Click_Parcel(object sender, RoutedEventArgs e)
        {
            new parcelListWindow(myBl, C1).ShowDialog();
        }
    }
}
