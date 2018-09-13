using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class SpeziolistDelete : RadForm
    {
        private BindingSource _bindingSource_personel;
        private BindingSource _bindingSource_personel_time;
        private BindingSource _bindingSource_off;
        //private BindingSource _bindingSource_off_time;
        private bool _status = false;
        //BackgroundWorker myBackgroundWorker;

        //private bool _status = false;

        public SpeziolistDelete()
        {
            InitializeComponent();
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            //radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;
            HandleCreated += Form_HandleCreated;
            
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            LoadGridFio();
            LoadGridFioOff();
        }
        
        /// <summary>
        /// НОВОЕ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Load
        public void LoadGridFio()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView5.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView5.EnablePaging = true;
                    _bindingSource_personel = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                        from spezialistViewColor('0') 
                        where date_end is null
                        order by [ФИО]").Tables[0] };
                    radGridView5.DataSource = _bindingSource_personel;

                    if (radGridView5.Columns.Count > 0)
                    {
                        radGridView5.Columns[0].IsVisible = false;
                        radGridView5.Columns[2].IsVisible = false;
                        radGridView5.Columns[3].IsVisible = false;
                        radGridView5.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        public void LoadGridFioOff()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView2.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView2.EnablePaging = true;
                    _bindingSource_off = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                        from spezialistViewColor('0') 
                        where date_end is not null
                        order by [ФИО]").Tables[0] };
                    radGridView2.DataSource = _bindingSource_off;

                    if (radGridView2.Columns.Count > 0)
                    {
                        radGridView2.Columns[0].IsVisible = false;
                        radGridView2.Columns[2].IsVisible = false;
                        radGridView2.Columns[3].IsVisible = false;
                        radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }
        #endregion
        
        /*private void Load_Personel(object status)
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView1.EnablePaging = true;
                    _bindingSource_personel = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select key_speziolist, fio as [ФИО]
                        from speziolist
                        where statusDelete = 0
                        order by fio").Tables[0] };
                    radGridView1.DataSource = _bindingSource_personel;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.Columns[0].IsVisible = false;
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }

                    if((bool)status)
                    {
                        GridViewCommandColumn commandColumn = new GridViewCommandColumn();
                        commandColumn.Name = "операция";
                        commandColumn.UseDefaultText = true;
                        commandColumn.DefaultText = "удалить";
                        commandColumn.FieldName = "key_speziolist";
                        commandColumn.HeaderText = "команда";
                        radGridView1.MasterTemplate.Columns.Add(commandColumn);
                        radGridView1.CommandCellClick += new CommandCellClickEventHandler(radGridView1_CommandCellClick);
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void Load_Personel_Off(object status)
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView2.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView2.EnablePaging = true;
                    _bindingSource_off = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select key_speziolist, fio as [ФИО]
                        from speziolist
                        where 
                        ete = 1
                        order by fio").Tables[0] };
                    radGridView2.DataSource = _bindingSource_off;

                    if (radGridView2.Columns.Count > 0)
                    {
                        radGridView2.Columns[0].IsVisible = false;
                        radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }

                    if ((bool)status)
                    {
                        GridViewCommandColumn commandColumn = new GridViewCommandColumn();
                        commandColumn.Name = "операция";
                        commandColumn.UseDefaultText = true;
                        commandColumn.DefaultText = "восстановить";
                        commandColumn.FieldName = "key_speziolist";
                        commandColumn.HeaderText = "команда";
                        radGridView2.MasterTemplate.Columns.Add(commandColumn);
                        radGridView2.CommandCellClick += new CommandCellClickEventHandler(radGridView2_CommandCellClick);
                        radGridView2.Columns[2].BestFit();
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }*/

        private void radGridView5_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if(_status)
            {
                _status = false;
                return;
            }

            if (e.Rows[0].Cells[1].Value == null || e.Rows[0].Cells[1].Value.ToString() == "")
            {
                AlertOperation("speziolist_add", new string[] { "Не указано ФИО.", "1" });
                e.Cancel = true;
                return;
            }
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            string fio;
            var line = e.Rows[0].Cells[1].Value.ToString();

            if (e.Rows[0].Cells[1].Value.ToString().Split(' ').Length == 3)
            {
                fio = commandClient.CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[0]) + " " +
                    commandClient.CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[1]) + " " +
                    commandClient.CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[2]);
            }
            else
                fio = e.Rows[0].Cells[1].Value.ToString();

            var parameters = "'" + fio + "','0'";
            var returnSqlServer = commandServer.GetServerCommandExecNoReturnServer("speziolist_add", parameters);
            if (returnSqlServer[1] == "0")
                e.Cancel = true;

            AlertOperation("socRabotnik_add " + parameters, returnSqlServer);
            radGridView5.DataSource = _bindingSource_personel;
            //Load_Personel(false);
        }

        private void radGridView5_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource_personel = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                        from spezialistViewColor('0') 
                        where date_end is null
                        order by [ФИО]").Tables[0] };

            radGridView5.Invoke(new MethodInvoker(delegate ()
            {
                radGridView5.DataSource = _bindingSource_personel;
            }));
        }

        private void radGridView1_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            if(_status)
            {
                _status = false;
                return;
            }
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    
                    if (e.PropertyName == "ФИО")
                    {
                        var parameters = "'" + line.Cells[1].Value.ToString() + "','";
                        string text = null;

                        if (e.NewValue.ToString().Split(' ').Length == 3)
                        {
                            text = commandClient.CharTo(e.NewValue.ToString().Split(' ')[0]) + " " +
                                commandClient.CharTo(e.NewValue.ToString().Split(' ')[1]) + " " +
                                commandClient.CharTo(e.NewValue.ToString().Split(' ')[2]);
                        }
                        else
                            text = e.NewValue.ToString();
                        parameters += text + "','0'";

                        var returnSqlServer = commandServer.GetServerCommandExecNoReturnServer("speziolist_edit", parameters);
                        if (returnSqlServer[1] == "0")
                            e.Cancel = true;

                        AlertOperation("speziolist_edit " + line.Cells[1].Value, returnSqlServer);
                    }
                    //var returnSqlServer = commandServer.GetServerCommandExecNoReturnServer("speziolist_edit", parameters);
                    //Load_Personel(false);
                }
            }
        }

        private void radGridView5_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();
            try
            {
                if (radGridView5.CurrentRow.Cells[0].Value != null)
                {
                    _bindingSource_personel_time = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select key_job_spezialist, date_start, date_end
                    from job_spezialist left join speziolist on job_spezialist.fk_spezialist = speziolist.key_speziolist
                    where fk_spezialist = " + radGridView5.CurrentRow.Cells[0].Value).Tables[0] };
                    radGridView6.DataSource = _bindingSource_personel_time;

                    GridViewDateTimeColumn dateS = new GridViewDateTimeColumn("Дата начала");
                    radGridView6.Columns[1] = dateS;
                    dateS.Name = "date_s";
                    dateS.FieldName = "date_start";
                    dateS.FormatString = "{0:dd/MM/yyyy}";
                    dateS.Format = DateTimePickerFormat.Custom;
                    dateS.CustomFormat = "dd.MM.yyyy";

                    GridViewDateTimeColumn dateE = new GridViewDateTimeColumn("Дата конца");
                    radGridView6.Columns[2] = dateE;
                    dateE.Name = "date_e";
                    dateE.FieldName = "date_end";
                    dateE.FormatString = "{0:dd/MM/yyyy}";
                    dateE.Format = DateTimePickerFormat.Custom;
                    dateE.CustomFormat = "dd.MM.yyyy";
                    
                    radGridView6.Columns[0].IsVisible = false;

                    radGridView6.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridView6.Columns[1].BestFit();
                    radGridView6.Columns[2].BestFit();
                    if (radGridView6.RowCount > 0)
                        UpdateLoadJobTime();
                    else
                        richTextBox1.Text = "";
                }
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void radGridView6_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (e.Rows[0].Cells[1].Value == null)
            {
                AlertOperation("job_spezialist_add", new string[] { "Не указана дата начала.", "1" });
                e.Cancel = true;
                return;
            }
            _status = true;
            var commandServer = new CommandServer();
            var line = radGridView5.CurrentRow.Cells[0].Value + ",'" + e.Rows[0].Cells[1].Value + "',";
            radGridView5.CurrentRow.Cells[2].Value = e.Rows[0].Cells[1].Value;

            if (e.Rows[0].Cells[2].Value != null)
            {
                line += "'" + e.Rows[0].Cells[2].Value + "'";
                radGridView5.CurrentRow.Cells[3].Value = e.Rows[0].Cells[2].Value;
            }
            else
            {
                line += "null";
            }

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("job_spezialist_add", line);
            UpdateLoadJobTime();
            AlertOperation("job_spezialist_add " + line, returnSqlServer);
        }

        private void AlertOperation(string operation, string[] resultOperation)
        {
            var commandClient = new CommandClient();

            if (resultOperation[1] == "1")
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
            else
            {
                commandClient.WriteFileError(null, operation);
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
        }

        private void radGridView6_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            bool update = false;

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    _status = true;
                    var parameters = "'" + line.Cells[0].Value.ToString() + "','";
                    if (e.PropertyName == "date_start")
                    {
                        parameters += e.NewValue.ToString() + "',";
                        radGridView5.CurrentRow.Cells[2].Value = e.NewValue;

                    }
                    else
                    {
                        parameters += line.Cells[1].Value.ToString() + "',";
                    }
                    if (e.PropertyName == "date_end")
                    {
                        if (e.NewValue == null)
                        {
                            parameters += "null";
                        }
                        else
                        {
                            parameters += "'" + e.NewValue.ToString() + "'";
                            radGridView5.CurrentRow.Cells[3].Value = e.NewValue;
                            update = true;
                        }
                    }
                    else
                    {
                        if (line.Cells[2].Value == null || line.Cells[2].Value.ToString() == "")
                        {
                            parameters += "null";
                        }
                        else
                            parameters += "'" + line.Cells[2].Value.ToString() + "'";
                    }

                    var returnSqlServer = commandServer.GetServerCommandExecReturnServer("job_spezialist_edit", parameters);
                    //UpdateLoadJobTime();
                    AlertOperation("job_spezialist_edit " + line.Cells[1].Value, returnSqlServer);
                    if(update)
                    {
                        LoadGridFio();
                        LoadGridFioOff();
                        //UpdateLoadJobTime();
                    }else
                    {
                        UpdateLoadJobTime();
                    }
                }
            }
        }

        private void radGridView5_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            if (_status)
            {
                _status = false;
                return;
            }
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    if (e.PropertyName == "ФИО")
                    {
                        var parameters = "'" + line.Cells[1].Value.ToString() + "','";
                        string text = null;

                        if (e.NewValue.ToString().Split(' ').Length == 3)
                        {
                            text = commandClient.CharTo(e.NewValue.ToString().Split(' ')[0]) + " " +
                                commandClient.CharTo(e.NewValue.ToString().Split(' ')[1]) + " " +
                                commandClient.CharTo(e.NewValue.ToString().Split(' ')[2]);
                        }
                        else
                            text = e.NewValue.ToString();
                        parameters += text + "','0'";

                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("speziolist_edit", parameters);
                        if (returnSqlServer[1] == "0")
                            e.Cancel = true;
                        AlertOperation("speziolist_edit " + line.Cells[1].Value, returnSqlServer);
                    }
                }
            }
        }

        private void radGridView6_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();
            commandServer.GetServerCommandExecNoReturnServer("job_spezialist_delete", e.Rows[0].Cells[0].Value.ToString());
            LoadGridFio();
            LoadGridFioOff();
        }

        private void UpdateLoadJobTime()
        {
            var commandServer = new CommandServer();
            DataSet val = commandServer.GetDataGridSet("select * from CalcStagResultPersonel(" + radGridView5.CurrentRow.Cells[0].Value + ")");
            richTextBox1.Text = "";
            if (val != null)
            {
                if(val.Tables[0].Rows.Count > 0)
                {
                    if (val.Tables[0].Rows[0][1].ToString() != "0" && val.Tables[0].Rows[0][1].ToString() != "")
                    {
                        richTextBox1.Text = val.Tables[0].Rows[0][1].ToString() + " лет\n";
                    }
                    if (val.Tables[0].Rows[0][2].ToString() != "0" && val.Tables[0].Rows[0][2].ToString() != "")
                    {
                        richTextBox1.Text += val.Tables[0].Rows[0][2].ToString() + " месяцев\n";
                    }
                    if (val.Tables[0].Rows[0][3].ToString() != "0" && val.Tables[0].Rows[0][3].ToString() != "")
                    {
                        richTextBox1.Text += val.Tables[0].Rows[0][3].ToString() + " дней\n";
                    }
                }                
            }
        }

        private void radGridView2_Click(object sender, EventArgs e)
        {

        }

        private void dateTime_ValueChanged(object sender, EventArgs e)
        {
            SendKeys.Send(".");
        }

        private void radGridView6_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadDateTimeEditor dateTimeEditor = e.ActiveEditor as RadDateTimeEditor;
            if(dateTimeEditor != null)
            {
                radGridView6.CellEditorInitialized -= radGridView6_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                MaskDateTimeProvider provider = editroElement.TextBoxElement.Provider as MaskDateTimeProvider;
                if (provider != null)
                    provider.AutoSelectNextPart = true;
            }
        }
    }
}
