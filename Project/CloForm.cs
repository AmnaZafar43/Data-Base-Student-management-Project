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
    public partial class CloForm : Form
    {
        int ide = 0;
        public CloForm()
        {
            InitializeComponent();
        }

        private void CloForm_Load(object sender, EventArgs e)
        {
            viewGrid();
        }
        private void viewGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
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
                string clo;
                DateTime create;
                DateTime update;
                clo = txtclo.Text;
                create = DateTime.Parse(dcreate.Text);
                update = DateTime.Parse(dupdate.Text);
                string result = validation(clo) == "" ? "" : validation(clo);
                if (result == "")
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Insert into Clo values (@Name,@DateCreated,@DateUpdated)", con);
                    cmd.Parameters.AddWithValue("@Name", clo);
                    cmd.Parameters.AddWithValue("@DateCreated", create);
                    cmd.Parameters.AddWithValue("@DateUpdated", update);
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
        private string validation(string clo)
        {
            if (clo == "")
            {
                return ("Enter valid clo");
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
                string clo;
                DateTime create;
                DateTime update;
                clo = txtclo.Text;
                create = DateTime.Parse(dcreate.Text);
                update = DateTime.Parse(dupdate.Text);
                string result = validation(clo) == "" ? "" : validation(clo);
                if (result == "")
                {
                    updateCLO(ide,clo,create,update);
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
        private void updateCLO(int id,string clo,DateTime create,DateTime update)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Clo SET Name = @Name, DateCreated = @DateCreated, DateUpdated = @DateUpdated WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", clo);
            cmd.Parameters.AddWithValue("@DateCreated", create);
            cmd.Parameters.AddWithValue("@DateUpdated", update);
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                ide = id;
                txtclo.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                dcreate.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                dupdate.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
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
                string clo;
                DateTime create;
                DateTime update;
                clo = txtclo.Text;
                create = DateTime.Parse(dcreate.Text);
                update = DateTime.Parse(dupdate.Text);
                string result = validation(clo) == "" ? "" : validation(clo);
                if (result == "")
                {
                    deleteClo(ide,clo,create,update);
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
        private void deleteClo(int id, string clo,DateTime create,DateTime update)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("DELETE from Clo where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", clo);
            cmd.Parameters.AddWithValue("@DateCreated", create);
            cmd.Parameters.AddWithValue("@DateUpdated", update);
            cmd.ExecuteNonQuery();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            cloReport myForm = new cloReport();
            myForm.ShowDialog();
        }
    }
}
