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
            string connectionString = "Data Source=DESKTOP-N6HDOK0\\sqlexpress;Initial Catalog=SmallShopDB;Integrated Security=True";

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




        
        
        
        
        

        private void View_date_Click(object sender, RoutedEventArgs e)
        {
            try
            {   
                con.Open();
                
                string Query = "Select ProductId,ProductName,Amount,Price from product";
                
                SqlCommand cmd = new SqlCommand(Query, con);
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                DataTable dt = new DataTable();
                da.Fill(dt);
                productView.ItemsSource = dt.AsDataView();
                DataContext = da;
                con.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //open the Database Connection
                con.Open();
                string query = "insert into product values(@ProductId,@ProductName,@Amount,@Price)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", int.Parse(ProductIdTextBox.Text));//Convert.ToInt64(Id.Text)
                                                                                     //we need to call the textBox name and then grab the text
                cmd.Parameters.AddWithValue("@ProductName", ProductNameTextBox.Text);
                cmd.Parameters.AddWithValue("@Amount", int.Parse(AmountTextBox.Text));
                cmd.Parameters.AddWithValue("@Price", float.Parse(PriceTextBox.Text));
                //we now need to execute our Query

                cmd.ExecuteNonQuery();//?
                MessageBox.Show("Inserted perfectly to the Database");
                con.Close();
                ProductIdTextBox.Clear();
                ProductNameTextBox.Clear();
                AmountTextBox.Clear();
                PriceTextBox.Clear();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //open the Database Connection
                con.Open();
                string Query = "Update product set ProductName=@ProductName,Amount=@Amount,Price=@Price where ProductId=@ProductId";
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@ProductId", int.Parse(ProductIdTextBox.Text));
                cmd.Parameters.AddWithValue("@ProductName", ProductNameTextBox.Text);
                cmd.Parameters.AddWithValue("@Amount", float.Parse(AmountTextBox.Text));
                cmd.Parameters.AddWithValue("@Price", float.Parse(PriceTextBox.Text));
                //we now need to execute our Query

                cmd.ExecuteNonQuery();
                MessageBox.Show("Updated Perectly to the Database");
                con.Close();
                ProductIdTextBox.Clear();
                ProductNameTextBox.Clear();
                AmountTextBox.Clear();
                PriceTextBox.Clear();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();
                string Query = "Delete product where ProductId=@ProductId";
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@ProductId", int.Parse(ProductIdTextBox.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Delete successfully");
                con.Close();
                ProductIdTextBox.Clear();
                ProductNameTextBox.Clear();
                AmountTextBox.Clear();
                PriceTextBox.Clear();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();
                string Query = "Select ProductName,Amount,Price from product where ProductId=@ProductName";


                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@ProductId", int.Parse(ProductIdTextBox.Text));
                SqlDataReader sqlReader = cmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    ProductNameTextBox.Text = (string)sqlReader.GetValue(0);
                    AmountTextBox.Text = sqlReader.GetValue(1).ToString();
                    PriceTextBox.Text = sqlReader.GetValue(2).ToString();
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWin = new MainWindow();
            this.Close();
            MainWin.Show();
        }
    }
}
