using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using Telerik.WinControls;

namespace Одиноко_проживающие
{
    public partial class LoadProgram : Form
    {
        public string version = "1.0.5";
        public string newVersion;
        private static readonly string StrConnect = null;
        public static SqlConnection Connect = new SqlConnection(StrConnect);
        public static SqlConnectionStringBuilder ConnectBuilder = new SqlConnectionStringBuilder();
        private BackgroundWorker helpBackgroundWorker;
        private string[] args;

        public LoadProgram()
        {
            InitializeComponent();
        }

        public LoadProgram(string[] args)
        {
            InitializeComponent();
            this.args = args;
        }

        public static bool InizializeConnectString()
        {
            var commandClient = new CommandClient();
            try
            {
                var sr = commandClient.ReaderFile(@"config.cfg");
                if (sr.Length > 3)
                {
                    var nameBase = sr[0].Split(' ')[2];
                    if (nameBase != null)
                    {
                        var nameServer = sr[1].Split(' ')[2];
                        if (nameServer != null)
                        {
                            var nameUser = sr[2].Split(' ')[2];
                            if (nameUser != null)
                            {
                                var password = sr[3].Split(' ')[2];
                                if (password != null)
                                {
                                    ConnectBuilder.InitialCatalog = nameBase;
                                    ConnectBuilder.DataSource = nameServer;
                                    ConnectBuilder.IntegratedSecurity = false;
                                    ConnectBuilder.UserID = nameUser;
                                    ConnectBuilder.Password = password;
                                    ConnectBuilder.ConnectTimeout = 5;
                                    Connect.ConnectionString = ConnectBuilder.ConnectionString;
                                    return true;
                                }
                                else
                                {
                                    MessageBox.Show(@"Не указанн пароль пользователя в конфигурационном файле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    commandClient.WriteFileError(null, "Не указанно имя сервера в конфигурационном файле");
                                }
                            }
                            else
                            {
                                MessageBox.Show(@"Не указанно имя пользователя в конфигурационном файле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                commandClient.WriteFileError(null, "Не указанно имя сервера в конфигурационном файле");
                            }
                        }
                        else
                        {
                            MessageBox.Show(@"Не указанно имя сервера в конфигурационном файле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            commandClient.WriteFileError(null, "Не указанно имя сервера в конфигурационном файле");
                        }
                    }
                    else
                    {
                        MessageBox.Show(@"Не указанно имя базы данных в конфигурационном файле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        commandClient.WriteFileError(null, "Не указанно имя базы данных в конфигурационном файле");
                    }
                } else
                {
                    MessageBox.Show(@"Неверный конфигурационный файл. Замените config.cfg в папке с программой.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    commandClient.WriteFileError(null, "Неверный конфигурационный файл.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Произошла ошибка: " + ex.Message, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var ip = new IPAddress.IpAddress();
            //var ipList = ip.ListIp();
            helpBackgroundWorker = new BackgroundWorker();
            helpBackgroundWorker.WorkerReportsProgress = true;
            helpBackgroundWorker.WorkerSupportsCancellation = true;
            helpBackgroundWorker.DoWork += new DoWorkEventHandler(FormOpen);

            if (!helpBackgroundWorker.IsBusy)
                helpBackgroundWorker.RunWorkerAsync();
        }

        private void FormOpen(object sender, DoWorkEventArgs e)
        {
            label1.Invoke(new Action(() =>
            {
                label1.Text = @"Проверка конф. файла";
            }));
            if (InizializeConnectString())
            {
                label1.Invoke(new Action(() =>
                {
                    label1.Text = @"Проверка подключения";
                }));
                if (new CommandServer().ConnectDB())
                {
                    label1.Invoke(new Action(() =>
                    {
                        label1.Text = @"Проверка версии";
                    }));
                    if(CheackVersion())
                    {
                        label1.Invoke(new Action(() =>
                        {
                            label1.Text = @"Скачивание новых файлов";
                        }));
                        new Update(version, newVersion);
                        return;
                    }else
                    {
                        Invoke(new Action(() => { Hide(); }));

                        if (args == null)
                        {
                            Invoke(new Action(() =>
                            {
                                new Home(version).ShowDialog();
                            }));
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                new Home(args, version).ShowDialog();
                            }));
                        }
                    }
                }
                else
                {
                    MessageBox.Show(@"Нет подключения к серверу.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }else
            {
                Application.Exit();
            }
        }

        private bool CheackVersion()
        {
            try
            {
                string version = new CommandServer().GetComboBoxList("select val from config where name='version_db'", false)[0];

                if (version != this.version)
                {
                    newVersion = version;
                    return true;
                }
                    
                return false;
            }catch(Exception ex)
            {
                new CommandClient().WriteFileError(ex, ConnectBuilder.ConnectionString);
            }finally{ }
            return false;
        }
    }
}
