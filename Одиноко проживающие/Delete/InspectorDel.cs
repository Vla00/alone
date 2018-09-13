﻿using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class InspectorDelete : RadForm
    {
        private BindingSource _bindingSource_personel;
        private BindingSource _bindingSource_personel_time;
        private BindingSource _bindingSource_off;
        private BindingSource _bindingSource_off_time;
        private bool _status = false;

        public InspectorDelete()
        {
            InitializeComponent();
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            CheckForIllegalCrossThreadCalls = false;
            HandleCreated += Form_HandleCreated;            
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            LoadGridFio();
            LoadGridFioOff();
        }        
        
        #region действующие

        #region ФИО
        public void LoadGridFio()
        {
            try
            {
                radGridView5.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView5.EnablePaging = true;
                    _bindingSource_personel = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from spezialistViewColor('1') 
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

        private void radGridView5_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (_status)
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
            
            string fio;
            var line = e.Rows[0].Cells[1].Value.ToString();

            if (e.Rows[0].Cells[1].Value.ToString().Split(' ').Length == 3)
            {
                var commandClient = new CommandClient();
                fio = commandClient.CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[0]) + " " +
                    commandClient.CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[1]) + " " +
                    commandClient.CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[2]);
            }
            else
                fio = e.Rows[0].Cells[1].Value.ToString();

            var parameters = "'" + fio + "','1'";
            var returnSqlServer = new CommandServer().GetServerCommandExecNoReturnServer("speziolist_add", parameters);
            if (returnSqlServer[1] == "0")
                e.Cancel = true;

            AlertOperation("socRabotnik_add " + parameters, returnSqlServer);
            radGridView5.DataSource = _bindingSource_personel;
        }

        private void radGridView5_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            _bindingSource_personel = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from spezialistViewColor('1') 
                        where date_end is null
                        order by [ФИО]").Tables[0] };

            radGridView5.Invoke(new MethodInvoker(delegate ()
            {
                radGridView5.DataSource = _bindingSource_personel;
            }));
        }

        private void radGridView5_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            if (_status)
            {
                _status = false;
                return;
            }

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
                            var commandClient = new CommandClient();
                            text = commandClient.CharTo(e.NewValue.ToString().Split(' ')[0]) + " " +
                                commandClient.CharTo(e.NewValue.ToString().Split(' ')[1]) + " " +
                                commandClient.CharTo(e.NewValue.ToString().Split(' ')[2]);
                        }
                        else
                            text = e.NewValue.ToString();
                        parameters += text + "','1'";

                        var returnSqlServer = new CommandServer().GetServerCommandExecReturnServer("speziolist_edit", parameters);
                        if (returnSqlServer[1] == "0")
                            e.Cancel = true;
                        AlertOperation("speziolist_edit " + line.Cells[1].Value, returnSqlServer);
                    }
                }
            }
        }

        private void radGridView5_Click(object sender, EventArgs e)
        {
            try
            {
                if (radGridView5.CurrentRow.Cells[0].Value != null)
                {
                    _bindingSource_personel_time = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select key_job_spezialist, date_start, date_end
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
                    dateE.CustomFormat = "d.M.y";

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
        #endregion

        #region Дата
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
                    AlertOperation("job_spezialist_edit " + line.Cells[1].Value, returnSqlServer);
                    if (update)
                    {
                        LoadGridFio();
                        LoadGridFioOff();
                    }
                    else
                    {
                        UpdateLoadJobTime();
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

        private void radGridView6_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadDateTimeEditor dateTimeEditor = e.ActiveEditor as RadDateTimeEditor;
            if (dateTimeEditor != null)
            {
                radGridView6.CellEditorInitialized -= radGridView6_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                MaskDateTimeProvider provider = editroElement.TextBoxElement.Provider as MaskDateTimeProvider;
                if (provider != null)
                    provider.AutoSelectNextPart = true;
            }
        }

        private void UpdateLoadJobTime()
        {
            var commandServer = new CommandServer();
            DataSet val = commandServer.GetDataGridSet("select * from CalcStagResultPersonel(" + radGridView5.CurrentRow.Cells[0].Value + ")");
            richTextBox1.Text = "";
            if (val != null)
            {
                if (val.Tables[0].Rows.Count > 0)
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
        #endregion

        #endregion

        #region Уволеные
        public void LoadGridFioOff()
        {
            try
            {
                radGridView2.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView2.EnablePaging = true;
                    _bindingSource_off = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from spezialistViewColor('1') 
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

        private void radGridView2_Click(object sender, EventArgs e)
        {
            try
            {
                if (radGridView2.CurrentRow.Cells[0].Value != null)
                {
                    _bindingSource_off_time = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select key_job_spezialist, date_start, date_end
                    from job_spezialist left join speziolist on job_spezialist.fk_spezialist = speziolist.key_speziolist
                    where fk_spezialist = " + radGridView2.CurrentRow.Cells[0].Value).Tables[0] };
                    radGridView4.DataSource = _bindingSource_off_time;

                    GridViewDateTimeColumn dateS_off = new GridViewDateTimeColumn("Дата начала");
                    radGridView4.Columns[1] = dateS_off;
                    dateS_off.Name = "date_s_off";
                    dateS_off.FieldName = "date_start";
                    dateS_off.FormatString = "{0:dd/MM/yyyy}";
                    dateS_off.Format = DateTimePickerFormat.Custom;
                    dateS_off.CustomFormat = "dd.MM.yyyy";

                    GridViewDateTimeColumn dateE_off = new GridViewDateTimeColumn("Дата конца");
                    radGridView4.Columns[2] = dateE_off;
                    dateE_off.Name = "date_e_off";
                    dateE_off.FieldName = "date_end";
                    dateE_off.FormatString = "{0:dd/MM/yyyy}";
                    dateE_off.Format = DateTimePickerFormat.Custom;
                    dateE_off.CustomFormat = "dd.MM.yyyy";

                    radGridView4.Columns[0].IsVisible = false;
                    radGridView4.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridView4.Columns[1].BestFit();
                    radGridView4.Columns[2].BestFit();
                }
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void radGridView4_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (e.Rows[0].Cells[1].Value == null)
            {
                AlertOperation("job_spezialist_add", new string[] { "Не указана дата начала.", "1" });
                e.Cancel = true;
                return;
            }
            _status = true;
            var commandServer = new CommandServer();
            var line = radGridView2.CurrentRow.Cells[0].Value + ",'" + e.Rows[0].Cells[1].Value + "',";

            if (e.Rows[0].Cells[2].Value != null)
            {
                line += "'" + e.Rows[0].Cells[2].Value + "'";
            }
            else
            {
                line += "null";
            }

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("job_spezialist_add", line);
            if (returnSqlServer[1] == "1")
            {
                LoadGridFio();
                LoadGridFioOff();
            }
            AlertOperation("job_spezialist_add " + line, returnSqlServer);
        }

        private void radGridView4_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
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
                        radGridView4.CurrentRow.Cells[2].Value = e.NewValue;

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

                    var returnSqlServer = new CommandServer().GetServerCommandExecReturnServer("job_spezialist_edit", parameters);
                    AlertOperation("job_spezialist_edit " + line.Cells[1].Value, returnSqlServer);
                    if (update)
                    {
                        LoadGridFio();
                        LoadGridFioOff();
                    }
                }
            }
        }

        private void radGridView4_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            new CommandServer().GetServerCommandExecNoReturnServer("job_spezialist_delete", e.Rows[0].Cells[0].Value.ToString());
            LoadGridFio();
            LoadGridFioOff();
        }

        #endregion

        private void AlertOperation(string operation, string[] resultOperation)
        {
            if (resultOperation[1] == "1")
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
            else
            {
                new CommandClient().WriteFileError(null, operation);
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
        }
    }
}
