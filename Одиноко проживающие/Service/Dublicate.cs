using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.service
{
    public partial class Dublicate : RadForm
    {
        private BindingSource _bindingSource;
        private BindingSource _bindingSourceDublicate;
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Dublicate()
        {
            InitializeComponent();
            RadMessageBox.SetThemeName(theme.ThemeName);
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;
            
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            Thread _thread = new Thread(new ThreadStart(StartDataGrid));
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void StartDataGrid()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView1.EnablePaging = true;
                    _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select max(alone.fio) as [ФИО], max(year(alone.date_ro)) as [год рождения], count(*) as [кол. дубликатов]
                        from alone
                        where alone.dublicate is null
                        group by alone.fio, year(alone.date_ro)
                        having count(*) > 1
                        order by ФИО").Tables[0] };

                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.Columns[0].BestFit();
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void radGridView1_Click(object sender, EventArgs e)
        {
            LoadGrid1();
        }

        private void LoadGrid1()
        {
            var commandServer = new CommandServer();
            radGridView2.EnablePaging = true;
            _bindingSourceDublicate = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select alone.key_alone as [Дело], alone.fio as [ФИО]
                from alone
                where alone.fio = '" + radGridView1.CurrentRow.Cells[0].Value.ToString() + "' and YEAR(alone.date_ro) = '" + radGridView1.CurrentRow.Cells[1].Value.ToString() + "' and dublicate is null")
            .Tables[0] };

            radGridView2.DataSource = _bindingSourceDublicate;
            radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView2.Columns[0].BestFit();
        }

        private void radGridView2_Click(object sender, EventArgs e)
        {
            LoadGrid2();
        }

        private void LoadGrid2()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.GetDataGridSet(@"select * from AloneDublicate(" + radGridView2.CurrentRow.Cells[0].Value.ToString() + ")").Tables[0];
            radLabel2.Text = dt.Rows[0].ItemArray[0].ToString();
            radLabel4.Text = dt.Rows[0].ItemArray[1].ToString().Split(' ')[0];
            if (dt.Rows[0].ItemArray[2].ToString() == "false")
                radLabel6.Text = "Ж";
            else
                radLabel6.Text = "M";
            radLabel8.Text = dt.Rows[0].ItemArray[3].ToString();
            radLabel10.Text = dt.Rows[0].ItemArray[4].ToString();
            radLabel16.Text = dt.Rows[0].ItemArray[5].ToString();
            radLabel15.Text = dt.Rows[0].ItemArray[6].ToString();
            radLabel14.Text = dt.Rows[0].ItemArray[7].ToString();

            BindingSource _bs = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select category.category as [категории]
                from category
                where category.fk_alone = " + radGridView2.CurrentRow.Cells[0].Value.ToString()).Tables[0] };
            radGridView3.DataSource = _bs;
            radGridView3.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private void radGridView2_KeyDownUp(object sender, KeyEventArgs e)
        {
            LoadGrid2();
        }

        private void radGridView1_KeyDownUp(object sender, KeyEventArgs e)
        {
            LoadGrid1();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if(RadMessageBox.Show(@"Вы точно хотите удалить запись БЕЗВОЗВРАТНО?", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Question) == DialogResult.OK )
            {
                if (radGridView2.SelectedRows.Count > 0)
                {
                    var commandServer = new CommandServer();
                    var returnSqlServer = commandServer.GetServerCommandExecReturnServer("AloneDelete", radGridView2.CurrentRow.Cells[0].Value.ToString());
                    if (returnSqlServer[1] != "Успешно")
                    {
                        if (radGridView2.RowCount == 2)
                        {
                            StartDataGrid();
                            radGridView2.DataSource = null;
                        }
                        else
                        {
                            LoadGrid1();
                        }
                    }
                }
                else
                {
                    RadMessageBox.Show(@"Должна быть выделена только запись.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }            
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if(radGridView2.SelectedRows.Count > 0)
            {
                var commandServer = new CommandServer();
                var returnSqlServer = commandServer.GetServerCommandExecReturnServer("DublicateDelete", radGridView2.CurrentRow.Cells[0].Value.ToString());
                if (returnSqlServer[1] != "Успешно")
                {
                    if(radGridView2.RowCount == 2)
                    {
                        StartDataGrid();
                        radGridView2.DataSource = null;
                    }else
                    {
                        LoadGrid1();
                    }
                }
            }
            else
            {
                RadMessageBox.Show(@"Должна быть выделена одна запись.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
    }
}
