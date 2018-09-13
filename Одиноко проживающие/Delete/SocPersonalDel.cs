using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class SocPersonalDelete : RadForm
    {
        private BindingSource _bindingSource_join;
        private BindingSource _bindingSource_soc;
        private BindingSource _binding = new BindingSource();
        DataTable gridViewDataTable = new DataTable();
        private bool _status = false;

        public SocPersonalDelete()
        {
            InitializeComponent();

            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView2.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;            
            HandleCreated += Form_HandleCreated;
        }

        private void fillTheDataGrid()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    loadGridSoc();
                    LoadGridInspector();
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            Thread _thread = new Thread(new ThreadStart(fillTheDataGrid));
            _thread.IsBackground = true;
            _thread.Start();
        }

        #region социальные работники

        #region соц ФИО
        private void loadGridSoc()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridViewSoc.Invoke(new MethodInvoker(delegate ()
                {
                    radGridViewSoc.EnablePaging = true;
                    _bindingSource_soc = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                from socPersonelViewColor()  order by [ФИО]").Tables[0] };
                    radGridViewSoc.DataSource = _bindingSource_soc;

                    if (radGridViewSoc.Columns.Count > 0)
                    {
                        radGridViewSoc.Columns[0].IsVisible = false;
                        radGridViewSoc.Columns[2].IsVisible = false;
                        radGridViewSoc.Columns[3].IsVisible = false;
                        radGridViewSoc.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        //обновление таблицы
        private void radGridViewSoc_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource_soc = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                from socPersonelViewColor() order by [ФИО]").Tables[0] };

            radGridViewSoc.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewSoc.DataSource = _bindingSource_soc;
            }));
        }

        //добавление
        private void radGridView1_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (_status)
            {
                _status = false;
                return;
            }
            if (e.Rows[0].Cells[1].Value == null || e.Rows[0].Cells[1].Value.ToString() == "")
            {
                AlertOperation("addSocRabotnik", new string[] { "Не указано ФИО.", "1" });
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

            var parameters = "'" + fio + "'";
            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("socRabotnik_add", parameters);
            if (returnSqlServer[1] == "0")
                e.Cancel = true;
            //loadGridSoc();
            //LoadGridSoc();
            AlertOperation("socRabotnik_add " + parameters, returnSqlServer);
        }

        //редактирование
        private void radGridView1_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
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
                        parameters += text + "'";

                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("socRabotnik_edit", parameters);
                        if (returnSqlServer[1] == "0")
                            e.Cancel = true;
                        //loadGridSoc();
                        //LoadGridSoc();
                        AlertOperation("socRabotnik_edit " + line.Cells[1].Value, returnSqlServer);
                    }
                    
                }
            }
        }
        #endregion

        #region дата работы
        private void ClickSOCTime(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();
            try
            {
                if (radGridViewSoc.CurrentRow.Cells[0].Value != null)
                {
                    _bindingSource_soc = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select key_job_soc_rabotnik, date_start, date_end
                    from job_soc_rabotnik left join socRabotnik on job_soc_rabotnik.fk_soc_rabotnik = socRabotnik.key_socRabotnik
                    where fk_soc_rabotnik = " + radGridViewSoc.CurrentRow.Cells[0].Value).Tables[0] };
                    radGridViewSocTime.DataSource = _bindingSource_soc;

                    GridViewDateTimeColumn dateS = new GridViewDateTimeColumn("Дата начала");
                    radGridViewSocTime.Columns[1] = dateS;
                    dateS.Name = "date_s";
                    dateS.FieldName = "date_start";
                    dateS.FormatString = "{0:dd/MM/yyyy}";
                    dateS.Format = DateTimePickerFormat.Custom;
                    dateS.CustomFormat = "dd.MM.yyyy";

                    GridViewDateTimeColumn dateE = new GridViewDateTimeColumn("Дата конца");
                    radGridViewSocTime.Columns[2] = dateE;
                    dateE.Name = "date_e";
                    dateE.FieldName = "date_end";
                    dateE.FormatString = "{0:dd/MM/yyyy}";
                    dateE.Format = DateTimePickerFormat.Custom;
                    dateE.CustomFormat = "dd.MM.yyyy";

                    radGridViewSocTime.Columns[0].IsVisible = false;
                    radGridViewSocTime.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridViewSocTime.Columns[1].BestFit();
                    radGridViewSocTime.Columns[2].BestFit();

                    if (radGridViewSocTime.Columns.Count > 0)
                    {                        
                        UpdateLoadJobTime();
                    }else
                    {
                        richTextBox1.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void UpdateLoadJobTime()
        {
            var commandServer = new CommandServer();
            DataSet val = commandServer.GetDataGridSet("select * from CalcStagResultSoc(" + radGridViewSoc.CurrentRow.Cells[0].Value + ")");
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

        private void radGridView3_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (e.Rows[0].Cells[1].Value == null)
            {
                AlertOperation("job_soc_rabotnik_add", new string[] { "Не указана дата начала.", "1" });
                e.Cancel = true;
                return;
            }
            _status = true;
            var commandServer = new CommandServer();
            var line = radGridViewSoc.CurrentRow.Cells[0].Value + ",'" + e.Rows[0].Cells[1].Value + "',";
            radGridViewSoc.CurrentRow.Cells[2].Value = e.Rows[0].Cells[1].Value;
            
            if (e.Rows[0].Cells[2].Value != null)
            {
                line += "'" + e.Rows[0].Cells[2].Value + "'";
                radGridViewSoc.CurrentRow.Cells[3].Value = e.Rows[0].Cells[2].Value;
            }
            else
            {
                line += "null";
            }

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("job_soc_rabotnik_add", line);
            UpdateLoadJobTime();
            AlertOperation("job_soc_rabotnik_add " + line, returnSqlServer);
        }

        private void radGridView3_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

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
                        radGridViewSoc.CurrentRow.Cells[2].Value = e.NewValue;

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
                            radGridViewSoc.CurrentRow.Cells[3].Value = e.NewValue;
                        }
                    }
                    else
                    {
                        if (line.Cells[2].Value == null)
                        {
                            parameters += "null";
                        }
                        else
                            parameters += "'" + line.Cells[2].Value.ToString() + "'";
                    }
                    
                    var returnSqlServer = commandServer.GetServerCommandExecReturnServer("job_soc_rabotnik_edit", parameters);
                    UpdateLoadJobTime();
                    AlertOperation("job_soc_rabotnik_edit " + line.Cells[1].Value, returnSqlServer);
                    //radGridViewSoc.Invoke(new MethodInvoker(delegate ()
                    //{
                    //    radGridViewSoc.DataSource = _bindingSource_soc;
                    //}));

                    //loadGridSoc();
                    //LoadGridSoc();
                }
            }
        }

        private void radGridView3_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();
            commandServer.GetServerCommandExecNoReturnServer("job_soc_rabotnik_delete", e.Rows[0].Cells[0].Value.ToString());
            UpdateLoadJobTime();
            //loadGridSoc();
            //LoadGridSoc();
        }

        private void radGridView4_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.Cells.Count > 3)
            {
                if (e.RowElement.RowInfo.Cells[2].Value.ToString() == "" && e.RowElement.RowInfo.Cells[3].Value.ToString() == "")
                {
                    e.RowElement.DrawFill = true;
                    e.RowElement.GradientStyle = GradientStyles.Solid;
                    e.RowElement.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    if (e.RowElement.RowInfo.Cells[2].Value != null && e.RowElement.RowInfo.Cells[3].Value != null)
                    {
                        if (e.RowElement.RowInfo.Cells[3].Value.ToString() != "")
                        {
                            e.RowElement.DrawFill = true;
                            e.RowElement.GradientStyle = GradientStyles.Solid;
                            e.RowElement.BackColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            e.RowElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
                            e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                            e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                        }

                    }
                    else
                    {
                        e.RowElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    }
                }
            }
        }

        private void radGridViewSocTime_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadDateTimeEditor dateTimeEditor = e.ActiveEditor as RadDateTimeEditor;
            if (dateTimeEditor != null)
            {
                radGridViewSocTime.CellEditorInitialized -= radGridViewSocTime_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                MaskDateTimeProvider provider = editroElement.TextBoxElement.Provider as MaskDateTimeProvider;
                if (provider != null)
                    provider.AutoSelectNextPart = true;
            }
        }
        #endregion
        #endregion

        #region привязка к инспекторам

        private void LoadGridInspector()
        {
            var commandServer = new CommandServer();

            _binding = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select*
                from spezialistViewColor('1')
                where date_end is null
                order by[ФИО]").Tables[0] };
            
            radGridView1.DataSource = _binding;

            if (radGridView1.Columns.Count > 0)
            {
                radGridView1.Columns[0].IsVisible = false;
                radGridView1.Columns[2].IsVisible = false;
                radGridView1.Columns[3].IsVisible = false;
                radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void radGridView1_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();
            try
            {
                if (radGridViewSoc.CurrentRow.Cells[0].Value != null)
                {
                    _bindingSource_join = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select keys, fio as [ФИО], joi
                    from InspectorJoinSocView(" + radGridView1.CurrentRow.Cells[0].Value + ")").Tables[0] };

                    radGridView2.DataSource = _bindingSource_join;

                    GridViewCheckBoxColumn date_join = new GridViewCheckBoxColumn("Привязка");
                    radGridView2.Columns[2] = date_join;
                    date_join.Name = "date_j";
                    date_join.FieldName = "joi";

                    radGridView2.Columns[0].IsVisible = false;
                    radGridView2.Columns[1].ReadOnly = true;
                    radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridView2.Columns[1].BestFit();
                    radGridView2.Columns[2].BestFit();
                }
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        #endregion
        
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

        private void radGridView2_ValueChanging(object sender, ValueChangingEventArgs e)
        {
            //RadCheckBoxEditor editor = sender as RadCheckBoxEditor;
            //if(editor != null)
            //{
            //    radGridView2.EndEdit();

            //    foreach(GridViewRowInfo row in radGridView2.ChildRows)
            //    {
            //        if(row != radGridView2.CurrentRow)
            //        {
            //            row.Cells[radGridView2.CurrentColumn.Name].Value = false;
            //        }
            //    }
            //}
        }

        private void radGridView2_ValueChanged(object sender, EventArgs e)
        {
            string isChecked = radGridView2.ActiveEditor.Value.ToString();
            if(isChecked == "On")
            {
                new CommandServer().GetServerCommandExecNoReturnServer("spec_soc_add", radGridView1.CurrentRow.Cells[0].Value + "," + radGridView2.CurrentRow.Cells[0].Value);
            }else
            {
                new CommandServer().GetServerCommandExecNoReturnServer("spec_soc_delete", radGridView2.CurrentRow.Cells[0].Value.ToString());
            }
        }
    }
}
