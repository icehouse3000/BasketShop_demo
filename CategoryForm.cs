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
    public partial class CategoryForm : Form
    {
        Database database = new Database();
        public CategoryForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();

            var name = textBox_name.Text;

            var addQuery =
                $"insert into category values ('{name}')";

                var command = new SqlCommand(addQuery, database.getConnection());

                command.ExecuteNonQuery();

                MessageBox.Show("New category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            database.closeConnection();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
