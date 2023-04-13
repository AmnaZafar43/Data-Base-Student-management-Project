using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class rubLevelForm : Form
    {
        int ide = 0;
        public rubLevelForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main myForm = new Main();
            myForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               var con = Configuration.getInstance().getConnection();
               SqlCommand cmd = new SqlCommand("Insert into RubricLevel values (@RubricId,@Details,@MeasurementLevel)", con);
               cmd.Parameters.AddWithValue("@RubricId", cmbrubric.Text);
               cmd.Parameters.AddWithValue("@Details", txtdetail.Text);
               cmd.Parameters.AddWithValue("@MeasurementLevel", cmblevel.Text);
               cmd.ExecuteNonQuery();
               MessageBox.Show("Successfully Added");
               viewGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void viewGrid()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM RubricLevel", con);
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
        private void AddRubricId()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT id from Rubric", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(string));
                da.Fill(dt);
                cmbrubric.ValueMember = "id";
                cmbrubric.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void rubLevelForm_Load(object sender, EventArgs e)
        {
            viewGrid();
            AddRubricId();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int rId;
            string details;
            int level;
            rId = int.Parse(cmbrubric.Text);
            details = txtdetail.Text;
            level = int.Parse(cmblevel.Text);
            updateRubricLevel(rId, details, level);
            MessageBox.Show("Successfully Updated");
            viewGrid();
        }
        private void updateRubricLevel(int rId,string details,int level)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE RubricLevel SET RubricId = @RubricId,Details = @Details,MeasurementLevel = @MeasurementLevel where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", ide);
            cmd.Parameters.AddWithValue("@RubricId", rId);
            cmd.Parameters.AddWithValue("@Details", details);
            cmd.Parameters.AddWithValue("@MeasurementLevel", level);
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                ide = id;
                cmbrubric.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtdetail.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                cmblevel.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rid;
                int level;
                string detail;
                rid = int.Parse(cmbrubric.Text);
                detail = txtdetail.Text;
                level = int.Parse(cmblevel.Text);
                deleteRubricLevel(rid,detail,level);
                MessageBox.Show("Successfully Deleted");
                viewGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void deleteRubricLevel(int rid,string detail,int level)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("DELETE from RubricLevel where Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", ide);
                cmd.Parameters.AddWithValue("@RubricId", rid);
                cmd.Parameters.AddWithValue("@Details", detail);
                cmd.Parameters.AddWithValue("@MeasurementLevel", level);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
