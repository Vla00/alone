using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.search
{
    public partial class SearchFam : RadForm
    {
        public SearchFam()
        {
            InitializeComponent();
            textBox1.GotFocus += GotFocusTextBox;
            textBox2.GotFocus += GotFocusTextBox;
            textBox3.GotFocus += GotFocusTextBox;
            KeyPreview = true;

            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));
        }

        private void Button2_Click(object sender, System.EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            date_ro_date.Value = DateTime.Now;
            radCheckBox1.Checked = false;
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Вы не ввели данные для поиска.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Hide();
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                Result result = null;
                if (radCheckBox1.Checked)
                    result = new Result(textBox1.Text, textBox2.Text, textBox3.Text, date_ro_date.Value.ToShortDateString(), "SearchFamily", true, null);
                else
                    result = new Result(textBox1.Text, textBox2.Text, textBox3.Text, null, "SearchFamily", true, null);
                result.ShowDialog();
                int val = result.Values;

                if(val == 0)
                {
                    if (MessageBox.Show("По вашему поиску ничего не найдено. Вы хотите создать личное дело?", "Результат", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        var item = new StructuresAlone
                        {
                            Family = textBox1.Text,
                            Name = textBox2.Text,
                            Surname = textBox3.Text
                        };

                        if (date_ro_date.Checked)
                            item.DateRo = date_ro_date.Value;
                        else
                            item.DateRo = null;

                        new Alone(true, 0, null, false, item).ShowDialog();
                    }
                }
            }
            else
            {
                new Alone(false, Convert.ToInt32(textBox4.Text), null, null, null).ShowDialog();
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
            if (sender is TextBox text) text.SelectAll();
        }

        private void TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void Date_ro_date_ValueChanged(object sender, EventArgs e)
        {
            SendKeys.Send(".");
        }

        private void RadCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            date_ro_date.Enabled = radCheckBox1.Checked;
        }
    }
}
