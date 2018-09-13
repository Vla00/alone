using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.search
{
    public partial class ExendedSearch : RadForm
    {
        private List<string> country;
        private List<string> category;
        private List<string> help;
        private bool statusWhere = false;
        TelerikMetroTheme theme = new TelerikMetroTheme();
        BackgroundWorker helpBackgroundWorker;
        Thread countryThread;        

        public ExendedSearch()
        {
            InitializeComponent();
            MyRadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            RadMessageBox.SetThemeName(theme.ThemeName);
            country = new List<string>();
            category = new List<string>();
            help = new List<string>();

            helpBackgroundWorker = new BackgroundWorker();
            helpBackgroundWorker.WorkerReportsProgress = true;
            helpBackgroundWorker.WorkerSupportsCancellation = true;
            helpBackgroundWorker.DoWork += new DoWorkEventHandler(LoadHelp);
        }

        public void LoadCountry()
        {
            List<string> countryRegion = new CommandServer().GetComboBoxList("select selsovet.selsovet from selsovet order by selsovet", true);

            foreach (string coun in countryRegion)
            {
                if (!string.IsNullOrEmpty(coun))
                {
                    radTreeView1.Invoke(new MethodInvoker(delegate ()
                    {
                        radTreeView1.Nodes.Add(coun);
                        List<string> country = new CommandServer().GetComboBoxList("select country.country from selsovet inner join country on country.fk_selsovet = selsovet.key_selsovet where selsovet = '"
                            + coun + "'order by country", true);
                        foreach (string c in country)
                        {
                            if (!string.IsNullOrEmpty(c))
                                radTreeView1.Nodes[coun].Nodes.Add(c);
                        }
                    }));
                }
            }
        }

        private void LoadHelp(object sender, DoWorkEventArgs e)
        {
            List<string> help = new CommandServer().GetComboBoxList(@"select typeHelp
                from help inner join typeHelp on help.fk_typeHelp = typeHelp.key_typeHelp
                group by typeHelp", true);
            countryThread = new Thread(LoadCountry);
            countryThread.Start();

            foreach (string h in help)
            {
                if (!string.IsNullOrEmpty(h))
                {
                    radTreeView2.Invoke(new MethodInvoker(delegate ()
                    {
                        radTreeView2.Nodes.Add(h);
                    }));
                }
            }
        }

        private void сформироватьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            int c = radTreeView1.Nodes.Count;
            statusWhere = false;
            string Head;
            string HeadDead;
            string Group;
            string GroupDead;            

            if (RadMessageBox.Show("Показывать записи которые повторяются?", "Внимание", MessageBoxButtons.YesNo, RadMessageIcon.Question, "При соглашении в результате будут показываться категории") ==
                DialogResult.Yes)
            {
                Head = @"select alone.key_alone, alone.fio as [ФИО], alone.date_ro as [Дата рождения],
		            selsovet.selsovet as [Сельский совет], country.country as [Населенный пункт], 
		            alone.street as [адрес], protivopojar.ApiDate as [АПИ], protivopojar.SzuDate as [СЗУ], category.category as [Категория]";

                Group = @" group by alone.key_alone, alone.fio, alone.date_ro, selsovet.selsovet,
	                country.country, alone.street, protivopojar.ApiDate, protivopojar.SzuDate, category.category";

                HeadDead = @"select alone.key_alone, alone.fio as [ФИО], alone.date_ro as [Дата рождения],
		            selsovet.selsovet as [Сельский совет], country.country as [Населенный пункт], 
		            alone.street as [адрес], protivopojar.ApiDate as [АПИ], protivopojar.SzuDate as [СЗУ],
		            alone.date_exit as [Выезд], alone.date_sm as [Смерть], category.category as [Категория]";

                GroupDead = @" group by alone.key_alone, alone.fio, alone.date_ro, selsovet.selsovet,
	                country.country, alone.street, protivopojar.ApiDate, protivopojar.SzuDate,
	                alone.date_exit, alone.date_sm, category.category";
            }
            else
            {
                Head = @"select alone.key_alone, alone.fio as [ФИО], alone.date_ro as [Дата рождения],
		            selsovet.selsovet as [Сельский совет], country.country as [Населенный пункт], 
		            alone.street as [адрес], protivopojar.ApiDate as [АПИ], protivopojar.SzuDate as [СЗУ]";

                Group = @" group by alone.key_alone, alone.fio, alone.date_ro, selsovet.selsovet,
	            country.country, alone.street, protivopojar.ApiDate, protivopojar.SzuDate";

                HeadDead = @"select alone.key_alone, alone.fio as [ФИО], alone.date_ro as [Дата рождения],
		            selsovet.selsovet as [Сельский совет], country.country as [Населенный пункт], 
		            alone.street as [адрес], protivopojar.ApiDate as [АПИ], protivopojar.SzuDate as [СЗУ],
		            alone.date_exit as [Выезд], alone.date_sm as [Смерть]";

                GroupDead = @" group by alone.key_alone, alone.fio, alone.date_ro, selsovet.selsovet,
	                country.country, alone.street, protivopojar.ApiDate, protivopojar.SzuDate,
	                alone.date_exit, alone.date_sm";
            }

            

            string From = @" from alone inner join category on alone.key_alone = category.fk_alone
		        inner join country on alone.fk_country = country.key_country
		        inner join selsovet on selsovet.key_selsovet = country.fk_selsovet
		        left join protivopojar on protivopojar.fk_alone = alone.key_alone ";

            if (help.Count != 0)
            {
                Head += ", typeHelp.typeHelp as [Помощь], help.dateHelp as [Дата]";
                HeadDead += ", typeHelp.typeHelp as [Помощь], help.dateHelp as [Дата]";
                Group += ", typeHelp, dateHelp";
                GroupDead += ", typeHelp, dateHelp";
                From += "left join help on help.fk_alone = alone.key_alone left join typeHelp on help.fk_typeHelp = key_typeHelp ";
            }

            string Where = " where ";

            if (category.Count != 0)
            {
                if (radioButton1.IsChecked)
                {
                    for (int i = 0; i < category.Count; i++)
                    {
                        From += " join category category" + i + " on alone.key_alone = category" + i + ".fk_alone and category" + i + ".category = '" + category[i] + "' ";
                    }
                }
                else
                {
                    Where += "(";
                    for (int i = 0; i < category.Count; i++)
                    {
                        Where += "category.category = '" + category[i] + "'";

                        if (i != category.Count - 1)
                        {
                            if (radioButton1.IsChecked)
                                Where += " and ";
                            else
                                Where += " or ";
                        }
                        else
                            Where += ")";
                    }
                    statusWhere = true;
                }
            }

            if(help.Count != 0)
            {
                if (statusWhere)
                    Where += " and";
                Where += " (";
                for(int i = 0; i < help.Count; i++)
                {
                    Where += "typeHelp.typeHelp = '" + help[i] + "'";

                    if (i != help.Count - 1)
                    {
                        Where += " or ";
                    }
                    else
                        Where += ")";
                }
                statusWhere = true;
            }

            if (country.Count != 0)
            {
                if (statusWhere)
                    Where += " and";
                Where += " (";
                for (int i = 0; i < country.Count; i++)
                {
                    Where += "country.country = '" + country[i] + "'";

                    if (i != country.Count - 1)
                    {
                        Where += " or ";
                    }
                    else
                        Where += ")";
                }
                statusWhere = true;
            }

            if (radCheckBox39.Checked)
            {
                if(statusWhere)
                {
                    Where += " and (";
                }else
                {
                    Where += " (";
                }
                Where += "dateHelp >= '" + dateTimePicker3.Value.ToString() + "'";

                if(dateTimePicker4.Value.Date != DateTime.Now.Date)
                {
                    Where += " and dateHelp <= '" + dateTimePicker4.Value.ToString() + "'";
                }
                Where += ")";
                statusWhere = true;
            }

            if (dateTimePicker1.Value.Date != DateTime.Now.Date)
            {
                if (statusWhere)
                {
                    Where += " and (";
                }
                else
                {
                    Where += " (";
                }
                Where += "date_ro >= '" + dateTimePicker1.Value.ToString() + "'";

                if (dateTimePicker2.Value.Date != DateTime.Now.Date)
                {
                    Where += " and date_ro <= '" + dateTimePicker2.Value.ToString() + "'";
                }
                Where += ")";
                statusWhere = true;
            }

            if (checkBox7.Checked)
            {
                if (statusWhere)
                {
                    Where += " and (";
                }
                else
                {
                    Where += " (";
                }
                Where += "ApiDate = " + numericUpDown2.Value + ")";
                statusWhere = true;
            }

            if (checkBox6.Checked)
            {
                if (statusWhere)
                {
                    Where += " and (";
                }
                else
                {
                    Where += " (";
                }
                Where += "SzuDate = " + numericUpDown3.Value + ")";
                statusWhere = true;
            }

            Hide();
            if (radRadioButton3.IsChecked)
            {
                if(statusWhere)
                {
                    Where += " and (";
                }else
                {
                    Where += " (";
                }
                Where += "date_sm is null and date_exit is null)";

                if (Where.Length < 10)
                    new Family(Head + From + Group, 0).ShowDialog();
                else
                    new Family(Head + From + Where + Group, 0).ShowDialog();
            }
            else
            {
                if(radRadioButton1.IsChecked)
                {
                    if(statusWhere)
                    {
                        Where += " and (";
                    }else
                    {
                        Where += " (";
                    }
                    Where += "date_exit is not null)";
                    statusWhere = true;
                }

                if(radRadioButton2.IsChecked)
                {
                    if (statusWhere)
                    {
                        Where += " and (";
                    }
                    else
                    {
                        Where += " (";
                    }
                    Where += "date_sm is not null)";
                }
                new Family(HeadDead + From + Where + GroupDead, 1).ShowDialog();
            }
            Show();
        }

        private void Country(Control control)
        {
            foreach (Control c in control.Controls)
            {
                Country(c);
            }

            var cb = control as RadCheckBox;
            if (cb == null) return;
            var controlText = cb.Text;
        }

        private void radCheckBox_Click(object sender, EventArgs e)
        {
            var checkBox = (RadCheckBox)sender;
            var text = checkBox.Text;

            if (!checkBox.Checked)
                category.Add(text);
            else
                category.Remove(text);
        }

        private void radCheckBoxHelp(object sender, TreeNodeCheckedEventArgs e)
        {
            if (e.Node.Checked)
                help.Add(e.Node.Text);
            else
                help.Remove(e.Node.Text);
        }

        private void checkBox7_Click(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
                numericUpDown2.Enabled = false;
            else
                numericUpDown2.Enabled = true;
        }

        private void checkBox6_Click(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                numericUpDown3.Enabled = false;
            else
                numericUpDown3.Enabled = true;
        }

        private void radTreeView1_NodeCheckedChanged(object sender, TreeNodeCheckedEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                foreach (RadTreeNode nod in radTreeView1.Nodes[e.Node.Text].Nodes)
                {
                    if (e.Node.Checked)
                        nod.Checked = true;
                    else
                        nod.Checked = false;
                }
            }
            else
            {
                if (e.Node.Checked)
                    country.Add(e.Node.Text);
                else
                    country.Remove(e.Node.Text);
            }
        }

        private void ExendedSearch_Load(object sender, EventArgs e)
        {
            if (!helpBackgroundWorker.IsBusy)
                helpBackgroundWorker.RunWorkerAsync();
        }

        private void ExendedSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void radCheckBox39_Click(object sender, EventArgs e)
        {
            if(radCheckBox39.Checked)
            {
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;
            }else
            {
                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = true;
            }
        }
    }
}
