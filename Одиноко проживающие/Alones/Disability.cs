using System;
using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.Alones
{
    public partial class Disability : RadForm
    {
        private bool _status;
        private string _key;
        private BindingSource _bindingSource;
        private BindingList<string> _binding_every;
        private RadListView _powered = new RadListView();
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Disability(string key)
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            _status = false;
            _key = key;
            ComboBox_every();
            Grid();
        }

        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private void Grid()
        {
            var commandServer = new CommandServer();

            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from Disability_get_table(" + _key + ") order by [Дата инв.]").Tables[0] };
            radGridView1.Refresh();
            radGridView1.AutoSizeRows = true;
            radGridView1.DataSource = _bindingSource;
            radGridView1.Columns[0].IsVisible = false;

            GridViewComboBoxColumn comboColumn_powered = new GridViewComboBoxColumn("Степ. утр.");
            comboColumn_powered.DataSource = new String[] { "1", "2", "3", "4"};
            comboColumn_powered.Name = "_powered";
            radGridView1.Columns[1] = comboColumn_powered;
            comboColumn_powered.FieldName = "powered";

            GridViewDateTimeColumn dat = new GridViewDateTimeColumn("Дата инв.");
            dat.Name = "date1";
            dat.FormatString = "{0:dd/MM/yyyy}";
            dat.Format = DateTimePickerFormat.Custom;
            radGridView1.Columns[2] = dat;
            dat.CustomFormat = "dd.MM.yyyy";

            GridViewDateTimeColumn dat1 = new GridViewDateTimeColumn("Дата переосв.");
            dat1.Name = "date2";
            dat1.FormatString = "{0:dd/MM/yyyy}";
            dat1.Format = DateTimePickerFormat.Custom;
            radGridView1.Columns[3] = dat1;
            dat1.CustomFormat = "dd.MM.yyyy";

            GridViewComboBoxColumn comboColumn_every = new GridViewComboBoxColumn("Диагноз");
            comboColumn_every.DataSource = _binding_every;
            comboColumn_every.Name = "_every";
            radGridView1.Columns[4] = comboColumn_every;
            comboColumn_every.FieldName = "name";

            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private void ComboBox_every()
        {
            var commandServer = new CommandServer();
            _binding_every = new BindingList<string>(commandServer.ComboBoxList(@"select name
                from every_set
                where tabl = 'disability'
                order by name", false));
        }

        private void radGridView1_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            //изменение
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                bool flag = false;
                var line = (GridViewRowInfo)e.NewItems[0];
                if (line.Cells[0].Value != null)
                {
                    var parameters = line.Cells[0].Value.ToString() + ",'";

                    if (e.PropertyName == "powered")
                    {
                        flag = true;
                        parameters += e.NewValue.ToString() + "','";
                    }
                    else
                    {
                        parameters += line.Cells[1].Value.ToString() + "','";
                    }

                    if (e.PropertyName == "Дата инв.")
                    {
                        flag = true;
                        if(e.NewValue != null)
                            parameters += e.NewValue.ToString() + "',";
                        else
                        {
                            AlertOperation(new string[] { "Не указана дата инвалидности", "0" });
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        parameters += line.Cells[2].Value.ToString() + "',";
                    }

                    if (e.PropertyName == "Дата переосв.")
                    {
                        if(e.NewValue != null)
                        {
                            parameters += "'" + e.NewValue.ToString() + "','";
                            flag = true;
                        }
                        else
                        {
                            if(e.OldValue.ToString() != "")
                            {
                                flag = true;
                                parameters += "null,'";
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(line.Cells[3].Value.ToString()))
                            parameters += "'" + line.Cells[3].Value.ToString() + "','";
                        else
                            parameters += "null,'";
                    }

                    if (e.PropertyName == "name")
                    {
                        flag = true;
                        parameters += e.NewValue.ToString() + "'";
                    }
                    else
                    {
                        parameters += line.Cells[4].Value.ToString() + "'";
                    }

                    if (flag)
                    {
                        var resultOperation = commandServer.ExecReturnServer("Disability_edit", parameters);
                        AlertOperation(resultOperation);
                    }
                }
            }
        }

        private void radGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from Disability_get_table(" + _key + ") order by [Дата инв.]").Tables[0] };

            radGridView1.Invoke(new MethodInvoker(delegate ()
            {
                radGridView1.DataSource = _bindingSource.DataSource;
            }));
        }

        private void radGridView1_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            bool error = false;
            var parameters = _key;

            if (e.Rows[0].Cells["_powered"].Value == null)
            {
                AlertOperation(new string[] { "Не указана степень", "0"});
                e.Cancel = true;
                var cell = radGridView1.MasterView.TableAddNewRow.Cells["_powered"];
                cell.Style.BackColor = System.Drawing.Color.Tomato;
                cell.Style.CustomizeFill = true;
                error = true;
            }else
            {
                radGridView1.MasterView.TableAddNewRow.Cells["_powered"].Style.CustomizeFill = false;
                parameters += "," + e.Rows[0].Cells["_powered"].Value.ToString();
            }

            if(e.Rows[0].Cells["date1"].Value == null)
            {
                AlertOperation(new string[] { "Не указана дата инвалидности", "0" });
                e.Cancel = true;
                var cell = radGridView1.MasterView.TableAddNewRow.Cells["date1"];
                cell.Style.BackColor = System.Drawing.Color.Tomato;
                cell.Style.CustomizeFill = true;
                error = true;
            }
            else
            {
                radGridView1.MasterView.TableAddNewRow.Cells["date1"].Style.CustomizeFill = false;
                parameters += ",'" + e.Rows[0].Cells["date1"].Value.ToString() + "'";
            }

            if (e.Rows[0].Cells["date2"].Value != null)
            {
                parameters += ",'" + e.Rows[0].Cells["date2"].Value.ToString() + "'";
            }
            else
            {
                parameters += ",null";
            }

            if (e.Rows[0].Cells["_every"].Value == null)
            {
                AlertOperation(new string[] { "Не указан диагноз", "0" });
                e.Cancel = true;
                var cell = radGridView1.MasterView.TableAddNewRow.Cells["_every"];
                cell.Style.BackColor = System.Drawing.Color.Tomato;
                cell.Style.CustomizeFill = true;
                error = true;
            }
            else
            {
                radGridView1.MasterView.TableAddNewRow.Cells["_every"].Style.CustomizeFill = false;
                parameters += ",'" + e.Rows[0].Cells["_every"].Value.ToString() + "'";
            }

            if (error)
                return;

            var commandServer = new CommandServer();

            var returnSqlServer = commandServer.ExecReturnServer("Disability_add", parameters);
            AlertOperation(returnSqlServer);
        }

        private void radGridView1_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadDateTimeEditor dateTimeEditor = e.ActiveEditor as RadDateTimeEditor;
            if (dateTimeEditor != null)
            {
                radGridView1.CellEditorInitialized -= radGridView1_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                MaskDateTimeProvider provider = editroElement.TextBoxElement.Provider as MaskDateTimeProvider;
                if (provider != null)
                    provider.AutoSelectNextPart = true;
            }
        }

        private void AlertOperation(string[] resultOperation)
        {
            if (resultOperation[1] == "1")
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
                Status = true;
            }
            else
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
                Status = false;
            }
        }
    }
}
