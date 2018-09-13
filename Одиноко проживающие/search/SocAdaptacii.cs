using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.search
{
    public partial class SocAdaptacii : RadForm
    {
        private BindingSource _bindingSource;
        private string _func;

        public SocAdaptacii(string func)
        {
            InitializeComponent();
            _func = func;
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;
            
        }

        private void fillTheDataGrid()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView1.EnablePaging = true;
                    //_bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from ListKategorySoc() order by [ФИО]").Tables[0] };
                    _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from " + _func + "() order by [ФИО]").Tables[0] };
                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.Columns[0].IsVisible = false;
                        radGridView1.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        radGridView1.Columns[1].BestFit();
                        radGridView1.Columns[5].BestFit();
                        radGridView1.EnableFiltering = true;
                        radGridView1.Columns["Дата рождения"].AllowFiltering = false;
                        radGridView1.Columns["Адрес"].AllowFiltering = false;
                        radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
                    }
                }));
            }
            catch(Exception ex) {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, "select * from ListKategorySoc() order by[ФИО]");
            }
        }

        private void radGridView1_DoubleClick(object sender, EventArgs e)
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

        private void SocAdaptacii_Shown(object sender, EventArgs e)
        {
            Thread _thread = new Thread(new ThreadStart(fillTheDataGrid));
            _thread.IsBackground = true;
            radLabelElement1.Text = @"Загрузка данных...";
            while (true)
            {
                if(IsHandleCreated)
                {
                    _thread.Start();
                    break;
                }
            }            
        }

        private void radGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
        }
    }
}
