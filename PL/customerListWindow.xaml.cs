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
        bool isWorker = false;
        /// <summary>
        /// the constructor of a regular customers 
        /// </summary>
        /// <param name="bl"></param>
        public customerListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bl.GetCustomersToList().Where(x => x.isCustomer));
            DataContext = myObservableCollectionCustomer;
        }
        /// <summary>
        /// the constryuctor of workers
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="bl2"></param>
        public customerListWindow(IBl bl, IBl bl2)
        {
            InitializeComponent();
            this.bL = bl;
            isWorker = true;
            myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bl.GetCustomersToList().Where(x => !x.isCustomer));
            DataContext = myObservableCollectionCustomer;
        }
        /// <summary>
        /// to see the details of the customer and the ability to update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerInList cil = (CustomerInList)customerListBox.SelectedItem;
            Customer customer = new();
            if (cil == null)
                MessageBox.Show($"choose a customer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                customer = bL.GetCustomer(cil.id);
                DataContext = customer;
                new CustomerWindow(customer).ShowDialog();
                if(isWorker)
                     myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bL.GetCustomersToList().Where(x => !x.isCustomer)); 
                else
                     myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bL.GetCustomersToList().Where(x => x.isCustomer)); 
                DataContext = myObservableCollectionCustomer;
            }
        }
        /// <summary>
        /// the adding act of a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow wnd = new CustomerWindow();
            wnd.ShowDialog();
            if (isWorker)
                myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bL.GetCustomersToList().Where(x => !x.isCustomer));
            else
                myObservableCollectionCustomer = new ObservableCollection<CustomerInList>(bL.GetCustomersToList().Where(x => x.isCustomer));
            DataContext = myObservableCollectionCustomer;
        }
        #endregion
    }
}
