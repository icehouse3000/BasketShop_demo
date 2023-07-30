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
    public partial class Form3 : Form
    {   
        Database database = new Database();

        int selectedRow;

        public Form3()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_imp", "id");
            dataGridView1.Columns.Add("id_i", "importer id");
            dataGridView1.Columns.Add("imp_date", "date");
            dataGridView1.Columns.Add("id_empl", "employee id");
            dataGridView1.Columns.Add("id_prod", "product id");
            dataGridView1.Columns.Add("amount", "amount");
            dataGridView1.Columns.Add("price", "price");
            dataGridView1.Columns.Add("IsNew", String.Empty);

        }

        private void ReadSingleRow(DataGridView dgv, IDataRecord record)
        {
            dgv.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetDateTime(2),
                record.GetInt32(3), record.GetInt32(4), record.GetInt32(5),
                record.GetDecimal(6), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgv)
        {
            dataGridView1.Rows.Clear();
            string queryString = $"SELECT import_1.id_imp, id_i, imp_date, id_empl, id_prod, amount, price FROM import_1 inner join import_2 ON import_1.id_imp = import_2.id_imp;";


            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgv, reader);
            }
            reader.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[7].Value = RowState.Deleted;
                return;
            }

            dataGridView1.Rows[index].Cells[7].Value = RowState.Deleted;
        }

        private void Update()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[7].Value;
                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id_imp = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from id_imp where id_prod = {id_imp}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());

                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id_imp = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var id_i = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var date = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var id_empl = dataGridView1.Rows[index].Cells[3].Value.ToString();

                    var changeQuery =
                    $"update import_1 set id_i = '{id_i}', imp_date = '{date}', id_empl = '{id_empl}' where id_imp = {id_imp}";

                    var command = new SqlCommand(changeQuery, database.getConnection());

                    command.ExecuteNonQuery();
                }
            }

            database.closeConnection();
        }

        private void Update2()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[7].Value;
                if (rowState == RowState.Existed)
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id_imp = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from id_imp where id_prod = {id_imp}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());

                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id_imp = dataGridView1.Rows[index].Cells[0].Value.ToString();                    
                    var id_prod = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    var amount = dataGridView1.Rows[index].Cells[5].Value.ToString();
                    var price = dataGridView1.Rows[index].Cells[6].Value.ToString();

                    var changeQuery =
                    $"update import_2 set id_imp = '{id_imp}', id_prod = '{id_prod}', amount = '{amount}', price = '{price}' where id_imp = {id_imp}";

                    var command = new SqlCommand(changeQuery, database.getConnection());

                    command.ExecuteNonQuery();
                }
            }

            database.closeConnection();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id_imp.Text;
            var i = textBox_id_i.Text;
            var date = textBox_date.Text;
            var empl = textBox_id_empl.Text;
            var prod = textBox_id_prod.Text;
            var amount = textBox_amount.Text;
            decimal price;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (decimal.TryParse(textBox_price.Text, out price))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, i, date, empl, prod, amount, price);
                    dataGridView1.Rows[selectedRowIndex].Cells[7].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Unable to perform. Please check the input data.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Update2();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            deleteRow();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Change();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_id_imp.Text = row.Cells[0].Value.ToString();
                textBox_id_i.Text = row.Cells[1].Value.ToString();
                textBox_date.Text = row.Cells[2].Value.ToString();
                textBox_id_empl.Text = row.Cells[3].Value.ToString();
                textBox_id_prod.Text = row.Cells[4].Value.ToString();
                textBox_amount.Text = row.Cells[5].Value.ToString();
                textBox_price.Text = row.Cells[6].Value.ToString();
            }
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
    }
}
