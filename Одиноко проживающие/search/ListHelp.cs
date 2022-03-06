using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;

namespace Одиноко_проживающие.search
{
    public partial class ListHelp : RadForm
    {
        private BindingSource _bindingSource;
        private readonly DateTime? dateS;
        private readonly DateTime ?dateE;

        public ListHelp(DateTime start, DateTime end)
        {
            InitializeComponent();
            dateS = start;
            dateE = end;
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;
            
            radLabelElement1.Text = @"Загрузка данных...";
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            Thread _thread = new Thread(new ThreadStart(fillTheDataGrid))
            {
                IsBackground = true
            };
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

                    if(dateS == null)
                        _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from HelpList() order by [Дата] desc").Tables[0] };
                    else
                        _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from HelpList()
                    where [Дата] between '" + dateS.Value + "' and '" + dateE.Value + "'order by [Дата] desc").Tables[0] };

                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.Columns[0].IsVisible = false;
                        radGridView1.Columns[5].FormatString = "{0:dd/MM/yyyy}";
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        radGridView1.Columns[1].BestFit();
                        radGridView1.Columns[5].BestFit();

                        radGridView1.EnableFiltering = true;
                        radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
                        radGridView1.MasterTemplate.ShowFilteringRow = false;
                        radGridView1.Columns["Адрес"].AllowFiltering = false;
                        radGridView1.Columns["ФИО"].AllowFiltering = false;
                        radGridView1.Columns[5].AllowFiltering = false;
                        radGridView1.Columns[7].AllowFiltering = false;
                    }
                        
                        radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                radLabelElement1.Text = @"Произошла ошибка при загрузке данных. Сообщите разработчику.";
                commandClient.WriteFileError(ex, "select * from HelpList() order by [Дата] desc");
            }
        }

        private void radGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
        }

        private void radGridView1_DoubleClick(object sender, EventArgs e)
        {
            Hide();
            try
            {
                new Alone(false, Convert.ToInt32(radGridView1.CurrentRow.Cells[0].Value), null, null, null).ShowDialog();
            }catch(Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }            
            Show();
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
                radGridView1.Columns[6].ExcelExportFormatString = "dd.MM.yyyy";
                ExportToExcelML exporter = new ExportToExcelML(radGridView1);
                exporter.RunExport(saveFileDialog1.FileName);
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            }
        }

        private void radGridView1_FilterPopupInitialized(object sender, FilterPopupInitializedEventArgs e)
        {
            RadListFilterPopup popupList = e.FilterPopup as RadListFilterPopup;
            if (popupList != null)
            {
                foreach (RadItem item in popupList.Items)
                {
                    if (item.Text == RadGridLocalizationProvider.CurrentProvider.GetLocalizedString(RadGridStringId.FilterMenuAvailableFilters))
                    {
                        item.Visibility = ElementVisibility.Collapsed;
                    }
                }
            }else
            {
                RadDateFilterPopup popupDate = e.FilterPopup as RadDateFilterPopup;
                if(popupDate != null)
                {
                    foreach (RadItem item in popupDate.Items)
                    {
                        if (item.Text == RadGridLocalizationProvider.CurrentProvider.GetLocalizedString(RadGridStringId.FilterMenuAvailableFilters))
                        {
                            item.Visibility = ElementVisibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void ListHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
