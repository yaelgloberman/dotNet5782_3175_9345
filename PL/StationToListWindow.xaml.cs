using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for StationToListWindow.xaml
    /// </summary>
    public partial class StationToListWindow : Window
    {
        IBl bL;
       // private static DroneToList drt = new();
      //  private static Drone dr = new();
        public StationToListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            StationToListView.ItemsSource = bl.GetBaseStationToLists(); //ספציפית פנויות
           // AvailableSlots.ItemsSource = Enum.GetValues(typeof(BO.Weight));
                                                                        // statusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatus));
        }
        private void fillListView()
        {
            IEnumerable<BaseStationToList> stationToLists = new List<BaseStationToList>();
            stationToLists = bL.GetBaseStationToList();
            StationToListView.ItemsSource = stationToLists;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           var filteredStation= bL.allStations(x=>x.avilableChargeSlots>0);
            StationToListView.ItemsSource = filteredStation;//not sure if it actually returns the filter

        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            AddStation wnd = new AddStation(bL);
            wnd.ShowDialog();
            fillListView();
        }
    } 
}
