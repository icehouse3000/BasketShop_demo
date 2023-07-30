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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.VisualStyles;

namespace basketball_magaz
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        Database database = new Database();

        int selectedRow;

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("intend_id", "1");
            dataGridView1.Columns.Add("intend_startDate", "2");
            dataGridView1.Columns.Add("intend_endDate", "3");
            dataGridView1.Columns.Add("intend_user_id", "4");
            dataGridView1.Columns.Add("intend_employee_id", "5");
            dataGridView1.Columns.Add("listIntend_count", "6");
            dataGridView1.Columns.Add("listIntend_product_id", "7");
            dataGridView1.Columns.Add("intend_condition", "8");
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
            string queryString = $"SELECT intend_id, intend_startDate, intend_endDate, intend_user_id, intend_employee_id, listIntend_count,listIntend_product_id, intend_condition from intend inner join listIntend on intend.intend_id = listIntend.listIntend_intend_id";


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

            dataGridView1.Rows[index].Cells[8].Value = RowState.Deleted;
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

        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
