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
    public partial class resultForm : Form
    {
       
        public resultForm()
        {
            InitializeComponent();
        }

        private void resultForm_Load(object sender, EventArgs e)
        {
            try
            {
                viewGrid();
                AddActive();
               AddAssessmentName();
               AddAComponent();
               AddRbricMLevelId();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void AddActive()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT RegistrationNumber,Id from Student WHERE Status = 5", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("RegistrationNumber", typeof(string));
                da.Fill(dt);
                cmbstuId.DisplayMember = "RegistrationNumber";
                cmbstuId.ValueMember = "Id";
                cmbstuId.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void AddAComponent()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Id,Name from AssessmentComponent WHERE AssessmentId=@AssessmentId", con);
                cmd.Parameters.AddWithValue("@AssessmentId",int.Parse(cmbAName.SelectedValue.ToString()));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                da.Fill(dt);
                comboBox1.ValueMember = "Id";
                comboBox1.DisplayMember = "Name";
                comboBox1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void AddRbricMLevelId()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Id, Details from RubricLevel", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("Details", typeof(string));
                da.Fill(dt);
                cmbrbId.DisplayMember = "Details";
                cmbrbId.ValueMember = "Id";
                cmbrbId.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel);
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel);
            }
        }
        private void AddAssessmentName()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Id,Title from Assessment", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("Title", typeof(string));
                da.Fill(dt);
                cmbAName.ValueMember = "Id";
                cmbAName.DisplayMember = "Title";
                cmbAName.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DateTime edate;
            edate = DateTime.Parse(cmbdate.Text);
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Insert into StudentResult values (@StudentId,@AssessmentComponentId,@RubricMeasurementId,@EvaluationDate)", con);
                cmd.Parameters.AddWithValue("@StudentId", cmbstuId.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@AssessmentComponentId", comboBox1.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@RubricMeasurementId", cmbrbId.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@EvaluationDate", edate);
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM StudentResult", con);
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

        private void cmbAName_TextChanged(object sender, EventArgs e)
        {
            comboBox1.ResetText();
            AddAComponent();
        }

        private void cmbstuId_TextChanged(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT FirstName FROM Student WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id",int.Parse(cmbstuId.SelectedValue.ToString()));
                SqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    txtName.Text = da.GetValue(0).ToString();
                }
                da.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main myForm = new Main();
            myForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ResultReport myForm = new ResultReport();
            myForm.ShowDialog();
        }
    }
}
