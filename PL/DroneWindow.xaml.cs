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
using IBL.BO;
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBl bL;
        private static DroneToList drt=new();
        public DroneWindow(IBL.IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            station.ItemsSource = bl.GetBaseStationToLists(); //ספציפית פנויות
            weightCategories.ItemsSource = Enum.GetValues(typeof(Weight));
            update.Visibility = Visibility.Hidden;
        }
        public DroneWindow(IBL.IBl bl,IBL.BO.DroneToList drtl)
        {
            InitializeComponent();
            this.bL = bl;
            addDrone.Visibility = Visibility.Hidden;

        }

        private void buttonAddDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BaseStationToList s = (BaseStationToList)station.SelectedItem;
                bL.addDrone(Convert.ToInt32(id.Text), Convert.ToInt32(s.id), model.Text, (Weight)(weightCategories.SelectedItem));
                MessageBox.Show("succsesfully added a drone!");
                this.Close();
            }
            catch(AlreadyExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (validException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.updateDroneName(Convert.ToInt32(txbDroneId), txbUpdateModel.Text);
                MessageBox.Show("succsesfully update the drone name!");
                this.Close();
            }
            catch (validException exp)
            {
                MessageBox.Show(exp.Message);
            }
            catch(dosntExisetException exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.SendToCharge(Convert.ToInt32(txbDroneId));
                MessageBox.Show("succsesfully drone sent to charge!");
                this.Close();
            }
            catch(unavailableException exp)
                { MessageBox.Show(exp.Message); }
        }
    }
}
