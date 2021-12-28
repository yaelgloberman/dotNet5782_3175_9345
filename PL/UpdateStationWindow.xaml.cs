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
    /// Interaction logic for UpdateStationWindow.xaml
    /// </summary>
    public partial class UpdateStationWindow : Window
    {
        //IBl bL;
        private BlApi.IBl bL;
        BO.BaseStation s;
        public UpdateStationWindow(IBl bl)
        {

            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            GridUpdate.Visibility = Visibility.Hidden;
            GridStationinfo.Visibility = Visibility.Hidden;

        }
        public UpdateStationWindow(IBl bl, BO.BaseStationToList station)
        {

            InitializeComponent();
            bL = bl;
            s =  new BO.BaseStation();
            s = bL.GetStation(station.id);
            DataContext = station;
            GridUpdate.Visibility = Visibility.Visible;
            GridAdd.Visibility = Visibility.Hidden;
            GridStationinfo.Visibility = Visibility.Hidden;

        }

        private void txbID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public static void ValidateString(string string1)
        {
            List<string> invalidChars = new List<string>() { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-" };
            if (string1.Length > 100)
            {
                throw new validException("String too Long");
            }
            else if (!(!string1.Equals(string1.ToLower())))
            {
                throw new validException("Requres at least one uppercase");
            }
            else
            {
                foreach (string s in invalidChars)
                {
                    if (string1.Contains(s))
                    {
                        throw new validException("String contains invalid character: " + s);
                    }
                }
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int chargeslots;
                string name;
                if (TxbchargeSlots.Text == "")
                    chargeslots = -1;
                else
                {
                    chargeslots = Convert.ToInt32(TxbChargeSlotsUpdate.Text); 
                }
                if (TxbNameUpdate.Text=="")
                    name = " ";
                else
                {
                    name = TxbNameUpdate.Text;
                    ValidateString(TxbNameUpdate.Text);
                }
                bL.updateStation(Convert.ToInt32(TxbIdUpdate.Text), chargeslots,name);//have to find the right id
                var s = bL.GetBaseStationToList();
                MessageBox.Show("succsesfully update station!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void buttonAddStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateString(Txbname.Text);
                //SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
                //  BaseStationToList s = (BaseStationToList)station;
                if (Txbname.Text == "" || Txbid.Text == null || TxbchargeSlots.Text == null)
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var location = new Location { latitude = Convert.ToDouble(TxbLatitude.Text), longitude = Convert.ToDouble(TxbLongitude.Text) };
                    var stationToAdd = new BaseStation { id = Convert.ToInt32(Txbid.Text), stationName = Txbname.Text, location = location, avilableChargeSlots = Convert.ToInt32(TxbchargeSlots.Text), unavailableChargeSlots = 0 };///problem with converting here!!!! need to fix!!!
                    bL.addStation(stationToAdd);
                    MessageBox.Show("succsesfully added a Station!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void TxbId_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BO.BaseStation stationObj = new BO.BaseStation();
            stationObj=bL.GetStation(Convert.ToInt32(TxbIdUpdate.Text));
            
            chargingDronesListView.ItemsSource =stationObj.DroneInChargeList;
            GridStationinfo.Visibility = Visibility.Visible;
            
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chargingDronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.DroneInCharge updateDrone = new DroneInCharge();
            updateDrone = (DroneInCharge)chargingDronesListView.SelectedItem;
            Drone realDrone = new Drone();
            realDrone = bL.returnsDrone(updateDrone.id);
            new DroneWindow(realDrone).ShowDialog();
        }

        private void chargingDronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
