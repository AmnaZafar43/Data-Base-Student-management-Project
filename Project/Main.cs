using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Color c1 = Color.FromArgb(32, 178, 170);
            Color c2 = Color.FromKnownColor(KnownColor.SkyBlue);
            Color c = Color.FromKnownColor(KnownColor.LavenderBlush);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            student myForm = new student();
            myForm.ShowDialog();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            assesmentForm myForm = new assesmentForm();
            myForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            attendanceForm myForm = new attendanceForm();
            myForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            CloForm myForm = new CloForm();
            myForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rubricForm myForm = new rubricForm();
            myForm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            rubLevelForm myForm = new rubLevelForm();
            myForm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            AComponentForm myForm = new AComponentForm();
            myForm.ShowDialog();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            resultForm myForm = new resultForm();
            myForm.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            ReportForm myForm = new ReportForm();
            myForm.ShowDialog();
        }
    }
}
