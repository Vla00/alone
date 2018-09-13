using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IPAddress;

namespace Одиноко_проживающие
{
    class Update
    {
        private string _programName = "Одиноко_проживающие_update.exe";
        private string _oldVersion;
        private string _newVersion;

        public Update(string OldVersion, string NewVersion)
        {
            try
            {
                _oldVersion = OldVersion;
                _newVersion = NewVersion;
                ScanFile();
            }
            catch (Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void ScanFile()
        {
            CommandClient commandClient = new CommandClient();

            var conf = commandClient.ReaderFile(@"config.cfg");
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");

            var network = new NetworkShareAccesser();
            //network.SaveACopy(@"\\DC-1\Alldoc\ТЦСОН\Программы\exe\Одиноко проживающие\", Path.Combine(Application.StartupPath, "Temp"), 1500);
            network.SaveACopy(@"\\S1\Alldoc\Install\Программы\Обновления\Одиноко проживающие\", Path.Combine(Application.StartupPath, "Temp"), 1500);
            Thread.Sleep(2500);

            MessageBox.Show(
                @"Обнаружена новая версия программы (" + _newVersion + @"). Текущая версия: " +
                _oldVersion + @". Приложение будет автоматически обновлено и перезапущено.",
                @"Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);           

            Process update = new Process();
            try
            {
                update.StartInfo.UseShellExecute = true;
                update.StartInfo.FileName = _programName;
                update.Start();
            }catch(Exception ex)
            {
                commandClient.WriteFileError(ex, "1 Обновление");
                MessageBox.Show(
                @"Произошла ошибка при обновлении. Обратитесь к разработчику.",
                @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}