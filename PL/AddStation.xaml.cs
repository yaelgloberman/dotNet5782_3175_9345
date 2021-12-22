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
    /// Interaction logic for AddStation.xaml
    /// </summary>
    public partial class AddStation : Window
    {
        IBl bL;
        private static BaseStationToList stationToList = new();
        private static BaseStation station = new();
        public AddStation(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
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
        private void buttonAddStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateString(name.Text);
                SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
              //  BaseStationToList s = (BaseStationToList)station;
                if (name.Text == "" || id.Text == null || chargeSlots.Text == null)
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var location = new Location { latitude = Convert.ToInt32(Latitude.Text), longitude = Convert.ToInt32(Longitude.Text) };
                    var stationToAdd = new BaseStation { id = Convert.ToInt32(id.Text), stationName = name.Text, location = location, avilableChargeSlots = Convert.ToInt32(chargeSlots), unavailableChargeSlots = 0 };
                bL.addStation(stationToAdd);
                    MessageBox.Show("succsesfully added a drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
