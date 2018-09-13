using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.count
{
    public partial class CountSelsovets : RadForm
    {
        private BindingSource _bindingSource;

        public CountSelsovets()
        {
            InitializeComponent();
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;
            Thread _thread = new Thread(new ThreadStart(fillTheDataGrid));
            _thread.IsBackground = true;
            _thread.Start();
            radLabelElement1.Text = @"Загрузка данных...";
            
        }

        private void fillTheDataGrid()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView1.EnablePaging = true;
                    _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from ListCountCountry() order by [ФИО]").Tables[0] };
                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.Columns[1].FormatString = "{0:MM/dd/yyyy}";
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        radGridView1.MasterTemplate.BestFitColumns(BestFitColumnMode.DisplayedCells);

                        radGridView1.EnableFiltering = true;
                        radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
                        radGridView1.MasterTemplate.ShowFilteringRow = false;
                        radGridView1.Columns["ФИО"].AllowFiltering = false;
                        radGridView1.Columns["Дата рождения"].AllowFiltering = false;
                        radGridView1.Columns["Адрес"].AllowFiltering = false;
                        radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
                    }
                }));
            }
            catch { }            
        }

        private void radGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
        }
    }
}
