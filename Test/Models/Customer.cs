using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Customer : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _FirstName;
        public string FirstName {
            get { return _FirstName; }
            set { SetField(ref _FirstName, value, nameof(FirstName)); }
        }

        private string _LastName;
        public string LastName {
            get { return _LastName; }
            set { SetField(ref _LastName, value, nameof(LastName)); }
        }

        private int _Age;
        public int Age {
            get { return _Age; }
            set { SetField(ref _Age, value, nameof(Age)); }
        }

         
        protected void SetField<T>(ref T field, T value, string propName)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static List<Customer> GetCustomers()
        {
            return new List<Customer>() {
                new Customer()
                {
                    Id = 1,
                    FirstName = "Jan",
                    LastName = "Novák",
                    Age = 40
                },
                new Customer()
                {
                    Id = 2,
                    FirstName = "Zuzana",
                    LastName = "Báňská",
                    Age = 32
                },
                new Customer()
                {
                    Id = 3,
                    FirstName = "Petra",
                    LastName = "Ostravská",
                    Age = 15
                },
                new Customer()
                {
                    Id = 4,
                    FirstName = "Karel",
                    LastName = "Svoboda",
                    Age = 60
                }
            };
        }
    }
}
