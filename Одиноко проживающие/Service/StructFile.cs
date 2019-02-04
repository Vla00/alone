/*
 * Создано в SharpDevelop.
 * Пользователь: "Строк В.В. +357298046734"
 * Дата: 29.05.2017
 * Время: 10:38
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace FTPClient
{	
	public class FTP_Client
	{
		//поле для хранения имени фтп-сервера
        private string _Host;
 
        //поле для хранения логина
        private string _UserName;
 
        //поле для хранения пароля
        private string _Password;
 
        //объект для запроса данных
        FtpWebRequest ftpRequest;
 
        //объект для получения данных
        FtpWebResponse ftpResponse;
 
        //флаг использования SSL
        private bool _UseSSL = false;
 
        //фтп-сервер
        public string Host
        {
            get
            {
                return _Host;
            }
            set
            {
                _Host = value;
            }
        }
       //логин
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
        //пароль
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        //Для установки SSL-чтобы данные нельзя было перехватить
        public bool UseSSL
        {
            get
            {
                return _UseSSL;
            }
            set
            {
                _UseSSL = value;
            }
        }
        //Реализеум команду LIST для получения подробного списока файлов на FTP-сервере
        public FileStruct[] ListDirectory(string path)
        {
            if (path == null || path == "")
            {
                path = "/";
            }
            //Создаем объект запроса
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + _Host + path);
            //логин и пароль
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            //команда фтп LIST
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
 
            ftpRequest.EnableSsl = _UseSSL;
            //Получаем входящий поток
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
 
            //переменная для хранения всей полученной информации
            string content = "" ;
            
                StreamReader sr = new StreamReader(ftpResponse.GetResponseStream(), Encoding.Default);
                content = sr.ReadToEnd();
                sr.Close();
            ftpResponse.Close();
 
            DirectoryListParser parser = new DirectoryListParser(content);
            return parser.FullListing;
        }
 
        //метод протокола FTP RETR для загрузки файла с FTP-сервера
        public void DownloadFile(string path, string fileName, string output)
        {
 
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"  + _Host + path + "/"  + fileName);
 
            ftpRequest.Credentials = new NetworkCredential(_UserName, _Password);
            //команда фтп RETR
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
 
            ftpRequest.EnableSsl = _UseSSL;
            //Файлы будут копироваться в кталог программы
            FileStream downloadedFile = new FileStream(output + "/" + path + "/" + fileName, FileMode.Create, FileAccess.ReadWrite);
 
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            //Получаем входящий поток
            Stream responseStream = ftpResponse.GetResponseStream();
 
            //Буфер для считываемых данных
            byte[] buffer = new byte[1024];
            int size=0;
 
            while ((size=responseStream.Read(buffer, 0, 1024))>0)
            {
                downloadedFile.Write(buffer, 0, size);
                 
            }
            ftpResponse.Close();
            downloadedFile.Close();
            responseStream.Close();
        }
	}
	
	public struct FileStruct
    {
        public string Flags;
        public string Owner;
        public bool IsDirectory;
        public DateTime CreateTime;
        public string Name;
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
 
        public FileStruct[] DirectoryList
        {
            get
            {
                List<FileStruct> _dirList = new List<FileStruct>();
                foreach (FileStruct thisstruct in _myListArray)
                {
                    if (thisstruct.IsDirectory)
                    {
                        _dirList.Add(thisstruct);
                    }
                }
                return _dirList.ToArray();
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
                    FileStruct f = new FileStruct();
                    f.Name = "..";
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
         
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }
    }
}
