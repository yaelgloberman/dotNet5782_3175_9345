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
    ///  BlApi.IBl bL;
   
    public partial class customerListWindow : Window
    {
        BlApi.IBl bL;

        public customerListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            customerListBox.ItemsSource = bL.GetCustomersToList();
        }
        private List<BO.CustomerInList> customerLists = new List<BO.CustomerInList>();
        public customerListWindow()
        {
            InitializeComponent();
            DataContext = customerLists;
        }
        private void fillListView()
        {
            IEnumerable<CustomerInList> c = new List<CustomerInList>();
            c = bL.GetCustomersToList();
            customerListBox.ItemsSource = c;  
        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerInList customer = new CustomerInList();
            customer = (CustomerInList)customerListBox.SelectedItem;
            new CustomerWindow(bL, customer).ShowDialog();
            fillListView();
        }
      
    }
}
