using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;

namespace Одиноко_проживающие.search
{
    public partial class Family : RadForm
    {
        private BindingSource _bindingSource;
        private BindingSource _bindingSourceParam;
        private string _fun;

        private string _fam;
        private string _name;
        private string _surname;
        private string _fio;
        private string _country;
        private byte _close;
        private bool _message = true;

        public Family(BindingSource _source)
        {
            InitializeComponent();
            _bindingSourceParam = _source;
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;

            radLabelElement1.Text = @"Загрузка данных...";
            HandleCreated += Form_HandleCreated;
        }

        public Family(string fun, byte close)
        {
            InitializeComponent();
            _fun = fun;
            _close = close;
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;
            
            radLabelElement1.Text = @"Загрузка данных...";
            HandleCreated += Form_HandleCreated;
        }

        public Family(string fun, string fio, string country, bool message)
        {
            InitializeComponent();
            _fun = fun;
            _fio = fio;
            _country = country;
            _message = message;
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;

            radLabelElement1.Text = @"Загрузка данных...";
            HandleCreated += Form_HandleCreated;
        }

        public Family(string fam, string name, string surname, byte close, string fun, bool message)
        {
            InitializeComponent();
            _message = message;
            _fun = fun;
            _fam = fam;
            _name = name;
            _surname = surname;
            _close = close;

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

            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;

            radLabelElement1.Text = @"Загрузка данных...";
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            //myBackgroundWorker = new BackgroundWorker();
            //myBackgroundWorker.WorkerReportsProgress = true;
            //myBackgroundWorker.WorkerSupportsCancellation = true;
            //myBackgroundWorker.DoWork += new DoWorkEventHandler(fillTheDataGrid);
            Thread _thread = new Thread(new ThreadStart(fillTheDataGrid));
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void fillTheDataGrid()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView1.EnablePaging = true;
                    if (!string.IsNullOrEmpty(_fam) || !string.IsNullOrEmpty(_name) || !string.IsNullOrEmpty(_surname))
                    {
                        _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from " + _fun + "(" + _fam + _name + _surname + _close + ") order by [ФИО]").Tables[0] };
                    }else
                    {
                        if(!string.IsNullOrEmpty(_fio))
                        {
                            if(!string.IsNullOrEmpty(_country))
                            {
                                _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet("select * from " + _fun + "('" + _fio + "','" + _country + "') order by [ФИО]").Tables[0] };
                            }
                        }else
                        {
                            if (Text == "Результат расширенного поиска")
                            {
                                _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(_fun).Tables[0] };
                            }
                            else
                            {
                                if(_bindingSourceParam != null)
                                {
                                    _bindingSource = _bindingSourceParam;
                                    radGridView1.DataSource = _bindingSource;
                                }
                                else
                                {
                                    _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from " + _fun + "() order by [ФИО]").Tables[0] };
                                }
                            }
                        }
                    }
                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.Columns[0].IsVisible = false;
                        
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        radGridView1.EnableFiltering = true;
                        radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
                        radGridView1.MasterTemplate.ShowFilteringRow = false;
                        
                        radGridView1.Columns["ФИО"].AllowFiltering = false;
                        radGridView1.Columns["ФИО"].BestFit();                        
                        
                        if(radGridView1.Columns["Дата рождения"] != null)
                        {
                            radGridView1.Columns["Дата рождения"].AllowFiltering = false;
                            radGridView1.Columns["Дата рождения"].FormatString = "{0:dd/MM/yyyy}";
                        }
                                                
                        if (radGridView1.Columns["Адрес"] != null)
                        {
                            radGridView1.Columns["Адрес"].BestFit();
                            radGridView1.Columns["Адрес"].AllowFiltering = false;
                        }

                        if (radGridView1.Columns["Выезд"] != null)
                        {
                            if (_close != 0)
                            {
                                ConditionalFormattingObject obj2 = new ConditionalFormattingObject("DateCondition1", ConditionTypes.NotEqual, null, null, true);
                                obj2.RowBackColor = Color.Goldenrod;
                                radGridView1.Columns["Выезд"].ConditionalFormattingObjectList.Add(obj2);
                                radGridView1.Columns["Выезд"].FormatString = "{0:dd/MM/yyyy}";
                            }else
                            {
                                radGridView1.Columns["Выезд"].IsVisible = false;
                            }
                                
                        }

                        if (radGridView1.Columns["Смерть"] != null)
                        {
                            if(_close != 0)
                            {
                                ConditionalFormattingObject obj = new ConditionalFormattingObject("DateCondition", ConditionTypes.NotEqual, null, null, true);
                                obj.RowBackColor = Color.LightCoral;
                                radGridView1.Columns["Смерть"].ConditionalFormattingObjectList.Add(obj);
                                radGridView1.Columns["Смерть"].FormatString = "{0:dd/MM/yyyy}";
                            }else
                            {
                                radGridView1.Columns["Смерть"].IsVisible = false;
                            }
                            
                        }


                        //if (Text == "Поиск по фамилии")
                        //{
                        //}
                        //else
                        //{
                        //    if (Text == "Результат расширенного поиска")
                        //    {
                        //        if(radGridView1.ColumnCount == 10)
                        //        {
                        //            radGridView1.Columns[8].FormatString = "{0:dd/MM/yyyy}";
                        //            radGridView1.Columns[9].FormatString = "{0:dd/MM/yyyy}";
                        //        }
                        //    }
                        //}
                        
                        radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
                    }

                    if(radGridView1.RowCount == 0)
                    {
                        Close();
                        if(_message)
                            MessageBox.Show("По данному поиску ничего не найдено.", "Выполнение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                radLabelElement1.Text = @"Произошла ошибка при загрузке данных. Сообщите разработчику.";
                commandClient.WriteFileError(ex, "select * from " + _fun + "() order by[ФИО]");
            }
        }

        private void radGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
        }

        private void radGridView1_DoubleClick(object sender, EventArgs e)
        {
            //Hide();
            //try
            //{
            //    new AddAlone(false, Convert.ToInt32(radGridView1.CurrentRow.Cells[0].Value)).ShowDialog();
            //}catch(Exception ex)
            //{
            //    var commandClient = new CommandClient();
            //    commandClient.WriteFileError(ex, null);
            //}            
            //Show();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "*.xls|*.xls";
            saveFileDialog1.FileName = "export";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                radGridView1.Columns[6].ExcelExportType = DisplayFormatType.Text;
                radGridView1.Columns[7].ExcelExportType = DisplayFormatType.Text;
                radGridView1.Columns[2].ExcelExportType = DisplayFormatType.Custom;
                radGridView1.Columns[2].ExcelExportFormatString = "dd.MM.yyyy";
                ExportToExcelML exporter = new ExportToExcelML(radGridView1);
                exporter.PagingExportOption = PagingExportOption.AllPages;
                exporter.RunExport(saveFileDialog1.FileName);                
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            }
        }

        private void radGridView1_FilterPopupRequired(object sender, FilterPopupRequiredEventArgs e)
        {
            //if(e.Column.Name == "АПИ" || e.Column.Name == "СЗУ")
            //{
            //    e.FilterPopup = new RadSimpleListFilterPopup(e.Column);
            //}

            //if(e.Column.Name == "СЗУ")
            //{
            //    e.FilterPopup = new RadSimpleListFilterPopup(e.Column);
            //}
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            Hide();
            try
            {
                new AddAlone(false, Convert.ToInt32(radGridView1.CurrentRow.Cells[0].Value)).ShowDialog();
            }
            catch (Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
            Show();
        }

        private void radGridView1_FilterPopupInitialized(object sender, FilterPopupInitializedEventArgs e)
        {
            RadListFilterPopup popup = e.FilterPopup as RadListFilterPopup;
            if(popup != null)
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
    }
}
