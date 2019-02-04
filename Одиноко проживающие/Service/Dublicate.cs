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
                    _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select (family.family + ' ' + family.name + ' ' + family.surname) as [ФИО], count(*) as [кол.]
                        from dublicate inner join alone on dublicate.fk_alone = alone.key_alone
	                        inner join family on family.fk_alone = alone.key_alone
                        group by family.family, family.name, family.surname").Tables[0] };

                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.Columns[0].BestFit();
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }

                    radGridView6.EnablePaging = true;
                    _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select sojitel.fk_alone, sojitel.fio as [старое], sojitel.family as [нов. фам.], sojitel.name as [нов. имя], sojitel.surname as [новое отч.]
	                    from sojitel
	                    where sojitel.fio != (sojitel.family + ' ' + sojitel.name + ' ' + sojitel.surname)").Tables[0] };

                    radGridView6.DataSource = _bindingSource;

                    if (radGridView6.Columns.Count > 0)
                    {
                        radGridView6.Columns[0].IsVisible = false;
                        radGridView6.Columns[0].BestFit();
                        radGridView6.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }

                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }        

        private void LoadGrid2()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from AloneDublicate(" + radGridView2.CurrentRow.Cells[0].Value.ToString() + ")").Tables[0];
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

            BindingSource _bs = new BindingSource { DataSource = commandServer.DataGridSet(@"select category.category as [категории]
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
            DublicateDelo();
        }        

        private void radGridView5_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Hide();
                try
                {
                    new Alone(false, Convert.ToInt32(adres_radGridView.CurrentRow.Cells[0].Value)).ShowDialog();
                }
                catch (Exception ex)
                {
                    var commandClient = new CommandClient();
                    commandClient.WriteFileError(ex, null);
                }
                Show();
            }
        }

        private void radGridView6_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Hide();
                try
                {
                    new Alone(false, Convert.ToInt32(radGridView6.CurrentRow.Cells[0].Value)).ShowDialog();
                }
                catch (Exception ex)
                {
                    var commandClient = new CommandClient();
                    commandClient.WriteFileError(ex, null);
                }
                Show();
            }
        }

        #region Дубликат
        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView2.SelectedRows.Count > 0)
            {
                var commandServer = new CommandServer();
                var returnSqlServer = commandServer.ExecNoReturnServer("DublicateDelete", radGridView2.Rows[0].Cells[0].Value.ToString() + "," + radGridView2.Rows[1].Cells[0].Value.ToString() + "," + radGridView2.RowCount);
                //if (returnSqlServer[1] != "Успешно")
               // {
                    if (radGridView2.RowCount == 2)
                    {
                        StartDataGrid();
                        radGridView2.DataSource = null;
                    }
                    else
                    {
                        DublicateDelo();
                    }
                //}else
                //{
                //    radDesktopAlert1.ContentText = returnSqlServer[0];
                //    radDesktopAlert1.Show();
                //}
            }
            else
            {
                RadMessageBox.Show(@"Должна быть выделена одна запись.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (RadMessageBox.Show(@"Вы точно хотите объединить записи БЕЗВОЗВРАТНО?", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Question) == DialogResult.OK)
            {
                var commandServer = new CommandServer();
                var returnSqlServer = commandServer.ExecReturnServer("DublicateUnion", radGridView2.Rows[0].Cells[0].Value.ToString() + "," + radGridView2.Rows[1].Cells[0].Value.ToString() + "," + radGridView2.RowCount);
                if (returnSqlServer[1] == "1")
                {
                    if (radGridView2.RowCount == 2)
                    {
                        StartDataGrid();
                        radGridView2.DataSource = null;
                    }
                    else
                    {
                        DublicateDelo();
                    }
                }
                else
                {
                    radDesktopAlert1.ContentText = returnSqlServer[0];
                    radDesktopAlert1.Show();
                }
            }
        }

        private void radGridView1_Click(object sender, EventArgs e)
        {
            DublicateDelo();
        }

        private void DublicateDelo()
        {
            var commandServer = new CommandServer();
            radGridView2.EnablePaging = true;
            _bindingSourceDublicate = new BindingSource
            {
                DataSource = commandServer.DataGridSet(@"select alone.key_alone as [Дело], (family.family + ' ' + family.name + ' ' + family.surname) as [ФИО], 
	                case when row_number() over (partition by family.family, family.name, family.surname order by alone.key_alone) = 1 then 'Эталон' else 'Дубликат' end as [Доп. поле]
                from dublicate inner join alone on dublicate.fk_alone = alone.key_alone
	                inner join family on family.fk_alone = alone.key_alone
                where (family.family + ' ' + family.name + ' ' + family.surname) = '" + radGridView1.CurrentRow.Cells[0].Value.ToString() + "'")
            .Tables[0]
            };

            radGridView2.DataSource = _bindingSourceDublicate;
            radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView2.Columns[0].BestFit();
        }

        private void radGridView2_Click(object sender, EventArgs e)
        {
            LoadGrid2();
        }
        #endregion

        private void radGridView2_DoubleClick(object sender, EventArgs e)
        {
            Hide();
            try
            {
                new Alone(false, Convert.ToInt32(radGridView2.CurrentRow.Cells[0].Value)).ShowDialog();
            }
            catch (Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
            Show();
            }
    }
}
