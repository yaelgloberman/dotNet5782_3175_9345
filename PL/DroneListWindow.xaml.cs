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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {

        BlApi.IBl bL;
        static Weight? weightFilter;
        static DroneStatus? statusFilter;
        public DroneListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            statusSelector.ItemsSource= Enum.GetValues(typeof(BO.DroneStatus));
            DroneListView.ItemsSource = bl.GetDrones();
        }
        private void addDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow wnd = new DroneWindow(bL);
            wnd.ShowDialog();
            fillListView();   
        }
        private void fillListView()
        {
            IEnumerable<DroneToList> d = new List<DroneToList>();
            d = bL.GetDrones();
            if (statusSelector.Text != "")
                d = this.bL.allDrones(x => x.weight == (Weight)weightFilter);
            if (WeightSelector.Text != "")
                d = bL.allDrones(x=>x.droneStatus==(DroneStatus)statusFilter);
            DroneListView.ItemsSource = d;
        }
        private void CloseWindowList_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void selectWeight(Object sender,SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedIndex != -1)
            {
                weightFilter = (Weight)WeightSelector.SelectedItem;
                DroneListView.ItemsSource = bL.allDrones(x => x.weight == weightFilter);
            }
        }
        private void selectStatus(object sender, SelectionChangedEventArgs e)
        {
            if (statusSelector.SelectedIndex != -1)
            {
                statusFilter = (DroneStatus)statusSelector.SelectedItem;
                DroneListView.ItemsSource = bL.allDrones(x => x.droneStatus == statusFilter);
            }
        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drtl = new DroneToList();
            drtl = (DroneToList)DroneListView.SelectedItem;
            new DroneWindow(bL, drtl).ShowDialog();
            fillListView();
        }

       
    }
}
