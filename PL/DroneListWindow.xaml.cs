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
using System.ComponentModel;

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

        #region constructor
        /// <summary>
        ///the drone list constructor puts the values on the weight comboBox and status comboBox and the list of the drones
        /// </summary> 
        /// <param name="bl"></param>
        public DroneListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            statusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatus));
            myObservableCollectionDrone = new ObservableCollection<DroneToList>(bL.GetDrones());
            DataContext = myObservableCollectionDrone;
        }
        #endregion
        #region opens add drone window
        /// <summary>
        /// function that opening the grid add in the drone window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow wnd = new DroneWindow();
            wnd.ShowDialog();
            myObservableCollectionDrone = new ObservableCollection<DroneToList>(bL.GetDrones());
            DataContext = myObservableCollectionDrone;
        }
        #endregion
        #region closing window
        /// <summary>
        /// function theat when you cick on the button the window closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindowList_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
        #region weight selector & status selector
        /// <summary>
        /// function that letting the user select weight if the user press on wight and a status filter 
        /// so the funcrion returns all the drones in that same weight and specific status  and if he isnt so the wight filter is null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// function that letting the user select status on the drone if the user press a status
        /// so the function returns all the drones in that same status and if he also select weight is returns only both if he isnt selected so the status filter is null  
        /// /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion
        #region double click button
        /// <summary>
        /// functions that when you double click one of the drones in the drones list it opens up the drone if the user press on empy 
        /// space in the list the program shows massage box that says that he sould coose a drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                new DroneWindow(drone,this).ShowDialog();
                myObservableCollectionDrone = new ObservableCollection<DroneToList>(bL.GetDrones());
                DataContext = myObservableCollectionDrone;
            }  

        }
        #endregion
        #region refresh button
        /// <summary>
        /// reset the drone list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Click_1(object sender, RoutedEventArgs e)
        {
            DroneListView.ItemsSource = bL.GetDrones();
            WeightSelector.SelectedIndex = -1;
            statusSelector.SelectedIndex = -1;
        }

        #endregion
        #region help simulator function
        /// <summary>
        /// help function for the simulater that let the user see the drone and the drone list simulator runing at the same time
        /// /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Worker_ProgressChangedDTL(object sender, ProgressChangedEventArgs e)
        {
            myObservableCollectionDrone = new ObservableCollection<DroneToList>(bL.GetDrones());
            DataContext = myObservableCollectionDrone;
        }
        #endregion
        #region Group by button
        /// <summary>
        /// function that making groups of the list by the status of the drone 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupByStatus_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("droneStatus");
            view.GroupDescriptions.Add(groupDescription);
        }
        #endregion
    }
}