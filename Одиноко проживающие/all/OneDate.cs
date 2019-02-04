using System;
using System.Windows.Forms;

namespace Одиноко_проживающие.all
{
    public partial class OneDate : Form
    {
        private DateTime date_time;
        public OneDate(string str)
        {
            InitializeComponent();
            label1.Text = str;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            Date_time = dateTimePicker1.Value;
            this.Hide();
        }

        public DateTime Date_time
        {
            get { return date_time; }
            set { date_time = value; }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
