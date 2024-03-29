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
using System.ComponentModel;
using BO;
using BlApi;
namespace PL
{
    /// <summary>
    /// Interaction logic for parcelListWindow.xaml
    /// </summary>
    public partial class parcelListWindow : Window
    {
        IBl bL;
        private bool isCustomer = false;
        ObservableCollection<ParcelToList> myObservableCollectionParcel;
        private static ParcelToList ptl = new();
        private static Parcel parcel = new();
        static ParcelStatus? statusFilter;
        public  BO.Customer customer { get; set; }
        /// <summary>
        /// constructor to see all the parcels in the system 
        /// </summary>
        /// <param name="bl"></param>
        public parcelListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            comboBoxStatusSelectorParcel.ItemsSource = Enum.GetValues(typeof(BO.ParcelStatus));
            myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
            DataContext = myObservableCollectionParcel;
        }
        /// <summary>
        /// constructior to see all the parcels that the given costomer has
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="C1"></param>
        public parcelListWindow(IBl bl, BO.Customer C1)
        {
            InitializeComponent();
            this.bL = bl;
            customer = C1;
            isCustomer = true;
            comboBoxStatusSelectorParcel.ItemsSource = Enum.GetValues(typeof(BO.ParcelStatus));
            myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists().Where(x => x.senderName == customer.Name || x.receiveName == customer.Name));
            DataContext = myObservableCollectionParcel;
        }
        /// <summary>
        /// adding a parcel to the system 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            parcelWindow wnd = new parcelWindow();
            wnd.ShowDialog();
            myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
            DataContext = myObservableCollectionParcel;
        }
        /// <summary>
        /// details of the parcel and the ability to update by double cliicking 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ParcelToList ptl = new();
                ptl = (ParcelToList)parcelListBox.SelectedItem;
                BO.Parcel parcel = new();
                if (ptl is null)
                {
                    MessageBox.Show($"choose a parcel", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    parcel = bL.GetParcel(ptl.id);
                    DataContext = parcel;
                    new parcelWindow(parcel).ShowDialog();
                    if(isCustomer)
                        myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists().Where(x => x.senderName == customer.Name || x.receiveName == customer.Name));
                    else
                      myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
                    DataContext = myObservableCollectionParcel;
                }
               
            }
            catch (System.NullReferenceException ex) { MessageBox.Show(ex.Message + "the parcel is empty"); };
          
        }
        /// <summary>
        /// delete parcel from the systom by selcting it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParcelToList p = (ParcelToList)parcelListBox.SelectedItem;
                bL.deleteParcel(p.id);
                MessageBox.Show("succsesfully delete parcel!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);                    myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
                if(isCustomer)
                    myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists().Where(x => x.senderName == customer.Name || x.receiveName == customer.Name));
                else
                    myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
                DataContext = myObservableCollectionParcel;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR couldnt delete parcel", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
        /// <summary>
        /// selecting the item in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteParcel_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ParcelToList p = (ParcelToList)parcelListBox.SelectedItem;

        }
        /// <summary>
        /// grouping the parcels by the given feature->sender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupByS_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parcelListBox.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("senderName");
            view.GroupDescriptions.Add(groupDescription);
        }


        /// <summary>
        ///  grouping the parcels by the given feature->reciever
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupByR_Click_(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parcelListBox.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("receiveName");
            view.GroupDescriptions.Add(groupDescription);
        }
   /// <summary>
   /// filtering the list based on the selected feature from the combo box
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
        private void selectStatusParcel(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxStatusSelectorParcel.SelectedIndex != -1)
            {
                statusFilter = (ParcelStatus)comboBoxStatusSelectorParcel.SelectedItem;
                parcelListBox.ItemsSource = bL.allParcelsToList(x => x.parcelStatus == statusFilter);

            }
        }
        /// <summary>
        /// refreshing the list to the most recent changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            if (isCustomer)
                parcelListBox.ItemsSource=bL.GetParcelToLists().Where(x => x.senderName == customer.Name || x.receiveName == customer.Name);
            else
                parcelListBox.ItemsSource = bL.GetParcelToLists();
            comboBoxStatusSelectorParcel.SelectedIndex = -1;
        }
        /// <summary>
        /// combo box filtering the parcels based on the feature selected by the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxDateFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<ParcelToList> ptl = new List<ParcelToList>();
            ptl = bL.GetParcelToLists();
            if (comboBoxDateFilter.SelectedIndex == 0)
                ptl = bL.allParcelsToList().Where(x => bL.GetParcel(x.id).requested > DateTime.Now.AddDays(-1));
            if (comboBoxDateFilter.SelectedIndex == 1)
                ptl = bL.allParcelsToList().Where(x => bL.GetParcel(x.id).requested > DateTime.Now.AddDays(-7));
            if (comboBoxDateFilter.SelectedIndex == 2)
                ptl = bL.allParcelsToList().Where(x => bL.GetParcel(x.id).requested > DateTime.Now.AddMonths(-1));
            if (comboBoxDateFilter.SelectedIndex == 3)
                ptl = bL.allParcelsToList().Where(x => bL.GetParcel(x.id).requested > DateTime.Now.AddYears(-1));
            parcelListBox.ItemsSource = ptl;
        }
    }
}
