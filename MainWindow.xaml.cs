using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace SmallShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void admin_button_Click(object sender, RoutedEventArgs e)
        {
            Admin AdminWin = new Admin();
            this.Close();
            AdminWin.Show();
        }

        private void sales_button_Click(object sender, RoutedEventArgs e)
        {
            Sales SalesWin = new Sales();
            this.Close();
            SalesWin.Show();
        }
    }
}
