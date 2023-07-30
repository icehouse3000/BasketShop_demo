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
using static System.Data.SqlClient.SqlConnection;



namespace basketball_magaz
{   
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class Form1 : Form
    {
        Database database = new Database();

        int selectedRow;
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_prod", "id");
            dataGridView1.Columns.Add("prod_name", "name");
            dataGridView1.Columns.Add("prod_price", "price");
            dataGridView1.Columns.Add("id_m", "manufacturer id");
            dataGridView1.Columns.Add("size", "size");
            dataGridView1.Columns.Add("id_category", "category id");

            dataGridView1.Columns.Add("availibility", "availibility");
            dataGridView1.Columns.Add("amount", "amount");
            dataGridView1.Columns.Add("IsNew", String.Empty);

        }

        private void ReadSingleRow(DataGridView dgv, IDataRecord record)
        {
            dgv.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDecimal(2),
                record.GetInt32(3), record.GetInt32(4), record.GetInt32(5),
                record.GetString(6), record.GetInt32(7), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgv)
        {
            dataGridView1.Rows.Clear();
            string queryString = $"select * from product";


            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgv, reader);
            }
            reader.Close();
        }



        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shopDataSet.product". При необходимости она может быть перемещена или удалена.
            this.productTableAdapter.Fill(this.shopDataSet.product);
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_id.Text = row.Cells[0].Value.ToString();
                textBox_name.Text = row.Cells[1].Value.ToString();
                textBox_price.Text = row.Cells[2].Value.ToString();
                textBox_id_m.Text = row.Cells[3].Value.ToString();
                textBox_size.Text = row.Cells[4].Value.ToString();
                textBox_id_cat.Text = row.Cells[5].Value.ToString();
                textBox_availibility.Text = row.Cells[6].Value.ToString();
                textBox_amount.Text = row.Cells[7].Value.ToString();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            add_product_form add = new add_product_form();
            add.Show();
        }


        

        private void button1_Click(object sender, EventArgs e)
        {
            
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }



        private void Search(DataGridView dgv)
        {
            dgv.Rows.Clear();

            string searchString = 
                $"select * from product where concat (id_prod, prod_name, prod_price, id_m, size, id_category, availibility, amount) like '%" + textBox1.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRow(dgv, read);

            }

            read.Close();
        }

        private void CatSearch(DataGridView dgv)
        {
            dgv.Rows.Clear();

            string searchString =
                $"select * from product where id_category like '%" + textBox2.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRow(dgv, read);

            }

            read.Close();
        }

        private void PriceSearch(DataGridView dgv)
        {
            dgv.Rows.Clear();

            string searchString =
                $"SELECT * FROM product WHERE prod_price like '%" + textBox_from.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRow(dgv, read);

            }

            read.Close();
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false ;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[8].Value = RowState.Deleted;
                return;
            }

            dataGridView1.Rows[index].Cells[8].Value = RowState.Deleted;
        }

        private void Update()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[8].Value;
                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from product where id_prod = {id}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());

                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var price = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var manufacturer = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var size = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var category = dataGridView1.Rows[index].Cells[5].Value.ToString();
                    var availibility = dataGridView1.Rows[index].Cells[6].Value.ToString();
                    var amount = dataGridView1.Rows[index].Cells[7].Value.ToString();

                    var changeQuery = 
                    $"update product set prod_name = '{name}', prod_price = '{price}', id_m = '{manufacturer}', size = '{size}', id_category = '{category}', availibility = '{availibility}', amount = '{amount}' where id_prod = '{id}'";

                    var command = new SqlCommand(changeQuery, database.getConnection());

                    command.ExecuteNonQuery();                
                }
            }

            database.closeConnection();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id.Text;
            var name = textBox_name.Text;
            decimal price;
            var manufacturer = textBox_id_m.Text;
            var size = textBox_size.Text;
            var category = textBox_id_cat.Text;
            var availibility = textBox_availibility.Text;
            var amount = textBox_amount.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (decimal.TryParse(textBox_price.Text, out price))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, name, price, manufacturer, size, category, availibility, amount);
                    dataGridView1.Rows[selectedRowIndex].Cells[8].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Unable to create new product. Please check the input data.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            deleteRow();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Update();
        }


        private void SizeSearch(DataGridView dgv)
        {
            dgv.Rows.Clear();

            string searchString =
                $"select * from product where size like '%" + textBox3.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRow(dgv, read);

            }

            read.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Change();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CatSearch(dataGridView1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            CategoryForm cat = new CategoryForm();
            cat.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            ImportsForm imps = new ImportsForm();
            imps.ShowDialog();
        }

        private void textBox_from_TextChanged(object sender, EventArgs e)
        {
            PriceSearch(dataGridView1);
        }

        private void textBox_to_TextChanged(object sender, EventArgs e)
        {
            PriceSearch(dataGridView1);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            SizeSearch(dataGridView1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.ShowDialog();
        }

        private void textBox_id_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.ShowDialog();
        }
    }


}

