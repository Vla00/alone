using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Одиноко_проживающие.Load
{
    public partial class ProgramLoad
    {
        public string _version = "1.8.1";
        public static string User = null;
        public static SqlConnection Connect;
        public static SqlConnectionStringBuilder ConnectBuilder = new SqlConnectionStringBuilder();
        private string[] args;
        private Wait _wait;
        public static ConfigurationProgram _confConnection;
        private BackgroundWorker _backgroundWorker1;
        private ConfigurationProgramConn programConn;
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);

        public ProgramLoad()
        {
            _backgroundWorker1 = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            _backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            _backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;

            _backgroundWorker1.RunWorkerAsync();
            _wait = new Wait(true);
            _wait.ShowDialog();
            _resetEvent.WaitOne();
        }

        public ProgramLoad(string[] args)
        {
            this.args = args;

            _backgroundWorker1 = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            _backgroundWorker1.DoWork += BackgroundWorker1_DoWork;

             _wait = new Wait(false);
             _wait.Show();
            _backgroundWorker1.RunWorkerAsync();
        }

        public ConfigurationProgramConn ConfigurationProgramReturn()
        {
            return programConn;
        }

        #region Configuration
        public bool InizializeConnectString()
        {
            var commandClient = new CommandClient();
            commandClient.CheackConfiguration();

            while(true)
            {
                Load_File_Configuration();

                try
                {
                    ConnectBuilder.InitialCatalog = _confConnection.Base;
                    ConnectBuilder.DataSource = _confConnection.Source;
                    if (!string.IsNullOrEmpty(_confConnection.Port))
                        ConnectBuilder.DataSource += "," + _confConnection.Port;
                    ConnectBuilder.IntegratedSecurity = false;
                    ConnectBuilder.UserID = _confConnection.Login;
                    ConnectBuilder.Password = _confConnection.Password;
                    ConnectBuilder.ConnectTimeout = 15;
                    Connect = new SqlConnection
                    {
                        ConnectionString = ConnectBuilder.ConnectionString
                    };

                    bool stat = InizializeConnectStringCheack();
                    if (stat)
                    {
                        programConn = new ConfigurationProgramConn
                        {
                            configurationProgram = _confConnection,
                            sqlConnection = Connect,
                            connectionStringBuilder = ConnectBuilder
                        };
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Произошла ошибка: " + ex.Message, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private bool InizializeConnectStringCheack()
        {
            ///TODO доделать открытие конфигурации
            if (string.IsNullOrEmpty(_confConnection.User))
            {
                Configuration conf = new Configuration("user", "name", true);
                conf.ShowDialog();
                return false;
            }

            if (!string.IsNullOrEmpty(_confConnection.NameComp))
            {
                if (Environment.MachineName != _confConnection.NameComp)
                {
                    Configuration conf = new Configuration("user", "name", true);
                    conf.ShowDialog();
                    return false;
                }
            }

            return true;
        }

        public void Load_File_Configuration()
        {
            try
            {
                var document = new XmlDocument();
                document.Load("config.xml");

                XmlNode root = document.DocumentElement;
                _confConnection = new ConfigurationProgram();
                foreach (XmlNode node in root.ChildNodes)
                {
                    foreach (XmlNode nod in node.ChildNodes)
                    {
                        switch (nod.Name)
                        {
                            case "DataBase":
                                _confConnection.Base = nod.InnerText;
                                break;
                            case "DataSource":
                                _confConnection.Source = nod.InnerText;
                                break;
                            case "Port":
                                _confConnection.Port = nod.InnerText;
                                break;
                            case "Login":
                                _confConnection.Login = nod.InnerText;
                                break;
                            case "Password":
                                _confConnection.Password = nod.InnerText;
                                break;
                            case "Users":
                                _confConnection.User = nod.InnerText;
                                break;
                            case "AutoSearch":
                                _confConnection.AutoSearch = nod.InnerText;
                                break;
                            case "NameComputer":
                                _confConnection.NameComp = nod.InnerText;
                                break;
                        }
                    }
                }
            }
            catch (Exception) { }
        }
        #endregion

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _wait.Hide();
            _wait.Close();
            _resetEvent.Set();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            FormOpen();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(e.UserState as string))
                _wait.Message = e.UserState as string;
            if(e.ProgressPercentage > 0)
                _wait.ProgressVal = e.ProgressPercentage;
        }        

        public bool CheackVersion()
        {
            try
            {
                string version = new CommandServer().ComboBoxList("select val from config where name='version_db'", false)[0];

                if (version != _version)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                new CommandClient().WriteFileError(ex, ConnectBuilder.ConnectionString);
            }
            finally { }
            return false;
        }

        private void FormOpen()
        {
            _backgroundWorker1.ReportProgress(0, "Проверка конф. файла");
            if (InizializeConnectString())
            {
                _backgroundWorker1.ReportProgress(0, "Проверка подключения");
                if (new CommandServer().IsServerConnected(Connect.ConnectionString))
                {
                    _backgroundWorker1.ReportProgress(0, "Проверка версии");
                    if (CheackVersion())
                    {
                        _backgroundWorker1.ReportProgress(0, "Скачивание новых файлов");
                        new Update(_backgroundWorker1);
                        return;
                    }
                    else
                    {
                        _backgroundWorker1.ReportProgress(0, "Отправка ошибок на сервер");
                        new CommandClient().ReadFileError();
                        programConn.connect = true;
                    }
                }
                else
                {
                    if (Convert.ToBoolean(_confConnection.AutoSearch))
                    {

                        if (new CommandServer().SearchServer())
                            MessageBox.Show(@"Не удалось найти сервер.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        _backgroundWorker1.ReportProgress(0, "Отправка ошибок на сервер");
                        new CommandClient().ReadFileError();
                    }
                    else
                    {
                        new Configuration("base", null, true).ShowDialog();
                    }
                }
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
