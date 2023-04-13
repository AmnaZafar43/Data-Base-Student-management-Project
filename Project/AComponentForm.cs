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
using System.Xml.Linq;

namespace Project
{
    public partial class AComponentForm : Form
    {
        
        int id = 0;
        public AComponentForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main myForm = new Main();
            myForm.ShowDialog();
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
                cmbRubricId.ValueMember = "id";
                cmbRubricId.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void AddAssessmentId()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT id from Assessment", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(string));
                da.Fill(dt);
                cmbAssessId.ValueMember = "id";
                cmbAssessId.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void AComponentForm_Load(object sender, EventArgs e)
        {
            viewGrid();
            AddRubricId();
            AddAssessmentId();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            string name;
            int marks;
            int rubId;
            int assId;
            name = txtName.Text;
            marks = int.Parse(txtmarks.Text);
            rubId = int.Parse(cmbRubricId.Text);
            assId = int.Parse(cmbAssessId.Text);
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Insert into AssessmentComponent values (@Name,@RubricId,@TotalMarks,@DateCreated,@DateUpdated,@AssessmentId)", con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@RubricId", rubId);
                cmd.Parameters.AddWithValue("@TotalMarks", marks);
                cmd.Parameters.AddWithValue("@DateCreated", date);
                cmd.Parameters.AddWithValue("@DateUpdated", date);
                cmd.Parameters.AddWithValue("@AssessmentId", assId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Added");
                viewGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }

        }
        private void viewGrid()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM AssessmentComponent", con);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = DateTime.Now;
                string name;
                int marks;
                int rubId;
                int assId;
                name = txtName.Text;
                marks = int.Parse(txtmarks.Text);
                rubId = int.Parse(cmbRubricId.Text);
                assId = int.Parse(cmbAssessId.Text);
                deleteAComponent(name,marks,rubId,assId,date);
                MessageBox.Show("Successfully Deleted");
                viewGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void deleteAComponent(string name,int marks,int rubId,int assId,DateTime date)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("DELETE from AssessmentComponent where Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@RubricId", rubId);
                cmd.Parameters.AddWithValue("@TotalMarks", marks);
                cmd.Parameters.AddWithValue("@DateCreated", date);
                cmd.Parameters.AddWithValue("@DateUpdated", date);
                cmd.Parameters.AddWithValue("@AssessmentId", assId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            string name;
            int marks;
            int rubId;
            int assId;
            name = txtName.Text;
            marks = int.Parse(txtmarks.Text);
            rubId = int.Parse(cmbRubricId.Text);
            assId = int.Parse(cmbAssessId.Text);
            updateAComponent(name, marks, rubId,assId,date);
            MessageBox.Show("Successfully Updated");
            viewGrid();
        }
        private void updateAComponent(string name,int marks,int rubId,int assId,DateTime date)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE AssessmentComponent SET Name = @Name,RubricId = @RubricId,TotalMarks = @TotalMarks,DateCreated = @DateCreated,DateUpdated = @DateUpdated,AssessmentId = @AssessmentId WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@RubricId", rubId);
            cmd.Parameters.AddWithValue("@TotalMarks", marks);
            cmd.Parameters.AddWithValue("@DateCreated", date);
            cmd.Parameters.AddWithValue("@DateUpdated", date);
            cmd.Parameters.AddWithValue("@AssessmentId", assId);
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DateTime date = DateTime.Now;
                int ide = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                id = ide;
                txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cmbRubricId.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtmarks.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                date = DateTime.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                date = DateTime.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString());
                cmbAssessId.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            
        }
    }
}
