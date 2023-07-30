using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace basketball_magaz
{   
    public partial class ImportsInfo : Form
    {   
        Database database = new Database();

        public ImportsInfo()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();
            

            var id_prod = textBox_id_prod.Text;
            var amount = textBox_amount.Text;
            decimal price;

            if (decimal.TryParse(textBox_price.Text, out price))
            {
                var addQuery =
                $"insert into import_2 values (max(id_imp from import_1), '{id_prod}', '{amount}', '{price}')";

                var command = new SqlCommand(addQuery, database.getConnection());

                command.ExecuteNonQuery();

                MessageBox.Show("New import added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unable to create new product. Please check the input data.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            database.closeConnection();
        }
    }
}
