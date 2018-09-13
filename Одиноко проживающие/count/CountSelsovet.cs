using System;
using System.Windows.Forms;

namespace Одиноко_проживающие.count
{
    public partial class CountSelsovet : Form
    {
        private readonly bool _statusLoad;
        public CountSelsovet()
        {
            InitializeComponent();

            dataGridView1.MultiSelect = false;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _statusLoad = true;
            UpdateDateGrid();
            UpdateComboBoxSelsovet();
            _statusLoad = false;
        }

        private void UpdateComboBoxSelsovet()
        {
            var commandServer = new CommandServer();
            comboBox1.DataSource = commandServer.GetComboBoxList("select Selsovet from Selsovet order by Selsovet");
        }

        private void UpdateComboBoxNasPunct()
        {
            var commandServer = new CommandServer();
            comboBox2.DataSource = commandServer.GetComboBoxList(@"select country
                    from country inner join selsovet on 
                        country.fk_selsovet = selsovet.key_selsovet
                    where selsovet.selsovet = " + "'" + comboBox1.Text + "'");
        }

        private void UpdateDateGrid()
        {
            var command = @"select * from ListCountCountry(";
            if (string.IsNullOrEmpty(comboBox1.Text))
                command += "null";
            else
                command += "'" + comboBox1.Text + "'";
            if (string.IsNullOrEmpty(comboBox2.Text))
                command += ",null)";
            else
                command += ",'" + comboBox2.Text + "')";
            command += " order by [ФИО]";

            var commandServer = new CommandServer();
            dataGridView1.DataSource = commandServer.GetDataGridSet(command).Tables[0];

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
            toolStripStatusLabel1.Text = @"Записей: " + dataGridView1.Rows.Count;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxNasPunct();
            if(!_statusLoad)
                UpdateDateGrid();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!_statusLoad)
                UpdateDateGrid();
        }
    }
}
