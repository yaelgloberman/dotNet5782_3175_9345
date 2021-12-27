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
    /// Interaction logic for Temp.xaml
    /// </summary>
    public partial class Temp : Window
    {
        IBl bL;

        public Temp(IBl bl)
        {
            //InitializeComponent();
            //bL = bl;
            //stationsId = new ComboBox();
            //stationsId.ItemsSource = bl.GetBaseStationToList();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == "1234")
            {
                new MainWindow().ShowDialog();
            }
            else
            {
                MessageBox.Show("the password is incorrect!");
                passwordBox.Clear();    
            }
        }
    }
}
