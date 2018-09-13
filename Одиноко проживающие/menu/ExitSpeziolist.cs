using System;
using System.Windows.Forms;

namespace Одиноко_проживающие.menu
{
    public partial class ExitSpeziolist : Form
    {
        public ExitSpeziolist()
        {
            InitializeComponent();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = @"dd.MM.yyyy";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = @"dd.MM.yyyy";
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            var commandTextCount = "select * from ListExitSpeziolistCount('" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "')";
            var commandTextSelsovet = "select * from ListExitSpeziolistSelsov('" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "')";
            var commandServer = new CommandServer();

            dataGridView1.DataSource = radioButton1.Checked ? commandServer.GetDataGridSet(commandTextCount).Tables[0] : 
                commandServer.GetDataGridSet(commandTextSelsovet).Tables[0];

            if (radioButton1.Checked)
            {
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
            else
            {
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }

            Cursor = Cursors.Default;
        }
    }
}
