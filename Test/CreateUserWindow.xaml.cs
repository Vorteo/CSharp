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
    public delegate void SaveCustomer(Customer c);

    public partial class CreateUserWindow : Window
    {
        public Customer customer { get; set; } = new Customer();
        public event SaveCustomer? OnSaveCustomer;
        public CreateUserWindow()
        {
            InitializeComponent();
            this.DataContext = this.customer;
        }

        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            this.OnSaveCustomer?.Invoke(this.customer);
            this.Close();
        }
    }
}
