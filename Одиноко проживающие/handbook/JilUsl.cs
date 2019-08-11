using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class JilUsl : Form
    {
        private bool _statusOperation1;
        private bool _statusOperation2;
        private bool _statusOperation3;
        private bool _statusOperation4;

        public JilUsl()
        {
            InitializeComponent();

            ///radRadioButton1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;

            button3.Visible = false;
            button4.Visible = false;
            button3_2.Visible = false;
            button4_2.Visible = false;
            button3_3.Visible = false;
            button4_3.Visible = false;
            button3_4.Visible = false;
            button4_4.Visible = false;
            textBox1.Visible = false;
            textBox3.Visible = false;
            textBox2.Visible = false;
            textBox4.Visible = false;

            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView3.MultiSelect = false;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView2.MultiSelect = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView4.MultiSelect = false;
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            UpdateGridViewKanal();
            UpdateGridViewOtopl();
            UpdateGridViewPlita();
            UpdateGridViewWoter();
        }

        //вода
        private void button1_Click(object sender, EventArgs e)
        {
            _statusOperation1 = true;
            button1.Visible = false;
            button2.Visible = false;

            button3.Visible = true;
            button4.Visible = true;
            textBox1.Visible = true;
            textBox1.Text = string.Empty;
            button3.Text = @"Добавить";
        }

        //канализация
        private void button1_1_Click(object sender, EventArgs e)
        {
            _statusOperation2 = true;
            button1_2.Visible = false;
            button2_2.Visible = false;

            button3_2.Visible = true;
            button4_2.Visible = true;
            textBox2.Visible = true;
            textBox2.Text = string.Empty;
            button3_2.Text = @"Добавить";
        }

        //отопление
        private void button1_4_Click(object sender, EventArgs e)
        {
            _statusOperation4 = true;
            button1_4.Visible = false;
            button2_4.Visible = false;

            button3_4.Visible = true;
            button4_4.Visible = true;
            textBox4.Visible = true;
            textBox4.Text = string.Empty;
            button3_4.Text = @"Добавить";
        }

        //плита
        private void button1_3_Click(object sender, EventArgs e)
        {
            _statusOperation3 = true;
            button1_3.Visible = false;
            button2_3.Visible = false;

            button3_3.Visible = true;
            button4_3.Visible = true;
            textBox3.Visible = true;
            textBox3.Text = string.Empty;
            button3_3.Text = @"Добавить";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            button4.Visible = false;
            textBox1.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            dataGridView1.Visible = true;
        }

        private void button4_2_Click(object sender, EventArgs e)
        {
            button3_2.Visible = false;
            button4_2.Visible = false;
            textBox2.Visible = false;
            button1_2.Visible = true;
            button2_2.Visible = true;
            dataGridView3.Visible = true;
        }

        private void button4_4_Click(object sender, EventArgs e)
        {
            button3_4.Visible = false;
            button4_4.Visible = false;
            textBox4.Visible = false;
            button1_4.Visible = true;
            button2_4.Visible = true;
            dataGridView4.Visible = true;
        }

        private void button4_3_Click(object sender, EventArgs e)
        {
            button3_3.Visible = false;
            button4_3.Visible = false;
            textBox3.Visible = false;
            button1_3.Visible = true;
            button2_3.Visible = true;
            dataGridView2.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (_statusOperation1)
                {
                    var returnOperation = commandServer.ExecNoReturnServer("addWoter", "'" + textBox1.Text + "'");
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была добавленна.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была добавленна. Вы хотите добавить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewWoter();
                            button4.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
                else
                {
                    dataGridView1.Visible = false;
                    if (dataGridView1.CurrentRow == null) return;
                    var returnOperation = commandServer.ExecNoReturnServer("editWoter", "'" + dataGridView1.CurrentRow.Cells[0].Value + "','" + textBox1.Text + "'");
                    dataGridView1.Visible = true;
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была изменена.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была изменена. Вы хотите изменить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewWoter();
                            button4.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Заполните поле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_2_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();

            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (_statusOperation2)
                {
                    var returnOperation = commandServer.ExecNoReturnServer("addKanal", "'" + textBox2.Text + "'");
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была добавленна.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была добавленна. Вы хотите добавить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewKanal();
                            button4_2.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
                else
                {
                    dataGridView2.Visible = false;
                    if (dataGridView2.CurrentRow == null) return;
                    var returnOperation = commandServer.ExecNoReturnServer("editKanal", "'" + dataGridView2.CurrentRow.Cells[0].Value + "','" + textBox2.Text + "'");
                    dataGridView2.Visible = true;
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была изменена.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была изменена. Вы хотите изменить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewKanal();
                            button4_2.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Заполните поле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateGridViewKanal()
        {
            dataGridView2.MultiSelect = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            var commandServer = new CommandServer();

            dataGridView2.DataSource = commandServer.DataGridSet(@"select statusKanal as [Канализация]
                from statusKanal
                order by statusKanal").Tables[0];
        }

        private void button3_3_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();

            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                if (_statusOperation3)
                {
                    var returnOperation = commandServer.ExecNoReturnServer("addPlita", "'" + textBox3.Text + "'");
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была добавленна.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была добавленна. Вы хотите добавить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewPlita();
                            button4_3.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
                else
                {
                    dataGridView3.Visible = false;
                    if (dataGridView3.CurrentRow == null) return;
                    var returnOperation = commandServer.ExecNoReturnServer("editPlita", "'" + dataGridView3.CurrentRow.Cells[0].Value + "','" + textBox3.Text + "'");
                    dataGridView3.Visible = true;
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была изменена.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была изменена. Вы хотите изменить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewPlita();
                            button4_3.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Заполните поле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateGridViewPlita()
        {
            dataGridView3.MultiSelect = false;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            var commandServer = new CommandServer();
            dataGridView3.DataSource = commandServer.DataGridSet(@"select statusPlita as [Плита]
                from statusPlita
                order by statusPlita").Tables[0];
        }

        private void button3_4_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();

            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                if (_statusOperation4)
                {
                    var returnOperation = commandServer.ExecNoReturnServer("addOtopl", "'" + textBox4.Text + "'");
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была добавленна.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была добавленна. Вы хотите добавить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewOtopl();
                            button4_4.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
                else
                {
                    dataGridView4.Visible = false;
                    if (dataGridView4.CurrentRow == null) return;
                    var returnOperation = commandServer.ExecNoReturnServer("editOtopl", "'" + dataGridView4.CurrentRow.Cells[0].Value + "','" + textBox4.Text + "'");
                    dataGridView4.Visible = true;
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была изменена.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была изменена. Вы хотите изменить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridViewOtopl();
                            button4_4.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Заполните поле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateGridViewOtopl()
        {
            dataGridView4.MultiSelect = false;
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            var commandServer = new CommandServer();
            dataGridView4.DataSource = commandServer.DataGridSet(@"select statusOtopl as [Отопление]
                from statusOtopl
                order by statusOtopl").Tables[0];
        }

        private void UpdateGridViewWoter()
        {
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            var commandServer = new CommandServer();
            dataGridView1.DataSource = commandServer.DataGridSet(@"select statusWoter as [Водоснобжение]
                from statusWoter
                order by statusWoter").Tables[0];
        }

        private void button2_4_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                _statusOperation4 = false;
                button4_4.Visible = false;
                button2_4.Visible = false;

                button3_4.Visible = true;
                button4_4.Visible = true;
                textBox4.Visible = true;
                textBox4.Text = string.Empty;
                button3_4.Text = @"Изменить";
                if (dataGridView4.CurrentRow != null)
                    textBox4.Text = dataGridView4.CurrentRow.Cells[0].Value.ToString();
                dataGridView4.Visible = false;
            }
            else
            {
                MessageBox.Show(@"Выделите запись для изменения.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                _statusOperation2 = false;
                button1_2.Visible = false;
                button2_2.Visible = false;

                button3_2.Visible = true;
                button4_2.Visible = true;
                textBox2.Visible = true;
                textBox2.Text = string.Empty;
                button3_2.Text = @"Изменить";
                if (dataGridView2.CurrentRow != null)
                    textBox2.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                dataGridView2.Visible = false;
            }
            else
            {
                MessageBox.Show(@"Выделите запись для изменения.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                _statusOperation1 = false;
                button1.Visible = false;
                button2.Visible = false;

                button3.Visible = true;
                button4.Visible = true;
                textBox1.Visible = true;
                textBox1.Text = string.Empty;
                button3.Text = @"Изменить";
                if (dataGridView1.CurrentRow != null)
                    textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                dataGridView1.Visible = false;
            }
            else
            {
                MessageBox.Show(@"Выделите запись для изменения.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_3_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                _statusOperation3 = false;
                button1_3.Visible = false;
                button2_3.Visible = false;

                button3_3.Visible = true;
                button4_3.Visible = true;
                textBox3.Visible = true;
                textBox3.Text = string.Empty;
                button3_3.Text = @"Изменить";
                if (dataGridView3.CurrentRow != null)
                    textBox3.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                dataGridView3.Visible = false;
            }
            else
            {
                MessageBox.Show(@"Выделите запись для изменения.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void radMaskedEditBox1_Initialized(object sender, EventArgs e)
        {
            //radMaskedEditBox1.MaskType = MaskType.DateTime;
            //MaskDateTimeProvider provider = radMaskedEditBox1.MaskedEditBoxElement.Provider as MaskDateTimeProvider;
            //provider.AutoSelectNextPart = true;
        }

        private void radRadioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            var radioButton = sender as RadRadioButton;


            if(e.Button == MouseButtons.Right)
            {
                radioButton.IsChecked = false;
            }

            //if (check)
            //{
            //    radioButton.IsChecked = false;
            //    check = false;
            //    return;
            //}
            //else
            //{
            //    radioButton.IsChecked = true;
            //    check = true;
            //    return;
            //}
        }

        private void radRadioButton3_CheckStateChanging(object sender, CheckStateChangingEventArgs args)
        {
            //radRadioButton1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
        }

        private void radRadioButton3_CheckStateChanging(object sender, EventArgs e)
        {
            //var radioButton = sender as RadRadioButton;
            
            //if (radioButton.IsChecked)
            //{
            //    radioButton.IsChecked = false;
            //    //return;
            //}
            //else
            //{
            //    radioButton.IsChecked = true;
            //    //radioButton.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            //    //return;
            //}
        }
        private bool check = false;
        private void radioButton2_Click(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (check)
            {
                radioButton.Checked = false;
                check = false;
                return;
            }
            else
            {
                radioButton.Checked = true;
                check = true;
                return;
            }
        }
    }
}