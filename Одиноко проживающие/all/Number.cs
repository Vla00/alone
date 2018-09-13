using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;

namespace Одиноко_проживающие.all
{
    public partial class Number : Form
    {
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Number()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            Hide();
            try
            {
                new AddAlone(false, Convert.ToInt32(textBox1.Text)).ShowDialog();
            }
            catch (Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
            Show();
        }
    }
}
