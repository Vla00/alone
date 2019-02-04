using System;
using System.Windows.Forms;

namespace Одиноко_проживающие
{
    public partial class Country : Form
    {
        private bool _statusOperation;

        public Country()
        {
            InitializeComponent();

            button4.Visible = false;
            button5.Visible = false;
            textBox1.Visible = false;
            comboBox1.Visible = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            UpdateComboBoxSelsovet();
            UpdateGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _statusOperation = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;

            comboBox1.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
            textBox1.Visible = true;
            textBox1.Text = string.Empty;
            button4.Text = @"Добавить";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                _statusOperation = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;

                comboBox1.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                textBox1.Visible = true;
                textBox1.Text = string.Empty;
                button4.Text = @"Изменить";
                if (dataGridView1.CurrentRow != null)
                {
                    textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    comboBox1.SelectedIndex = comboBox1.FindStringExact(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                }
                    
                dataGridView1.Visible = false;
            }
            else
            {
                MessageBox.Show(@"Выделите запись для изменения.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (
                    MessageBox.Show(@"Вы точно хотите удалить запись?", @"Внимание", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var commandServer = new CommandServer();
                    if (dataGridView1.CurrentRow == null) return;
                    var returnOperation = commandServer.ExecNoReturnServer("deleteCountry", "'" + dataGridView1.CurrentRow.Cells[1].Value + "'");
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была удалена.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была удалена. Хотите закрыть текущее окно?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            UpdateGridView();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
                else
                {
                    button5.Refresh();
                }
            }
            else
            {
                MessageBox.Show(@"Выделите запись для удаления.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateGridView()
        {
            var commandServer = new CommandServer();
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.DataSource = commandServer.DataGridSet(@"select * from ListCountry()").Tables[0];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(comboBox1.Text))
            {
                if (_statusOperation)
                {
                    var returnOperation = commandServer.ExecNoReturnServer("addCountry", "'" + comboBox1.Text + "','" + textBox1.Text + "'");
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
                            UpdateGridView();
                            button5.PerformClick();
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
                    var returnOperation = commandServer.ExecNoReturnServer("editCountry", "'" + dataGridView1.CurrentRow.Cells[1].Value + "','" + textBox1.Text + "','" + comboBox1.Text + "'");
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
                            UpdateGridView();
                            button5.PerformClick();
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

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            button5.Visible = false;
            comboBox1.Visible = false;
            textBox1.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            dataGridView1.Visible = true;
        }

        private void UpdateComboBoxSelsovet()
        {
            var commandServer = new CommandServer();
            comboBox1.Items.Clear();
            comboBox1.DataSource = commandServer.ComboBoxList(@"select Selsovet from Selsovet order by Selsovet", true);
        }
    }
}
