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
using System.Collections.ObjectModel;
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
        ObservableCollection<ParcelToList> myObservableCollectionParcel;
        private static ParcelToList ptl = new();
        private static Parcel parcel = new();
        public parcelListWindow(IBl bl)
        {
            InitializeComponent();
            this.bL = bl;
            myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
            DataContext = myObservableCollectionParcel;
        }
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            parcelWindow wnd = new parcelWindow();
            wnd.ShowDialog();
            myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
            DataContext = myObservableCollectionParcel;
        }
        private void DoubleClick(object sender, RoutedEventArgs e)
        {
            ParcelToList ptl = new();
            ptl = (ParcelToList)parcelListBox.SelectedItem;
            BO.Parcel parcel = new();
            parcel = bL.GetParcel(ptl.id);
            DataContext = parcel;
            new parcelWindow(parcel).ShowDialog();
            myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
            DataContext = myObservableCollectionParcel;
        }
       
        private void btnDeleteParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParcelToList p = (ParcelToList)parcelListBox.SelectedItem;
                bL.deleteParcel(p.id);
                MessageBox.Show("succsesfully delete parcel!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                myObservableCollectionParcel = new ObservableCollection<ParcelToList>(bL.GetParcelToLists());
                DataContext = myObservableCollectionParcel;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR couldnt delete parcel", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void btnDeleteParcel_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ParcelToList p = (ParcelToList)parcelListBox.SelectedItem;
           
        }
    }

}
