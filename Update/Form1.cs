using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Update
{
    public partial class Form1 : Telerik.WinControls.UI.RadForm
    {
        private string _programName = "Одиноко проживающие.exe";
        Thread _pool;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            _pool = new Thread(LoadFile);
            _pool.Start();
        }

        private void LoadFile()
        {
            try
            {
                while (Process.GetProcessesByName("Одиноко проживающие").Length > 0) { }

                DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.StartupPath, @"Temp\"));
                foreach (FileInfo file in dir.GetFiles("*.*"))
                {
                    try
                    {
                        if (file.Name != "Одиноко_проживающие_update.exe")
                            File.Copy(file.FullName, file.Name, true);
                    }
                    catch (Exception) { }
                    
                } 
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + @"! " + exception.StackTrace);
            }
            finally
            {
                if (Directory.Exists("Temp"))
                    Directory.Delete(Path.Combine(Application.StartupPath, "Temp"), true);
                Process.Start(_programName, "-u");
                Application.Exit();
            }
        }
    }
}
