using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using testt.Models;

namespace testt
{
    public partial class ExchangeWindow : Window
    {
        public Kurz Kurz { get; set; }
        public Exchange Exchange { get; set; } = new Exchange();
        public ExchangeWindow(Kurz k)
        {
            this.Kurz = k;
            InitializeComponent();

            this.DataContext = Exchange;

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (fnameBox.Text == "")
            {
                MessageBox.Show("Chybi jmeno");
                return;
            }

            if (lnameBox.Text == "")
            {
                MessageBox.Show("Chybi prijmeni");
                return;
            }

            string rgx = @"^[0-9]{6}\/[0-9]{4}$";
            Regex regex = new Regex(rgx, RegexOptions.Compiled);
            if (rnumberBox.Text == "")
            {
                MessageBox.Show("Chybi rodne cislo");
                return;
            }
            else if (!regex.IsMatch(rnumberBox.Text))
            {
                MessageBox.Show("Rodne cislo ve spatnem formatu");
                return;
            }


            Exchange.fname = fnameBox.Text;
            Exchange.lname = lnameBox.Text;
            Exchange.rnumber = rnumberBox.Text;
            Exchange.code = Kurz.code;

            if(!int.TryParse(countBox.Text, out int count))
            {
                MessageBox.Show("Musis zadat cislo v mnozstvi");
                return;
            }

            Exchange.count = count;


            if(r1.IsChecked == true)
            {
                double result = (double)(int.Parse(countBox.Text) / Kurz.exchangeRate)* Kurz.count;
                Exchange.exchangeValue = result;
            }
            else if(r2.IsChecked == true)
            {
                double result = (double)(int.Parse(countBox.Text) * Kurz.exchangeRate) / Kurz.count;
                Exchange.exchangeValue = result;
            }

            CustomSerializer.Serialize(Exchange);

            this.Hide();
        }
    }
}
