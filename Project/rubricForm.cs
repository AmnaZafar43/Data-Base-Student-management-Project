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

namespace Project
{
    public partial class rubricForm : Form
    {
        int rubricID = 0;
        int rid = 0;
        int id = 0;
        
        public rubricForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main myForm = new Main();
            myForm.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void AddCLO()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Name from Clo", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                da.Fill(dt);
                cmbCLO.ValueMember = "Name";
                cmbCLO.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void rubricForm_Load(object sender, EventArgs e)
        {
            viewGrid();
            AddCLO();
        }

        private void cmbCLO_TextChanged(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Id FROM Clo WHERE Name = @Name", con);
                cmd.Parameters.AddWithValue("Name", cmbCLO.Text);
                SqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    id = int.Parse(da.GetValue(0).ToString());
                    Console.WriteLine(id);
                }
                da.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rid = int.Parse(dataGridView1.RowCount.ToString());
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Insert into Rubric values (@Id,@Details,@CloId)", con);
            cmd.Parameters.AddWithValue("@Id", rid);
            cmd.Parameters.AddWithValue("@Details", txtdetails.Text);
            cmd.Parameters.AddWithValue("@CloId", id);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Added");
            viewGrid();
        }
        private void viewGrid()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Rubric", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string clo;
                string detail;
                clo = cmbCLO.Text;
                detail = txtdetails.Text;
                deleteRubric(detail);
                MessageBox.Show("Successfully Deleted");
                viewGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        private void deleteRubric(string detail)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("DELETE from Rubric where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", rubricID);
            cmd.Parameters.AddWithValue("@Detail", detail);
            Console.WriteLine(rubricID);
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                rubricID = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                txtdetails.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cmbCLO.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string details;
            string clo;
            details = txtdetails.Text;
            clo = cmbCLO.Text;
            updateRubric(rubricID,details,clo,id);
            MessageBox.Show("Successfully Updated");
            viewGrid();
        }
        private void updateRubric(int rubricID,string detials,string clo,int id)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Rubric SET Details = @Details,CloId = @CloId where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", rubricID);
            cmd.Parameters.AddWithValue("@Details", detials);
            cmd.Parameters.AddWithValue("@CloId", id);
            cmd.ExecuteNonQuery();
        }
    }
}
