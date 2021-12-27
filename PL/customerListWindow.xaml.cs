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
    /// Interaction logic for customerListWindow.xaml
    /// </summary>
   
    public partial class customerListWindow : Window
    {
        IBl bL;
        ObservableCollection<CustomerInList> myObservableCollectionCustomer;
        private static CustomerInList customerInList = new();
        private static Customer customer = new();
       
        public customerListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bl.GetCustomersToList());
            DataContext = myObservableCollectionCustomer;
        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerInList cil = new();
            cil = (CustomerInList)customerListBox.SelectedItem;
            Customer customer = new();
            customer = bL.GetCustomer(cil.id);
            DataContext = customer;
            new CustomerWindow(customer).ShowDialog();
            myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bL.GetCustomersToList());
            DataContext = myObservableCollectionCustomer;
        }
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow wnd = new CustomerWindow();
            wnd.ShowDialog();
            myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bL.GetCustomersToList());
            DataContext = myObservableCollectionCustomer;
        }
      
    }
}
