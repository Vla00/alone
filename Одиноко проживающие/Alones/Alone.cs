using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;
using Одиноко_проживающие.Alones;

namespace Одиноко_проживающие
{
    public sealed partial class Alone : RadForm
    {
        #region Переменные
        private bool _status;
        private bool _statusDouble;
        private bool _statusExit;
        private bool _load;
        private bool _loadGrid;
        private bool? _dublicate;
        private int _keyAlone;
        private double _dateR;
        private double _dateRTr;
        private string _date_sm;
        private string _item;
        private string _category_history;
        private bool _loadAlone;
        private bool? _blocked;

        private StructuresAlone _alone;
        private StructuresSojitel _sojitel;
        private StructuresJilUsl _jilUsl;
        private StructuresZemeln _zemeln;
        private BindingSource _bindingSource;
        private string _bindingDopKinder;
        
        private BindingSource _bindingSourceKinder;
        private BindingSource _bindingSourceSurvey;
        private BindingSource _bindingSourceHelp;
        private BindingSource _bindingRelative;
        private BindingList<string> _bindingRelativeComboBox;
        private RadListView _radSpeziolist = new RadListView();
        private BindingList<string> _bindingHelp;
        private StructStartParameters StructStartParameter;
        Alone_history history;

        TelerikMetroTheme theme = new TelerikMetroTheme();
        #endregion
        
        #region Конструкторы
        public Alone(bool status, int keyAlone, string item, bool? blocked, StructuresAlone aloneSearch)
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            RadMessageBox.SetThemeName(theme.ThemeName);

            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            MyRadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            CalendarLocalizationProvider.CurrentProvider = new MyRussionCalendarLocalizationProvider();
            radPageView4.Pages[0].Item.DrawBorder = true;
            radPageView4.Pages[1].Item.DrawBorder = true;

            radPageView2.Pages[0].Item.DrawBorder = true;
            radPageView2.Pages[1].Item.DrawBorder = true;
            radPageView2.Pages[2].Item.DrawBorder = true;
            radPageView2.Pages[3].Item.DrawBorder = true;

            StructStartParameter = new StructStartParameters
            {
                KeyAlone = keyAlone,
                Status = status,
                Item = item, 
                Blocked = blocked
            };

            if(aloneSearch != null)
            {
                if (!string.IsNullOrEmpty(aloneSearch.Family))
                    family_text.Text = aloneSearch.Family;

                if (!string.IsNullOrEmpty(aloneSearch.Name))
                    name_text.Text = aloneSearch.Name;

                if (!string.IsNullOrEmpty(aloneSearch.Surname))
                    surname_text.Text = aloneSearch.Surname;
                date_ro_date.Value = aloneSearch.DateRo ?? DateTime.Now;
            }
        }

        private void AddAlone_Shown(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread _thread = new Thread(new ParameterizedThreadStart(Start))
            {
                IsBackground = true
            };
            Text = @"Загрузка данных...";
            _thread.Start(StructStartParameter);
            
        }

        private void Start(object obj)
        {
            try
            {
                dead_button.Visible = false;
                exit_button.Visible = false;
                BlockedPage(false);
                StructStartParameters structStartParameters = (StructStartParameters)obj;

                _status = structStartParameters.Status;
                _statusExit = structStartParameters.Status;
                _blocked = structStartParameters.Blocked;


                if (!string.IsNullOrEmpty(structStartParameters.Item))
                {
                    switch (structStartParameters.Item.Split(' ')[0])
                    {
                        case "sm":
                            _date_sm = structStartParameters.Item.Split(' ')[1];
                            break;
                        default:
                            _item = structStartParameters.Item.Split(' ')[1];
                            break;
                    }
                }

                _load = true;

                RelativesComboBox();
                UpdateComboBoxSelsovet();
                UpdateComboBoxWoter();
                UpdateComboBoxPlita();
                UpdateComboBoxKanal();
                UpdateComboBoxOtopl();
                UpdateComboBoxSpeziolist();             
                UpdateComboBoxHelp();
                numericUpDown2.Value = DateTime.Now.Year;
                numericUpDown3.Value = DateTime.Now.Year;
                date_sm_date.MaxDate = DateTime.Now;
                dateTimePicker3.MaxDate = DateTime.Now;
                dateTimePicker4.MaxDate = DateTime.Now;

                if (!structStartParameters.Status)
                {
                    _keyAlone = structStartParameters.KeyAlone;
                    _loadAlone = true;

                    AloneLoad();
                    DisabilityLoad();

                    BlockedPage(true);

                    SojitelLoad();
                    KinderLoad();
                    UpdateRelative();
                    KinderOther();

                    JilUslLoad();
                    ZemelnLoad();

                    CategoryLoad();

                    SurveyLoad();
                    HelpLoad();
                    _loadAlone = false;
                    if (_blocked == true)
                    {
                        Blocked(true);
                    }else
                    {
                        if (!string.IsNullOrEmpty(_date_sm))
                        {
                            if (MessageBox.Show("Хотите установить дату смерти: " + _date_sm, "Уточнение", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                sm_check.Checked = true;
                                date_sm_date.Value = Convert.ToDateTime(_date_sm);
                                date_sm_date.Text = _date_sm;
                            }else
                            {
                                _date_sm = null;
                            }
                        }
                    }
                }
                else
                {
                    BlockedPage(true);
                    Text = @"Добавление новой записи (режим создания)";
                    //dead_button.Visible = false;
                    radPageViewPage2.Enabled = false;
                    radPageViewPage3.Enabled = false;
                    radPageViewPage5.Enabled = false;
                    radPageViewPage6.Enabled = false;
                }
                _load = false;
            }
            catch(Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
        }

        private void AddAlone_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_dublicate != null)
            {
                if (_dublicate == true)
                {
                    e.Cancel = false;
                    return;
                }
            }

            if (!_loadGrid)
            {
                if (!string.IsNullOrEmpty(family_text.Text))
                {
                    CategoryHistory();
                    if (_category_history == "0")
                    {
                        if(RadMessageBox.Show("Вы убрали все категории. Закрыть окно? (Данные сохранятся без категории)", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Question) == DialogResult.OK)
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            radPageView1.SelectedPage = radPageViewPage4;
                            e.Cancel = true;
                        }                        
                    }
                    else
                    {
                        if (CategoryLoadCount() <= 0)
                        {
                            if(_alone.DateSm == null)
                            {
                                if (RadMessageBox.Show("Вы не выбрали категорию. При закрытии данные не сохраняться. Выйти без сохранения?", "Ошибка", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
                                {
                                    CommandServer commandServer = new CommandServer();
                                    commandServer.ExecNoReturnServer("AloneDelete", _keyAlone.ToString());
                                    e.Cancel = false;
                                }
                                else
                                {
                                    radPageView1.SelectedPage = radPageViewPage4;
                                    e.Cancel = true;
                                }
                            }
                        }
                    }
                }

                if (radPageView1.SelectedPage == radPageViewPage1)
                {
                    //добавление
                    if (_status) { }
                    else
                    {
                        string error = null;

                        if (!CompareAlone())
                        {
                            if (RadMessageBox.Show("Сохранить измененные данные?", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
                            {
                                if (string.IsNullOrEmpty(family_text.Text))
                                {
                                    error = "Не заполнено ФИО." + Environment.NewLine;
                                }

                                if (string.IsNullOrEmpty(error))
                                {
                                    string[] operation = AloneEdit(false);
                                    AlertOperation(operation);
                                    if (operation[1] != "1")
                                        e.Cancel = true;
                                    e.Cancel = false;
                                }
                                else
                                {
                                    RadMessageBox.Show("Заполните обязательные поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, error);
                                    e.Cancel = true;
                                }
                            }
                            else
                            {
                                e.Cancel = false;

                            }
                        }
                    }
                }
                else
                {
                    if (radPageView1.SelectedPage == radPageViewPage2)
                    {
                        if (radPageView2.SelectedPage == radPageViewPage7)
                        {
                            if (_status)
                            {

                            }
                            else
                            {
                                if (_sojitel != null)
                                {
                                    if (!CompareSojitel())
                                    {
                                        if (RadMessageBox.Show("Сохранить измененные данные?", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
                                        {
                                            string[] operation = SojitelEdit();
                                            AlertOperation(operation);
                                            if (operation[1] != "1")
                                                e.Cancel = true;
                                            e.Cancel = false;
                                        }
                                        else
                                        {
                                            e.Cancel = false;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (radPageView1.SelectedPage == radPageViewPage3)
                        {
                            if (_status)
                            {

                            }
                            else
                            {
                                if (_sojitel != null)
                                {
                                    if (!CompareJilUsl() || !CompareZemeln())
                                    {
                                        if (RadMessageBox.Show("Сохранить измененные данные?", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
                                        {
                                            if (!CompareJilUsl())
                                            {
                                                string[] operation = JilUslEdit();
                                                AlertOperation(operation);
                                                if (operation[1] != "1")
                                                    e.Cancel = true;
                                                e.Cancel = false;
                                            }

                                            if (!CompareZemeln())
                                            {
                                                string[] operation = ZemelnEdit();
                                                AlertOperation(operation);
                                                if (operation[1] != "1")
                                                    e.Cancel = true;
                                                e.Cancel = false;
                                            }
                                        }
                                        else
                                        {
                                            e.Cancel = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (e.Cancel == false)
            {
                if(history != null && !history.IsDisposed && history.Visible)
                {
                    history.Close();
                }
                Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        #endregion

        #region Сведения
        private string[] AddAloneSql()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            var parameters = ParameterAlone(true);
            var returnSqlServer = commandServer.ExecReturnServer("Alone_add", parameters);

            try
            {
                if (returnSqlServer[0].Split(' ')[0] == "Успешно")
                {
                    _keyAlone = Convert.ToInt32(returnSqlServer[0].Split(' ')[1]);
                    returnSqlServer[0] = "Запись успешно добавлена.";
                    this.Text += "Дело №" + _keyAlone.ToString();
                }

                _alone = new StructuresAlone()
                {
                    Family = family_text.Text,
                    Name = name_text.Text,
                    Surname = surname_text.Text,
                    DateRo = date_ro_date.Value,
                    Country = nas_punkt_combo.Text,
                    Dop = dop_text.Text,
                    Street = street_text.Text,
                    Phone = phone_text.Text,
                    PlaceWork = rab_text.Text
                };

                if (pol_m_che.Checked)
                    _alone.Pol = 1;

                if (sm_check.Checked)
                    _alone.DateSm = date_sm_date.Value;

                if (exit_check.Checked)
                    _alone.DateExit = date_exit_date.Value;

                if (returnSqlServer[0] == "Запись успешно добавлена.")
                {
                    AddSojitel();
                    AddDopKinder();
                    AddJilUsl();
                    AddZemeln();

                    _status = false;
                    _statusDouble = true;

                    KinderLoad();
                    UpdateRelative();
                    SurveyLoad();
                    HelpLoad();

                    if (pol_m_che.Checked)
                        _dateRTr = Convert.ToDouble(new CommandServer().ComboBoxList(@"select name
                            from every_set
                            where tabl = 'order_m_vozr'", false)[0]);
                    else
                        _dateRTr = Convert.ToDouble(new CommandServer().ComboBoxList(@"select name
                            from every_set
                            where tabl = 'order_j_vozr'", false)[0]);

                    _dateR = Convert.ToDouble(new CommandServer().ComboBoxList("select str(round(CONVERT(FLOAT, getdate() - '" + 
                        date_ro_date.Value.ToString("dd.MM.yyyy") + "')/365.25, 1), 6, 2)", false)[0].Replace('.', ','));

                    return returnSqlServer;
                }

                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }
        
        private void AloneLoad()
        {
            Text = @"Дело №" + _keyAlone + " (режим редактирования)";
            _alone = new StructuresAlone();
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from AloneGet(" + _keyAlone + ")").Tables[0];

            if (dt.Rows.Count == 0)
            {
                RadMessageBox.Show("Данное дело не найдено.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                Close();
            }
            _alone.Family = dt.Rows[0].ItemArray[0].ToString();
            _alone.Name = dt.Rows[0].ItemArray[1].ToString();
            _alone.Surname = dt.Rows[0].ItemArray[2].ToString();
            if (Convert.ToBoolean(dt.Rows[0].ItemArray[3].ToString()))
            {
                pol_m_che.Checked = true;
                _alone.Pol = 1;
            }else
                pol_j_che.Checked = true;

            _alone.DateRo = Convert.ToDateTime(dt.Rows[0].ItemArray[4].ToString());

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[5].ToString()))
            {
                _alone.DateSm = Convert.ToDateTime(dt.Rows[0].ItemArray[5].ToString());
                date_sm_date.Value = _alone.DateSm;
                dead_button.Visible = true;

                if(_blocked == null)
                    _blocked = true;
                sm_check.Checked = true;
            }

            selsovet_combo.SelectedIndex = selsovet_combo.FindStringExact(dt.Rows[0].ItemArray[6].ToString());
            _alone.Country = dt.Rows[0].ItemArray[7].ToString();
            _alone.TypeUl = dt.Rows[0].ItemArray[8].ToString();
            _alone.Street = dt.Rows[0].ItemArray[9].ToString();
            _alone.House = dt.Rows[0].ItemArray[10].ToString();
            _alone.Apartament = dt.Rows[0].ItemArray[11].ToString();
            _alone.Housing = dt.Rows[0].ItemArray[12].ToString();
            _alone.Phone = dt.Rows[0].ItemArray[13].ToString();
            _alone.PlaceWork = dt.Rows[0].ItemArray[14].ToString();
            _alone.Dop = dt.Rows[0].ItemArray[15].ToString();

            family_text.Text = _alone.Family;
            name_text.Text = _alone.Name;
            surname_text.Text = _alone.Surname;
            date_ro_date.Value = _alone.DateRo ?? DateTime.Now;            
            nas_punkt_combo.SelectedIndex = nas_punkt_combo.FindStringExact(_alone.Country);
            type_street_combo.SelectedIndex = type_street_combo.FindStringExact(_alone.TypeUl);
            street_text.Text = _alone.Street;
            house_text.Text = _alone.House;
            apartament_text.Text = _alone.Apartament;
            housing_text.Text = _alone.Housing;
            phone_text.Text = _alone.Phone;
            rab_text.Text = _alone.PlaceWork;
            dop_text.Text =_alone.Dop;
            
            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[16].ToString()))
            {
                _alone.DateExit = Convert.ToDateTime(dt.Rows[0].ItemArray[16].ToString());
                date_exit_date.Value = _alone.DateExit;
                exit_button.Visible = true;
                if (_blocked == null)
                    _blocked = true;
                exit_check.Checked = true;
            }

            if(!string.IsNullOrEmpty(dt.Rows[0].ItemArray[17].ToString()))
            {
                label8.Text = "Дата добавления: " + dt.Rows[0].ItemArray[17].ToString().Split(' ')[0] + ". Пользователь: " + dt.Rows[0].ItemArray[18].ToString();
            }

            if (_blocked == true)
            {
                Text = @"Дело №" + _keyAlone + " (режим просмотра)";
            }

            _dublicate = null;
        }

        private string[] AloneEdit(bool revival)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            var parameters = _keyAlone + "," + ParameterAlone(false);
            if (!revival)
                parameters += ",0";
            else
                parameters += ",1";

            var returnSqlServer = commandServer.ExecReturnServer("Alone_edit", parameters);
            
            if (returnSqlServer[1] != "1") return returnSqlServer;
            try
            {
                _alone = new StructuresAlone();

                if (pol_m_che.Checked)
                    _alone.Pol = 1;

                if (sm_check.Checked)
                    _alone.DateSm = date_sm_date.Value;

                if (exit_check.Checked)
                    _alone.DateExit = date_exit_date.Value;

                if (revival)
                    _alone.DateSm = new DateTime();

                _alone.Family = family_text.Text;
                _alone.Name = name_text.Text;
                _alone.Surname = surname_text.Text;
                _alone.Country = nas_punkt_combo.Text;
                _alone.DateRo = date_ro_date.Value;
                _alone.Street = street_text.Text;
                _alone.Phone = phone_text.Text;
                _alone.Dop = dop_text.Text;

                _alone.TypeUl = type_street_combo.Text;
                _alone.Street = street_text.Text;
                _alone.House = house_text.Text;
                _alone.Apartament = apartament_text.Text;
                _alone.Housing = housing_text.Text;
                _alone.PlaceWork = rab_text.Text;

                if (returnSqlServer[0] == "Запись успешно изменена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }

        private string ParameterAlone(bool operation)
        {
            var parameters = "'" + family_text.Text + "','" + name_text.Text + "','" + surname_text.Text + "',";

            if (pol_m_che.Checked)
                parameters += 1;
            else
                parameters += 0;

            parameters += ",'" + date_ro_date.Text + "',";

            if (sm_check.Checked)
            {
                if(string.IsNullOrEmpty(_date_sm))
                    parameters += "'" + date_sm_date.Text + "'";
                else
                    parameters += "'" + _date_sm + "'";
            }
            else
                parameters += "null";

            parameters += ",'" + nas_punkt_combo.Text + "','" + street_text.Text + "',";

            if (string.IsNullOrEmpty(house_text.Text))
            {
                parameters += "null,";
            }
            else
            {
                parameters += "'" + house_text.Text + "',";
            }


            if (string.IsNullOrEmpty(housing_text.Text))
            {
                parameters += "null,";
            }
            else
            {
                parameters += "'" + housing_text.Text + "',";
            }

            if (string.IsNullOrEmpty(apartament_text.Text))
            {
                parameters += "null";
            }
            else
            {
                parameters += "'" + apartament_text.Text + "'";
            }

            parameters += ",'" + type_street_combo.Text + "','" + phone_text.Text + "','" + rab_text.Text + "',";

            if (string.IsNullOrEmpty(dop_text.Text))
                parameters += "null,";
            else
                parameters += "'" + dop_text.Text + "',";

            if (exit_check.Checked)
                parameters += "'" + date_exit_date.Text + "'";
            else
                parameters += "null";

            if (operation)
                parameters += ",'" + Одиноко_проживающие.Load.ProgramLoad._confConnection.User + "'";

            return parameters;
        }

        private void UpdateComboBoxSelsovet()
        {
            var commandServer = new CommandServer();
            selsovet_combo.Invoke(new MethodInvoker(delegate ()
            {
                selsovet_combo.DataSource = commandServer.ComboBoxList(@"select Selsovet from Selsovet order by Selsovet", true);
            }));
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxNasPunct();
        }

        private void UpdateComboBoxNasPunct()
        {
            var commandServer = new CommandServer();
            nas_punkt_combo.DataSource = commandServer.ComboBoxList(@"select country
					from country inner join selsovet on 
						country.fk_selsovet = selsovet.key_selsovet
					where selsovet.selsovet = " + "'" + selsovet_combo.Text + "'", true);
        }

        private bool CheackCopyAlone()
        {
            var command = @"select * from AloneCopy('" + family_text.Text + "','" + name_text.Text + "','" + surname_text.Text + "','" 
                + date_ro_date.Text + "')";
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource
            {
                DataSource = commandServer.DataGridSet(command).Tables[0]
            };
            return _bindingSource.Count > 0;
        }

        private void TextBox1_Validated(object sender, EventArgs e)
        {
            TextBox text = (TextBox)sender;
            
            if(!string.IsNullOrEmpty(text.Text))
            {
                text.Text = text.Text.Substring(0, 1).ToUpper() + text.Text.Substring(1, text.Text.Length - 1).ToLower();
                text.Text = text.Text.Trim();
                sender = (object)text;
            }
        }

        private void CheckBox26_CheckStateChanging(object sender, CheckStateChangingEventArgs args)
        {
            RadCheckBox cheack = sender as RadCheckBox;

            if (_blocked == null)
            {
                if (cheack != null)
                {
                    if (cheack.Text == "Дата смерти")
                    {
                        if(!string.IsNullOrEmpty(_date_sm))
                        {
                            date_sm_date.Value = Convert.ToDateTime(_date_sm);
                            date_sm_date.Text = _date_sm;
                        }
                            
                        new search.Result(family_text.Text, name_sojitel_text.Text, surname_sojitel_text.Text, null, "ListSojitel", false, nas_punkt_combo.Text).ShowDialog();
                    }
                }
            }
        }

        private void History_exit_button_Click(object sender, EventArgs e)
        {
            Alone_history history = new Alone_history(_keyAlone);
            history.ShowDialog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Exit_button_Click(object sender, EventArgs e)
        {
            if (RadMessageBox.Show("Вы подтверждаете возврат?", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
            {
                CommandServer commandServer = new CommandServer();
                var returnSqlServer = commandServer.ExecReturnServer("Alone_exit_update", _keyAlone.ToString());

                if (returnSqlServer[0] == "Запись успешно изменена.")
                {
                    Blocked(false);
                    exit_check.Checked = false;
                    date_sm_date.Enabled = false;
                    AlertOperation(new string[] { "Запись успешно изменена", "1" });
                    RadMessageBox.Show("Установите категории.", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Info);
                }
                else
                {
                    CommandClient commandClient = new CommandClient();
                    commandClient.WriteFileError(null, "Alone_exit_update" + _keyAlone.ToString() + " " + returnSqlServer[0]);
                }
            }
        }

        private void Disability_Click(object sender, EventArgs e)
        {
            Disability dis = new Disability(_keyAlone.ToString());
            dis.ShowDialog();
            if (dis.Status)
            {
                DisabilityLoad();
            }
        }

        private void DisabilityLoad()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from Disability_get_table(" + _keyAlone + ") order by [Дата инв.] desc").Tables[0];

            if (dt.Rows.Count == 0)
                return;
            label26.Text = "Степ. утраты здоровья: " + dt.Rows[0].ItemArray[1].ToString();
            label28.Text = "Дата инвалидности: " + dt.Rows[0].ItemArray[2].ToString().Split(' ')[0];
            label29.Text = "Дата переосв.: " + dt.Rows[0].ItemArray[3].ToString().Split(' ')[0];
            label27.Text = "Диагноз: " + dt.Rows[0].ItemArray[4].ToString();
        }
        #endregion

        #region Члены семьи

        #region Супруг(а)
        private string[] AddSojitel()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            var structure = new StructuresSojitel
            {
                Family = family_sojitel_text.Text,
                Name = name_sojitel_text.Text,
                Surname = surname_sojitel_text.Text
            };

            var parameters = _keyAlone + ",'" + family_sojitel_text.Text + "','" + name_sojitel_text.Text + "','" + surname_sojitel_text.Text + "',";

            if (radioButton4.Checked)
            {
                parameters += 1;
                structure.Pol = 1;
            }
            else
            {
                parameters += 0;
                structure.Pol = 0;
            }

            parameters += ",'" + textBox4.Text + "'";
            structure.Dop = textBox4.Text;

            if (checkBox28.Checked)
            {
                parameters += ",'" + dateTimePicker4.Text + "'";
                structure.DateRo = dateTimePicker4.Value;
            }
            else
                parameters += ",null";

            if (checkBox27.Checked)
            {
                parameters += ",'" + dateTimePicker3.Text + "'";
                structure.DateSm = dateTimePicker3.Value;
            }
            else
                parameters += ",null";

            var returnSqlServer = commandServer.ExecReturnServer("Sojitel_add", parameters);

            try
            {
                _sojitel = structure;

                if (returnSqlServer[0] == "Запись успешно добавлена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }

        private void SojitelLoad()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from SojitelGet(" + _keyAlone + ")").Tables[0];

            if (dt.Rows.Count <= 0)
            {
                radButton1.Enabled = false;
                return;
            }
            radButton1.Enabled = true;
            _sojitel = new StructuresSojitel();

            family_sojitel_text.Text = dt.Rows[0].ItemArray[0].ToString();
            _sojitel.Family = family_sojitel_text.Text;

            name_sojitel_text.Text = dt.Rows[0].ItemArray[1].ToString();
            _sojitel.Name = name_sojitel_text.Text;

            surname_sojitel_text.Text = dt.Rows[0].ItemArray[2].ToString();
            _sojitel.Surname = surname_sojitel_text.Text;

            if (Convert.ToBoolean(dt.Rows[0].ItemArray[3].ToString()))
            {
                radioButton4.Checked = true;
                _sojitel.Pol = 1;
            }
            else
            {
                radioButton3.Checked = true;
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[4].ToString()))
            {
                dateTimePicker4.Value = Convert.ToDateTime(dt.Rows[0].ItemArray[4].ToString());
                checkBox28.Checked = true;
                _sojitel.DateRo = Convert.ToDateTime(dt.Rows[0].ItemArray[4].ToString());
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[5].ToString()))
            {
                dateTimePicker3.Value = Convert.ToDateTime(dt.Rows[0].ItemArray[5].ToString());
                checkBox27.Checked = true;
                _sojitel.DateSm = Convert.ToDateTime(dt.Rows[0].ItemArray[5].ToString());
            }

            textBox4.Text = dt.Rows[0].ItemArray[6].ToString();
            _sojitel.Dop = textBox4.Text;
        }

        private string[] SojitelEdit()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            var parameters = _keyAlone + ",'" + family_sojitel_text.Text + "','" + name_sojitel_text.Text + "','" + surname_sojitel_text.Text + "',";

            if (radioButton4.Checked)
                parameters += 1;
            else
                parameters += 0;

            parameters += ",'" + textBox4.Text + "'";

            if (checkBox28.Checked)
                parameters += ",'" + dateTimePicker4.Text + "'";
            else
                parameters += ",null";

            if (checkBox27.Checked)
                parameters += ",'" + dateTimePicker3.Text + "'";
            else
                parameters += ",null";

            var returnSqlServer = commandServer.ExecReturnServer("Sojitel_edit", parameters);

            if (returnSqlServer[1] != "Успешно") return returnSqlServer;
            try
            {
                _sojitel = new StructuresSojitel
                {
                    Family = family_sojitel_text.Text,
                    Name = name_sojitel_text.Text,
                    Surname = surname_sojitel_text.Text,
                    Dop = textBox4.Text
                };

                if (radioButton4.Checked)
                    _sojitel.Pol = 1;
                else
                    _sojitel.Pol = 0;

                if (checkBox28.Checked)
                    _sojitel.DateRo = dateTimePicker4.Value;

                if (checkBox27.Checked)
                    _sojitel.DateSm = dateTimePicker3.Value;

                if (returnSqlServer[0] == "Запись успешно изменена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            var serverCommand = new CommandServer();

            if (RadMessageBox.Show(@"Вы точно хотите удалить сожителя?", @"Подтверждение", MessageBoxButtons.OKCancel,
                    RadMessageIcon.Question) != DialogResult.OK) return;

            serverCommand.ExecNoReturnServer("Sojitel_delete", _keyAlone.ToString());

            AlertOperation(new string[] { "Запись успешно удалена", "1" });

            family_sojitel_text.Text = "";
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            checkBox28.Checked = false;
            checkBox27.Checked = false;
            textBox4.Text = "";
            radButton1.Enabled = false;
        }

        private void TextBox6_Validated(object sender, EventArgs e)
        {
            string[] s = family_sojitel_text.Text.Split(' ');
            family_sojitel_text.Text = "";

            for (int i = 0; i < s.Count(); i++)
            {
                if(!string.IsNullOrEmpty(s[i]))
                {
                    family_sojitel_text.Text += s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
                    if (i != s.Count() - 1)
                    {
                        if(!string.IsNullOrEmpty(s[i + 1]))
                            family_sojitel_text.Text += " ";
                    }
                }
            }
        }
        #endregion

        #region Родственики
        private void UpdateRelative()
        {
            _loadGrid = true;
            var commandServer = new CommandServer();
            _bindingRelative = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from ListRelative(" + _keyAlone + ")").Tables[0] };

            radGridView4.Invoke(new MethodInvoker(delegate ()
            {
                radGridView4.Refresh();
                radGridView4.DataSource = _bindingRelative;
                radGridView4.AutoSizeRows = true;                
                radGridView4.Columns[0].IsVisible = false;
                radGridView4.Columns[1].WrapText = true;
                radGridView4.Columns[2].WrapText = true;
                
                radGridView4.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                radGridView4.Columns[1].Width = 100;
                radGridView4.Columns[3].Width = 30;

                GridViewComboBoxColumn comboColumn_operation = new GridViewComboBoxColumn("родствен. отн.")
                {
                    DataSource = _bindingRelativeComboBox,
                    Name = "relatives_eve"
                };
                radGridView4.Columns[3] = comboColumn_operation;
                comboColumn_operation.FieldName = "relatives_ever";
            }));
            _loadGrid = false;
        }

        private void RelativesComboBox()
        {
            var commandServer = new CommandServer();
            _bindingRelativeComboBox = new BindingList<string>(commandServer.ComboBoxList(@"select * from Relatives_relationships()", true));
        }

        private void RadGridView4_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();
            var line = e.Rows[0].Cells[1].Value.ToString();

            if (e.Rows[0].Cells[1].Value != null && e.Rows[0].Cells[2].Value != null)
            {
                var parameters = _keyAlone + ",'" + e.Rows[0].Cells[1].Value.ToString() + "','" + e.Rows[0].Cells[2].Value.ToString() + "',";

                if(e.Rows[0].Cells[3].Value != null)
                {
                    parameters += "'" + e.Rows[0].Cells[3].Value.ToString() + "'";
                }else
                {
                    parameters += "null";
                }

                var returnSqlServer = commandServer.ExecReturnServer("Relatives_add", parameters);
                AlertGridOperation(sender, null, e, "Relatives_add " + parameters, returnSqlServer);
            }
            else
            {
                RadMessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void RadGridView4_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingRelative.DataSource = commandServer.DataGridSet(@"select * from ListRelative(" + _keyAlone + ")").Tables[0];
            radGridView4.Invoke(new MethodInvoker(delegate ()
            {
                radGridView4.DataSource = _bindingRelative;
            }));
        }

        private void RadGridView4_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                bool flag = false;
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    var parameters = line.Cells[0].Value.ToString() + ",'";
                    if (e.PropertyName == "ФИО")
                    {
                        string text = null;

                        flag = true;
                        if (e.NewValue.ToString().Split(' ').Length == 3)
                        {
                            text = CharTo(e.NewValue.ToString().Split(' ')[0]) + " " +
                                CharTo(e.NewValue.ToString().Split(' ')[1]) + " " +
                                CharTo(e.NewValue.ToString().Split(' ')[2]);
                        }
                        else
                            text = e.NewValue.ToString();
                        parameters += text + "',";
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[1].Value.ToString()))
                            parameters += "null,";
                        else
                            parameters += line.Cells[1].Value.ToString() + "',";
                    }

                    if (e.PropertyName == "Доп. данные")
                    {
                        flag = true;
                        parameters += "'" + e.NewValue.ToString() + "',";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[2].Value.ToString()))
                            parameters += "null,";
                        else
                            parameters += "'" + line.Cells[2].Value.ToString() + "',";
                    }

                    if(e.PropertyName == "relatives_ever")
                    {
                        flag = true;
                        parameters += "'" + e.NewValue.ToString() + "'";
                    }else
                    {
                        if (string.IsNullOrEmpty(line.Cells[3].Value.ToString()))
                            parameters += "null";
                        else
                            parameters += "'" + line.Cells[3].Value.ToString() + "'";
                    }

                    if (flag)
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("Relatives_edit", parameters);
                        AlertGridOperation(sender, e, null, "Relatives_edit" + line.Cells[1].Value, returnSqlServer);
                    }
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (RadMessageBox.Show("Вы точно хотите удалить запись?", "Подтверждение", MessageBoxButtons.OKCancel, RadMessageIcon.Exclamation) == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    var line = (GridViewRowInfo)e.NewItems[0];
                    if (!string.IsNullOrEmpty(line.Cells[0].Value.ToString()))
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("Relatives_delete", line.Cells[0].Value.ToString());
                        AlertGridOperation(sender, e, null, "Relatives_delete" + line.Cells[0].Value, returnSqlServer);
                    }
                }
            }
        }
        #endregion

        #region Дети
        private void KinderLoad()
        {
            _loadGrid = true;
            var commandServer = new CommandServer();
            _bindingSourceKinder = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from GetKinder(" + _keyAlone + ")").Tables[0] };
            radGridView1.Invoke(new MethodInvoker(delegate ()
            {
                radGridView1.Refresh();
                radGridView1.AutoSizeRows = true;
                radGridView1.DataSource = _bindingSourceKinder;
                radGridView1.Columns[0].IsVisible = false;
                radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                radGridView1.Columns[1].WrapText = true;
                radGridView1.Columns[3].WrapText = true;
                radGridView1.Columns[3].BestFit();
                radGridView1.Columns[4].WrapText = true;
                radGridView1.Columns[4].BestFit();
            }));
            _loadGrid = false;
                        
            radGridViewSurvey.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewSurvey.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }));
            _loadGrid = false;
        }

        private void RadGridView1_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (e.Rows[0].Cells[1].Value == null || e.Rows[0].Cells[1].Value.ToString() == "")
            {
                AlertOperation(new string[] { "Не указано ФИО.", "1" });
                e.Cancel = true;
                return;
            }
            var commandServer = new CommandServer();
            string fio;
            var line = e.Rows[0].Cells[1].Value.ToString();

            if (e.Rows[0].Cells[1].Value.ToString().Split(' ').Length == 3)
            {
                fio = CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[0]) + " " +
                    CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[1]) + " " +
                    CharTo(e.Rows[0].Cells[1].Value.ToString().Split(' ')[2]);
            }
            else
                fio = e.Rows[0].Cells[1].Value.ToString();

            var parameters = _keyAlone + ",'" + fio + "',";
            if (e.Rows[0].Cells[2].Value == null)
                parameters += "null";
            else
                parameters += "'" + e.Rows[0].Cells[2].Value.ToString() + "'";

            if (e.Rows[0].Cells[3].Value == null)
                parameters += ",null";
            else
                parameters += ",'" + e.Rows[0].Cells[3].Value.ToString() + "'";

            if (e.Rows[0].Cells[4].Value == null)
                parameters += ",null";
            else
                parameters += ",'" + e.Rows[0].Cells[4].Value.ToString() + "'";

            var returnSqlServer = commandServer.ExecReturnServer("kinder_add", parameters);
            AlertGridOperation(sender, null, e, "kinder_add " + parameters, returnSqlServer);
        }

        private void RadGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSourceKinder = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from GetKinder(" + _keyAlone + ")").Tables[0] };

            radGridView1.Invoke(new MethodInvoker(delegate ()
            {
                radGridView1.DataSource = _bindingSourceKinder.DataSource;
            }));
        }

        private void RadGridView1_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                bool flag = false;
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    var parameters = line.Cells[0].Value.ToString() + ",'";

                    if (e.PropertyName == "ФИО")
                    {
                        string text = null;
                        if (e.OldValue != e.NewValue)
                        {
                            flag = true;
                            if (e.NewValue.ToString().Split(' ').Length == 3)
                            {
                                text = CharTo(e.NewValue.ToString().Split(' ')[0]) + " " +
                                    CharTo(e.NewValue.ToString().Split(' ')[1]) + " " +
                                    CharTo(e.NewValue.ToString().Split(' ')[2]);
                            }
                            else
                                text = e.NewValue.ToString();
                            parameters += text + "',";
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        parameters += line.Cells[1].Value.ToString() + "',";
                    }

                    if (e.PropertyName == "д.р.")
                    {
                        flag = true;
                        parameters += e.NewValue.ToString();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[2].Value.ToString()))
                            parameters += "null";
                        else
                            parameters += line.Cells[2].Value.ToString();
                    }

                    if (e.PropertyName == "Место работы")
                    {
                        flag = true;
                        parameters += ",'" + e.NewValue.ToString() + "',";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[3].Value.ToString()))
                            parameters += ",null,";
                        else
                            parameters += ",'" + line.Cells[3].Value.ToString() + "',";
                    }

                    if (e.PropertyName == "Адрес")
                    {
                        flag = true;
                        parameters += "'" + e.NewValue.ToString() + "'";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[4].Value.ToString()))
                            parameters += "null";
                        else
                            parameters += "'" + line.Cells[4].Value.ToString() + "'";
                    }

                    if (flag)
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("kinder_edit", parameters);
                        AlertGridOperation(sender, e, null, "kinder_edit" + line.Cells[1].Value, returnSqlServer);
                    }
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (RadMessageBox.Show("Вы точно хотите удалить запись?", "Подтверждение", MessageBoxButtons.OKCancel, RadMessageIcon.Exclamation) == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    var line = (GridViewRowInfo)e.NewItems[0];
                    if (!string.IsNullOrEmpty(line.Cells[0].Value.ToString()))
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("kinder_delete", line.Cells[0].Value.ToString());
                        AlertGridOperation(sender, e, null, "kinder_delete" + line.Cells[0].Value, returnSqlServer);
                    }
                }
            }
        }
        #endregion

        #region Прочее
        private string[] AddDopKinder()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            var returnSqlServer = commandServer.ExecReturnServer("EditDopKinder", _keyAlone + ",'" + textBox11.Text + "'");

            //if (returnSqlServer[1] != "Успешно") return returnSqlServer;
            try
            {
                _bindingDopKinder = textBox11.Text;
                if (returnSqlServer[0] == "Запись успешно изменена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, textBox11.Text + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, textBox11.Text);
                return returnSqlServer;
            }
        }

        private void KinderOther()
        {
            var command = @"select alone.dopKinder
					from alone
					where alone.key_alone = " + _keyAlone;
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(command).Tables[0];

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[0].ToString()))
            {
                textBox11.Text = dt.Rows[0].ItemArray[0].ToString();
            }
        }
        #endregion
        #endregion  

        #region Категории
        private void CategoryLoad()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from GetListCategory(" + _keyAlone + ")").Tables[0];
            if (dt.Rows.Count <= 0) return;

            foreach (Control control in radPageViewPage4.Controls)
            {
                CategoryChecked(dt, control);
            }
        }

        private void CategoryHistory()
        {
            var commandServer = new CommandServer();
            _category_history = commandServer.DataGridSet(@"select count(*)
                from category_time
                where fk_alone = " + _keyAlone).Tables[0].Rows[0].ItemArray[0].ToString();
        }

        private static void CategoryChecked(DataTable table, Control control)
        {
            foreach (Control c in control.Controls)
            {
                CategoryChecked(table, c);
            }


            if (!(control is RadCheckBox cb))
            {
                if (!(control is RadRadioButton cb2)) return;

                var controlText = cb2.Text;

                var findRows = table.Select("category = '" + controlText + "'");
                if (findRows.Length > 0)
                    cb2.IsChecked = true;
            }
            else
            {
                var controlText = cb.Text;

                var findRows = table.Select("category = '" + controlText + "'");
                if (findRows.Length > 0)
                    cb.Checked = true;
            }
        }

        private void RadCheckBox35_Click(object sender, EventArgs e)
        {
            if (_loadAlone)
            {

                if (!(sender is RadRadioButton radioButton))
                    return;

                if (radioButton.Text == "Пенсионер")
                {
                    radGroupBox3.Enabled = true;
                    return;
                }else
                {
                    if(radioButton.Text == "Не пенсионер")
                    {
                        radGroupBox3.Enabled = false;
                        return;
                    }
                }
            }else
            {
                var checkBox = sender as RadCheckBox;
                var radioButton = sender as RadRadioButton;
                string text;

                if (checkBox == null)
                {
                    if (radioButton == null)
                    {
                        return;
                    }
                    else
                    {
                        text = radioButton.Text;
                    }
                }
                else
                {
                    text = checkBox.Text;
                }

                var commandServer = new CommandServer();
                string[] result;

                if ((checkBox != null && checkBox.Checked) || (radioButton != null && radioButton.IsChecked))
                {
                    result = commandServer.ExecReturnServer("Category_add", _keyAlone + ",'" + text + "','user'");
                }
                else
                {
                    result = commandServer.ExecReturnServer("Category_delete", _keyAlone + ",'" + text + "'");
                }

                AlertGridOperation(sender, null, null, "category", result);
                radPageViewPage2.Enabled = true;
                radPageViewPage3.Enabled = true;
                radPageViewPage5.Enabled = true;
                radPageViewPage6.Enabled = true;
            }
        }

        private void RadCheckBox28_MouseClick_1(object sender, MouseEventArgs e)
        {
            var radioButton = sender as RadRadioButton;
            if (e.Button == MouseButtons.Right)
            {
                radioButton.IsChecked = false;

                //var commandServer = new CommandServer();
                //string[] result = commandServer.ExecReturnServer("Category_delete", _keyAlone + ",'" + radioButton.Text + "'");
                //AlertGridOperation(sender, null, null, "category", result);

            }
        }

        public int CategoryLoadCount()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from GetListCategory(" + _keyAlone + ")").Tables[0];
            return dt.Rows.Count;
        }
        #endregion

        #region Обследование
        private void SurveyLoad()
        {
            _loadGrid = true;
            var commandServer = new CommandServer();
            _bindingSourceSurvey = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from AloneSurvey(" + _keyAlone + ") order by date_obsl desc").Tables[0] };

            radGridViewSurvey.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewSurvey.Refresh();
                radGridViewSurvey.AutoSizeRows = true;
                radGridViewSurvey.DataSource = _bindingSourceSurvey;

                GridViewDateTimeColumn dateSurvey = new GridViewDateTimeColumn("Дата");
                radGridViewSurvey.Columns[2] = dateSurvey;
                dateSurvey.Name = "date_survey";
                dateSurvey.FieldName = "date_obsl";
                dateSurvey.FormatString = "{0:dd/MM/yyyy}";
                dateSurvey.Format = DateTimePickerFormat.Custom;
                dateSurvey.CustomFormat = "dd.MM.yyyy";

                GridViewComboBoxColumn comboColumn = new GridViewComboBoxColumn("Специалист")
                {
                    FieldName = "ФИО",
                    AutoCompleteMode = AutoCompleteMode.Append
                };
                radGridViewSurvey.Columns[1] = comboColumn;

                radGridViewSurvey.Columns[0].IsVisible = false;
                radGridViewSurvey.Columns[3].WrapText = true;
                radGridViewSurvey.Columns[1].WrapText = true;
                radGridViewSurvey.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //radGridViewSurvey.CellEditorInitialized += new GridViewCellEventHandler(radGridView_CellEditorInitialized);
            }));
            _loadGrid = false;
        }
        private void RadGridView2_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            bool error = false;

            if(e.Rows[0].Cells[1].Value == null)
            {
                AlertOperation(new string[] { "Не указан специалист", "0" });
                e.Cancel = true;
                var cell = radGridViewSurvey.MasterView.TableAddNewRow.Cells[1];
                cell.Style.BackColor = Color.Tomato;
                cell.Style.CustomizeFill = true;
                error = true;
            }else
            {
                radGridViewSurvey.MasterView.TableAddNewRow.Cells[1].Style.CustomizeFill = false;
            }

            if (e.Rows[0].Cells[2].Value == null)
            {
                AlertOperation(new string[] { "Не указана дата обследования", "0" });
                e.Cancel = true;
                var cell = radGridViewSurvey.MasterView.TableAddNewRow.Cells[2];
                cell.Style.BackColor = Color.Tomato;
                cell.Style.CustomizeFill = true;
                error = true;
            }
            else
            {
                radGridViewSurvey.MasterView.TableAddNewRow.Cells[2].Style.CustomizeFill = false;
            }

            if (e.Rows[0].Cells[3].Value == null)
            {
                AlertOperation(new string[] { "Не указан результат обследования", "0" });
                e.Cancel = true;
                var cell = radGridViewSurvey.MasterView.TableAddNewRow.Cells[3];
                cell.Style.BackColor = Color.Tomato;
                cell.Style.CustomizeFill = true;
                error = true;
            }
            else
            {
                radGridViewSurvey.MasterView.TableAddNewRow.Cells[3].Style.CustomizeFill = false;
            }

            if (error)
                return;

            var commandServer = new CommandServer();
            var parameters = _keyAlone + ",'" + e.Rows[0].Cells[1].Value.ToString() + "','" + e.Rows[0].Cells[2].Value.ToString() + "','" + e.Rows[0].Cells[3].Value.ToString() + "'";
            var returnSqlServer = commandServer.ExecReturnServer("addSurvey", parameters);
            AlertGridOperation(sender, null, e, "addSurvey " + parameters, returnSqlServer);
        }
        private void RadGridView2_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSourceSurvey = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from AloneSurvey(" + _keyAlone + ")").Tables[0] };

            radGridViewSurvey.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewSurvey.DataSource = _bindingSourceSurvey;
            }));
        }
        private void RadGridView2_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                bool flag = false;
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    var parameters = line.Cells[0].Value.ToString() + ",";

                    if (e.PropertyName == "ФИО")
                    {
                        flag = true;
                        parameters += "'" + e.NewValue.ToString() + "',";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[1].Value.ToString()))
                            parameters += "null,";
                        else
                            parameters += "'" + line.Cells[1].Value.ToString() + "',";
                    }

                    if (e.PropertyName == "Дата" || e.PropertyName == "date_obsl")
                    {
                        flag = true;
                        parameters += "'" + e.NewValue.ToString() + "',";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[2].Value.ToString()))
                            parameters += "null,";
                        else
                            parameters += "'" + line.Cells[2].Value.ToString() + "',";
                    }

                    if (e.PropertyName == "Результат")
                    {
                        flag = true;
                        parameters += "'" + e.NewValue.ToString() + "'";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[3].Value.ToString()))
                            parameters += "null";
                        else
                            parameters += "'" + line.Cells[3].Value.ToString() + "'";
                    }

                    if (flag)
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("editSurvey", parameters);
                        AlertGridOperation(sender, e, null, "editSurvey" + line.Cells[1].Value, returnSqlServer);
                    }
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (RadMessageBox.Show("Вы точно хотите удалить запись?", "Подтверждение", MessageBoxButtons.OKCancel, RadMessageIcon.Exclamation) == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    var line = (GridViewRowInfo)e.NewItems[0];
                    if (!string.IsNullOrEmpty(line.Cells[0].Value.ToString()))
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("deleteSurvey", line.Cells[0].Value.ToString());
                        AlertGridOperation(sender, e, null, "deleteSurvey" + line.Cells[0].Value, returnSqlServer);
                    }
                }
            }
        }
        private void UpdateComboBoxSpeziolist()
        {
            var commandServer = new CommandServer();

            _radSpeziolist.SelectedItemChanged += new EventHandler(Lv_SelectedItemChanged);
            _radSpeziolist.DisplayMember = "fio";
            _radSpeziolist.ValueMember = "fio";
            _radSpeziolist.DataSource = commandServer.DataGridSet(@"select *
                from spezialistSurvey()").Tables[0];
            _radSpeziolist.EnableGrouping = true;
            _radSpeziolist.ShowGroups = true;

            GroupDescriptor group = new GroupDescriptor(new SortDescriptor[] { new SortDescriptor("cat", ListSortDirection.Descending) });
            _radSpeziolist.GroupDescriptors.Add(group);
            _radSpeziolist.AllowEdit = true;
            _radSpeziolist.CollapseAll();            
        }

        private void RadGridView2_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            if(e.ActiveEditor is RadDropDownListEditor)
            {
                RadDropDownListEditor editor = e.ActiveEditor as RadDropDownListEditor;
                RadDropDownListEditorElement element = editor.EditorElement as RadDropDownListEditorElement;
                element.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                element.Popup.Controls.Add(_radSpeziolist);
                element.DropDownMinSize = new Size(300, 300);
                element.PopupOpening += new CancelEventHandler(Element_PopupOpening);
            }

            if (e.ActiveEditor is RadDateTimeEditor dateTimeEditor)
            {
                dateTimeEditor.MaxValue = DateTime.Now;
                radGridViewSurvey.CellEditorInitialized -= RadGridView2_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                if (editroElement.TextBoxElement.Provider is MaskDateTimeProvider provider)
                    provider.AutoSelectNextPart = true;
            }
        }
        void Element_PopupOpening(object sender, CancelEventArgs e)
        {
            _radSpeziolist.Size = ((RadDropDownListEditorElement)sender).Popup.Size;
            _radSpeziolist.AutoScroll = true;

        }
        private void Lv_SelectedItemChanged(object sender, EventArgs e)
        {
            ListViewItemEventArgs args = (ListViewItemEventArgs)e;
            if (args.Item != null && radGridViewSurvey.CurrentCell != null)
            {
                this.radGridViewSurvey.CurrentCell.Value = args.Item.Value;
                ((DropDownPopupForm)args.ListViewElement.ElementTree.Control.Parent).ClosePopup(RadPopupCloseReason.Mouse);
                radGridViewSurvey.CancelEdit();
            }
        }

        private void RadGridView_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

            if (!(e.ActiveEditor is RadDropDownListEditor editor))
            {
                return;
            }

            RadDropDownListElement element = editor.EditorElement as RadDropDownListEditorElement;

            int scrolBarWidth = 0;

            if (element.DefaultItemsCountInDropDown < element.Items.Count)
            {
                scrolBarWidth = 35;
            }

            foreach (RadListDataItem item in element.Items)
            {
                string text = item.Text;
                Size size = TextRenderer.MeasureText(text, element.Font);

                if (element.DropDownWidth < size.Width)
                {
                    element.DropDownWidth = size.Width + scrolBarWidth;
                }
            }
        }
        #endregion

        #region Помощь
        private void HelpLoad()
        {
            _loadGrid = true;
            var commandServer = new CommandServer();
            _bindingSourceHelp = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from AloneHelp(" + _keyAlone + ")").Tables[0] };

            radGridViewHelp.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewHelp.AutoSizeRows = true;
                radGridViewHelp.DataSource = _bindingSourceHelp;

                GridViewDateTimeColumn dateHelp = new GridViewDateTimeColumn("Дата");
                radGridViewHelp.Columns[2] = dateHelp;
                dateHelp.Name = "date_help";
                dateHelp.FieldName = "dateHelp";
                dateHelp.FormatString = "{0:dd/MM/yyyy}";
                dateHelp.Format = DateTimePickerFormat.Custom;
                dateHelp.CustomFormat = "dd.MM.yyyy";

                GridViewComboBoxColumn comboColumn = new GridViewComboBoxColumn("Тип помощи")
                {
                    DataSource = _bindingHelp
                };
                radGridViewHelp.Columns[0].IsVisible = false;
                comboColumn.FieldName = "type";
                radGridViewHelp.Columns[1] = comboColumn;
                radGridViewHelp.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                radGridViewHelp.Columns[2].MinWidth = 45;
                radGridViewHelp.Columns[3].WrapText = true;
                radGridViewHelp.Columns[1].WrapText = true;
                radGridViewHelp.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                radGridViewHelp.CellEditorInitialized += new GridViewCellEventHandler(RadGridView_CellEditorInitialized);
            }));
            _loadGrid = false;
        }

        private void RadGridView3_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();

            if (e.Rows[0].Cells[1].Value != null && e.Rows[0].Cells[2].Value != null && e.Rows[0].Cells[3].Value != null)
            {
                var parameters = _keyAlone + ",'" + e.Rows[0].Cells[1].Value.ToString() + "','" + e.Rows[0].Cells[2].Value.ToString() + "','" + e.Rows[0].Cells[3].Value.ToString() + "'";
                var returnSqlServer = commandServer.ExecReturnServer("addHelp", parameters);
                AlertGridOperation(sender, null, e, "addHelp " + parameters, returnSqlServer);
            }
            else
            {
                RadMessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void RadGridView3_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSourceHelp = new BindingSource { DataSource = commandServer.DataGridSet(@"select * from AloneHelp(" + _keyAlone + ")").Tables[0] };

            radGridViewHelp.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewHelp.DataSource = _bindingSourceHelp;
            }));
        }

        private void RadGridView3_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                bool flag = false;
                var line = (GridViewRowInfo)e.NewItems[0];

                if (line.Cells[0].Value != null)
                {
                    var parameters = line.Cells[0].Value.ToString() + ",'";
                    if (e.PropertyName == "type")
                    {
                        flag = true;
                        parameters += e.NewValue.ToString() + "','";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[1].Value.ToString()))
                            parameters += "null,'";
                        else
                            parameters += line.Cells[1].Value.ToString() + "','";
                    }

                    if (e.PropertyName == "Дата")
                    {
                        flag = true;
                        parameters += e.NewValue.ToString() + "','";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[2].Value.ToString()))
                            parameters += "null";
                        else
                            parameters += line.Cells[2].Value.ToString();
                    }

                    if (e.PropertyName == "Доп. информация")
                    {
                        flag = true;
                        parameters += "','" + e.NewValue.ToString() + "'";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[3].Value.ToString()))
                            parameters += "',null";
                        else
                            parameters += "','" + line.Cells[3].Value.ToString() + "'";
                    }

                    if (flag)
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("editHelp", parameters);
                        AlertGridOperation(sender, e, null, "editHelp" + line.Cells[1].Value, returnSqlServer);
                    }
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (RadMessageBox.Show("Вы точно хотите удалить запись?", "Подтверждение", MessageBoxButtons.OKCancel, RadMessageIcon.Exclamation) == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }

                    var line = (GridViewRowInfo)e.NewItems[0];
                    if (!string.IsNullOrEmpty(line.Cells[0].Value.ToString()))
                    {
                        var returnSqlServer = commandServer.ExecReturnServer("deleteHelp", line.Cells[0].Value.ToString());
                        AlertGridOperation(sender, e, null, "deleteHelp" + line.Cells[0].Value, returnSqlServer);
                    }
                }
            }
        }

        private void RadGridView3_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            if (e.ActiveEditor is RadDateTimeEditor dateTimeEditor)
            {
                dateTimeEditor.MaxValue = DateTime.Now;
                radGridViewHelp.CellEditorInitialized -= RadGridView3_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                if (editroElement.TextBoxElement.Provider is MaskDateTimeProvider provider)
                    provider.AutoSelectNextPart = true;
            }
        }
        #endregion

        #region Дополнительно
        private void Blocked(bool flag)
        {
            //true - блокировка, false - разблокировка
            foreach (Control control in radPageView1.Controls)
            {
                BlockedControl(control, flag);
            }
        }

        private void BlockedPage(bool flag)
        {
            radPageViewPage1.Enabled = flag;
            radPageViewPage2.Enabled = flag;
            radPageViewPage3.Enabled = flag;
            radPageViewPage4.Enabled = flag;
            radPageViewPage5.Enabled = flag;
            radPageViewPage6.Enabled = flag;
        }

        private void BlockedControl(Control control, bool flag)
        {
            foreach (Control c in control.Controls)
            {
                BlockedControl(c, flag);
            }

            if (control is RadPageViewPage cb) return;
            if (control is RadScrollablePanel sc) return;
            if (control is RadScrollablePanelContainer scr) return;
            if (control is RadPageView cd) return;
            if (control is RadButton cbut)
            {
                if (!sm_check.Checked)
                {
                    if (cbut.Text == "возобновить" || cbut.Text == "история")
                        return;
                }
                else
                {
                    if (cbut.Text == "история" || cbut.Text == "возобновить ")
                        return;
                }
            }

            if (flag)
                control.Enabled = false;
            else
                control.Enabled = true;
        }

        private static string CharTo(string s)
        {
            try
            {
                return s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1).ToLower();
            }
            catch (Exception)
            {
                return s;
            }
        }

        private void CheackDateCategory()
        {
            if(_dateR >= _dateRTr)
            {
                radCheckBox44.IsChecked = true;
                radGroupBox3.Enabled = true;
                if (_dateR < 70)
                {
                    radCheckBox36.IsChecked = true;
                    return;
                }

                //70-79
                if (70 <= _dateR && _dateR < 80)
                {
                    radCheckBox41.IsChecked = true;
                    return;
                }
                //80-89
                if (80 <= _dateR && _dateR < 90)
                {
                    radCheckBox51.IsChecked = true;
                    return;
                }
                //90-99
                if (90 <= _dateR && _dateR < 100)
                {
                    radCheckBox50.IsChecked = true;
                    return;
                }

                //100-...
                if (100 <= _dateR)
                {
                    radCheckBox49.IsChecked = true;
                    return;
                }
            }
            else
            {
                radCheckBox39.IsChecked = true;
                radGroupBox3.Enabled = false;
            }
        }

        //узнать текущую вкладку
        private void RadPageView1_SelectedPageChanging(object sender, RadPageViewCancelEventArgs e)
        {
            if (!_loadGrid)
            {
                if (radPageView1.SelectedPage == radPageViewPage1)
                {
                    //добавление
                    if (_status)
                    {
                        string error = null;
                        if (string.IsNullOrEmpty(family_text.Text))
                        {
                            error = "Не заполнено ФИО." + Environment.NewLine;
                        }
                        if (date_ro_date.Value.Year == DateTime.Now.Year)
                        {
                            error += "Год рождения не должен совпадать с текущим годом." + Environment.NewLine;
                        }else
                        {
                            if(date_ro_date.Value >= DateTime.Now)
                            {
                                error += "Дата рождения не должна превышать текущую дату." + Environment.NewLine;
                            }
                        }

                        if (string.IsNullOrEmpty(selsovet_combo.Text))
                        {
                            error += "Не выбран сельский совет." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(nas_punkt_combo.Text))
                        {
                            error += "Не выбран населенный пункт." + Environment.NewLine;
                        }

                        if(string.IsNullOrEmpty(type_street_combo.Text))
                        {
                            error += "Не выбран тип адреса." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(error))
                        {
                            AddAloneSql();
                        }
                        else
                        {
                            RadMessageBox.Show("Заполните обязательные поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, error);
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        string error = null;
                        if (string.IsNullOrEmpty(family_text.Text))
                        {
                            error = "Не заполнено ФИО." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(error))
                        {
                            if (!CompareAlone())
                            {
                                string[] operation = AloneEdit(false);
                                AlertOperation(operation);
                                if (operation[1] != "1")
                                    e.Cancel = true;
                            }
                        }
                        else
                        {
                            RadMessageBox.Show("Заполните обязательные поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, error);
                            e.Cancel = true;
                        }
                    }
                }
                else
                {
                    if (radPageView1.SelectedPage == radPageViewPage2)
                    {
                        if (radPageView2.SelectedPage == radPageViewPage7)
                        {
                            if (_status)
                            {
                                string[] operation = AddSojitel();
                                AlertOperation(operation);
                                if (operation[1] != "1")
                                    e.Cancel = true;
                                else
                                    radButton1.Enabled = true;
                            }
                            else
                            {
                                if (_sojitel != null)
                                {
                                    if (!CompareSojitel())
                                    {
                                        string[] operation = SojitelEdit();
                                        AlertOperation(operation);
                                        if (operation[1] != "1")
                                            e.Cancel = true;
                                        else
                                            radButton1.Enabled = true;
                                    }
                                }
                                else
                                {
                                    string[] operation = AddSojitel();
                                    AlertOperation(operation);
                                    if (operation[1] != "1")
                                        e.Cancel = true;
                                    else
                                        radButton1.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            if (radPageView2.SelectedPage == radPageViewPage12)
                            {
                                //дополнительно
                                if (_bindingDopKinder == null && string.IsNullOrEmpty(textBox11.Text))
                                    return;
                                if (_bindingDopKinder != textBox11.Text)
                                {
                                    string[] operation = AddDopKinder();
                                    AlertOperation(operation);
                                    if (operation[1] != "1")
                                        e.Cancel = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (radPageView1.SelectedPage == radPageViewPage3)
                        {
                            if (_status){ }
                            else
                            {
                                if (!CompareJilUsl())
                                {
                                    string[] operation = JilUslEdit();
                                    AlertOperation(operation);
                                    if (operation[1] != "1")
                                        e.Cancel = true;
                                }

                                if (!CompareZemeln())
                                {
                                    string[] operation = ZemelnEdit();
                                    AlertOperation(operation);
                                    if (operation[1] != "1")
                                        e.Cancel = true;
                                }
                            }
                        }
                        else
                        {
                            if (radPageView1.SelectedPage == radPageViewPage4)
                            {
                                history.Close();
                            }
                        }
                    }
                }
            }
        }

        //на которую нажали
        private void RadPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (radPageView1.SelectedPage == radPageViewPage1){ }
            else
            {
                if (radPageView1.SelectedPage == radPageViewPage2)
                {
                    if (_status)
                    {
                        if (pol_m_che.Checked)
                            radioButton3.Checked = true;
                        else
                        {
                            radioButton4.Checked = true;
                        }
                    }
                }
                else
                {
                    if (radPageView1.SelectedPage == radPageViewPage3)
                    {
                    }
                    else
                    {
                        if (radPageView1.SelectedPage == radPageViewPage4)
                        {
                            if (this._statusDouble)
                            {
                                CheackDateCategory();
                            }
                            history = new Alone_history(_keyAlone);
                            history.Show();

                        }
                    }
                }
            }
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!_load)
                SendKeys.Send(".");
        }

        private void House_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void Housing_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        #endregion

        #region UpdateComboBox

        private void UpdateComboBoxHelp()
        {
            var commandServer = new CommandServer();
            _bindingHelp = new BindingList<string>(commandServer.ComboBoxList(@"select typeHelp from typeHelp order by typeHelp", true));
        }

        private void UpdateComboBoxWoter()
        {
            var commandServer = new CommandServer();
            comboBox6.DataSource = commandServer.ComboBoxList(@"select statusWoter from statusWoter", true);
        }

        private void UpdateComboBoxPlita()
        {
            var commandServer = new CommandServer();
            comboBox7.DataSource = commandServer.ComboBoxList(@"select statusPlita from statusPlita", true);
        }

        private void UpdateComboBoxKanal()
        {
            var commandServer = new CommandServer();
            comboBox8.DataSource = commandServer.ComboBoxList(@"select statusKanal from statusKanal", true);
        }

        private void UpdateComboBoxOtopl()
        {
            var commandServer = new CommandServer();
            comboBox9.DataSource = commandServer.ComboBoxList(@"select statusOtopl from statusOtopl", true);
        }
        
        #endregion

        #region Sql
        #region Add
        private string[] AddJilUsl()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            var structure = new StructuresJilUsl();

            var parameters = _keyAlone + "," + numericUpDown1.Text + ",'" + textBox13.Text + "'";
            structure.CountRoom = Convert.ToInt32(numericUpDown1.Text);
            structure.Place = textBox13.Text;

            if (checkBox2.Checked)
            {
                parameters += ",'" + comboBox6.Text + "'";
                structure.Woter = comboBox6.Text;
            }
            else
                parameters += ",null";

            if (checkBox1.Checked)
            {
                parameters += ",'" + comboBox7.Text + "'";
                structure.Plita = comboBox6.Text;
            }
            else
                parameters += ",null";

            if (checkBox3.Checked)
            {
                parameters += ",'" + comboBox8.Text + "'";
                structure.Kanal = comboBox6.Text;
            }
            else
                parameters += ",null";

            if (checkBox4.Checked)
            {
                parameters += ",'" + comboBox9.Text + "'";
                structure.Otopl = comboBox6.Text;
            }
            else
                parameters += ",null";

            var returnSqlServer = commandServer.ExecReturnServer("addJilUsl", parameters);
            
            try
            {
                _jilUsl = structure;
                if (returnSqlServer[0] == "Запись успешно добавлена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }
        private string[] AddZemeln()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            _zemeln = new StructuresZemeln();

            var parameters = _keyAlone + ",";

            if (checkBox5.Checked)
            {
                parameters += 1;
                _zemeln.Podsobn = true;
            }
            else
                parameters += 0;

            if (checkBox8.Checked)
            {
                parameters += ",1,";
                _zemeln.Zemeln = true;
            }
            else
            {
                parameters += ",0,";
            }

            if (!string.IsNullOrEmpty(textBox9.Text))
            {
                parameters += "'" + textBox9.Text + "'";
                _zemeln.Place = textBox9.Text;
            }
            else
            {
                parameters += "null";
            }

            if (!string.IsNullOrEmpty(comboBox10.Text))
            {
                parameters += ",'" + comboBox10.Text + "',";
                _zemeln.Status = comboBox10.Text;
            }
            else
            {
                parameters += ",null,";
            }

            if (checkBox7.Checked)
            {
                parameters += "1," + numericUpDown2.Text;
                _zemeln.Api = Convert.ToInt32(numericUpDown2.Text);
            }
            else
            {
                parameters += "0,null";
            }

            if (checkBox6.Checked)
            {
                parameters += ",1," + numericUpDown3.Text;
                _zemeln.Szu = Convert.ToInt32(numericUpDown3.Text);
            }
            else
            {
                parameters += ",0,null";
            }

            var returnSqlServer = commandServer.ExecReturnServer("addZemeln", parameters);

            //if (returnSqlServer[1] != "Успешно") return returnSqlServer;
            try
            {
                if (returnSqlServer[0] == "Запись успешно добавлена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }
        
        #endregion
        #region Load
               
        private void JilUslLoad()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from GetJilUsl(" + _keyAlone + ")").Tables[0];
            _jilUsl = new StructuresJilUsl();
            if (dt.Rows.Count <= 0)
            {
                _jilUsl.Place = "";
                return;
            }            
            
            numericUpDown1.Value = Convert.ToDecimal(dt.Rows[0].ItemArray[0].ToString());
            _jilUsl.CountRoom = Convert.ToInt32(numericUpDown1.Text);
            textBox13.Text = dt.Rows[0].ItemArray[1].ToString();
            _jilUsl.Place = textBox13.Text;

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[2].ToString()))
            {
                comboBox6.SelectedIndex = comboBox6.FindStringExact(dt.Rows[0].ItemArray[2].ToString());
                checkBox2.Checked = true;
                _jilUsl.Woter = comboBox6.Text;
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[3].ToString()))
            {
                comboBox7.SelectedIndex = comboBox7.FindStringExact(dt.Rows[0].ItemArray[3].ToString());
                checkBox1.Checked = true;
                _jilUsl.Plita = comboBox7.Text;
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[4].ToString()))
            {
                comboBox8.SelectedIndex = comboBox8.FindStringExact(dt.Rows[0].ItemArray[4].ToString());
                checkBox3.Checked = true;
                _jilUsl.Kanal = comboBox8.Text;
            }

            if (string.IsNullOrEmpty(dt.Rows[0].ItemArray[5].ToString())) return;

            comboBox9.SelectedIndex = comboBox9.FindStringExact(dt.Rows[0].ItemArray[5].ToString());
            checkBox4.Checked = true;
            _jilUsl.Otopl = comboBox9.Text;
        }
        private void ZemelnLoad()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.DataGridSet(@"select * from GetZemeln(" + _keyAlone + ")").Tables[0];
            _zemeln = new StructuresZemeln();

            if (dt.Rows.Count <= 0)
            {
                _zemeln.Place = "";
                _zemeln.Status = "";
                return;
            }            

            if (Convert.ToBoolean(dt.Rows[0].ItemArray[0]))
            {
                checkBox5.Checked = true;
                _zemeln.Podsobn = true;
            }

            if (Convert.ToBoolean(dt.Rows[0].ItemArray[1]))
            {
                checkBox8.Checked = true;
                _zemeln.Zemeln = true;
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[2].ToString()))
            {
                textBox9.Text = dt.Rows[0].ItemArray[2].ToString();
                _zemeln.Place = dt.Rows[0].ItemArray[2].ToString();
            }else
            {
                _zemeln.Place = "";
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[3].ToString()))
            {
                _zemeln.Status = dt.Rows[0].ItemArray[3].ToString();
                comboBox10.SelectedIndex = comboBox10.FindStringExact(dt.Rows[0].ItemArray[3].ToString());
            }
            else
                _zemeln.Status = "";

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[4].ToString()))
            {
                numericUpDown2.Value = Convert.ToDecimal(dt.Rows[0].ItemArray[4]);
                checkBox7.Checked = true;
                _zemeln.Api = (int)numericUpDown2.Value;
            }

            if (string.IsNullOrEmpty(dt.Rows[0].ItemArray[5].ToString())) return;

            numericUpDown3.Value = Convert.ToDecimal(dt.Rows[0].ItemArray[5]);
            checkBox6.Checked = true;
            _zemeln.Szu = (int)numericUpDown3.Value;
        }

        #endregion
        #region Edit
        private string[] JilUslEdit()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            var parameters = _keyAlone + "," + numericUpDown1.Text + ",'" + textBox13.Text + "'";
            var structur = new StructuresJilUsl()
            {
                CountRoom = Convert.ToInt32(numericUpDown1.Text),
                Place = textBox13.Text
            };
            if (checkBox2.Checked)
            {
                parameters += ",'" + comboBox6.Text + "'";
                structur.Woter = comboBox6.Text;
            }
            else
                parameters += ",null";

            if (checkBox1.Checked)
            {
                parameters += ",'" + comboBox7.Text + "'";
                structur.Plita = comboBox7.Text;
            }
            else
                parameters += ",null";

            if (checkBox3.Checked)
            {
                parameters += ",'" + comboBox8.Text + "'";
                structur.Kanal = comboBox8.Text;
            }
            else
                parameters += ",null";

            if (checkBox4.Checked)
            {
                parameters += ",'" + comboBox9.Text + "'";
                structur.Otopl = comboBox9.Text;
            }
            else
                parameters += ",null";

            var returnSqlServer = commandServer.ExecReturnServer("editJilsl", parameters);

            if (returnSqlServer[1] != "Успешно") return returnSqlServer;
            try
            {
                _jilUsl = structur;
                if (returnSqlServer[0] == "Запись успешно изменена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }
        private string[] ZemelnEdit()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            var structure = new StructuresZemeln();
            var parameters = _keyAlone + ",";

            if (checkBox5.Checked)
            {
                parameters += 1;
                structure.Podsobn = true;
            }
            else
                parameters += 0;

            if (checkBox8.Checked)
            {
                parameters += ",1,";
                structure.Zemeln = true;
            }
            else
                parameters += ",0,";

            if (!string.IsNullOrEmpty(textBox9.Text))
            {
                parameters += "'" + textBox9.Text + "'";
                structure.Place = textBox9.Text;
            }
            else
                parameters += "null";

            if (!string.IsNullOrEmpty(comboBox10.Text))
            {
                parameters += ",'" + comboBox10.Text + "',";
                structure.Status = comboBox10.Text;
            }
            else
                parameters += ",null,";

            if (checkBox7.Checked)
            {
                parameters += "1," + numericUpDown2.Text;
                structure.Api = Convert.ToInt32(numericUpDown2.Text);
            }
            else
                parameters += "0,null";

            if (checkBox6.Checked)
            {
                parameters += ",1," + numericUpDown3.Text;
                structure.Szu = Convert.ToInt32(numericUpDown3.Text);
            }
            else
                parameters += ",0,null";

            var returnSqlServer = commandServer.ExecReturnServer("editZemeln", parameters);

            if (returnSqlServer[1] != "Успешно") return returnSqlServer;
            try
            {
                _zemeln = structure;

                if (returnSqlServer[0] == "Запись успешно изменена.")
                    return returnSqlServer;
                commandClient.WriteFileError(null, parameters + " " + returnSqlServer[0]);
                return returnSqlServer;
            }
            catch (Exception ex)
            {
                commandClient.WriteFileError(ex, parameters);
                return returnSqlServer;
            }
        }

        #endregion
        #endregion

        #region checkBox_Checked
        private void RadCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            date_exit_date.Enabled = exit_check.Checked;
        }

        private void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Enabled = checkBox8.Checked;
            comboBox10.Enabled = checkBox8.Checked;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox6.Enabled = checkBox2.Checked;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox7.Enabled = checkBox1.Checked;
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Enabled = checkBox3.Checked;
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            comboBox9.Enabled = checkBox4.Checked;
        }

        private void CheckBox27_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Enabled = checkBox27.Checked;
        }

        private void CheckBox28_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker4.Enabled = checkBox28.Checked;
        }

        private void CheckBox26_CheckedChanged(object sender, EventArgs e)
        {
            date_sm_date.Enabled = sm_check.Checked;
        }

        private void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox7.Checked;
        }

        private void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown3.Enabled = checkBox6.Checked;
        }
        #endregion

        #region Alert
        private void AlertGridOperation(object sender, GridViewCollectionChangingEventArgs eDelete, GridViewRowCancelEventArgs eAdd, string operation, string[] resultOperation)
        {
            if (resultOperation[1] == "1")
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
            else
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
                if (eDelete == null)
                    eAdd.Cancel = true;
                else
                    eDelete.Cancel = true;
            }
        }

        private void AlertOperation(string[] resultOperation)
        {
            if (resultOperation[1] == "1")
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
            else
            {
                radDesktopAlert1.ContentText = resultOperation[0];
                radDesktopAlert1.Show();
            }
        }
        #endregion

        #region Compare
        private bool CompareAlone()
        {
            var structuresAlone = new StructuresAlone
            {
                Country = nas_punkt_combo.Text,
                DateRo = date_ro_date.Value,
                Family = family_text.Text,
                Name = name_text.Text,
                Surname = surname_text.Text,
                Phone = phone_text.Text,
                Street = street_text.Text,
                PlaceWork = rab_text.Text,
                Dop = dop_text.Text,
                Apartament = apartament_text.Text,
                TypeUl = type_street_combo.Text,
                Housing = housing_text.Text,
                House = house_text.Text
            };

            if (sm_check.Checked)
                structuresAlone.DateSm = date_sm_date.Value;

            if (pol_m_che.Checked)
                structuresAlone.Pol = 1;

            if (exit_check.Checked)
                structuresAlone.DateExit = date_exit_date.Value;
            return _alone.Equals(structuresAlone);
        }

        private bool CompareSojitel()
        {
            var structuresSojitel = new StructuresSojitel
            {
                Family = family_sojitel_text.Text,
                Name = name_sojitel_text.Text,
                Surname = surname_sojitel_text.Text,
                Dop = textBox4.Text
            };

            if (radioButton4.Checked)
                structuresSojitel.Pol = 1;
            else
                structuresSojitel.Pol = 0;

            if (checkBox28.Checked)
                structuresSojitel.DateRo = dateTimePicker4.Value;

            if (checkBox27.Checked)
                structuresSojitel.DateSm = dateTimePicker3.Value;

            return _sojitel.Equals(structuresSojitel);
        }

        private bool CompareJilUsl()
        {
            var structure = new StructuresJilUsl
            {
                CountRoom = Convert.ToInt32(numericUpDown1.Text),
                Place = textBox13.Text
            };

            if (checkBox2.Checked)
                structure.Woter = comboBox6.Text;

            if (checkBox1.Checked)
                structure.Plita = comboBox7.Text;

            if (checkBox3.Checked)
                structure.Kanal = comboBox8.Text;

            if (checkBox4.Checked)
                structure.Otopl = comboBox9.Text;

            return _jilUsl.Equals(structure);
        }

        private bool CompareZemeln()
        {
            var structures = new StructuresZemeln();

            if (checkBox5.Checked)
                structures.Podsobn = true;

            if (checkBox8.Checked)
                structures.Zemeln = true;

            if (!string.IsNullOrEmpty(textBox9.Text))
                structures.Place = textBox9.Text;
            else
                structures.Place = "";

            if (!string.IsNullOrEmpty(comboBox10.Text))
                structures.Status = comboBox10.Text;
            else
                structures.Status = "";

            if (checkBox7.Checked)
                structures.Api = Convert.ToInt32(numericUpDown2.Text);

            if (checkBox6.Checked)
                structures.Szu = Convert.ToInt32(numericUpDown3.Text);
            return _zemeln.Equals(structures); 
        }
        #endregion

        private void Date_ro_date_Validating(object sender, CancelEventArgs e)
        {
            if (!_status)
                return;
            if (CheackCopyAlone())
            {
                new search.Result(_bindingSource).ShowDialog();
            }
        }

        #region Печать
        private void RadMenuItem2_Click(object sender, EventArgs e)
        {
            StructuresAlones alones = new StructuresAlones
            {
                Alone = _alone,
                Invalidnost = Inval(),
                Family = Family()
            };
            Print print = new Print();
            print.Inv(alones);
        }

        private StructuresInvalidnost Inval()
        {
            StructuresInvalidnost inval = new StructuresInvalidnost();
            if (label28.Text.Split(':')[1] != " ")
                inval.Date_start = label28.Text.Split(':')[1];
            else
                inval.Date_start = "___________________________________";

            if (label26.Text.Split(':')[1] != " ")
                inval.Stepen = label26.Text.Split(':')[1];
            else
                inval.Stepen = "___________________________________";

            if (label29.Text.Split(':')[1] != " ")
                inval.Date_pere = label29.Text.Split(':')[1];
            else
                inval.Date_pere = "___________________________________";

            if (label27.Text.Split(':')[1] != " ")
                inval.Diagnoz = label27.Text.Split(':')[1];
            else
                inval.Diagnoz = "___________________________________";

            return inval;
        }

        private StructuresFamily Family()
        {
            var structuresFamily = new StructuresFamily();
            string value;

            foreach(GridViewRowInfo relativ in radGridView4.Rows)
            {
                value = relativ.Cells[3].Value.ToString();
                if(!string.IsNullOrEmpty(value))
                {
                    if(value == "Мать")
                    {
                        structuresFamily.FioMather = relativ.Cells[1].Value.ToString();
                    }else
                    {
                        if(value == "Отец")
                        {
                            structuresFamily.FioFather = relativ.Cells[1].Value.ToString();
                        }
                    }
                    value = null;
                }
            }

            if(string.IsNullOrEmpty(structuresFamily.FioFather))
                structuresFamily.FioFather = "___________________________________";
            if(string.IsNullOrEmpty(structuresFamily.FioMather))
                structuresFamily.FioMather = "___________________________________";

            return structuresFamily;
        }
        #endregion

        private void Dead_button_Click(object sender, EventArgs e)
        {
            if (RadMessageBox.Show("Вы подтверждаете возобновление умершего?", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
            {
                string[] operation = AloneEdit(true);
                if (operation[0] == "Запись успешно изменена.")
                {
                    Blocked(false);
                    sm_check.Checked = false;
                    date_exit_date.Enabled = false;
                }
            }            
        }
    }
}