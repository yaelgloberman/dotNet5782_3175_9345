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
using System.Collections.ObjectModel;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for parcelWindow.xaml
    /// </summary>
    public partial class parcelWindow : Window
    {
       
        private static ParcelToList parcelToList = new();
        private static Parcel parcel = new();
        BlApi.IBl bL;

        public parcelWindow()//add
        {
            bL = BlApi.BlFactory.GetBl();
            InitializeComponent();
            general.Visibility = Visibility.Hidden;
            comboBoxP.ItemsSource = Enum.GetValues(typeof(BO.Priority));
            comboBoxW.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            comboBoxS.ItemsSource = bL.GetCustomersToList();
            comboBoxR.ItemsSource = bL.GetCustomersToList();
        }
        public parcelWindow(BO.Parcel ptl) //update
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            add.Visibility = Visibility.Hidden;
            parcel= bL.GetParcel(ptl.id);
            this.DataContext = parcel;
            txbID.IsReadOnly = true;
            if(parcel.droneInParcel!=null)
            {
                btnDeliveryToCustomer.Visibility = Visibility.Visible;
                lblDroneInParcel.Visibility = Visibility.Visible;
                txbDroneInParcelBattery.Visibility = Visibility.Visible;
                txbDroneInParcelId.Visibility = Visibility.Visible;
                txbDroneInParcelLocationLatitude.Visibility = Visibility.Visible;
                txbDroneInParcelLocationLongitude.Visibility = Visibility.Visible;
                if (parcel.delivered != null)
                {
                    lblDelivered.Visibility = Visibility.Visible;
                    txbDeliverd.Visibility = Visibility.Visible;
                }
            }
            else
                btnPickUpParcelByDrone.Visibility = Visibility.Visible;
            if (parcel.requested!=null)
            {
                lblRequested.Visibility = Visibility.Visible;
                txbRequested.Visibility = Visibility.Visible;
            }
            if(parcel.scheduled!=null)
            {
                lblScheduled.Visibility = Visibility.Visible;
                txbScheduled.Visibility = Visibility.Visible;
            }
            if (parcel.pickedUp != null)
            {
                lblPickedUp.Visibility = Visibility.Visible;
                txbPickedUp.Visibility = Visibility.Visible;
            }
        }
        private void btnDeliveryToCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.deliveryParcelToCustomer(parcel.droneInParcel.id);
                MessageBox.Show("succsesfully delivery parcel to customer!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnPickUpParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                bL.pickedUpParcelByDrone(Convert.ToInt32(txbID.Text));
                MessageBox.Show("succsesfully pick up parcel by drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (validException exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch(dosntExisetException exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parcel parcelToAdd = new();

                if (comboBoxS.SelectedItem == null)
                    throw new Exception("choose a sender");
                if (comboBoxR.SelectedItem == null)
                    throw new Exception("choose a recive");
                if (comboBoxW.SelectedItem==null)
                    throw new Exception("choose a weight");
                parcelToAdd.weightCategorie = (Weight)comboBoxW.SelectedItem;
                if (comboBoxP.SelectedItem== null)
                   throw new Exception("choose a priorty");
                parcelToAdd.priority = (Priority)(comboBoxP.SelectedItem);
                CustomerInParcel senderTmp = new()
                {
                    id = Convert.ToInt32(((CustomerInList)comboBoxS.SelectedItem).id),
                    name = ((CustomerInList)(comboBoxS.SelectedItem)).Name
                };
                parcelToAdd.sender = senderTmp;
                CustomerInParcel reciveTemp = new()
                {
                    id = Convert.ToInt32(((CustomerInList)comboBoxR.SelectedItem).id),
                    name = ((CustomerInList)(comboBoxR.SelectedItem)).Name
                };
                parcelToAdd.receive = reciveTemp;
                if (parcelToAdd.receive.id == parcelToAdd.sender.id)
                    throw new Exception("the sender and the reciver can not be the same person");
                bL.addParcel(parcelToAdd);
                MessageBox.Show("succsesfully added a drone!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
