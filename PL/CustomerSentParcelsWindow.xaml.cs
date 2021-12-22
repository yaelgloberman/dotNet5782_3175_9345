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
using BlApi;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerSentParcelsWindow.xaml
    /// </summary>
    /// 
    public partial class CustomerSentParcelsWindow : Window
    {
        BlApi.IBl bL;
        private List<ParcelCustomer> CustomerReceiveParcel = new List<ParcelCustomer>();
        public CustomerSentParcelsWindow()
        {
            InitializeComponent();
            DataContext = CustomerReceiveParcel;
            //CustomerReceiveParcel = bL.get;
        }
    }
}
