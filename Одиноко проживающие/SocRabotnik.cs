using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.menu
{
    public partial class SocRabotnik : RadForm
    {
        private BindingSource _bindingSourceSoc;

        public SocRabotnik()
        {
            InitializeComponent();
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            Thread _thread = new Thread(new ThreadStart(fillTheDataGridSoc));
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void fillTheDataGridSoc()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    _bindingSourceSoc = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select fio as [ФИО]
                        from socRabotnik
                        where statusDel = 0
                        order by fio").Tables[0] };
                    
                    radGridView1.DataSource = _bindingSourceSoc;

                    if (radGridView1.Columns.Count > 0)
                    {
                        //radGridView1.Columns[0].IsVisible = false;
                        //radGridView1.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        //radGridView1.Columns[1].BestFit();
                        //radGridView1.Columns[5].BestFit();
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void radGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();

            if(radPageView1.SelectedPage == radPageViewPage1)
            {
                //принят
            }else
            {
                if(radPageView1.SelectedPage == radPageViewPage2)
                {
                    //приостановлен
                }else
                {
                    //Закрыт
                }
            }

            BindingSource _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select fio as [ФИО]
                        from socRabotnik
                        where statusDel = 0
                        order by fio").Tables[0] };



            radGridView2.DataSource = _bindingSource;
        }
    }
}
