using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Alone_history : RadForm
    {
        private BindingSource _bindingSourceCategory;
        private BindingSource _bindingSourceExit;
        TelerikMetroTheme theme = new TelerikMetroTheme();


        public Alone_history(int key)
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            LoadGrid(key);
        }

        public void LoadGrid(int key)
        {
            var commandServer = new CommandServer();
            _bindingSourceExit = new BindingSource { DataSource = commandServer.DataGridSet(@"select date_start as [Выезд], date_stop as [Возврат]
                from alone_time inner join every_set on fk_every_set_operation = key_every
                where fk_alone = " + key + " and name = 'exit' order by date_start").Tables[0] };

            _bindingSourceCategory = new BindingSource { DataSource = commandServer.DataGridSet(@"select date_start as [Установление], date_stop as [Снятие], category as [Категория]
                from category_time
                where fk_alone = " + key + " order by date_start").Tables[0] };

            category_grid.AutoSizeRows = true;
            category_grid.DataSource = _bindingSourceCategory;
            category_grid.Columns[0].FormatString = "{0:dd/MM/yyyy}";
            category_grid.Columns[1].FormatString = "{0:dd/MM/yyyy}";
            category_grid.Columns[2].WrapText = true;
            category_grid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            exit_grid.AutoSizeRows = true;
            exit_grid.DataSource = _bindingSourceExit;
            exit_grid.Columns[0].FormatString = "{0:dd/MM/yyyy}";
            exit_grid.Columns[1].FormatString = "{0:dd/MM/yyyy}";
            exit_grid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }
    }
}
