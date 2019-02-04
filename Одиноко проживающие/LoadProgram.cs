using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace Одиноко_проживающие
{
    public partial class LoadProgram : Form
    {
        public string version = "1.3";
        public string newVersion;
        private static readonly string StrConnect = null;
        public static SqlConnection Connect = new SqlConnection(StrConnect);
        public static SqlConnectionStringBuilder ConnectBuilder = new SqlConnectionStringBuilder();
        private BackgroundWorker helpBackgroundWorker;
        private string[] args;
        private static ConfigurationProgram _confConnection;

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
            LoadProgram lp = new LoadProgram();
            lp.Load_File_Configuration();

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
                Connect.ConnectionString = ConnectBuilder.ConnectionString;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Произошла ошибка: " + ex.Message, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                if (new CommandServer().ConnectDb())
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
                        new Update(version, newVersion, _confConnection.Source);
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
                    new Configuration(true).ShowDialog();
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
                string version = new CommandServer().ComboBoxList("select val from config where name='version_db'", false)[0];

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
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
