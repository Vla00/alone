using System;
using System.Windows.Forms;

namespace Одиноко_проживающие.all
{
    public partial class OneDate : Form
    {
        public OneDate(string str)
        {
            InitializeComponent();
            label1.Text = str;
        }

        private void RadButton1_Click(object sender, EventArgs e)
        {
            Date_time = dateTimePicker1.Value;
            this.Hide();
        }

        public DateTime Date_time { get; set; }

        private void RadButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
