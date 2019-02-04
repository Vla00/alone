using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.handbook
{
    public partial class Powered : RadForm
    {
        private BindingSource _bindingSource;
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Powered()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            Grid();
        }

        private void Grid()
        {
            var commandServer = new CommandServer();

            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select key_every, name as [Название]
                from every_set
                where tabl = 'disability'
                order by name").Tables[0] };
            radGridView5.Refresh();
            radGridView5.AutoSizeRows = true;
            radGridView5.DataSource = _bindingSource;
            radGridView5.Columns[0].IsVisible = false;
            radGridView5.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private void radGridView5_UserAddedRow(object sender, Telerik.WinControls.UI.GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select key_every, name as [Название]
                from every_set
                where tabl = 'disability'
                order by name").Tables[0] };

            radGridView5.Invoke(new MethodInvoker(delegate ()
            {
                radGridView5.DataSource = _bindingSource.DataSource;
            }));
        }

        private void radGridView5_UserAddingRow(object sender, Telerik.WinControls.UI.GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();
            commandServer.ExecNoReturnServer("EverySet_add", "'" + e.Rows[0].Cells[1].Value.ToString() + "','disability'");
        }

        private void radGridView5_RowsChanging(object sender, Telerik.WinControls.UI.GridViewCollectionChangingEventArgs e)
        {
            var commandServer = new CommandServer();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    commandServer.ExecNoReturnServer("EverySet_edit", line.Cells[0].Value.ToString() + "'" + line.Cells[1].Value.ToString() + "'");
                }
            }
        }
    }
}
