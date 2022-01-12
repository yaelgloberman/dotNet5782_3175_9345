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
    /// Interaction logic for StationToListWindow.xaml
    /// </summary>
    public partial class StationToListWindow : Window
    {
       
        IBl bL;
        ObservableCollection<BaseStationToList> myObservableCollection;
        private static BaseStationToList ptl = new();
        public StationToListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            myObservableCollection = new ObservableCollection<BaseStationToList>(bL.allStations(x => x.id != 0));
            DataContext = myObservableCollection;
            StationToListView.Items.Clear();
            StationToListView.ItemsSource = myObservableCollection;
            StationToListView.ItemsSource = bL.GetBaseStationToLists().ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var filteredStation = bL.allStations(x => x.avilableChargeSlots > 0);
            StationToListView.ItemsSource = filteredStation;//not sure if it actually returns the filter

        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            UpdateStationWindow wnd = new UpdateStationWindow(bL);
            wnd.ShowDialog();
            myObservableCollection = new ObservableCollection<BaseStationToList>(bL.allStations(x=> x.id!=0));
            DataContext = myObservableCollection;
            StationToListView.ItemsSource = bL.GetBaseStationToLists().ToList();

        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var station = new BaseStationToList();
            station = (BaseStationToList)StationToListView.SelectedItem;
            if(station is null)
            {
                MessageBox.Show($"choose a station", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DataContext = station;
                new UpdateStationWindow(bL, station).ShowDialog();
                myObservableCollection = new ObservableCollection<BaseStationToList>(bL.allStations(x => x.id != 0));
                DataContext = myObservableCollection;
            }
          
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StationToListView.ItemsSource = bL.allStations(x => x.id != 0);

        }

        private void Button_Click_ChargingDrones(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StationToListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationToListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("stationName");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void groupByAvlChargeS_Click(object sender, RoutedEventArgs e)
        {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationToListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("stationName");
                view.GroupDescriptions.Add(groupDescription);
        }
    }
}