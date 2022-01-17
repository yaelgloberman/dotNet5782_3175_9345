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
        /// <summary>
        /// the first window - the user chooses if he is a customer or a worker
        /// </summary>
        public PasswordWindow()//user type- worker or customer
        {
            InitializeComponent();
            myBl = BlApi.BlFactory.GetBl();
            GridUser.Visibility = Visibility.Visible;
            GridPassword.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden;
            resetPasword.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// the password that the customer goes in as a worker or a costomer
        /// </summary>
        /// <param name="bl"></param>
        public PasswordWindow(BlApi.IBl bl)//password window
        {
            InitializeComponent();
            myBl = BlApi.BlFactory.GetBl();
            GridUser.Visibility = Visibility.Hidden;
            GridPassword.Visibility = Visibility.Visible;
            GridLogCustomer.Visibility = Visibility.Hidden;
            resetPasword.Visibility = Visibility.Hidden;

        }

 /// <summary>
 /// whne pressed enter after the user eneters the password - check of the password is valid->customer
 /// </summary>
 /// <param name="sender"></param>
 /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myBl.CheckValidPassword(NameBox.Text, passwordBox.Password))
                {
                   var C= myBl.GetCustomersToList().ToList().Find(x => x.Password == passwordBox.Password && x.isCustomer);
                    if (C != null)//varifyig the customer is a customer and not a worker 
                    { new MainCustomerWindow(C).ShowDialog();
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
        /// <summary>
        /// clears data of the password and name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            NameBox.Clear();
            passwordBox.Clear();
        }
        /// <summary>
        /// the password window for a worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Worker(object sender, RoutedEventArgs e)
        {
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Visible;
            btnEnterCustomer.Visibility = Visibility.Hidden;
            btnWorkerEnter.Visibility = Visibility.Visible;

        }
        /// <summary>
        /// opens the password window for the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Customer(object sender, RoutedEventArgs e)
        {
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility= Visibility.Visible;
            btnWorkerEnter.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// bring you to the adding customer window where you can add a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_CreateAccount(object sender, RoutedEventArgs e)
        {
            GridLogCustomer.Visibility = Visibility.Hidden;
            new CustomerWindow().ShowDialog();
            new PasswordWindow( myBl).ShowDialog();
            this.Close();
        }
        /// <summary>
        /// brings you to the password window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_LogIn(object sender, RoutedEventArgs e)
        {
            GridPassword.Visibility = Visibility.Visible;

            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// brings you back to teh begining of the project - users type choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            new PasswordWindow().ShowDialog();
            this.Close();

        }

        private void LogInBack_Click(object sender, RoutedEventArgs e)
        {
            new PasswordWindow().ShowDialog();
            this.Close();



        }
        /// <summary>
        /// brings you back to teh begining of the project - users type choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param n
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            new PasswordWindow().ShowDialog();
        }
        /// <summary>
        /// vip brings you straight to see all of the options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try { new MainWindow().ShowDialog(); } catch (System.NullReferenceException exp) { MessageBox.Show(exp.Message); }
            Close();
        }
        /// <summary>
        /// checking the password after entered for worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (myBl.CheckValidPassword(NameBox.Text, passwordBox.Password))
            {
                var C = myBl.GetCustomersToList().ToList().Find(x => x.Password == passwordBox.Password && !x.isCustomer);
                if (C != null)//varifyig the customer is a customer and not a worker 
                {
                    new MainWindow().ShowDialog();
                    Close();
                }
                else
                    try { throw new dosntExisetException(); }
                    catch (dosntExisetException exp) { MessageBox.Show("This Worker Dosent Exist"); }

                // GridPassword.Visibility = Visibility.Hidden;
            }
        }

        private void btnForgetPasword_Click(object sender, RoutedEventArgs e)
        {
            resetPasword.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// the ability to update the password if you forgot it 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = myBl.GetCustomer(Convert.ToInt32(txbEnterId.Text));
                if(c.id!=0)
                {
                    if(c.Name!= txbEnterName.Text)
                    {
                        throw new validException("the name does not matched to the customer id");
                    }
                    myBl.CheckValidPassword(myBl.GetCustomer(Convert.ToInt32(txbEnterId.Text)).Name, txbEnterYourNewP.Text);
                    myBl.resetPassword(Convert.ToInt32(txbEnterId.Text), txbEnterYourNewP.Text);
                    MessageBox.Show("succsesfully reset password!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                }
              else
                {
                    throw new validException("the id is not valid");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void closeButtom_Click(object sender, RoutedEventArgs e)
        {
            resetPasword.Visibility = Visibility.Hidden;
        }
    }
}
