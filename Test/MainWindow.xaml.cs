using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Customer> Customers { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            this.Customers = new ObservableCollection<Customer>(Customer.GetCustomers());
            this.DataContext = this;
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Customer c = (Customer)btn.DataContext;

            Customers.Remove(c);
        }

        private void AnonymizeUser(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Customer c = (Customer)btn.DataContext;

            c.LastName = "*****";
        }

        private void EditUser(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Customer c = (Customer)btn.DataContext;

            EditCustomerWindow window = new EditCustomerWindow(c);

            window.ShowDialog();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {   
            CreateUserWindow window = new CreateUserWindow();
            window.OnSaveCustomer += x => this.Customers.Add(x);
            window.ShowDialog();
        }
    }
}
