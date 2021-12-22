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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBl myBl;
        public MainWindow()
        {
            myBl = BL.BL.Instance;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow wnd = new DroneListWindow(myBl);
            wnd.Show();
        }

        private void customerLists_Click(object sender, RoutedEventArgs e)
        {
            customerListWindow wndC = new customerListWindow(myBl);
            wndC.Show();
        }

        private void btnStationLists_Click(object sender, RoutedEventArgs e)
        {
            StationToListWindow stationWindow=new StationToListWindow(myBl);
            stationWindow.Show();
        }
    }
}
