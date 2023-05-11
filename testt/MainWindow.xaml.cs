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
using testt.Models;

namespace testt
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Kurz> Kurzy { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Init();

            this.DataContext = this;
        }

        public async void Init()
        {
            this.Kurzy = new ObservableCollection<Kurz>(await DownloadData.Download());
        }

        private void ExchangeButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Kurz kurz = (Kurz)btn.DataContext;

            ExchangeWindow exchangeWindow = new ExchangeWindow(kurz);
            exchangeWindow.ShowDialog();
        }
    }
}
