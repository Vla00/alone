﻿using Telerik.WinControls;
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
        readonly TelerikMetroTheme _theme = new TelerikMetroTheme();
        private bool _close;

        private string _user;
        //private readonly string _name;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя вкладки</param>
        /// <param name="highlighting">Выделение поля красным</param>
        /// <param name="close">Закрытие программы при закрытии формы</param>
        public Configuration(string name, string highlighting, bool close)
        {
            InitializeComponent();
            
            ThemeResolutionService.ApplyThemeToControlTree(this, _theme.ThemeName);
            Load_File_Configuration();
            
            //_name = name;
            _close = close;

            switch(name)
            {
                case "base":
                    radPageView1.SelectedPage = radPageViewPage1;
                    break;
                case "user":
                    radPageView1.SelectedPage = radPageViewPage2;
                    if(!string.IsNullOrEmpty(highlighting))
                    {
                        switch(highlighting)
                        {
                            case "name":
                                user_text.BackColor = System.Drawing.Color.Pink;
                                break;
                        }
                    }
                    break;
            }
        }

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
                        switch (nod.Name)
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
                                _user = user_text.Text;
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

        #region Подключение

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

        private void RadButton2_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder connectBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = textBox_base.Text,
                DataSource = textBox_server.Text,
                IntegratedSecurity = false,
                UserID = textBox_login.Text,
                Password = textBox_password.Text,
                ConnectTimeout = 5
            };

            if (!string.IsNullOrEmpty(textBox_port.Text))
            {
                connectBuilder.DataSource += "," + textBox_port.Text;
            }

            if(new CommandServer().IsServerConnected(connectBuilder.ConnectionString))
            {
                MessageBox.Show(@"База данных успешно подключена.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Save();

                Application.Restart();
                ///TODO Доделать открытие
                //Home.programConn.configurationProgram.Connect.ConnectionString = connectBuilder.ConnectionString;
                //if (!LoadProgram.CheackVersion())
                //    Close();
                //else
                //{
                //    MessageBox.Show(@"Версия прогаммы не совпадает. Приложение перезапустится", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    Application.Restart();
                //}  
            }
            else
            {
                MessageBox.Show(@"Указанный сервер не доступен.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _close = true;
            }
        }

        private void RadButton1_Click(object sender, EventArgs e)
        {
            textBox_base.Text = "alone";
            textBox_server.Text = "86.57.207.146";
            textBox_port.Text = @"1434\TCSON";
            textBox_login.Text = "soc";
            textBox_password.Text = "karpos?827A";
        }

        private void RadButton5_Click(object sender, EventArgs e)
        {
            textBox_base.Text = "alone";
            textBox_server.Text = @"10.76.92.220\TCSON";
            textBox_port.Text = "";
            textBox_login.Text = "soc";
            textBox_password.Text = "karpos?827A";
        }
        #endregion
        
        public string ReturnName() { return user_text.Text.Trim(); }
        
        #region Пользователь

        private void SaveUserProc()
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            foreach (XElement xe in rootConnection.Elements("user").ToList())
            {
                xe.Element("Users").Value = user_text.Text;

                if(_user != user_text.Text)
                {
                    xe.Element("NameComputer").Value = Environment.MachineName;
                }
            }
            xdoc.Save("config.xml");
            _close = false;
            Close();
        }
        #endregion

        #region Автопоиск
        
        private void RadCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            foreach (XElement xe in rootConnection.Elements("user").ToList())
            {
                xe.Element("AutoSearch").Value = Convert.ToString(radCheckBox1.Checked);
                Home.programConn.configurationProgram.AutoSearch = Convert.ToString(radCheckBox1.Checked);
            }
            xdoc.Save("config.xml");
        }

        #endregion

        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_close)
                Process.GetCurrentProcess().Kill();
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void SaveUserButton(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(user_text.Text))
            {
                SaveUserProc();
            }else
            {
                RadMessageBox.Show("Не указано имя пользователя.", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }        
    }
}
