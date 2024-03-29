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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
//hihi
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
            wnd.ShowDialog();
        }

        private void customerLists_Click(object sender, RoutedEventArgs e)
        {
            customerListWindow wndC = new customerListWindow(myBl);
            wndC.ShowDialog();
        }
        private void btnParcelLists_Click(object sender, RoutedEventArgs e)
        {
            parcelListWindow wndP = new parcelListWindow(myBl);
            wndP.ShowDialog();
        }

        private void btnStationLists_Click_1(object sender, RoutedEventArgs e)
        {
            StationToListWindow wndS = new StationToListWindow(myBl);
             wndS.Show(); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var wndPassword = new PasswordWindow();
            wndPassword.ShowDialog();
        }


        private void btnWorkers_Click(object sender, RoutedEventArgs e)
        {
            customerListWindow wndC = new customerListWindow(myBl,myBl);
            wndC.ShowDialog();
        }
    }
}
