using System;
using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class SocPersonal : RadForm
    {
        #region Переменные
        private BindingSource _bindingSource_join;
        private BindingSource _bindingSource_soc;
        private BindingSource _bindingSource_soc_off;
        private BindingSource _binding = new BindingSource();
        BackgroundWorker myBackgroundWorker;
        BackgroundWorker myBackgroundWorkerOff;
        BackgroundWorker myBackgroundWorkerJoin;
        private bool _status = false;
        #endregion

        #region Конструкторы
        public SocPersonal()
        {
            InitializeComponent();

            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView2.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;

            myBackgroundWorker = new BackgroundWorker();
            myBackgroundWorker.WorkerReportsProgress = true;
            myBackgroundWorker.WorkerSupportsCancellation = true;
            myBackgroundWorker.DoWork += new DoWorkEventHandler(loadGridSoc);

            myBackgroundWorkerOff = new BackgroundWorker();
            myBackgroundWorkerOff.WorkerReportsProgress = true;
            myBackgroundWorkerOff.WorkerSupportsCancellation = true;
            myBackgroundWorkerOff.DoWork += new DoWorkEventHandler(LoadGridOff);

            myBackgroundWorkerJoin = new BackgroundWorker();
            myBackgroundWorkerJoin.WorkerReportsProgress = true;
            myBackgroundWorkerJoin.WorkerSupportsCancellation = true;
            myBackgroundWorkerJoin.DoWork += new DoWorkEventHandler(LoadGridInspector);
            //myBackgroundWorker.DoWork += new DoWorkEventHandler(LoadGridOff);
            //myBackgroundWorker.DoWork += new DoWorkEventHandler(LoadGridInspector);
        }

        private void SocPersonal_Load(object sender, EventArgs e)
        {
            if (!myBackgroundWorker.IsBusy)
                myBackgroundWorker.RunWorkerAsync();

            if (!myBackgroundWorkerOff.IsBusy)
                myBackgroundWorkerOff.RunWorkerAsync();

            if (!myBackgroundWorkerJoin.IsBusy)
                myBackgroundWorkerJoin.RunWorkerAsync();
        }
        #endregion

        #region Действующие

        public void loadGridSoc(object sender, DoWorkEventArgs e)
        {
            var commandServer = new CommandServer();
            try
            {
                radGridViewSoc.Invoke(new MethodInvoker(delegate ()
                {
                    radGridViewSoc.EnablePaging = true;
                    _bindingSource_soc = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                        from spezialistView(2,0)  order by [ФИО]").Tables[0] };
                    radGridViewSoc.DataSource = _bindingSource_soc;

                    radGridViewSoc.Columns[0].IsVisible = false;
                    GridViewCommandColumn command = new GridViewCommandColumn();
                    command.Name = "commandDelYes";
                    command.UseDefaultText = true;
                    command.DefaultText = "Удалить";
                    command.FieldName = "delete";
                    command.HeaderText = "Операция";
                    radGridViewSoc.MasterTemplate.Columns.Add(command);
                    radGridViewSoc.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridViewSoc.CommandCellClick += new CommandCellClickEventHandler(radGridViewButton_Click);
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
                from spezialistView(2,0) order by [ФИО]").Tables[0] };

            radGridViewSoc.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewSoc.DataSource = _bindingSource_soc;
            }));
        }

        //добавление
        private void radGridViewSoc_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
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

            var parameters = "'" + fio + "',2";
            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("speziolist_add", parameters);
            if (returnSqlServer[1] == "0")
                e.Cancel = true;
            AlertOperation("speziolist_add " + parameters, returnSqlServer);
        }

        //редактирование
        private void radGridViewSoc_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
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
                        parameters += text + "', 2";

                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("speziolist_edit", parameters);
                        if (returnSqlServer[1] == "0")
                            e.Cancel = true;
                        AlertOperation("speziolist_edit " + line.Cells[1].Value, returnSqlServer);
                    }                    
                }
            }
        }
        #endregion

        #region Уволенные
        private void LoadGridOff(object sender, DoWorkEventArgs e)
        {
            try
            {
                radGridView3.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView3.EnablePaging = true;
                    _bindingSource_soc_off = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from spezialistView(2,1)
                        order by [ФИО]").Tables[0] };
                    radGridView3.DataSource = _bindingSource_soc_off;

                    radGridView3.Columns[0].IsVisible = false;
                    GridViewCommandColumn command = new GridViewCommandColumn();
                    command.Name = "commandDelNo";
                    command.UseDefaultText = true;
                    command.DefaultText = "Восстановить";
                    command.FieldName = "restore";
                    command.HeaderText = "Операция";
                    radGridView3.MasterTemplate.Columns.Add(command);
                    radGridView3.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridView3.CommandCellClick += new CommandCellClickEventHandler(radGridViewButton_Click);
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }
        #endregion

        #region привязка к инспекторам

        private void LoadGridInspector(object sender, DoWorkEventArgs e)
        {
            var commandServer = new CommandServer();

            _binding = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select*
                from spezialistView(1, 0)
                order by[ФИО]").Tables[0] };
            
            radGridView1.DataSource = _binding;

            if (radGridView1.Columns.Count > 0)
            {
                radGridView1.Columns[0].IsVisible = false;
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

        private void radGridView2_ValueChanged(object sender, EventArgs e)
        {
            string isChecked = radGridView2.ActiveEditor.Value.ToString();
            if (isChecked == "On")
            {
                new CommandServer().GetServerCommandExecNoReturnServer("spec_soc_add", radGridView1.CurrentRow.Cells[0].Value + "," + radGridView2.CurrentRow.Cells[0].Value);
            }
            else
            {
                new CommandServer().GetServerCommandExecNoReturnServer("spec_soc_delete", radGridView2.CurrentRow.Cells[0].Value.ToString());
            }
        }

        #endregion

        #region Дополнительно
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
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
        }

        private void radGridViewButton_Click(object sender, EventArgs e)
        {
            if (((GridCommandCellElement)sender).Data.FieldName == "delete")
            {
                new CommandServer().GetServerCommandExecNoReturnServer("socRabotnik_del", radGridViewSoc.CurrentRow.Cells[0].Value.ToString() + ",1");
                UpdateGrid();
            }                
            else
                if(radGridView3.CurrentRow.Cells[0].Value != null)
                {
                    new CommandServer().GetServerCommandExecNoReturnServer("socRabotnik_del", radGridView3.CurrentRow.Cells[0].Value.ToString() + ",0");
                    UpdateGrid();
                }
        }

        private void UpdateGrid()
        {
            _bindingSource_soc = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from socPersonelView(0)
                        order by [ФИО]").Tables[0] };
            radGridViewSoc.DataSource = _bindingSource_soc;

            _bindingSource_soc_off = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from socPersonelView(1)
                        order by [ФИО]").Tables[0] };
            radGridView3.DataSource = _bindingSource_soc_off;
        }
        #endregion

        private void SocPersonal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
