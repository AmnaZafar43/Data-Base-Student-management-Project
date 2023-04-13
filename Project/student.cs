using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Project
{
    public partial class student : Form
    {
        int ide = 0;
        public student()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main myForm = new Main();
            myForm.ShowDialog();
        }

        private void addForm_Load(object sender, EventArgs e)
        {
            viewGrid();
        }
        private string validation(string fname,string contact,string mail,string lname,string regno,int status)
        {
           if (fname == "" || mail == "" || contact == "" || regno == "")
           {
               return ("Enter valid credentials");
           }
           else if (!mail.Contains("@") || !mail.Contains(".com"))
           {
               return ("Enter valid E-mail");
           }
           else if (fname.Contains("0") || fname.Contains("1") || fname.Contains("2") || fname.Contains("3") || fname.Contains("4") || fname.Contains("5") || fname.Contains("6") || fname.Contains("7") || fname.Contains("8") || fname.Contains("9") || fname.Contains("@") || fname.Contains("$") || fname.Contains("%") || fname.Contains("&") || fname.Contains("!") || fname.Contains("*"))
           {
               return ("Enter valid first name");
           }
           else if (lname.Contains("0") || lname.Contains("1") || lname.Contains("2") || lname.Contains("3") || lname.Contains("4") || lname.Contains("5") || lname.Contains("6") || lname.Contains("7") || lname.Contains("8") || lname.Contains("9") || lname.Contains("@") || lname.Contains("$") || lname.Contains("%") || lname.Contains("&") || lname.Contains("!") || lname.Contains("*"))
           {
               return ("Enter valid last name");
           }
           else if (contact.Length < 11 || contact.Length > 11)
           {
               return  ("Enter valid contact no");
           }
           else
           {
               return "";
           }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fname;
            string contact;
            string mail;
            string lname;
            string regno;
            int status;
            fname = txtfname.Text;
            lname = txtlname.Text;
            contact = txtcontact.Text;
            mail = txtmail.Text;
            regno = txtreg.Text;        
            status = cmbstatus.Text == "Active" ? 5 : 6;
            string result = validation(fname, contact, mail, lname, regno, status) == "" ? "" : validation(fname, contact, mail, lname, regno, status);
            if (result == "")
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Insert into Student values (@FirstName,@LastName,@Contact,@Email,@RegistrationNumber,@Status)", con);
                cmd.Parameters.AddWithValue("@FirstName", fname);
                cmd.Parameters.AddWithValue("@LastName", lname);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@Email", mail);
                cmd.Parameters.AddWithValue("@RegistrationNumber", regno);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Added");
                viewGrid();
            }
            else
            { 
                MessageBox.Show(result, "Error");
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string fname;
            string contact;
            string mail;
            string lname;
            string regno;
            int status;
            fname = txtfname.Text;
            lname = txtlname.Text;
            contact = txtcontact.Text;
            mail = txtmail.Text;
            regno = txtreg.Text;
            status = cmbstatus.Text == "Active" ? 5 : 6;
            string result = validation(fname, contact, mail, lname, regno, status) == "" ? "" : validation(fname, contact, mail, lname, regno, status);
            if (result == "")
            {
                updateStudent(ide, fname, contact, mail, lname, regno, status);
                MessageBox.Show("Successfully Updated");
                viewGrid();
            }
            else
            {

                MessageBox.Show(result, "Error");
            }
        }

        private void viewGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            stugrid.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string fname;
            string contact;
            string mail;
            string lname;
            string regno;
            int status;
            fname = txtfname.Text;
            lname = txtlname.Text;
            contact = txtcontact.Text;
            mail = txtmail.Text;
            regno = txtreg.Text;
            status = cmbstatus.Text == "Active" ? 5 : 6;
            string result = validation(fname, contact, mail, lname, regno, status) == "" ? "" : validation(fname, contact, mail, lname, regno, status);
            if (result == "")
            {
                deleteStudent(ide, fname, contact, mail, lname, regno, status);
                MessageBox.Show("Successfully Deleted");
                viewGrid();
            }
            else
            {

            MessageBox.Show(result,"Error");
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(stugrid.CurrentRow.Cells[0].Value.ToString());
                ide = id;
                txtfname.Text = stugrid.CurrentRow.Cells[1].Value.ToString();
                txtlname.Text = stugrid.CurrentRow.Cells[2].Value.ToString();
                txtcontact.Text = stugrid.CurrentRow.Cells[3].Value.ToString();
                txtmail.Text = stugrid.CurrentRow.Cells[4].Value.ToString();
                txtreg.Text = stugrid.CurrentRow.Cells[5].Value.ToString();
                cmbstatus.Text = stugrid.CurrentRow.Cells[6].Value.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error");
            }
        }

        private void deleteStudent(int id,string fname,string contact,string mail,string lname,string regno,int status)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("DELETE from Student where ID = @ID", con);
            cmd.Parameters.AddWithValue("@ID",id);
            cmd.Parameters.AddWithValue("@FirstName",fname);
            cmd.Parameters.AddWithValue("@LastName", lname);
            cmd.Parameters.AddWithValue("@Contact", contact);
            cmd.Parameters.AddWithValue("@Email", mail);
            cmd.Parameters.AddWithValue("@RegistrationNumber", regno);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.ExecuteNonQuery();         
        }
        private void updateStudent(int id, string fname, string contact, string mail, string lname, string regno, int status)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact,Email = @Email,RegistrationNumber = @RegistrationNumber,Status = @Status where ID = @ID", con);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@FirstName", fname);
            cmd.Parameters.AddWithValue("@LastName", lname);
            cmd.Parameters.AddWithValue("@Contact", contact);
            cmd.Parameters.AddWithValue("@Email", mail);
            cmd.Parameters.AddWithValue("@RegistrationNumber", regno);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.ExecuteNonQuery();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridView grid;
            string fileName;
            string name;
            grid = stugrid;
            fileName = "Student File";
            name = "Student";
            GenerateReport(grid,fileName,name);
        }
        private void GenerateReport(DataGridView dataGridView, string fileName, string name)
        {
            Document document = new Document(PageSize.A4, 20, 20, 20, 20);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName + ".pdf", FileMode.Create));
            document.Open();
            PdfPCell titleCell = new PdfPCell(new Phrase(name));
            PdfPTable table = new PdfPTable(dataGridView.Columns.Count);
            table.WidthPercentage = 100;
            titleCell.Colspan = dataGridView.ColumnCount;
            titleCell.BackgroundColor = new BaseColor(192, 192, 192);
            titleCell.Padding = 10;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                cell.BackgroundColor = new BaseColor(SystemColors.ControlLight);
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                table.AddCell(cell);
            }
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                    {
                        if (cell.Value == null) { continue; }
                        PdfPCell pdfCell = new PdfPCell(new Phrase(" "));
                        pdfCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pdfCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        table.AddCell(pdfCell);
                    }
                    else
                    {
                        PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString()));
                        pdfCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pdfCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        table.AddCell(pdfCell);
                    }
                }
            }

            table.AddCell(titleCell);
            document.Add(table);
            document.Close();
            MessageBox.Show("Report generated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

}
