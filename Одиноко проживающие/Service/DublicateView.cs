using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.service
{
    public partial class DublicateView : RadForm
    {
        private BindingSource _bindingSource;
        string fio;
        string street;
        string county;

        public DublicateView(string fio, string street, string country)
        {
            InitializeComponent();
            this.fio = fio;
            this.street = street;
            this.county = country;
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            radGridView1.TableElement.Text = MyRussionRadGridLocalizationProvider.TableElementText;
            CheckForIllegalCrossThreadCalls = false;
            
            radLabelElement1.Text = @"Загрузка данных...";
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
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
                    _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select alone.key_alone as [Дело], alone.fio as [ФИО], alone.date_ro as [Дата рождения], alone.street as [Улица], country.country as [Нас. пункт]
from alone inner join country on country.key_country = alone.fk_country
where alone.fio = '" + fio + "' and alone.street = '" + street + "' and country.country = '" + county + "'").Tables[0] };

                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        //radGridView1.Columns[0].IsVisible = false;
                        radGridView1.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        radGridView1.Columns[0].BestFit();
                        //radGridView1.Columns[5].BestFit();
                        

                        
                        radLabelElement1.Text = @"Записей: " + radGridView1.MasterTemplate.DataView.ItemCount;
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                radLabelElement1.Text = @"Произошла ошибка при загрузке данных. Сообщите разработчику.";
                commandClient.WriteFileError(ex, null);
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
                new AddAlone(false, Convert.ToInt32(radGridView1.CurrentRow.Cells[0].Value)).ShowDialog();
            }catch(Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }            
            Show();
        }
    }
}
