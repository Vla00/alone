using System;
using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Speziolist : RadForm
    {
        private BindingSource _bindingSource_personel;
        private BindingSource _bindingSource_off;
        BackgroundWorker myBackgroundWorker;
        BackgroundWorker myBackgroundWorkerOff;
        private bool _status = false;

        #region Конструкторы
        public Speziolist()
        {
            InitializeComponent();
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            CheckForIllegalCrossThreadCalls = false;

            myBackgroundWorker = new BackgroundWorker();
            myBackgroundWorker.WorkerReportsProgress = true;
            myBackgroundWorker.WorkerSupportsCancellation = true;
            myBackgroundWorker.DoWork += new DoWorkEventHandler(LoadGridFio);

            myBackgroundWorkerOff = new BackgroundWorker();
            myBackgroundWorkerOff.WorkerReportsProgress = true;
            myBackgroundWorkerOff.WorkerSupportsCancellation = true;
            myBackgroundWorkerOff.DoWork += new DoWorkEventHandler(LoadGridFioOff);
        }

        private void Speziolist_Load(object sender, EventArgs e)
        {
            if (!myBackgroundWorker.IsBusy)
                myBackgroundWorker.RunWorkerAsync();

            if (!myBackgroundWorkerOff.IsBusy)
                myBackgroundWorkerOff.RunWorkerAsync();
        }
        #endregion

        #region Действующие
        public void LoadGridFio(object sender, DoWorkEventArgs e)
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView5.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView5.EnablePaging = true;
                    _bindingSource_personel = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                        from spezialistView(0,0)
                        order by [ФИО]").Tables[0] };
                    radGridView5.DataSource = _bindingSource_personel;

                    radGridView5.Columns[0].IsVisible = false;
                    GridViewCommandColumn command = new GridViewCommandColumn();
                    command.Name = "commandDelYes";
                    command.UseDefaultText = true;
                    command.DefaultText = "Удалить";
                    command.FieldName = "delete";
                    command.HeaderText = "Операция";
                    radGridView5.MasterTemplate.Columns.Add(command);
                    radGridView5.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridView5.CommandCellClick += new CommandCellClickEventHandler(radGridViewButton_Click);
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
                        from spezialistView(0, 0) 
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
        #endregion

        #region Уволеные
        public void LoadGridFioOff(object sender, DoWorkEventArgs e)
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView2.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView2.EnablePaging = true;
                    _bindingSource_off = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                        from spezialistView(0, 1)
                        order by [ФИО]").Tables[0] };
                    radGridView2.DataSource = _bindingSource_off;

                    radGridView2.Columns[0].IsVisible = false;
                    GridViewCommandColumn command = new GridViewCommandColumn();
                    command.Name = "commandDelNo";
                    command.UseDefaultText = true;
                    command.DefaultText = "Восстановить";
                    command.FieldName = "restore";
                    command.HeaderText = "Операция";
                    radGridView2.MasterTemplate.Columns.Add(command);
                    radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    radGridView2.CommandCellClick += new CommandCellClickEventHandler(radGridViewButton_Click);
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
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
                commandClient.WriteFileError(null, operation);
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
        }

        private void radGridViewButton_Click(object sender, EventArgs e)
        {
            if (((GridCommandCellElement)sender).Data.FieldName == "delete")
                new CommandServer().GetServerCommandExecNoReturnServer("speziolist_del", radGridView5.CurrentRow.Cells[0].Value.ToString() + ",1");
            else
                new CommandServer().GetServerCommandExecNoReturnServer("speziolist_del", radGridView2.CurrentRow.Cells[0].Value.ToString() + ",0");
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            _bindingSource_personel = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from spezialistView(0, 0) 
                        order by [ФИО]").Tables[0] };
            radGridView5.DataSource = _bindingSource_personel;

            _bindingSource_off = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
                        from spezialistView(1, 1)
                        order by [ФИО]").Tables[0] };
            radGridView2.DataSource = _bindingSource_off;
        }
        #endregion

        private void Speziolist_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
