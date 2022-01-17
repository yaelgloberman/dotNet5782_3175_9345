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
    /// Interaction logic for parcelWindow.xaml
    /// </summary>
    public partial class parcelWindow : Window
    {
       
        private static ParcelToList parcelToList = new();
        private static Parcel parcel = new();
        BlApi.IBl bL;
        /// <summary>
        /// adding a parcel 
        /// </summary>
        public parcelWindow()//add
        {
            bL = BlApi.BlFactory.GetBl();
            InitializeComponent();
            general.Visibility = Visibility.Hidden;
            comboBoxP.ItemsSource = Enum.GetValues(typeof(BO.Priority));
            comboBoxW.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            comboBoxS.ItemsSource = bL.GetCustomersToList().Select(s=>s.id);
            comboBoxR.ItemsSource = bL.GetCustomersToList().Select(s=>s.id);
        }
        /// <summary>
        /// updating a parcel and the info ->hididng and dihiding the appropiate buttons based on the selected parcel
        /// </summary>
        /// <param name="ptl"></param>
        public parcelWindow(BO.Parcel ptl) //update
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBl();
            add.Visibility = Visibility.Hidden;
            try { parcel = bL.GetParcel(ptl.id); }
            catch(System.NullReferenceException exp) { MessageBox.Show(exp.Message); }
            this.DataContext = parcel;
            if (parcel.droneInParcel != null)
            {
                if (parcel.scheduled != null && parcel.pickedUp == null)
                {
                    btnPickUpParcelByDrone.Visibility = Visibility.Visible;
                    btnDroneParcelW.Visibility = Visibility.Visible;
                    btnCustomerSenderParcelW.Visibility = Visibility.Visible;
                    btnCustomerReciverParcelW.Visibility = Visibility.Visible;
                    checkBoxAgree.Visibility = Visibility.Visible;
                }
                if (parcel.pickedUp != null)
                {
                    btnDroneParcelW.Visibility = Visibility.Visible;
                    btnCustomerSenderParcelW.Visibility = Visibility.Visible;
                    btnCustomerReciverParcelW.Visibility = Visibility.Visible;
                    btnDeliveryToCustomer.Visibility = Visibility.Visible;
                    checkBoxAgree.Visibility = Visibility.Visible;
                    btnPickUpParcelByDrone.Visibility = Visibility.Hidden;
                }
                if (parcel.delivered != null)
                {
                    lblDelivered.Visibility = Visibility.Visible;
                    txbDeliverd.Visibility = Visibility.Visible;
                    btnDroneParcelW.Visibility = Visibility.Hidden;
                    btnDeliveryToCustomer.Visibility = Visibility.Visible;
                    checkBoxAgree.Visibility = Visibility.Visible;
                }
            }
            else
            {
                btnDroneParcelW.Visibility = Visibility.Hidden;

                if (parcel.requested != null)
                {
                    lblRequested.Visibility = Visibility.Visible;
                    txbRequested.Visibility = Visibility.Visible;
                    btnDroneParcelW.Visibility = Visibility.Hidden;
                    checkBoxAgree.Visibility = Visibility.Hidden;
                    lblDronInParcel.Visibility = Visibility.Hidden;
                }
            }
            if (parcel.scheduled != null)
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
        /// <summary>
        /// sending the parcel to delivery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// updating the system when the drone pickes up the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPickUpParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.pickedUpParcelByDrone(parcel.droneInParcel.id);
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
        /// <summary>
        /// adding a parcel to the system 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                CustomerInParcel senderTmp = new();
                senderTmp.id = Convert.ToInt32(comboBoxS.SelectedItem);
                senderTmp.name = bL.GetCustomer(Convert.ToInt32(comboBoxS.SelectedItem)).Name;
                parcelToAdd.sender = senderTmp;
                CustomerInParcel reciveTemp = new();
                reciveTemp.id = Convert.ToInt32(comboBoxR.SelectedItem);
                senderTmp.name = bL.GetCustomer(Convert.ToInt32(comboBoxR.SelectedItem)).Name;
                parcelToAdd.receive = reciveTemp;
                if (parcelToAdd.receive.id == parcelToAdd.sender.id)
                    throw new Exception("the sender and the reciver can not be the same person");
                parcelToAdd.id=bL.addParcel(parcelToAdd);
                MessageBox.Show("succsesfully added a parcel!", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show($"{exp.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        /// <summary>
        /// brings you to the details of the drone that is assigned with the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDroneParcel_Click(object sender, RoutedEventArgs e)
        {
            try { DroneWindow wnd = new DroneWindow(bL.returnsDrone(parcel.droneInParcel.id)); wnd.ShowDialog(); }
            catch (System.NullReferenceException exp) { MessageBox.Show(exp.Message); }

        }
        /// <summary>
        /// brings you to the info of the customer that sent the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomerSParcel_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow wnd = new CustomerWindow(bL.GetCustomerParcel(parcel.sender.id));
            wnd.ShowDialog();
        }
        /// <summary>
        /// brings you to the info of the customer that is supposed to recieve the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomerRParcel_Click(object sender, RoutedEventArgs e)
        {

            CustomerWindow wnd = new CustomerWindow(bL.GetCustomerParcel(parcel.receive.id));
            wnd.ShowDialog();

        }


    }
}
