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
    public partial class log_in : Form
    {   

        Database database = new Database();

        public log_in()
        {
            InitializeComponent();
            StartPosition= FormStartPosition.CenterScreen;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var loginUser = textBox_login.Text;
            var passwordUser = textBox_password.Text;

            SqlDataAdapter adapter= new SqlDataAdapter();
            DataTable table= new DataTable();

            string querystring = 
                $"select id_user, login_user, password_user from register where login_user = '{loginUser}' and password_user = '{passwordUser}'";

            SqlCommand command = new SqlCommand(querystring, database.getConnection());

           adapter.SelectCommand= command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                //MessageBox.Show("You have logged in successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 form1= new Form1();
                this.Hide();
                form1.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sorry, couldn't find this account :/", "Non-existent account", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void log_in_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '•';
            textBox_login.MaxLength= 50;
            textBox_password.MaxLength= 50;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            sign_up sign_up = new sign_up();
            sign_up.Show();
            this.Hide();
        }

        private void pictureBox0_Click(object sender, EventArgs e)
        {
            textBox_login.Text = "";
            textBox_password.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = (char)0;

            if (checkBox1.Checked)
            {
                textBox_password.UseSystemPasswordChar = false;
            }
            else
            {
                textBox_password.UseSystemPasswordChar = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
