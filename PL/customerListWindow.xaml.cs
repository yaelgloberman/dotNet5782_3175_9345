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
    /// Interaction logic for customerListWindow.xaml
    /// </summary>
   
    public partial class customerListWindow : Window
    {
        BlApi.IBl bL;
        private List<BO.CustomerInList> customerLists = new List<BO.CustomerInList>();
        public customerListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            customerListBox.ItemsSource = bL.GetCustomersToList();
        }
        public customerListWindow()
        {
            InitializeComponent();
            DataContext = customerLists;
        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerInList customer = new CustomerInList();
            customer = (CustomerInList)customerListBox.SelectedItem;
            DataContext = customer;
            new CustomerWindow(customer).ShowDialog();
            //fillListView();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {

        }
        //private void fillListView()
        //{
        //    IEnumerable<CustomerInList> c = new List<CustomerInList>();
        //    c = bL.GetCustomersToList();
        //    customerListBox.ItemsSource = c;  
        //}

    }
}
