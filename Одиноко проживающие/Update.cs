using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IPAddress;
using FTPClient;

namespace Одиноко_проживающие
{
    internal class Update
    {
        LoadProgram _load;
        private string _programName = "Одиноко_проживающие_update.exe";
        private readonly string _oldVersion;
        private readonly string _newVersion;
        private string _server;
        public FTP_Client ftp = new FTP_Client();

        public Update(string oldVersion, string newVersion, string server, LoadProgram load)
        {
            try
            {
                _oldVersion = oldVersion;
                _newVersion = newVersion;
                _server = server;
                _load = load;
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
            if (!Directory.Exists("Temp"))
                Directory.CreateDirectory("Temp");
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo("Temp");
                foreach(FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }

            var network = new NetworkShareAccesser();
            _load.SetLabel("Проверка доступности локального сервера");
            network.SaveACopy(@"\\S1\Alldoc\Install\Программы\Обновления\Одиноко проживающие\", Path.Combine(Application.StartupPath, "Temp"), 1500);
            Thread.Sleep(2500);

            DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, "Temp"));
            FileInfo[] files = di.GetFiles();
            if(files.Length != 0)
            {
                UpdateProgram();
            }
            else
            {
                try
                {
                    FTPConnect();
                    UpdateProgram();
                }
                catch(Exception ex){
                    MessageBox.Show(
                    @"Произошла ошибка при обновлении. Обратитесь к разработчику.",
                    @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    new Configuration(true).ShowDialog();
                    var commandClient = new CommandClient();
                    commandClient.WriteFileError(ex, "Обновление");
                }
            }            
        }

        private void FTPConnect()
        {
            ftp.Host = _server;

            FileStruct[] _fileL = ftp.ListDirectory(null);
            _load.SetProgressVisible(true);
            foreach(FileStruct s in _fileL)
            {
                _load.SetLabel("Скачивание: " + s.Name);
                _load.SetProgressValue(0);
                ftp.DownloadFile(null, s.Name, Path.Combine(Application.StartupPath, "Temp"), _load);
            }
        }

        private void UpdateProgram()
        {
            Process update = new Process();
            try
            {
                update.StartInfo.UseShellExecute = true;
                update.StartInfo.FileName = _programName;
                update.Start();
            }
            catch (Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, "Обновление");
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