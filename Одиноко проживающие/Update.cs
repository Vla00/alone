using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IPAddress;
using FTPClient;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Одиноко_проживающие
{
    internal class Update
    {
        BackgroundWorker _backgroundWorker1;
        private readonly string _programName = "Одиноко_проживающие_update.exe";
        private readonly string _server = "86.57.207.146";
        public FTP_Client ftp = new FTP_Client();

        public Update(BackgroundWorker backgroundWorker1)
        {
            try
            {
                _backgroundWorker1 = backgroundWorker1;
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
            _backgroundWorker1.ReportProgress(0, "Считывание обновлений");
            network.SaveACopy(@"\\S1\Alldoc\Install\Программы\Обновления\Одиноко проживающие\", Path.Combine(Application.StartupPath, "Temp"), 1500);
            Thread.Sleep(2500);

            DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, "Temp"));
            FileInfo[] files = di.GetFiles();
            _backgroundWorker1.ReportProgress(0, "Считывание новых файлов.");
            if (files.Length != 0)
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
                    new Configuration("base", null, true).ShowDialog();
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
                _backgroundWorker1.ReportProgress(0, "Скачивание: " + s.Name);
                //                _load.SetProgressValue(0);
                ftp.DownloadFile(null, s.Name, Path.Combine(Application.StartupPath, "Temp"), _backgroundWorker1);
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

namespace FTPClient
{
    public class FTP_Client
    {
        BackgroundWorker _backgroundWorker1;

        //объект для запроса данных
        FtpWebRequest ftpRequest;

        //объект для получения данных
        FtpWebResponse ftpResponse;

        //флаг использования SSL
        private readonly bool _UseSSL = false;

        //фтп-сервер
        public string Host { get; set; }

        //Реализеум команду LIST для получения подробного списока файлов на FTP-сервере
        public FileStruct[] ListDirectory(string path)
        {
            if (path == null || path == "")
            {
                path = "/";
            }
            //Создаем объект запроса
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + Host + path);
            //логин и пароль
            ftpRequest.Credentials = new NetworkCredential("", "");
            //команда фтп LIST
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            ftpRequest.EnableSsl = _UseSSL;
            //Получаем входящий поток
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

            //переменная для хранения всей полученной информации
            string content = "";

            StreamReader sr = new StreamReader(ftpResponse.GetResponseStream(), Encoding.Default);
            content = sr.ReadToEnd();
            sr.Close();
            ftpResponse.Close();

            DirectoryListParser parser = new DirectoryListParser(content);
            return parser.FullListing;
        }

        //метод протокола FTP RETR для загрузки файла с FTP-сервера
        public void DownloadFile(string path, string fileName, string output, BackgroundWorker backgroundWorker1)
        {

            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + Host + path + "/" + fileName);

            ftpRequest.Credentials = new NetworkCredential("", "");
            //команда фтп RETR
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            FtpWebResponse respSize = (FtpWebResponse)ftpRequest.GetResponse();
            //размер файла
            long sizeSrc = respSize.ContentLength;
            ftpResponse.Close();
            _backgroundWorker1 = backgroundWorker1;

            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + Host + path + "/" + fileName);

            ftpRequest.Credentials = new NetworkCredential("", "");
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            ftpRequest.EnableSsl = _UseSSL;
            //Файлы будут копироваться в кталог программы
            FileStream downloadedFile = new FileStream(output + "/" + path + "/" + fileName, FileMode.Create, FileAccess.ReadWrite);

            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            //Получаем входящий поток
            Stream responseStream = ftpResponse.GetResponseStream();

            //Буфер для считываемых данных
            byte[] buffer = new byte[1024];
            int size = 0;

            double check_sum = 0.00000;
            while ((size = responseStream.Read(buffer, 0, 1024)) > 0)
            {
                check_sum += size;
                
                downloadedFile.Write(buffer, 0, size);
                _backgroundWorker1.ReportProgress((int)(check_sum/ sizeSrc * 100), null);
            }
            ftpResponse.Close();
            downloadedFile.Close();
            responseStream.Close();

            FileInfo file = new FileInfo(output + "/" + path + "/" + fileName);
            long sizeDesc = file.Length;

            if (sizeDesc != sizeSrc)
            {

            }
        }
    }

    public struct FileStruct
    {
        public string Flags;
        public string Owner;
        public bool IsDirectory;
        public DateTime CreateTime;
        public string Name;
        public long Size;
    }

    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }
    //Класс для парсинга
    public class DirectoryListParser
    {
        private List<FileStruct> _myListArray;

        public FileStruct[] FullListing
        {
            get
            {
                return _myListArray.ToArray();
            }
        }

        public FileStruct[] FileList
        {
            get
            {
                List<FileStruct> _fileList = new List<FileStruct>();
                foreach (FileStruct thisstruct in _myListArray)
                {
                    if (!thisstruct.IsDirectory)
                    {
                        _fileList.Add(thisstruct);
                    }
                }
                return _fileList.ToArray();
            }
        }

        public DirectoryListParser(string responseString)
        {
            _myListArray = GetList(responseString);
        }

        private List<FileStruct> GetList(string datastring)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            string[] dataRecords = datastring.Split('\n');
            //Получаем стиль записей на сервере
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (string s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct
                    {
                        Name = ".."
                    };
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (f.Name != "" && f.Name != "." && f.Name != "..")
                    {
                        myListArray.Add(f);
                    }
                }
            }
            return myListArray;
        }
        //Парсинг, если фтп сервера работает на Windows
        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            //Предположим стиль записи 02-03-04  07:46PM       <DIR>     Append
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            //Получаем дату
            string dateStr = processstr.Substring(3, 2) + "-" + processstr.Substring(0, 2) + "-" + processstr.Substring(6, 2);
            //processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            //Получаем время
            //string timeStr = processstr.Substring(0, 7);
            //processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            f.CreateTime = Convert.ToDateTime(dateStr);
            //Это папка или нет
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                processstr = strs[3];
                if (strs.Length > 4)
                    processstr += " " + strs[4];
                f.IsDirectory = false;
            }
            //Остальное содержмое строки представляет имя каталога/файла
            f.Name = processstr;
            f.Size = processstr.Length;
            return f;
        }
        //Получаем на какой ОС работает фтп-сервер - от этого будет зависеть дальнейший парсинг
        public FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                //Если соблюдено условие, то используется стиль Unix
                if (s.Length > 10
                    && Regex.IsMatch(s.Substring(0, 10), "(-|d)((-|r)(-|w)(-|x)){3}"))
                {
                    return FileListStyle.UnixStyle;
                }
                //Иначе стиль Windows
                else if (s.Length > 8
                    && Regex.IsMatch(s.Substring(0, 8), "[0-9]{2}-[0-9]{2}-[0-9]{2}"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }
    }
}