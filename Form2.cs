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
using System.Data.SqlClient;

namespace basketball_magaz
{

    public partial class Form2 : Form
    {

        Database database = new Database();

        int selectedRow;

        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_category", "id");
            dataGridView1.Columns.Add("cat_name", "name");
            dataGridView1.Columns.Add("IsNew", String.Empty);

        }

        private void ReadSingleRow(DataGridView dgv, IDataRecord record)
        {
            dgv.Rows.Add(record.GetInt32(0), record.GetString(1), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgv)
        {
            dataGridView1.Rows.Clear();
            string queryString = $"select * from category";


            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgv, reader);
            }
            reader.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            deleteRow();
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[2].Value = RowState.Deleted;
                return;
            }

            dataGridView1.Rows[index].Cells[2].Value = RowState.Deleted;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Change();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id.Text;
            var name = textBox_name.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
               dataGridView1.Rows[selectedRowIndex].SetValues(id, name);
               dataGridView1.Rows[selectedRowIndex].Cells[2].Value = RowState.Modified;
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void Search(DataGridView dgv)
        {
            dgv.Rows.Clear();

            string searchString =
                $"select * from category where concat (category.id_category, cat_name) like '%" + textBox1.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRow(dgv, read);

            }

            read.Close();
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
                    var deleteQuery = $"delete from category where id_category = {id}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());

                    command.ExecuteNonQuery();
                }

                if (rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var name = dataGridView1.Rows[index].Cells[1].Value.ToString();

                    var changeQuery =
                    $"update category set cat_name = '{name}' where id_category = '{id}'";

                    var command = new SqlCommand(changeQuery, database.getConnection());

                    command.ExecuteNonQuery();
                }
            }

            database.closeConnection();
        }

        private void button14_Click(object sender, EventArgs e)
        {
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
           }
            
        }
    }
}
