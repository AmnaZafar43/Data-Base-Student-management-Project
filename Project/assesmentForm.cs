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
    public partial class assesmentForm : Form
    {
        int ide = 0;
        public assesmentForm()
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
              string title;
              int marks;
              int weight;
              DateTime date;
              title = txttitle.Text;
              marks = int.Parse(txtmarks.Text);
              weight = int.Parse(txtweight.Text);
              date = DateTime.Parse(dateTimePicker1.Text);
              string result = validation(title, marks, weight) == "" ? "" : validation(title, marks, weight);
              if (result == "")
              {
                  var con = Configuration.getInstance().getConnection();
                  SqlCommand cmd = new SqlCommand("Insert into Assessment values (@Title,@DateCreated,@TotalMarks,@TotalWeightage)", con);
                  cmd.Parameters.AddWithValue("@Title", title);
                  cmd.Parameters.AddWithValue("@DateCreated", date);
                  cmd.Parameters.AddWithValue("@TotalMarks", marks);
                  cmd.Parameters.AddWithValue("@TotalWeightage", weight);
                  cmd.ExecuteNonQuery();
                  MessageBox.Show("Successfully Added");
                  viewGrid();
              }
              else
              {
                  MessageBox.Show(result, "Error");
              }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void assesmentForm_Load(object sender, EventArgs e)
        {
            viewGrid();
        }
        private void viewGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private string validation(string title, int marks, int weight)
        {
            if (title == "")
            {
                return ("Enter valid credentials");
            }
            else if (marks < 0 || txtmarks.Text == "" )
            {
                return ("Enter valid marks");
            }
            else if (weight < 0 || txtweight.Text == "" )
            {
                return ("Enter valid weightage");
            }
            else
            {
                return "";
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
              string title;
              int marks;
              int weight;
              DateTime date;
              title = txttitle.Text;
              marks = int.Parse(txtmarks.Text);
              weight = int.Parse(txtweight.Text);
              date = DateTime.Parse(dateTimePicker1.Text);
              string result = validation(title, marks, weight) == "" ? "" : validation(title, marks, weight);
              if (result == "")
              {
                  updateAssessment(ide,title,date,marks,weight);
                  MessageBox.Show("Successfully Updated");
                  viewGrid();
              }
              else
              {
              
                  MessageBox.Show(result, "Error");
              }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        private void updateAssessment(int id, string title, DateTime date, int marks, int weightage)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Assessment SET Title = @Title, DateCreated = @DateCreated, TotalMarks = @TotalMarks, TotalWeightage = @TotalWeightage WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@DateCreated", date);
            cmd.Parameters.AddWithValue("@TotalMarks", marks);
            cmd.Parameters.AddWithValue("@TotalWeightage", weightage);
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                ide = id;
                txttitle.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtmarks.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtweight.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
               string title;
               int marks;
               int weight;
               DateTime date;
               title = txttitle.Text;
               marks = int.Parse(txtmarks.Text);
               weight = int.Parse(txtweight.Text);
               date = DateTime.Parse(dateTimePicker1.Text);
               string result = validation(title, marks, weight) == "" ? "" : validation(title, marks, weight);
               if (result == "")
               {
                   deleteAssessment(ide,title,date,marks,weight);
                   MessageBox.Show("Successfully Deleted");
                   viewGrid();
               }
               else
               {
              
                   MessageBox.Show(result, "Error");
               }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        private void deleteAssessment(int id, string title,DateTime date,int marks,int weight)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("DELETE from Assessment where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@DateCreated", date);
            cmd.Parameters.AddWithValue("@TotalMarks", marks);
            cmd.Parameters.AddWithValue("@TotalWeightage", weight);
            cmd.ExecuteNonQuery();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            assessmentReport myForm = new assessmentReport();
            myForm.ShowDialog();
        }
    }
}
