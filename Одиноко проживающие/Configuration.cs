using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;
using System.Xml;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Linq;
using System.Diagnostics;

namespace Одиноко_проживающие
{
    public partial class Configuration : RadForm
    {
        TelerikMetroTheme _theme = new TelerikMetroTheme();
        private bool _close;

        public Configuration()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, _theme.ThemeName);
            Load_File_Configuration();
        }

        #region Подключение
        public void Load_File_Configuration()
        {
            try
            {
                var document = new XmlDocument();
                document.Load("config.xml");

                XmlNode root = document.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    foreach (XmlNode nod in node.ChildNodes)
                    {
                        switch(nod.Name)
                        {
                            case "DataBase":
                                textBox_base.Text = nod.InnerText;
                                break;
                            case "DataSource":
                                textBox_server.Text = nod.InnerText;
                                break;
                            case "Port":
                                textBox_port.Text = nod.InnerText;
                                break;
                            case "Login":
                                textBox_login.Text = nod.InnerText;
                                break;
                            case "Password":
                                textBox_password.Text = nod.InnerText;
                                break;
                            case "Users":
                                user_text.Text = nod.InnerText;
                                break;
                            case "AutoSearch":
                                radCheckBox1.Checked = Convert.ToBoolean(nod.InnerText);
                                break;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void Save()
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            foreach(XElement xe in rootConnection.Elements("connection").ToList())
            {
                xe.Element("DataBase").Value = textBox_base.Text;
                xe.Element("DataSource").Value = textBox_server.Text;
                xe.Element("Port").Value = textBox_port.Text;
                xe.Element("Login").Value = textBox_login.Text;
                xe.Element("Password").Value = textBox_password.Text;
            }
            xdoc.Save("config.xml");
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder connectBuilder = new SqlConnectionStringBuilder();
            connectBuilder.InitialCatalog = textBox_base.Text;
            connectBuilder.DataSource = textBox_server.Text;

            if(!string.IsNullOrEmpty(textBox_port.Text))
            {
                connectBuilder.DataSource += "," + textBox_port.Text;
            }
            connectBuilder.IntegratedSecurity = false;
            connectBuilder.UserID = textBox_login.Text;
            connectBuilder.Password = textBox_password.Text;
            connectBuilder.ConnectTimeout = 5;

            if(new CommandServer().isServerConnected(connectBuilder.ConnectionString))
            {
                MessageBox.Show(@"База данных успешно подключена.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Save();
                LoadProgram.Connect.ConnectionString = connectBuilder.ConnectionString;
                if (!LoadProgram.CheackVersion())
                    Close();
                else
                {
                    MessageBox.Show(@"Версия прогаммы не совпадает. Приложение перезапустится", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Restart();
                }  
            }
            else
            {
                MessageBox.Show(@"Указанный сервер не доступен.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _close = true;
            }
        }

        #endregion

        private void radButton1_Click(object sender, EventArgs e)
        {
            textBox_base.Text = "alone";
            textBox_server.Text = "86.57.207.146";
            textBox_port.Text = @"1434\TCSON";
            textBox_login.Text = "soc";
            textBox_password.Text = "karpos?827A";
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            textBox_base.Text = "alone";
            textBox_server.Text = @"10.76.92.220\TCSON";
            textBox_port.Text = "";
            textBox_login.Text = "soc";
            textBox_password.Text = "karpos?827A";
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            if(!CheackUser())
            {
                CreateUser();
            }else
            {
                SaveUser();
            }
            LoadProgram.User = user_text.Text;
            MessageBox.Show(@"Данные сохранены.", @"Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void SaveUser()
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            foreach (XElement xe in rootConnection.Elements("user").ToList())
            {
                xe.Element("Users").Value = user_text.Text;
            }
            xdoc.Save("config.xml");
        }

        private void CreateUser()
        {
            XDocument xdoc = XDocument.Load("config.xml");

            XElement root = new XElement("user");
            root.Add(new XElement("Users", user_text.Text));
            xdoc.Element("mconfig").Add(root);
            xdoc.Save("config.xml");
        }

        private bool CheackUser()
        {
            try
            {
                var document = new XmlDocument();
                document.Load("config.xml");

                XmlNode root = document.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    foreach (XmlNode nod in node.ChildNodes)
                    {
                        switch (nod.Name)
                        {
                            case "Users":
                                return true;
                        }
                    }
                }
            }
            catch (Exception) { }
            return false;
        }


        public bool CheackeAutoSearch()
        {
            try
            {
                var document = new XmlDocument();
                document.Load("config.xml");

                XmlNode root = document.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    foreach (XmlNode nod in node.ChildNodes)
                    {
                        switch (nod.Name)
                        {
                            case "AutoSearch":
                                return true;
                        }
                    }
                }
            }
            catch (Exception) { }

            CreateAutoSearch();
            return false;
        }

        private void CreateAutoSearch()
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            XElement root = new XElement("AutoSearch", "true");
            foreach (XElement xe in rootConnection.Elements("user").ToList())
            {
                xe.Add(new XElement("AutoSearch", "true"));
            }
            xdoc.Save("config.xml");
        }

        private void radCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            foreach (XElement xe in rootConnection.Elements("user").ToList())
            {
                xe.Element("AutoSearch").Value = Convert.ToString(radCheckBox1.Checked);
                LoadProgram._confConnection.AutoSearch = Convert.ToString(radCheckBox1.Checked);
            }
            xdoc.Save("config.xml");
        }

        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_close)
                Process.GetCurrentProcess().Kill();
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
