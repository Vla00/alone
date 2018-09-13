using System;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.all
{
    public partial class Date : RadForm
    {
        private string _method;
        public Date(string method)
        {
            InitializeComponent();
            _method = method;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            switch(_method)
            {
                case "ListSurvey":
                    new search.ListSurvey(dateTimePicker1.Value, dateTimePicker2.Value).ShowDialog();
                    this.Close();
                    break;
                case "ListHelp":
                    new search.ListHelp(dateTimePicker1.Value, dateTimePicker2.Value).ShowDialog();
                    this.Close();
                    break;
            }
            //dateStart = dateTimePicker1.Value;
            //dateEnd = dateTimePicker2.Value;
            //statusM = true;
            //this.Hide();
        }

        private void radButton2_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void Date_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
