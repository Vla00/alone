using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using System.ComponentModel;

namespace Одиноко_проживающие.search
{
    public partial class Result : RadForm
    {
        private BindingSource _bindingSource;
        private BindingSource _bindingSourceParam;
        BackgroundWorker worker;
        private string _fun;

        private string _fam;
        private string _name;
        private string _surname;
        private string _fio;
        private string _country;
        private byte _close;
        private bool _message = true;

        public Result(BindingSource _source)
        {
            _bindingSourceParam = _source;
            Inizialization();
        }

        public Result(string fun, byte close)
        {
            _fun = fun;
            _close = close;
            Inizialization();
        }

        public Result(string fun, string fio, string country, bool message)
        {            
            _fun = fun;
            _fio = fio;
            _country = country;
            _message = message;
            Inizialization();
        }

        public Result(string fam, string name, string surname, byte close, string fun, bool message, string country)
        {
            _message = message;
            _fun = fun;
            _fam = fam;
            _name = name;
            _surname = surname;
            _close = close;
            _country = country;

            if (string.IsNullOrEmpty(_fam))
                _fam = "null";
            else
                _fam = "'" + _fam + "'";
            if (string.IsNullOrEmpty(_name))
                _name = ",null";
            else
                _name = ",'" + _name + "'";
            if (string.IsNullOrEmpty(_surname))
                _surname = ",null,";
            else
                _surname = ",'" + _surname + "',";

            Inizialization();
        }

        private void Inizialization()
        {
            InitializeComponent();
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;

            radLabelElement1.Text = @"Загрузка данных...";
        }

        private void SearchWork()
        {
            var commandServer = new CommandServer();
            try
            {
                Cursor.Current = Cursors.AppStarting;
                

                if (!string.IsNullOrEmpty(_fun))
                {
                    switch (_fun)
                    {
                        case "SearchFamily":
                            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from " + _fun + "(" + _fam + _name + _surname + _close + ") order by [ФИО]").Tables[0] };
                            break;
                        case "ListSojitel":
                            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet("select * from " + _fun + "(" + _fam + _name + _surname + "'" + _country + "') order by [ФИО]").Tables[0] };
                            break;
                        default:
                            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(_fun).Tables[0] };
                            break;
                    }
                }
                else
                {
                    if (_bindingSourceParam != null)
                    {
                        _bindingSource = _bindingSourceParam;
                    }
                }

                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView1.DataSource = _bindingSource;
                }));

                if (radGridView1.Columns.Count > 0)
                {
                    radGridView1.Invoke(new MethodInvoker(delegate ()
                    {
                        radGridView1.EnablePaging = true;
                        radGridView1.Columns[0].IsVisible = false;

                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        radGridView1.EnableFiltering = true;
                        radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
                        radGridView1.MasterTemplate.ShowFilteringRow = false;

                    
                        if (radGridView1.Columns["ФИО"] != null)
                        {
                            radGridView1.Columns["ФИО"].AllowFiltering = false;
                            radGridView1.Columns["ФИО"].BestFit();
                        }

                        if (radGridView1.Columns["Дата рождения"] != null)
                        {
                            radGridView1.Columns["Дата рождения"].AllowFiltering = false;
                            radGridView1.Columns["Дата рождения"].FormatString = "{0:dd/MM/yyyy}";
                        }

                        if (radGridView1.Columns["Адрес"] != null)
                        {
                            radGridView1.Columns["Адрес"].BestFit();
                            radGridView1.Columns["Адрес"].AllowFiltering = false;
                        }

                        if (radGridView1.Columns["Дата над. обсл."] != null)
                        {
                            radGridView1.Columns["Дата над. обсл."].AllowFiltering = false;
                            radGridView1.Columns["Дата над. обсл."].FormatString = "{0:dd/MM/yyyy}";
                        }

                        if (radGridView1.Columns["Выезд"] != null)
                        {
                            if (_close != 0)
                            {
                                ConditionalFormattingObject obj2 = new ConditionalFormattingObject("DateCondition1", ConditionTypes.NotEqual, null, null, true)
                                {
                                    RowBackColor = Color.Goldenrod
                                };
                                radGridView1.Columns["Выезд"].ConditionalFormattingObjectList.Add(obj2);
                                radGridView1.Columns["Выезд"].FormatString = "{0:dd/MM/yyyy}";
                            }
                            else
                            {
                                radGridView1.Columns["Выезд"].IsVisible = false;
                            }

                        }

                        if (radGridView1.Columns["Смерть"] != null)
                        {
                            if (_close != 0)
                            {
                                ConditionalFormattingObject obj = new ConditionalFormattingObject("DateCondition", ConditionTypes.NotEqual, null, null, true)
                                {
                                    RowBackColor = Color.LightCoral
                                };
                                radGridView1.Columns["Смерть"].ConditionalFormattingObjectList.Add(obj);
                                radGridView1.Columns["Смерть"].FormatString = "{0:dd/MM/yyyy}";
                            }
                            else
                            {
                                radGridView1.Columns["Смерть"].IsVisible = false;
                            }

                        }
                    }));

                    radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
                }

                if (radGridView1.RowCount == 0)
                {
                    
                    if (_message)
                        MessageBox.Show("По данному поиску ничего не найдено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if(_fun == "SearchFamily")
                    {
                        if (_close != 2)
                        {
                            if(MessageBox.Show("Вы хотите произвести поиск во всех делах (активные, выехавшие, умершие)?", "Результат", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                _close = 2;
                                SearchWork();
                            }else
                                Close();
                        }
                        else
                            Close();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                radLabelElement1.Text = @"Произошла ошибка при загрузке данных. Сообщите разработчику.";
                commandClient.WriteFileError(ex, _fun);
            }finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void RadGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
        }

        private void RadButtonElement1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "*.xls|*.xls";
            saveFileDialog1.FileName = "export";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                radGridView1.Columns[6].ExcelExportType = DisplayFormatType.Text;
                radGridView1.Columns[7].ExcelExportType = DisplayFormatType.Text;
                radGridView1.Columns[2].ExcelExportType = DisplayFormatType.Custom;
                radGridView1.Columns[2].ExcelExportFormatString = "dd.MM.yyyy";

                if(radGridView1.Columns["Выезд"] != null)
                {
                    radGridView1.Columns["Выезд"].ExcelExportType = DisplayFormatType.Custom;
                    radGridView1.Columns["Выезд"].ExcelExportFormatString = "dd.MM.yyyy";
                }                    

                if (radGridView1.Columns["Смерть"] != null)
                {
                    radGridView1.Columns["Смерть"].ExcelExportType = DisplayFormatType.Custom;
                    radGridView1.Columns["Смерть"].ExcelExportFormatString = "dd.MM.yyyy";
                }                    

                if(radGridView1.Columns["Дата над. обсл."] != null)
                {
                    radGridView1.Columns["Дата над. обсл."].ExcelExportType = DisplayFormatType.Custom;
                    radGridView1.Columns["Дата над. обсл."].ExcelExportFormatString = "dd.MM.yyyy";
                }

                ExportToExcelML exporter = new ExportToExcelML(radGridView1)
                {
                    PagingExportOption = PagingExportOption.AllPages
                };
                exporter.RunExport(saveFileDialog1.FileName);                
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            }
        }

        private void RadGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                Hide();
                try
                {
                    new Alone(false, Convert.ToInt32(radGridView1.CurrentRow.Cells[0].Value), null, null).ShowDialog();
                }
                catch (Exception ex)
                {
                    var commandClient = new CommandClient();
                    commandClient.WriteFileError(ex, null);
                }
                Show();
            }
        }

        private void RadGridView1_FilterPopupInitialized(object sender, FilterPopupInitializedEventArgs e)
        {
            if (e.FilterPopup is RadListFilterPopup popup)
            {
                foreach (RadItem item in popup.Items)
                {
                    if (item.Text == RadGridLocalizationProvider.CurrentProvider.GetLocalizedString(RadGridStringId.FilterMenuAvailableFilters))
                    {
                        item.Visibility = ElementVisibility.Collapsed;
                    }
                }
            }
        }

        private void Family_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Result_Load(object sender, EventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += (obj, ea) => SearchWork();
            worker.RunWorkerAsync();
        }

        private void Result_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
