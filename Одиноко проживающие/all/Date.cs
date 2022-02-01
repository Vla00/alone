using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.all
{
    public partial class Date : RadForm
    {
        private readonly string _method;
        public Date(string method)
        {
            InitializeComponent();
            _method = method;
        }

        private void RadButton1_Click(object sender, EventArgs e)
        {
            switch (_method)
            {
                case "ListSurvey":
                    Hide();
                    new search.ListSurvey(dateTimePicker1.Value, dateTimePicker2.Value).ShowDialog();
                    Close();
                    break;
                case "ListHelp":
                    Hide();
                    new search.ListHelp(dateTimePicker1.Value, dateTimePicker2.Value).ShowDialog();
                    Close();
                    break;
            }
        }

        private void RadButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Date_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SendKeys.Send(".");
        }
    }
}
