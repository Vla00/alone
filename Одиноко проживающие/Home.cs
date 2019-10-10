using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Одиноко_проживающие.all;
using Одиноко_проживающие.handbook;
using Одиноко_проживающие.search;
using Одиноко_проживающие.service;
using Одиноко_проживающие.Service;

namespace Одиноко_проживающие
{
    public partial class Home : Telerik.WinControls.UI.RadForm
    {
        
        TelerikMetroTheme theme = new TelerikMetroTheme();

        #region Конструктор
        public Home(string version)
        {
            InitializeComponent();
            Text = Text + ". Версия программы " + version;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public Home(string[] args, string version)
        {
            InitializeComponent();
            Text = Text + ". Версия программы " + version;

            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Update.docx");
            }
            catch { }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void HomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Home_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 && !e.Control) radMenuButtonItem5.PerformClick();
            if (e.KeyCode == Keys.F3) radMenuButtonItem1.PerformClick();
            if (e.Control && e.KeyCode == Keys.F2) radMenuButtonItem6.PerformClick();
            if (e.Control && e.KeyCode == Keys.M) radMenuButtonItem20_Click();
        }
        #endregion

        #region Меню
        private void Add(object sender, EventArgs e)
        {
            var commandClient = new CommandClient();
            try
            {
                Hide();
                new Alone(true, 0, null, null).ShowDialog();
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, null);
            }
            finally
            {
                Show();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void ListSurvey(object sender, EventArgs e)
        {
            Hide();
            new Date("ListSurvey").ShowDialog();
            Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void ListHelp(object sender, EventArgs e)
        {
            new Date("ListHelp").ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region Поиск
        private void SearchFam(object sender, EventArgs e)
        {
            Hide();
            new SearchFam().ShowDialog(); ;
            Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void radMenuItem10_Click(object sender, EventArgs e)
        {
            Hide();
            new Number().ShowDialog();
            Show();
        }

        private void ExendedSearch(object sender, EventArgs e)
        {
            Hide();
            new ExendedSearch().ShowDialog();
            Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region Справка

        private void Programmer(object sender, EventArgs e)
        {
            MessageBox.Show(@"Разработал: Строк Вадим Владимирович. Тел. +375298046734.
                Email: vadimstrok93@gmail.com, strok.v@mintrud.by", @"Разработчик. 2017", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Setting(object sender, EventArgs e)
        {
            new Configuration(false).ShowDialog();
        }
        #endregion

        #region Справочник
        private void Spezialist(object sender, EventArgs e)
        {
            new Speziolist().ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void SocPersonal(object sender, EventArgs e)
        {
            Hide();
            new SocPersonal().ShowDialog();
            Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Inspector(object sender, EventArgs e)
        {
            Hide();
            new Inspector().ShowDialog();
            Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Selsovet(object sender, EventArgs e)
        {
            new Selsovet().ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Country(object sender, EventArgs e)
        {
            new Country().ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void SemPol(object sender, EventArgs e)
        {
            new SemPol().ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void TypeHelp(object sender, EventArgs e)
        {
            new TypeHelp().ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void JilUsl(object sender, EventArgs e)
        {
            new JilUsl().ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void radMenuItem10_Click_1(object sender, EventArgs e)
        {
            new Powered().ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region Сервис
        private void Dublicate(object sender, EventArgs e)
        {
            Hide();
            new Dublicate().ShowDialog();
            Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Statistic(object sender, EventArgs e)
        {
            Statistic statistic = new Statistic();
            statistic.ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void radMenuItem8_Click(object sender, EventArgs e)
        {
            Hide();
            new Dead().ShowDialog();
            Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void radMenuButtonItem20_Click()
        {
           string text = RadInputBox.Show("Введите пароль для доступа", "Ввод пароля");
           if (text == "qwertyqwe")
           {
                Hide();
                new rename().ShowDialog();
                Show();
           }
           else
           {
               if(text != "null")
               {
                   RadMessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
               }
           }
        }

        
        #endregion
    }
}