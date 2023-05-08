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
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class EditCustomerWindow : Window
    {
        public Customer customer { get; set;}
        public EditCustomerWindow(Customer c)
        {
            this.customer = c;

            InitializeComponent();
            
            this.DataContext = this;
        }

        private void SaveUser(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
