using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.Service
{
    public partial class Dead : Form
    {
        TelerikMetroTheme theme = new TelerikMetroTheme();
        BackgroundWorker helpBackgroundWorker;
        private BindingSource _bindingSource;
        private string file;

        private int date_sm = 0;
        private int add = 0;
        private int doubl = 0;

        public Dead()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);

            helpBackgroundWorker = new BackgroundWorker();
            helpBackgroundWorker.WorkerReportsProgress = true;
            helpBackgroundWorker.WorkerSupportsCancellation = true;
            helpBackgroundWorker.DoWork += new DoWorkEventHandler(DeadLoad);
            radGridView3.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            file = openFileDialog1.FileName;

            //ExcelFile ef = ExcelFile.Load(openFileDialog1.FileName);
            //DataGridViewConverter

            Excel_load();
        }

        private void Excel_load()
        {
            string str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                file + ";Extended Properties='Excel 8.0;HDR=YES;';";
            OleDbConnection connection = new OleDbConnection(str);
            OleDbCommand command = new OleDbCommand("select * from [Лист1$]", connection);
            connection.Open();
            OleDbDataAdapter data_adapter = new OleDbDataAdapter(command);
            DataTable data = new DataTable();
            data_adapter.Fill(data);
            radGridView1.DataSource = data;

            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            //_bindingSourceLoad
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(SqlAdd);
            thread.Start();
        }

        private void SqlAdd()
        {
            radProgressBar1.Value1 = 0;
            radProgressBar1.Text = "0%";
            var commandServer = new CommandServer();
            double step = 100.00f / (double)radGridView1.RowCount;
            double value_progress = 0.00f;

            foreach (GridViewRowInfo row in radGridView1.Rows)
            {
                value_progress += step;
                row.IsSelected = true;

                string parameters = "'" + row.Cells[0].Value.ToString().Trim() + "', '" + row.Cells[1].Value.ToString().Trim() + "', '" +
                    row.Cells[2].Value.ToString().Trim() + "'";

                if (!string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                {
                    parameters += ",'" + row.Cells[3].Value.ToString().Trim() + "'";
                }
                else
                    parameters += ",null";

                if(!string.IsNullOrEmpty(row.Cells[4].Value.ToString()))
                {
                    parameters += ",'" + row.Cells[4].Value.ToString().Trim() + "'";
                }
                else
                    parameters += ",null";
                radProgressBar1.Value1 = (int)value_progress;
                radProgressBar1.Text = radProgressBar1.Value1 + "%";

                var returnSqlServer = commandServer.GetServerCommandExecReturnServer("dead_add", parameters);
                if (returnSqlServer[0] == "1")
                {
                    row.Cells["Результат"].Value = "Дата смерти уже установлена";
                    date_sm++;
                }
                else
                {
                    if(returnSqlServer[0] == "2")
                    {
                        row.Cells["Результат"].Value = "Данные выли занесены ранее";
                        doubl++;
                    }else
                    {
                        row.Cells["Результат"].Value = returnSqlServer[0];
                        add++;
                    }
                }
                //
                //ФИО, Дата рождения, сельсовет, улица, дата смерти
            }
            radProgressBar1.Value1 = 100;
            radProgressBar1.Text = "100%";
            MessageBox.Show("Установлена дата смерти: " + date_sm + "\nДанные выли занесены ранее: " + doubl + "\nУспешно: " + add, "Отчет", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void DeadLoad(object sender, DoWorkEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select fio as [ФИО], date_ro, selsovet as [С\С]
                from dead order by [ФИО]").Tables[0] };

            radGridView3.Invoke(new MethodInvoker(delegate ()
            {
                radGridView3.DataSource = _bindingSource;

                GridViewDateTimeColumn dateHelp = new GridViewDateTimeColumn("Дата рождения");
                radGridView3.Columns[2] = dateHelp;
                dateHelp.Name = "date_ro";
                dateHelp.FieldName = "date_ro";
                dateHelp.FormatString = "{0:dd/MM/yyyy}";
                dateHelp.Format = DateTimePickerFormat.Custom;
                dateHelp.CustomFormat = "dd.MM.yyyy";

            }));
        }

        private void Dead_Load(object sender, EventArgs e)
        {
            if (!helpBackgroundWorker.IsBusy)
                helpBackgroundWorker.RunWorkerAsync();
        }
    }
}
