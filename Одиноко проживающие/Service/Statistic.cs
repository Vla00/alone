using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Statistic : RadForm
    {
        //private BindingSource _bindingSource;
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Statistic()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            radButton2.Text += DateTime.Now;
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
            radDropDownList1.DataSource = commandServer.ComboBoxList(@"select ФИО
                        from spezialistView(2,0)  order by [ФИО]", true);
        }
    }
}
