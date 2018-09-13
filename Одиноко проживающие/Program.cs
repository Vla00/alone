using System;
using System.Windows.Forms;

namespace Одиноко_проживающие
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            if(args.Length == 0)
                Application.Run(new LoadProgram());
            else
                Application.Run(new LoadProgram(args));
        }
        
    }
}
