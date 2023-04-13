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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace Project
{
    public partial class attendanceForm : Form
    {
        int attenId=-1;
        int stuId=-1;
        public attendanceForm()
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
            classAtten myForm = new classAtten();
            myForm.ShowDialog();
            AddValueCombo();
        }

        private void cmbdate_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        private void AddStatus()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Name from Lookup WHERE Category = 'ATTENDANCE_STATUS'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                da.Fill(dt);
                cmbstatus.ValueMember = "Name";
                cmbstatus.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void AddValueCombo()
        {
            try
            {
               var con = Configuration.getInstance().getConnection();
               SqlCommand cmd = new SqlCommand("SELECT AttendanceDate from ClassAttendance", con);
               SqlDataAdapter da = new SqlDataAdapter(cmd);
               DataTable dt = new DataTable();
               dt.Columns.Add("AttendanceDate", typeof(string));
               da.Fill(dt);
               cmbdate.ValueMember = "AttendanceDate";
               cmbdate.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void attendanceForm_Load(object sender, EventArgs e)
        {
            AddValueCombo();
            AddActive();
            AddStatus();
            viewGrid();
        }
        private void AddActive()
        {
            try
            {
               var con = Configuration.getInstance().getConnection();
               SqlCommand cmd = new SqlCommand("SELECT RegistrationNumber from Student WHERE Status = 5", con);
               SqlDataAdapter da = new SqlDataAdapter(cmd);
               DataTable dt = new DataTable();
               dt.Columns.Add("RegistrationNumber", typeof(string));
               da.Fill(dt);
               cmbactive.ValueMember = "RegistrationNumber";
               cmbactive.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void cmbactive_TextChanged(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE RegistrationNumber =@RegistrationNumber ", con);
                cmd.Parameters.AddWithValue("RegistrationNumber", cmbactive.Text);
                SqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    txtid.Text = (da.GetValue(0).ToString());
                    stuId =int.Parse(da.GetValue(0).ToString());
                    string fName =da.GetValue(1).ToString();
                    string lName = da.GetValue(2).ToString();
                    txtname.Text = fName+" "+lName;
                }
                da.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
               int status = -1;
               if(cmbstatus.Text == "Present")
               {
                   status = 1;
               }
               else if (cmbstatus.Text == "Absent")
               {
                   status = 2;
               }
               else if (cmbstatus.Text == "Leave")
               {
                   status = 3;
               }
               else if (cmbstatus.Text == "Late")
               {
                   status = 4;
               }
               var con = Configuration.getInstance().getConnection();
               SqlCommand cmd = new SqlCommand("Insert into StudentAttendance values (@AttendanceId,@StudentId,@AttendanceStatus)", con);
               cmd.Parameters.AddWithValue("@AttendanceId", attenId);
               cmd.Parameters.AddWithValue("@StudentId", stuId);
               cmd.Parameters.AddWithValue("@AttendanceStatus", status);
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
               SqlCommand cmd = new SqlCommand("SELECT s.Id,CONCAT(s.FirstName,' ',s.LastName)as Name,s.RegistrationNumber,l.Name AS [Attendance Status] ,ca.AttendanceDate FROM Student s JOIN StudentAttendance sa ON sa.StudentId=s.id JOIN  ClassAttendance ca ON ca.Id=sa.AttendanceId JOIN Lookup l ON sa.AttendanceStatus=l.LookupId WHERE ca.AttendanceDate = @AtDate AND s.Status=5 ", con);
               cmd.Parameters.AddWithValue("@AtDate", DateTime.Parse(cmbdate.Text));
               SqlDataAdapter da = new SqlDataAdapter(cmd);
               DataTable dt = new DataTable();
               da.Fill(dt);
               attengrid.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        private void cmbdate_TextChanged(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Id FROM ClassAttendance WHERE AttendanceDate = @AttendanceDate", con);
                cmd.Parameters.AddWithValue("AttendanceDate", cmbdate.Text);
                SqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    attenId = int.Parse(da.GetValue(0).ToString());
                    Console.WriteLine(attenId);
                }
                da.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            viewGrid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridView grid;
            string fileName;
            string name;
            grid = attengrid;
            fileName = "Attendance Report";
            name = "StudentAttendance";
            GenerateReport(grid, fileName, name);
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

        private void button5_Click(object sender, EventArgs e)
        {
                     
        }
        
    }
}
