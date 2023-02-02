using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SmallShop
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        SqlConnection con;
        public string[] productNames { get; set; }
        public string updateSqls { get; set; }
        public double totalAll { get; set; }

        public Sales()
        {
           
            InitializeComponent();
            string connectionString = "Data Source=laptop-he38d91k\\sqlexpress;Initial Catalog=SmallShopDB;Integrated Security=True";
            con = new SqlConnection(connectionString);

            productNames = GetProductData();
            DataContext = this;
        }

        private string[] GetProductData()
        {
            con.Open();

            string querySql = "select ProductName from product";
            SqlCommand cmd = new SqlCommand(querySql, con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            ArrayList productNameList = new ArrayList();

            int counter = 0;

            while (sqlDataReader.Read())
            {
                //get rows
                counter++;
                productNameList.Add(sqlDataReader.GetValue(0).ToString());
            }

            string[] productNames = new string[counter];

            for(int i = 0; i < productNameList.Count; i++)
            {
                productNames[i] = (string)productNameList[i];

            }
            
            con.Close();
            return productNames;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(this.productComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select product.");
                return;
            }
            if(this.AmountTextBox.Text.Length == 0) 
            {
                MessageBox.Show("Please input amont.");
                return;
            }

            string productName = (string)productComboBox.SelectedItem;
            int amount = Convert.ToInt32(this.AmountTextBox.Text);
            double price = 0;
            double total = 0;

            con.Open();
            string querySql = "select ProductName, Amount, Price from product where ProductName=@ProductName";
            SqlCommand cmd = new SqlCommand(querySql, con);
            cmd.Parameters.AddWithValue("@ProductName", productName);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            int amountInventory = 0;
            while (sqlDataReader.Read())
            {
                amountInventory = Convert.ToInt32(sqlDataReader.GetValue(1).ToString());
                price = Convert.ToDouble(sqlDataReader.GetValue(2).ToString());
            }

            if(amount > amountInventory)
            {
                MessageBox.Show("Stock is out, please re-enter amount.");
                con.Close();
                return;
            }

            updateSqls = updateSqls + "update product set Amount = Amount - " + this.AmountTextBox.Text + "where ProductName = '" + this.productComboBox.SelectedItem + "'; ";
            total = amount * price;
            if(this.BillTextBox.Text.Length == 0)
            {
                this.BillTextBox.Text = productName.PadRight(18, ' ') + amount.ToString().PadRight(10, ' ') + "$" + total.ToString() + "\n";
                totalAll = totalAll + total;
                this.BillTextBox.Text = this.BillTextBox.Text + "total price: " + totalAll;
            }
            else
            {
                string tempStr = this.BillTextBox.Text;

                tempStr = tempStr.Substring(0, tempStr.Length- (tempStr.Length - tempStr.LastIndexOf("\n")));
                tempStr = tempStr + "\n";
                tempStr = tempStr + productName.PadRight(18, ' ') + amount.ToString().PadRight(10, ' ') + "$" + total.ToString() + "\n";
                totalAll = totalAll + total;

                this.BillTextBox.Text = tempStr + "total price: " + totalAll;

            }
            this.productComboBox.SelectedIndex = -1;
            this.AmountTextBox.Text = "";

            con.Close();
        }

        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(updateSqls, con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            updateSqls = "";
            totalAll= 0;

            MessageBox.Show(this.BillTextBox.Text);
            this.BillTextBox.Clear();
            con.Close() ;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWin = new MainWindow();
            this.Close();
            MainWin.Show();
        }
    }
}
