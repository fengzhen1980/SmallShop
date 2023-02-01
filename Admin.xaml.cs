using System;
using System.Collections.Generic;
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

namespace SmallShop
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        SqlConnection con;
        public Admin()
        {
            InitializeComponent();
            string connectionString = "Data Source=LAPTOP-3T67EE6I;Initial Catalog=SmallShopDB;Integrated Security=True";
            con = new SqlConnection(connectionString);

            this.productView.ItemsSource = GetProductData().DefaultView;
            this.productView.GridLinesVisibility = DataGridGridLinesVisibility.All;
        }

        private DataTable GetProductData()
        {
            con.Open();
            string querySql = "select ProductId, ProductName, Amount, Price from product";
            SqlCommand cmd = new SqlCommand(querySql, con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId", typeof(int));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Amount", typeof(int));
            dt.Columns.Add("Price", typeof(double));

            while (sqlDataReader.Read())
            {
                DataRow row = dt.NewRow();
                row["ProductId"] = sqlDataReader.GetValue(0);
                row["ProductName"] = sqlDataReader.GetValue(1);
                row["Amount"] = sqlDataReader.GetValue(2);
                row["Price"] = sqlDataReader.GetValue(3);
                dt.Rows.Add(row);
            }

            con.Close();
            
            return dt;
        }
    }
}
