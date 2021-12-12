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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBl bL;
        public DroneListWindow(IBL.IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            weigjtSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatus));
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
            if (ComboStatusSelector.Text != "")
                d = this.bL.droneFilterStatus((DroneStatus)ComboStatusSelector.SelectedItem);
            if (weigjtSelector.Text != "")
                d = bL.droneFilterWeight((Weight)weigjtSelector.SelectedItem);
            DroneListView.ItemsSource = d;
        }
        //private void fillTextbox(DroneToList d)
        //{

        //    statusTxt.Text = d.status.ToString();
        //    weightTxt.Text = d.weight.ToString();
        //    updateIdtxt.Text = d.ID.ToString();
        //    updateModeltxt.Text = d.droneModel.ToString();
        //    batteryTxt.Text = d.battery.ToString() + "%";
        //    parcelIdTxt.Text = d.parcelNumber.ToString();
        //    longitudeTxt.Text = d.currentLocation.longitude.ToString();
        //    latitudeTxt.Text = d.currentLocation.latitude.ToString();
        //}
        private void CloseWindowList_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void filter(object sender, SelectionChangedEventArgs e)
        {
             DroneListView.ItemsSource = bL.droneFilterStatus((DroneStatus)ComboStatusSelector.SelectedItem);
        }
        private void select(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = bL.droneFilterWeight((Weight)weigjtSelector.SelectedItem);
        }
        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drtl = new DroneToList();
            drtl = (DroneToList)DroneListView.SelectedItem;
            new DroneWindow(bL, drtl).ShowDialog();
        }
    }
}
