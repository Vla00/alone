using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private int date_sm = 0;
        private int add = 0;
        private int doubl = 0;
        private int not = 0;

        public Dead()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);

            helpBackgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            helpBackgroundWorker.DoWork += new DoWorkEventHandler(DeadLoad);
            radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        #region Загрузка файла
        private void RadButton2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string[] lines = System.IO.File.ReadAllLines(openFileDialog1.FileName, System.Text.Encoding.Default);
            DataTable dt = new DataTable();

            if (lines.Length > 0)
            {
                
                string firstLines = lines[0];

                string[] header = firstLines.Split(';');

                foreach(string head in header)
                {
                    if(!string.IsNullOrEmpty(head))
                        dt.Columns.Add(new DataColumn(head.Substring(1, head.Length - 2)));
                }

                for(int i = 1; i < lines.Length; i++)
                {
                    string[] words = lines[i].Split(';');
                    DataRow rd = dt.NewRow();
                    int colIndex = 0;
                    foreach (string headW in header)
                    {
                        
                        if (!string.IsNullOrEmpty(headW))
                        {
                            if(!string.IsNullOrEmpty(words[colIndex]))
                                rd[headW.Substring(1, headW.Length - 2)] = words[colIndex].Substring(1, words[colIndex].Length - 2);
                            else
                                rd[headW.Substring(1, headW.Length - 2)] = words[colIndex];
                        }
                        colIndex++;
                    }

                    dt.Rows.Add(rd);
                }
            }

            if(dt.Rows.Count > 0)
            {
                radGridView1.DataSource = dt;
            }

            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private void RadButton1_Click(object sender, EventArgs e)
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

                string parameters = "'" + row.Cells["Фамилия Имя Отчество"].Value.ToString().Trim() + "', '" + row.Cells["Дата рождения"].Value.ToString().Trim() + "', '" +
                    row.Cells["Дата смерти"].Value.ToString().Trim() + "','" + row.Cells["Адрес"].Value.ToString().Trim() + "'";              
                
                radProgressBar1.Value1 = (int)value_progress;
                radProgressBar1.Text = radProgressBar1.Value1 + "%";

                var returnSqlServer = commandServer.ExecReturnServer("dead_add", parameters);
                if (returnSqlServer[0] == "1")
                {
                    row.Cells[row.Cells.Count - 1].Value = "Нет такого человека";
                    not++;
                }
                else
                {
                    if(returnSqlServer[0] == "2")
                    {
                        row.Cells[row.Cells.Count - 1].Value = "Данный человека уже помечен как умерший";
                        date_sm++;
                    }
                    else
                    {
                        if(returnSqlServer[0] == "3")
                        {
                            row.Cells[row.Cells.Count - 1].Value = "Данный человека уже занесен ранее";
                            doubl++;
                        }
                        else
                        {
                            row.Cells[row.Cells.Count - 1].Value = returnSqlServer[0];
                            add++;
                        }
                        
                    }
                }
            }
            radProgressBar1.Value1 = 100;
            radProgressBar1.Text = "100%";
            MessageBox.Show("Уже установлена дата: " + date_sm + "\nУже занесен в таблицу: " + doubl + "\nНет такого:" + not + "\nУспешно: " + add, "Отчет", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateGrid();
        }
        #endregion
        #region Подбор
        private void DeadLoad(object sender, DoWorkEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from DeadSelect() order by [ФИО из базы]").Tables[0] };

            radGridView2.Invoke(new MethodInvoker(delegate ()
            {
                radGridView2.DataSource = _bindingSource;

                if (radGridView2.Columns["Дата р."] != null)
                    radGridView2.Columns["Дата р."].FormatString = "{0:dd/MM/yyyy}";

                if (radGridView2.Columns["Дата смерти"] != null)
                    radGridView2.Columns["Дата смерти"].FormatString = "{0:dd/MM/yyyy}";


                radGridView2.Columns[0].IsVisible = false;
                radGridView2.Columns["key_dead"].IsVisible = false;
                radGridView2.Columns["cou"].IsVisible = false;

                GridViewCommandColumn command = new GridViewCommandColumn
                {
                    Name = "commandDelYes",
                    UseDefaultText = true,
                    DefaultText = "Удалить",
                    FieldName = "delete",
                    HeaderText = "Операция"
                };
                radGridView2.MasterTemplate.Columns.Add(command);
                radGridView2.CommandCellClick += new CommandCellClickEventHandler(RadGridViewButton_Click);
                
                radGridView2.Columns["Дата р."].BestFit();
                radGridView2.Columns["Адрес ГИССЗ"].BestFit();
                radGridView2.Columns["ФИО из базы"].BestFit();
                radGridView2.Columns["Адрес из базы"].BestFit();
                radGridView2.Columns["Дата смерти"].BestFit();
                radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }));
        }

        private void RadGridViewButton_Click(object sender, EventArgs e)
        {
            if (((GridCommandCellElement)sender).Data.FieldName == "delete")
                new CommandServer().ExecNoReturnServer("Dead_delete", radGridView2.CurrentRow.Cells[0].Value.ToString());
            else
                new CommandServer().ExecNoReturnServer("Dead_delete", radGridView2.CurrentRow.Cells[0].Value.ToString());
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from DeadSelect() order by [ФИО из базы]").Tables[0] };
            radGridView2.DataSource = _bindingSource;
        }

        private void RadGridView3_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (!e.RowElement.RowInfo.Cells["Адрес ГИССЗ"].Value.ToString().Contains(e.RowElement.RowInfo.Cells["cou"].Value.ToString()))
            {
                e.RowElement.DrawFill = true;
                e.RowElement.GradientStyle = GradientStyles.Solid;
                e.RowElement.BackColor = Color.Red;
            }
            else
            {
                e.RowElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            }
        }

        

        #endregion

        private void Dead_Load(object sender, EventArgs e)
        {
            if (!helpBackgroundWorker.IsBusy)
                helpBackgroundWorker.RunWorkerAsync();
        }

        private void RadGridView3_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Hide();
                try
                {
                    new Alone(false, Convert.ToInt32(radGridView2.CurrentRow.Cells[0].Value), "sm " + radGridView2.CurrentRow.Cells["Дата смерти"].Value, null ).ShowDialog();
                    UpdateGrid();
                }
                catch (Exception ex)
                {
                    var commandClient = new CommandClient();
                    commandClient.WriteFileError(ex, null);
                }
                Show();
            }
        }

        private void Dead_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void RadGridView2_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if(e.CellElement.ColumnInfo is GridViewCommandColumn)
            {
                RadButtonElement button = (RadButtonElement)e.CellElement.Children[0];
                if (e.CellElement.BackColor == Color.Red)
                {
                    button.Enabled = true;
                }
                else
                {
                    button.Enabled = false;
                }
            }
        }
    }
}
