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
        private string _programName = "Одиноко_проживающие_update.exe";
        private readonly string _oldVersion;
        private readonly string _newVersion;
        private string _server;
        public FTP_Client ftp = new FTP_Client();

        public Update(string oldVersion, string newVersion, string server)
        {
            try
            {
                _oldVersion = oldVersion;
                _newVersion = newVersion;
                _server = server;
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

            var network = new NetworkShareAccesser();
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
                    var commandClient = new CommandClient();
                    commandClient.WriteFileError(ex, "Обновление");
                }
            }            
        }

        private void FTPConnect()
        {
            ftp.Host = _server;

            FileStruct[] _fileL = ftp.ListDirectory(null);

            foreach(FileStruct s in _fileL)
            {
                ftp.DownloadFile(null, s.Name, Path.Combine(Application.StartupPath, "Temp"));
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