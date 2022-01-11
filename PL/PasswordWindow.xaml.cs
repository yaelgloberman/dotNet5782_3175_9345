﻿using System;
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
            myBl = BlApi.BlFactory.GetBl();
            GridUser.Visibility = Visibility.Visible;
            GridPassword.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Hidden; 

        }
        //public PasswordWindow(BlApi.IBl bl)
        //{
        //    InitializeComponent();
        //    myBl = BlApi.BlFactory.GetBl();
        //    GridUser.Visibility = Visibility.Visible;
        //    GridPassword.Visibility = Visibility.Hidden;
        //    GridLogCustomer.Visibility = Visibility.Hidden;
        //}
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
        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            NameBox.Clear();
            passwordBox.Clear();
        }

        private void Button_Click_Worker(object sender, RoutedEventArgs e)
        {
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility = Visibility.Visible;
            btnEnterCustomer.Visibility = Visibility.Hidden;
            btnWorkerEnter.Visibility = Visibility.Visible;

        }

        private void Button_Click_Customer(object sender, RoutedEventArgs e)
        {
            GridUser.Visibility = Visibility.Hidden;
            GridLogCustomer.Visibility= Visibility.Visible;
            btnWorkerEnter.Visibility = Visibility.Hidden;

        }

        private void Button_Click_CreateAccount(object sender, RoutedEventArgs e)
        {
            GridLogCustomer.Visibility = Visibility.Hidden;

            var wnd = new CustomerWindow();
            wnd.ShowDialog();
            new CustomerWindow().ShowDialog();
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            new PasswordWindow().ShowDialog();


        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            new PasswordWindow().ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try { new MainWindow().ShowDialog(); } catch (System.NullReferenceException exp) { MessageBox.Show(exp.Message); }
            Close();
        }

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
    }
}
