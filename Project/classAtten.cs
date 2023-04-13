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

namespace Project
{
    public partial class classAtten : Form
    {
        int ide = 0;
        public classAtten()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date;
                date = DateTime.Parse(txtdate.Text);
                deletedate(ide);
                MessageBox.Show("Successfully Deleted");
                viewGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        private void viewGrid()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select * from ClassAttendance", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Erorr",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(txtdate.Text)))
            {
                DateTime date;
                date = DateTime.Parse(txtdate.Value.ToString());
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Insert into ClassAttendance values (@AttendanceDate)", con);
                cmd.Parameters.AddWithValue("@AttendanceDate", date);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Added");
                viewGrid();
            }
            else
            {
                MessageBox.Show("Enter valid date");
            }
        }
        private void deletedate(int id)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("DELETE from ClassAttendance where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@AttendanceDate", txtdate.Value);
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id =int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                ide = id;
                txtdate.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void classAtten_Load(object sender, EventArgs e)
        {
            viewGrid();
        }
    }
}
