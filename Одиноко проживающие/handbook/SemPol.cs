using System;
using System.Windows.Forms;

namespace Одиноко_проживающие
{
    public partial class SemPol : Form
    {
        //private readonly SqlConnection _connect = Form1.Connect;
        private bool _statusOperation;

        public SemPol()
        {
            InitializeComponent();

            button4.Visible = false;
            button5.Visible = false;
            textBox1.Visible = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            UpdateGridView();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            button5.Visible = false;
            textBox1.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            dataGridView1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _statusOperation = true;
            button1.Visible = false;
            button2.Visible = false;

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

                button4.Visible = true;
                button5.Visible = true;
                textBox1.Visible = true;
                textBox1.Text = string.Empty;
                button4.Text = @"Изменить";
                if (dataGridView1.CurrentRow != null)
                    textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                dataGridView1.Visible = false;
            }
            else
            {
                MessageBox.Show(@"Выделите запись для изменения.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (_statusOperation)
                {
                    var returnOperation = commandServer.ExecNoReturnServer("addSemPol", "'" + textBox1.Text + "'");
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
                    var returnOperation = commandServer.ExecNoReturnServer("editSemPol", "'" + dataGridView1.CurrentRow.Cells[0].Value + "','" + textBox1.Text + "'");
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

        private void UpdateGridView()
        {
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            var commandServer = new CommandServer();
            dataGridView1.DataSource = commandServer.DataGridSet(@"select sem_pol as [Сем. пол.]
                from sem_pol
                order by sem_pol").Tables[0];
        }
    }
}
