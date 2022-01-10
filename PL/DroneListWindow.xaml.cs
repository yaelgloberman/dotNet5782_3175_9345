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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {

        IBl bL;
        ObservableCollection<DroneToList> myObservableCollectionDrone;
        private static DroneToList dtl = new();
        private static Drone drone = new();
        static Weight? weightFilter;
        static DroneStatus? statusFilter;

        public DroneListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            statusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatus));
            myObservableCollectionDrone = new ObservableCollection<DroneToList>(bL.GetDrones());
            DataContext = myObservableCollectionDrone;
        }
        private void addDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow wnd = new DroneWindow();
            wnd.ShowDialog();
            myObservableCollectionDrone = new ObservableCollection<DroneToList>(bL.GetDrones());
            DataContext = myObservableCollectionDrone;

        }

        private void CloseWindowList_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void selectWeight(Object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedIndex != -1)
            {

                weightFilter = (Weight)WeightSelector.SelectedItem;
                if (statusFilter == null)
                {
                    DroneListView.ItemsSource = bL.allDrones(x => x.weight == weightFilter);

                }
                else
                {
                    DroneListView.ItemsSource = bL.allDrones(x => x.weight == weightFilter && x.droneStatus == statusFilter);
                }
            }
            else
            {
                weightFilter = null;
            }
        }
        private void selectStatus(object sender, SelectionChangedEventArgs e)
        {
            if (statusSelector.SelectedIndex != -1)
            {
                statusFilter = (DroneStatus)statusSelector.SelectedItem;
                if (weightFilter == null)
                {
                    DroneListView.ItemsSource = bL.allDrones(x => x.droneStatus == statusFilter);
                }
                else
                {
                    DroneListView.ItemsSource = bL.allDrones(x => x.droneStatus == statusFilter && x.weight == weightFilter);
                }
            }


        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drtl = new();
            drtl = (DroneToList)DroneListView.SelectedItem;
            Drone drone = new();
            if (drtl == null)
                MessageBox.Show($"choose a drone", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                drone = bL.returnsDrone(drtl.id);
                DataContext = drone;
                new DroneWindow(drone).ShowDialog();
                myObservableCollectionDrone = new ObservableCollection<DroneToList>(bL.GetDrones());
                DataContext = myObservableCollectionDrone;
            }  

        }


        private void refresh_Click_1(object sender, RoutedEventArgs e)
        {
            DroneListView.ItemsSource = bL.GetDrones();
            WeightSelector.SelectedIndex = -1;
            statusSelector.SelectedIndex = -1;
        }

        private void groupByStatus_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("droneStatus");
            view.GroupDescriptions.Add(groupDescription);
        }
    }
}