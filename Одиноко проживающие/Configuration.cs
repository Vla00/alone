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
        private SqlConnection _connect;
        TelerikMetroTheme _theme = new TelerikMetroTheme();
        private bool _close;

        public Configuration(bool close)
        {
            InitializeComponent();
            _close = close;
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
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void radButton3_Click(object sender, System.EventArgs e)
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

        private void radButton4_Click(object sender, System.EventArgs e)
        {
            if(_close)
                Process.GetCurrentProcess().Kill();
            else
                this.Close();
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

            _connect = new SqlConnection();
            _connect.ConnectionString = connectBuilder.ConnectionString;

            try
            {
                _connect.Open();
                MessageBox.Show(@"База данных успешно подключена.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if(MessageBox.Show(@"Вы хотите сохранить данные найстроки. И перезапустить приложение (требуется для применения новых данных).", @"Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    radButton3.PerformClick();
                    Application.Restart();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(@"Нет подключения к серверу.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _connect.Close();
            }
        }

        #endregion

        private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            switch(e.Position)
            {
                case 0:
                    //аква
                    break;
                case 1:
                    //бриз
                    break;
                case 2:
                    //десерт
                    break;
            }
        }

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
    }
}
