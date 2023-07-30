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
    public partial class sign_up : Form
    {   
        Database database = new Database();
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void sign_up_Load(object sender, EventArgs e)
        {
            textBox_password2.PasswordChar = '•';
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            var login = textBox_login2.Text;
            var password = textBox_password2.Text;

            string querystring = $"insert into register values ('{login}','{password}')";

            SqlCommand command= new SqlCommand(querystring, database.getConnection());

            database.openConnection();

            if(command.ExecuteNonQuery() == 1) 
            {
                MessageBox.Show("You have registered successfully!", "Welcome!", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
            
                Form1 form1 = new Form1();
                this.Hide();
                form1.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Something went wrong and your account hasn't been registered. Please check the data in textboxes you are currently using.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            }
            database.closeConnection();
            
        }

        private Boolean checkUser()
        {
            var loginUser = textBox_login2.Text;
            var passwordUser = textBox_password2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table= new DataTable();

            string querystring = $"select id_user, login_user, password_user from register where login_user = '{loginUser}' and password_user = '{passwordUser}'";
            
            SqlCommand command= new SqlCommand(querystring, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0) 
            {
                MessageBox.Show("User already exists.");
                return true;
            }
            else { return false; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox_password2.PasswordChar = (char)0;

            if (checkBox1.Checked)
            {
                textBox_password2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox_password2.UseSystemPasswordChar = true;
            }
        }

        private void pictureBox0_Click(object sender, EventArgs e)
        {
            textBox_login2.Text = "";
            textBox_password2.Text = "";
        }

        private void label4_Click(object sender, EventArgs e)
        {
            log_in log_in = new log_in();
            log_in.Show();
            this.Hide();
        }
    }
}
