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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        BlApi.IBl myBl;

        public PasswordWindow()
        {
            myBl = BL.BL.Instance;
            InitializeComponent();
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden; 
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == "1234")
            {
                GridUser.Visibility = Visibility.Visible;
               // GridPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("the password is incorrect!");
                passwordBox.Clear();    
            }
        }

        private void Button_Click_Worker(object sender, RoutedEventArgs e)
        {
            
            GridUser.Visibility = Visibility.Hidden;
            //new MainWindow().ShowDialog();
            Close();
        }

        private void Button_Click_Customer(object sender, RoutedEventArgs e)
        {
            GridLogCustomer.Visibility= Visibility.Visible; 
        }

        private void Button_Click_CreateAccount(object sender, RoutedEventArgs e)
        {
            new CustomerWindow().ShowDialog();
            Close();
        }

        private void Button_Click_LogIn(object sender, RoutedEventArgs e)
        {
            GridPassword.Visibility = Visibility.Visible;   
        }
    }
}
