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
            InitializeComponent();
            myBl = BL.BL.Instance;
            GridUser.Visibility = Visibility.Visible;
            GridPassword.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden; 
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myBl.CheckValidPassword(NameBox.Text, passwordBox.Text))
                {
                   var C= myBl.GetCustomersToList().ToList().Find(x => x.Password == passwordBox.Text && x.isCustomer);
                    if(C != null)//varifyig the customer is a customer and not a worker 
                    { new MainCustomerWindow().ShowDialog();
                        Close();
                    }
                    else
                        throw new dosntExisetException();
                    // GridPassword.Visibility = Visibility.Hidden;
                }
            }
            catch(dosntExisetException )
            {

                MessageBox.Show("the password is incorrect!");
                passwordBox.Clear();    
            }
        }

        private void Button_Click_Worker(object sender, RoutedEventArgs e)
        {
            
           // ridUser.Visibility = Visibility.Hidden;
            new MainWindow().ShowDialog();
            Close();
        }

        private void Button_Click_Customer(object sender, RoutedEventArgs e)
        {
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility= Visibility.Visible; 

        }

        private void Button_Click_CreateAccount(object sender, RoutedEventArgs e)
        {
            GridLogCustomer.Visibility = Visibility.Hidden;

            var wnd = new CustomerWindow();
            wnd.ShowDialog();
            new MainCustomerWindow().ShowDialog();
         //   Close();
        }

        private void Button_Click_LogIn(object sender, RoutedEventArgs e)
        {
            GridPassword.Visibility = Visibility.Visible;

            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            GridLogCustomer.Visibility=Visibility.Hidden;   
        }

    }
}
