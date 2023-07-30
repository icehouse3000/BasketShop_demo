using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace basketball_magaz
{
    public partial class add_product_form : Form
    {   
        Database database = new Database();

        public add_product_form()
        {
            InitializeComponent();
            StartPosition= FormStartPosition.CenterScreen;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();

            var name = textBox_name.Text;
            decimal price;
            var manufacturer = textBox_id_m.Text;
            var size = textBox_size.Text;
            var category = textBox_id_cat.Text;
            var availibility = textBox_availibility.Text;
            var amount = textBox_amount.Text;

            if(decimal.TryParse(textBox_price.Text, out price) )
            {
                var addQuery = 
                $"insert into product values ('{name}', '{price}', '{manufacturer}', '{size}', '{category}', '{availibility}', '{amount}')";
            
                var command = new SqlCommand(addQuery, database.getConnection()) ;

                command.ExecuteNonQuery();

                MessageBox.Show("New product added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else 
            {
                MessageBox.Show("Unable to create new product. Please check the input data.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            database.closeConnection();
        }
    }
}
