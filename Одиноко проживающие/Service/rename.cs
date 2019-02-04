using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.Service
{
    public partial class rename : Form
    {
        TelerikMetroTheme theme = new TelerikMetroTheme();
        private BindingSource _bindingSource;

        public rename()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            DeadAutoLoad();
        }

        private void DeadAutoLoad()
        {
            var commandServer = new CommandServer();

            _bindingSource = new BindingSource { DataSource = commandServer.DataGridSet(@"select *
                from family").Tables[0] };

            radGridView3.DataSource = _bindingSource;
            radGridView3.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            for (int i = 0; i < radGridView3.Rows.Count; i++)
            {
                GridViewRowInfo row = radGridView3.Rows[i];

                string fio = row.Cells[1].Value.ToString();
                string name = row.Cells[2].Value.ToString();
                string surname = row.Cells[3].Value.ToString();
                string key = row.Cells[0].Value.ToString();
                
                    var parameters = row.Cells[0].Value.ToString() + ",'" + fio.Trim() + "','" + name.Trim() + "','" + surname.Trim() + "'";
                    commandServer.ExecNoReturnServer("_renameFIO", parameters);
                
            }
        }
    }
}
