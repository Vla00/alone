using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Statistic : RadForm
    {
        private BindingSource _bindingSource;
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Statistic()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            radButton2.Text += DateTime.Now;
            LoadDrop();
        }


        private void Statistic_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void LoadDrop()
        {
            var commandServer = new CommandServer();
            radDropDownList1.DataSource = commandServer.ComboBoxList(@"select convert(nvarchar, dat, 20) 
                from One_soc order by dat desc", false);
        }

        private void RadButton2_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from One_soc_visible()").Tables[0] };
            Print print = new Print();
            print.ExcelOneSoc(_bindingSource);
        }

        private void RadDropDownList1_SelectedIndexChanging(object sender, Telerik.WinControls.UI.Data.PositionChangingCancelEventArgs e)
        {
            if (string.IsNullOrEmpty(radDropDownList1.Text))
                radButton3.Enabled = false;
            else
                radButton3.Enabled = true;
        }

        private void RadButton3_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from One_soc_visible_table('" + radDropDownList1.Text + "')").Tables[0] };
            Print print = new Print();
            print.ExcelOneSoc(_bindingSource);
        }
    }
}
