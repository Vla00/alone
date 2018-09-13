using System;
using System.Windows.Forms;

namespace Одиноко_проживающие.search
{
    public partial class HelpAndObsl : Form
    {
        public HelpAndObsl()
        {
            InitializeComponent();

            dataGridView1.MultiSelect = false;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dateTimePicker8.Enabled = false;
            dateTimePicker9.Enabled = false;
            dateTimePicker3.Enabled = false;
            dateTimePicker4.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;

            dateTimePicker8.Format = DateTimePickerFormat.Custom;
            dateTimePicker8.CustomFormat = @"dd.MM.yyyy";
            dateTimePicker9.Format = DateTimePickerFormat.Custom;
            dateTimePicker9.CustomFormat = @"dd.MM.yyyy";
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = @"dd.MM.yyyy";
            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = @"dd.MM.yyyy";

            radioButton2.Checked = true;
            UpdateComboBoxSelsovet();
            UpdateComboBoxSpeziolist();
            UpdateComboBoxHelp();
            toolStripStatusLabel1.Text = "";
        }

        private void UpdateComboBoxHelp()
        {
            var commandServer = new CommandServer();
            comboBox5.DataSource = commandServer.GetComboBoxList(@"select typeHelp from typeHelp", true);
        }

        private void UpdateComboBoxSpeziolist()
        {
            var commandServer = new CommandServer();
            comboBox2.DataSource = commandServer.GetComboBoxList(@"select fio
                    from speziolist
                    where statusDelete = 0
                    order by fio", true);
        }

        private void UpdateComboBoxSelsovet()
        {
            var commandServer = new CommandServer();
            comboBox3.DataSource = commandServer.GetComboBoxList(@"select Selsovet from Selsovet order by Selsovet", true);
        }

        private void UpdateComboBoxNasPunct()
        {
            var commandServer = new CommandServer();
            comboBox4.DataSource = commandServer.GetComboBoxList(@"select country
                    from country inner join selsovet on 
                        country.fk_selsovet = selsovet.key_selsovet
                    where selsovet.selsovet = " + "'" + comboBox3.Text + "'", true);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxNasPunct();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker9.Enabled = checkBox30.Checked;
            dateTimePicker8.Enabled = checkBox30.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Enabled = checkBox2.Checked;
            dateTimePicker4.Enabled = checkBox2.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string command;
            bool stat;

            if (radioButton2.Checked)
            {
                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    if (checkBox2.Checked)
                    {
                        if (checkBox3.Checked)
                        {
                            if (!string.IsNullOrEmpty(comboBox4.Text))
                            {
                                command = @"select * from ListObslSpezDateCountry('" + comboBox2.Text + "','" + dateTimePicker3.Text +
                                    "','" + dateTimePicker4.Text + "',null,'" + comboBox4.Text + "')";
                            }
                            else
                            {
                                command = @"select * from ListObslSpezDateCountry('" + comboBox2.Text + "','" +
                                          dateTimePicker3.Text + "','" + dateTimePicker4.Text + "','" + comboBox3.Text + "',null)";
                            }
                        }
                        else
                        {
                            command = @"select * from ListObslSpezDate('" + comboBox2.Text + "','" + 
                                dateTimePicker3.Text + "','" + dateTimePicker4.Text + "')";
                        }
                    }
                    else
                    {
                        if (checkBox3.Checked)
                        {
                            if (!string.IsNullOrEmpty(comboBox4.Text))
                            {
                                command = @"select * from ListObslSpezCountry('" + comboBox2.Text + "',null,'" +
                                          comboBox4.Text + "')";
                            }
                            else
                            {
                                command = @"select * from ListObslSpezCountry('" + comboBox2.Text + "','" +
                                          comboBox3.Text + "',null)";
                            }
                        }
                        else
                        {
                            command = @"select * from ListObslSpez('" + comboBox2.Text + "')";
                        }
                    }
                }
                else
                {
                    if (checkBox2.Checked)
                    {
                        if (checkBox3.Checked)
                        {
                            if (!string.IsNullOrEmpty(comboBox4.Text))
                            {
                                command = @"select * from ListObslSpezDateCountry(null,'" + dateTimePicker3.Text + "','" +
                                          dateTimePicker4.Text + "',null,'" + comboBox4.Text + "')";
                            }
                            else
                            {
                                command = @"select * from ListObslSpezDateCountry(null,'" + dateTimePicker3.Text + "','" +
                                          dateTimePicker4.Text + "','" + comboBox3.Text + "',null)";
                            }
                        }
                        else
                        {
                            command = @"select * from ListObslSpezDate(null,'" + dateTimePicker3.Text + "','" +
                                      dateTimePicker4.Text + "')";
                        }
                    }
                    else
                    {
                        if (checkBox3.Checked)
                        {
                            if (!string.IsNullOrEmpty(comboBox4.Text))
                            {
                                command = @"select * from ListObslSpezCountry(null,null,'" + comboBox4.Text + "')";
                            }
                            else
                            {
                                command = @"select * from ListObslSpezCountry(null,'" + comboBox3.Text + "',null)";
                            }
                        }
                        else
                        {
                            command = @"select * from ListObslSpez(null)";
                        }
                    }
                }
                stat = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(comboBox5.Text))
                {
                    if (checkBox30.Checked)
                    {
                        command = @"select * from ListHelpDate('" + comboBox5.Text + "','" + dateTimePicker9.Text +
                                  "','" + dateTimePicker8.Text + "')";
                    }
                    else
                    {
                        command = @"select * from ListHelp('" + comboBox5.Text + "')";
                    }
                }
                else
                {
                    if (!checkBox30.Checked)
                    {
                        command = @"select * from ListHelpNull()";
                    }
                    else
                    {
                        command = @"select * from ListDate('" + dateTimePicker9.Text + "','" + dateTimePicker8.Text +
                                  "')";
                    }
                }
                stat = true;
            }

            Cursor = Cursors.AppStarting;
            var commandServer = new CommandServer();
            dataGridView1.DataSource = commandServer.GetDataGridSet(command).Tables[0];
            toolStripStatusLabel1.Text = @"Записей: " + dataGridView1.RowCount;
            Cursor = Cursors.Arrow;

            if (dataGridView1.Columns.Count <= 0) return;
            dataGridView1.Columns[0].Visible = false;

            if (stat)
            {
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
            else
            {
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = checkBox3.Checked;
            comboBox4.Enabled = checkBox3.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = radioButton2.Checked;
            groupBox12.Enabled = radioButton3.Checked;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0) return;
            if (dataGridView1.CurrentRow == null) return;
            Hide();
            new AddAlone(false, Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value)).ShowDialog();
            Show();
        }
    }
}