using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Project
{
    public partial class ResultReport : Form
    {
        public ResultReport()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName;
            string name;
            fileName = "Result Report";
            name = "StudentResult";
            GenerateReport(resultGrid, fileName, name);
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
        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            resultForm myForm = new resultForm();
            myForm.ShowDialog();
        }
        private void viewGrid()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Student.FirstName AS StudentName, Clo.Name as CloName, Rubric.Details as RubricDetails, Assessment.Title as AssessmentTitle, AssessmentComponent.Name as AssessmentComponent, \r\nAssessmentComponent.TotalMarks, RubricLevel.MeasurementLevel as  ObtainedMeasurementLevel, ((CAST(MeasurementLevel as float) /  (SELECT MAX(MeasurementLevel) FROM RubricLevel)) *  AssessmentComponent.TotalMarks) as ObtainedMarks\r\nFROM Student \r\nJOIN StudentResult \r\nON Student.Id = StudentResult.StudentId\r\nJOIN AssessmentComponent \r\nON AssessmentComponent.Id = StudentResult.AssessmentComponentId\r\nJOIN Assessment \r\nON Assessment.Id = AssessmentComponent.AssessmentId\r\nJOIN RubricLevel \r\nON RubricLevel.Id = StudentResult.RubricMeasurementId\r\nJOIN Rubric \r\nON Rubric.Id = RubricLevel.RubricId\r\nJOIN Clo \r\nON Clo.Id = Rubric.CloId\r\nGROUP BY Student.FirstName, Clo.Name, Rubric.Details, Assessment.Title, AssessmentComponent.Name, AssessmentComponent.TotalMarks, RubricLevel.MeasurementLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            resultGrid.DataSource = dt;
        }

        private void ResultReport_Load(object sender, EventArgs e)
        {
           
        }

        private void ResultReport_Load_1(object sender, EventArgs e)
        {
            viewGrid();
        }
    }
}
