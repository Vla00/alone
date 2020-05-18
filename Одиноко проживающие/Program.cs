using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace Одиноко_проживающие
{
    class Program
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
                       
            if (args.Length == 0)
            {
                Application.Run(new Home(null));
            }
            else
            {
                Application.Run(new Home(args));
            }
        }

    }
}
