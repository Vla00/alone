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
        private readonly List<string> _country;
        private readonly List<string> _category;
        private readonly List<string> _help;
        private bool _statusWhere;
        private bool _statusArh;
        private bool _statusS;
        private bool _statusPo;
        private readonly TelerikMetroTheme _theme = new TelerikMetroTheme();
        readonly BackgroundWorker _helpBackgroundWorker;
        private Thread _countryThread;        

        public ExendedSearch()
        {
            InitializeComponent();
            MyRadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, _theme.ThemeName);
            RadMessageBox.SetThemeName(_theme.ThemeName);
            _country = new List<string>();
            _category = new List<string>();
            _help = new List<string>();

            _helpBackgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true, WorkerSupportsCancellation = true
            };
            _helpBackgroundWorker.DoWork += LoadHelp;
        }

        public void LoadCountry()
        {
            var countryRegion = new CommandServer().ComboBoxList("select selsovet.selsovet from selsovet order by selsovet", true);

            foreach (var coun in countryRegion)
            {
                if (!string.IsNullOrEmpty(coun))
                {
                    radTreeView1.Invoke(new MethodInvoker(delegate
                    {
                        radTreeView1.Nodes.Add(coun);
                        var country = new CommandServer().ComboBoxList("select country.country from selsovet inner join country on country.fk_selsovet = selsovet.key_selsovet where selsovet = '"
                            + coun + "'order by country", true);
                        foreach (var c in country)
                        {
                            if (!string.IsNullOrEmpty(c))
                                radTreeView1.Nodes[coun].Nodes.Add(c);
                        }
                    }));
                }
            }
        }

        private void LoadInv()
        {
            var inv = new CommandServer().ComboBoxList(@"select name
                from every_set
                where tabl = 'disability'
                order by name", true);
            foreach(var i in inv)
            {
                if(!string.IsNullOrEmpty(i))
                {
                    radTreeView3.Invoke(new MethodInvoker(delegate
                    {
                        radTreeView3.Nodes.Add(i);
                    }));
                }
            }
        }

        private void LoadHelp(object sender, DoWorkEventArgs e)
        {
            var help = new CommandServer().ComboBoxList(@"select typeHelp
                from help inner join typeHelp on help.fk_typeHelp = typeHelp.key_typeHelp
                group by typeHelp", true);
            _countryThread = new Thread(LoadCountry);
            _countryThread.Start();

            foreach (var h in help)
            {
                if (!string.IsNullOrEmpty(h))
                {
                    radTreeView2.Invoke(new MethodInvoker(delegate
                    {
                        radTreeView2.Nodes.Add(h);
                    }));
                }
            }
        }

        private void СформироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            #region
            var c = radTreeView1.Nodes.Count;
            _statusWhere = false;

            var head = @"select alone.key_alone, (family.family + ' ' + family.name + ' ' + family.surname) as [ФИО], alone.date_ro as [Дата рождения],
		            selsovet.selsovet as [Сельский совет], country.country as [Населенный пункт], 
		            every_set.name + ' ' + adres.street +
		            case when adres.house IS not null
                        then ' д.' + cast(adres.house as nvarchar(50))
                        else ''
                    end +
                    case when adres.housing is not null
                        then ' корп.' + adres.housing
                        else ''
                    end +
                    case when adres.apartment IS not null
                        then ' кв.' + cast(adres.apartment AS nvarchar(50))
                        else ''
                    end as [Адрес], protivopojar.ApiDate as [АПИ], protivopojar.SzuDate as [СЗУ]";

                var group = @" group by alone.key_alone, family.family, family.name, family.surname, alone.date_ro, selsovet.selsovet,
	            country.country, protivopojar.ApiDate, protivopojar.SzuDate, every_set.name, adres.street, adres.house, adres.housing, adres.apartment";

                var headDead = @"select alone.key_alone, (family.family + ' ' + family.name + ' ' + family.surname) as [ФИО], alone.date_ro as [Дата рождения],
		            selsovet.selsovet as [Сельский совет], country.country as [Населенный пункт], 
		            every_set.name + ' ' + adres.street +
		            case when adres.house IS not null
                        then ' д.' + cast(adres.house as nvarchar(50))
                        else ''
                    end +
                    case when adres.housing is not null
                        then ' корп.' + adres.housing
                        else ''
                    end +
                    case when adres.apartment IS not null
                        then ' кв.' + cast(adres.apartment AS nvarchar(50))
                        else ''
                    end as [Адрес], protivopojar.ApiDate as [АПИ], protivopojar.SzuDate as [СЗУ],
		            alone.date_exit as [Выезд], alone.date_sm as [Смерть]";

                var groupDead = @" group by alone.key_alone, family.family, family.name, family.surname, alone.date_ro, selsovet.selsovet,
	                country.country, protivopojar.ApiDate, protivopojar.SzuDate,
	                alone.date_exit, alone.date_sm, every_set.name, adres.street, adres.house, adres.housing, adres.apartment";

            const string headOne = @"select distinct *, 
                    STUFF(cast((select[text()] = '; ' + t1.category
                    from category as [t1]
                    where t1.fk_alone = t2.key_alone
                    for Xml path(''), type) as nvarchar(max)), 1, 2, '') as [категории]
                from(";

            var from = @" from alone inner join category on alone.key_alone = category.fk_alone
		        inner join country on alone.fk_country = country.key_country
		        inner join selsovet on selsovet.key_selsovet = country.fk_selsovet
                inner join family on family.fk_alone = alone.key_alone
		        inner join adres on adres.fk_alone = alone.key_alone
		        inner join every_set on every_set.key_every = adres.fk_every
		        left join protivopojar on protivopojar.fk_alone = alone.key_alone ";

            if (_help.Count != 0)
            {
                head += ", typeHelp.typeHelp as [Помощь], help.dateHelp as [Дата]";
                headDead += ", typeHelp.typeHelp as [Помощь], help.dateHelp as [Дата]";
                group += ", typeHelp, dateHelp";
                groupDead += ", typeHelp, dateHelp";
                from += "left join help on help.fk_alone = alone.key_alone left join typeHelp on help.fk_typeHelp = key_typeHelp ";
            }

            var where = " where ";

            if (_category.Count != 0)
            {
                if (radioButton1.IsChecked)
                {
                    if(!_statusArh)
                    {
                        for (var i = 0; i < _category.Count; i++)
                        {
                            from += " join category category" + i + " on alone.key_alone = category" + i + ".fk_alone and category" + i + ".category = '" + _category[i] + "' ";
                        }
                    }else
                    {
                        for (var i = 0; i < _category.Count; i++)
                        {
                            from += " join category_time category" + i + " on alone.key_alone = category" + i + ".fk_alone and category" + i + ".category = '" + _category[i] + "' ";
                        }
                    }
                    
                }
                else
                {
                    where += "(";
                    for (var i = 0; i < _category.Count; i++)
                    {
                        where += "category.category = '" + _category[i] + "'";

                        if (i != _category.Count - 1)
                        {
                            if (radioButton1.IsChecked)
                                where += " and ";
                            else
                                where += " or ";
                        }
                        else
                            where += ")";
                    }
                    _statusWhere = true;
                }
            }

            if(_help.Count != 0)
            {
                if (_statusWhere)
                    where += " and";
                where += " (";
                for(var i = 0; i < _help.Count; i++)
                {
                    where += "typeHelp.typeHelp = '" + _help[i] + "'";

                    if (i != _help.Count - 1)
                    {
                        where += " or ";
                    }
                    else
                        where += ")";
                }
                _statusWhere = true;
            }

            if (_country.Count != 0)
            {
                if (_statusWhere)
                    where += " and";
                where += " (";
                for (var i = 0; i < _country.Count; i++)
                {
                    where += "country.country = '" + _country[i] + "'";

                    if (i != _country.Count - 1)
                    {
                        where += " or ";
                    }
                    else
                        where += ")";
                }
                _statusWhere = true;
            }

            if (radCheckBox39.Checked)
            {
                if(_statusWhere)
                {
                    where += " and (";
                }else
                {
                    where += " (";
                }
                where += "dateHelp >= '" + dateTimePicker3.Value.ToString("dd.MM.yyyy") + "'";

                if(dateTimePicker4.Value.Date != DateTime.Now.Date)
                {
                    where += " and dateHelp <= '" + dateTimePicker4.Value.ToString("dd.MM.yyyy") + "'";
                }
                where += ")";
                _statusWhere = true;
            }

            if (dateTimePicker1.Value.Date != DateTime.Now.Date)
            {
                if (_statusWhere)
                {
                    where += " and (";
                }
                else
                {
                    where += " (";
                }
                where += "date_ro >= '" + dateTimePicker1.Value.ToString("dd.MM.yyyy") + "'";

                if (dateTimePicker2.Value.Date != DateTime.Now.Date)
                {
                    where += " and date_ro <= '" + dateTimePicker2.Value.ToString("dd.MM.yyyy") + "'";
                }
                where += ")";
                _statusWhere = true;
            }

            if (checkBox7.Checked)
            {
                if (_statusWhere)
                {
                    where += " and (";
                }
                else
                {
                    where += " (";
                }
                where += "ApiDate = " + numericUpDown2.Value.ToString() + ")";
                _statusWhere = true;
            }

            if (checkBox6.Checked)
            {
                if (_statusWhere)
                {
                    where += " and (";
                }
                else
                {
                    where += " (";
                }
                where += "SzuDate = " + numericUpDown3.Value.ToString() + ")";
                _statusWhere = true;
            }
            #endregion

            string pol;
            if (radRadioButton2.IsChecked)
            {
                pol = "alone.pol = 1";
            }else
            {
                if (radRadioButton3.IsChecked)
                    pol = "alone.pol = 0";
                else
                    pol = null;
            }

            if (!radRadioButton4.IsChecked)
            {
                if(_statusWhere)
                {
                    if(radRadioButton5.IsChecked)
                    {
                        where += " and (survey.date_obsl >= '" + radDateTimePicker1.Value.ToString("dd.MM.yyyy") + "' and survey.date_obsl <='" + radDateTimePicker2.Value.ToString("dd.MM.yyyy") + "')";
                    }else
                    {
                        where += @" and not exists(select * from survey
                    where survey.date_obsl >= '" + radDateTimePicker1.Value.ToString("dd.MM.yyyy") + "' and survey.date_obsl <= '" + radDateTimePicker2.Value.ToString("dd.MM.yyyy") + "' and survey.fk_alone = alone.key_alone)";
                    }
                    
                }else
                {
                    if(radRadioButton5.IsChecked)
                    {
                        where += " (survey.date_obsl >= '" + radDateTimePicker1.Value.ToString("dd.MM.yyyy") + "' and survey.date_obsl <='" + radDateTimePicker2.Value.ToString("dd.MM.yyyy") + "')";
                    }else
                    {
                        where += @" not exists(select * from survey
                    where survey.date_obsl >= '" + radDateTimePicker1.Value.ToString("dd.MM.yyyy") + "' and survey.date_obsl <= '" + radDateTimePicker2.Value.ToString("dd.MM.yyyy") + "' and survey.fk_alone = alone.key_alone)";
                    }                    
                }
                from += " left join survey on survey.fk_alone = alone.key_alone";
                _statusWhere = true;
            }

            Hide();
            if (view_radio.IsChecked)
            {
                if(_statusWhere)
                {
                    where += " and (";
                }else
                {
                    where += " (";
                }
                where += "date_sm is null and date_exit is null)";

                if(!string.IsNullOrEmpty(pol))
                {
                    where += " and (" + pol + ")";
                }

                if (where.Length < 10)
                {
                    new Result(headOne + head + from + group + ") as t2 order by [ФИО]", 0).ShowDialog();
                }                    
                else
                {
                    new Result(headOne + head + from + where + group + ") as t2 order by [ФИО]", 0).ShowDialog();
                }
                    
            }
            else
            {
                if (arch_radio.IsChecked)
                {
                    if (_statusWhere)
                    {
                        where += " and (";
                    }
                    else
                    {
                        where += " (";
                    }

                    if(!_statusS && !_statusPo)
                    {
                        where += "date_exit is not null or date_sm is not null)";
                    }else
                    {
                        where += "(date_exit >= '" + dateTimePicker5.Value.ToString("dd.MM.yyyy") + "' and date_exit <= '" + dateTimePicker6.Value.ToString("dd.MM.yyyy") +
                            "') or (date_sm >= '" + dateTimePicker5.Value.ToString("dd.MM.yyyy") + "' and date_sm <= '" + dateTimePicker6.Value.ToString("dd.MM.yyyy") + "'))";
                    }
                    
                    _statusWhere = true;
                }

                if (!string.IsNullOrEmpty(pol))
                {
                    if (_statusWhere)
                        where += " and (" + pol + ")";
                    else
                        where += " (" + pol + ")";
                }

                new Result(headOne + headDead + from + where + groupDead + ") as t2 order by [ФИО]", 1).ShowDialog();
            }
            Show();
        }

        private void RadCheckBox_Click(object sender, EventArgs e)
        {
            var checkBox = (RadCheckBox)sender;
            var text = checkBox.Text;

            if (!checkBox.Checked)
                _category.Add(text);
            else
                _category.Remove(text);
        }

        private void RadCheckBoxHelp(object sender, TreeNodeCheckedEventArgs e)
        {
            if (e.Node.Checked)
                _help.Add(e.Node.Text);
            else
                _help.Remove(e.Node.Text);
        }

        private void CheckBox7_Click(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
                numericUpDown2.Enabled = false;
            else
                numericUpDown2.Enabled = true;
        }

        private void CheckBox6_Click(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                numericUpDown3.Enabled = false;
            else
                numericUpDown3.Enabled = true;
        }

        private void RadTreeView1_NodeCheckedChanged(object sender, TreeNodeCheckedEventArgs e)
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
                    _country.Add(e.Node.Text);
                else
                    _country.Remove(e.Node.Text);
            }
        }

        private void ExendedSearch_Load(object sender, EventArgs e)
        {
            if (!_helpBackgroundWorker.IsBusy)
                _helpBackgroundWorker.RunWorkerAsync();
        }

        private void ExendedSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void RadCheckBox39_Click(object sender, EventArgs e)
        {
            if(radCheckBox39.Checked)
            {
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;
                radTreeView2.Enabled = false;
            }else
            {
                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = true;
                radTreeView2.Enabled = true;
            }
        }

        private void RadCheckBox40_Click(object sender, EventArgs e)
        {
            RadRadioButton check = (RadRadioButton)sender;
            if(check.Text == "Все")
            {
                radDateTimePicker1.Enabled = false;
                radDateTimePicker2.Enabled = false;
            }else
            {
                radDateTimePicker1.Enabled = true;
                radDateTimePicker2.Enabled = true;
            }
        }

        private void RadDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SendKeys.Send(".");
        }

        private void Arch_radio_CheckStateChanged(object sender, EventArgs e)
        {
            radCheckBox14.Enabled = arch_radio.IsChecked;
            radCheckBox15.Enabled = arch_radio.IsChecked;

            if(radCheckBox14.Checked)
            {
                radCheckBox14.Checked = arch_radio.IsChecked;
                radCheckBox15.Checked = arch_radio.IsChecked;
            }

            _statusArh = arch_radio.IsChecked;
        }

        private void RadCheckBox14_CheckStateChanged(object sender, EventArgs e)
        {
            dateTimePicker5.Enabled = radCheckBox14.Checked;
            _statusS = radCheckBox14.Checked;
        }

        private void RadCheckBox15_CheckStateChanged(object sender, EventArgs e)
        {
            dateTimePicker6.Enabled = radCheckBox15.Checked;
            _statusPo = radCheckBox15.Checked;
        }
    }
}