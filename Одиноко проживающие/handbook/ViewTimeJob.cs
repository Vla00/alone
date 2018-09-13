using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.handbook
{
    public partial class ViewTimeJob : RadForm
    {
        private BindingSource _bindingSource;

        public ViewTimeJob()
        {
            InitializeComponent();
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            CheckForIllegalCrossThreadCalls = false;
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            var commandServer = new CommandServer();
            try
            {
                radGridView1.Invoke(new MethodInvoker(delegate ()
                {
                    radGridView1.EnablePaging = true;
                    _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(
                        @"select fio as [ФИО], years as [лет], months as [месяцев], da as [дней]
                        from ViewTimeJob() order by years desc, months desc, da desc").Tables[0] };
                    radGridView1.DataSource = _bindingSource;

                    if (radGridView1.Columns.Count > 0)
                    {
                        radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        radGridView1.Columns[0].BestFit();
                    }
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }
    }
}
