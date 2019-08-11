using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.search
{
    public partial class SearchFam : RadForm
    {
        private byte number;
        public SearchFam()
        {
            InitializeComponent();
            textBox1.GotFocus += GotFocusTextBox;
            textBox2.GotFocus += GotFocusTextBox;
            textBox3.GotFocus += GotFocusTextBox;
            KeyPreview = true;

            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (radRadioButton1.IsChecked)
                number = 0;
            else
            {
                if (radRadioButton2.IsChecked)
                    number = 1;
                else
                    number = 2;
            }
            Hide();
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                new Result(textBox1.Text, textBox2.Text, textBox3.Text, number, "SearchFamily", true, null).ShowDialog();
            }
            else
            {
                new Alone(false, Convert.ToInt32(textBox4.Text), null, null).ShowDialog();
            }
            Show();
            textBox1.Focus();
        }

        private void SearchFam_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void SearchFam_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void GotFocusTextBox(object sender, EventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text != null) text.SelectAll();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }
    }
}
