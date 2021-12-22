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
    /// Interaction logic for AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        IBl bL;
        private static BaseStationToList stationToList = new();
        private static BaseStation station = new();
        
        public AddStationWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;

        }
        private voidbuttonAddStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateString(model.Text);
                SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
                BaseStationToList s = (BaseStationToList)station.SelectedItem;
                if (name.Text == "" || id.Text == null || weightCategories.SelectedItem == null || station.SelectedItem == null || SolidColorBrush.Equals(((SolidColorBrush)txbUpdateModel.BorderBrush).Color, red.Color))
                {
                    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    bL.addDrone(Convert.ToInt32(id.Text), Convert.ToInt32(s.id), model.Text, (Weight)(weightCategories.SelectedItem));
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
