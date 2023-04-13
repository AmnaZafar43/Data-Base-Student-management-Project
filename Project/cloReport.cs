using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class cloReport : Form
    {
        public cloReport()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            CloForm myForm = new CloForm();
            myForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridView grid;
            string fileName;
            string name;
            grid = clogrid;
            fileName = "CLO Report";
            name = "Clo";
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

        private void viewGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Clo.Name AS CloName, Student.FirstName AS StudentName, MAX(Rubric.Details) AS RubricDetails, Assessment.Title AS AssessmentName, SUM(AssessmentComponent.TotalMarks) AS TotalMarks, SUM(CAST(RubricLevel.MeasurementLevel AS float) / measureLevel * AssessmentComponent.TotalMarks) AS ObtainedMarks FROM Student JOIN StudentResult ON Student.Id = StudentResult.StudentId JOIN AssessmentComponent ON AssessmentComponent.Id = StudentResult.AssessmentComponentId JOIN Assessment ON Assessment.Id = AssessmentComponent.AssessmentId JOIN RubricLevel ON RubricLevel.Id = StudentResult.RubricMeasurementId JOIN Rubric ON Rubric.Id = RubricLevel.RubricId JOIN Clo ON Clo.Id = Rubric.CloId CROSS JOIN (SELECT MAX(MeasurementLevel) AS measureLevel FROM RubricLevel) maxRubric WHERE Clo.Id = 1 GROUP BY Student.FirstName, Clo.Name, Assessment.Title", con);          
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            clogrid.DataSource = dt;
        }

        private void cloReport_Load(object sender, EventArgs e)
        {
            viewGrid();
        }
    }
}
