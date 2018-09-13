using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Configuration : RadForm
    {
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Configuration()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
        }

        private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            switch(e.Position)
            {
                case 0:
                    //аква
                    break;
                case 1:
                    //бриз
                    break;
                case 2:
                    //десерт
                    break;
            }
        }
    }
}
