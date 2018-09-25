using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public sealed partial class AddAlone : RadForm
    {
        #region Переменные
        private bool _status;
        private bool _statusExit;
        private bool _statusEdit;
        private bool _load;
        private bool _loadGrid;
        private bool? _dublicate;
        private readonly SqlConnection _connection = LoadProgram.Connect;
        private int _keyAlone;
        //private int category;
        private string _endOperationSoc;
        
        private StructuresAlone _alone;
        private StructuresSojitel _sojitel;
        private StructuresJilUsl _jilUsl;
        private StructuresZemeln _zemeln;
        private BindingSource _bindingSource;
        private string _bindingDopKinder;
        //private bool _cheackBoxCategoryCheack;

        private BindingSource _bindingSourceKinder;
        private BindingSource _bindingSourceSurvey;
        private BindingSource _bindingSourceHelp;
        private BindingSource _bindingRelative;
        private BindingList<string> _bindingSocOperation;
        private BindingSource _bindingSocOperationGrid;
        private RadListView _radSpeziolist = new RadListView();
        private BindingList<string> _bindingSocRabotnik;
        private BindingList<string> _bindingHelp;
        private StructStartParameters StructStartParameter;

        TelerikMetroTheme theme = new TelerikMetroTheme();
        #endregion

        #region Конструкторы
        public AddAlone(bool status, int keyAlone)
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            RadMessageBox.SetThemeName(theme.ThemeName);

            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            MyRadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            CalendarLocalizationProvider.CurrentProvider = new MyRussionCalendarLocalizationProvider();

            StructStartParameter = new StructStartParameters
            {
                KeyAlone = keyAlone,
                Status = status
            };
        }

        private void AddAlone_Shown(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread _thread = new Thread(new ParameterizedThreadStart(Start));
            _thread.IsBackground = true;
            Text = @"Загрузка данных...";
            _statusEdit = true;
            _thread.Start(StructStartParameter);
        }

        private void Start(object obj)
        {
            try
            {
                StructStartParameters structStartParameters = (StructStartParameters)obj;
                //(radDateTimePicker1.DateTimePickerElement.TextBoxElement.Provider as MaskDateTimeProvider).AutoSelectNextPart = true;

                _status = structStartParameters.Status;
                _statusExit = structStartParameters.Status;
                _load = true;

                UpdateComboBoxSelsovet();
                UpdateComboBoxSemPol();
                UpdateComboBoxWoter();
                UpdateComboBoxPlita();
                UpdateComboBoxKanal();
                UpdateComboBoxOtopl();
                UpdateComboBoxSpeziolist();
                UpdateComboBoxSocRabotnik();
                UpdateComboBoxSocOperation();                
                UpdateComboBoxHelp();
                numericUpDown2.Value = DateTime.Now.Year;
                numericUpDown3.Value = DateTime.Now.Year;
                dateTimePicker2.MaxDate = DateTime.Now;
                dateTimePicker3.MaxDate = DateTime.Now;
                dateTimePicker4.MaxDate = DateTime.Now;
                dateTimePicker8.MaxDate = DateTime.Now;

                if (!structStartParameters.Status)
                {
                    _keyAlone = structStartParameters.KeyAlone;

                    AloneLoad();

                    SojitelLoad();
                    KinderLoad();
                    UpdateRelative();
                    KinderOther();

                    JilUslLoad();
                    ZemelnLoad();

                    CategoryLoad();
                                      
                    SurveyLoad();
                    HelpLoad();
                    SocOperationLoad();
                    if (!_statusEdit)
                    {
                        Blocked();
                    }
                }
                else
                {
                    Text = @"Добавление новой записи (режим создания)";
                    textBox2.Text = @"ул. ";
                }
                _load = false;
            }catch(Exception ex)
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
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    if (CategoryLoadCount() <= 0)
                    {
                        if (RadMessageBox.Show("Вы не выбрали категорию. При закрытии данные не сохраняться. Выйти без сохранения?", "Ошибка", MessageBoxButtons.OKCancel, RadMessageIcon.Info) == DialogResult.OK)
                        {
                            CommandServer commandServer = new CommandServer();
                            commandServer.GetServerCommandExecNoReturnServer("AloneDelete", _keyAlone.ToString());
                            e.Cancel = false;
                        }
                        else
                        {
                            radPageView1.SelectedPage = radPageViewPage4;
                            e.Cancel = true;
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
                                if (string.IsNullOrEmpty(textBox1.Text))
                                {
                                    error = "Не заполнено ФИО." + Environment.NewLine;
                                }

                                if (string.IsNullOrEmpty(error))
                                {
                                    string[] operation = AloneEdit();
                                    AlertOperation("aloneEdit", operation);
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
                                            AlertOperation("editSojitel", operation);
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
                                                AlertOperation("JilUslEdit", operation);
                                                if (operation[1] != "1")
                                                    e.Cancel = true;
                                                e.Cancel = false;
                                            }

                                            if (!CompareZemeln())
                                            {
                                                string[] operation = ZemelnEdit();
                                                AlertOperation("ZemelnEdit", operation);
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
                        else
                        {
                            if (radPageView1.SelectedPage == radPageViewPage4)
                            {
                                if (_status)
                                {
                                    CheackDateCategory();
                                }
                            }
                            /*else
                            {
                                if (radPageView1.SelectedPage == radPageViewPage5)
                                {
                                    //SurveyLoad();
                                    //UpdateComboBoxSpeziolist();
                                }
                                else
                                {

                                    //HelpLoad();
                                    //UpdateComboBoxHelp();
                                    //UpdateComboBoxSocRabotnik();
                                }
                            }*/
                        }
                    }
                }
            }

            if (e.Cancel == false)
            {
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

            var parameters = "'" + textBox1.Text + "',";

            if (radioButton1.Checked)
                parameters += 1;
            else
                parameters += 0;

            parameters += ",'" + dateTimePicker1.Text + "',";

            if (checkBox26.Checked)
                parameters += "'" + dateTimePicker2.Text + "'";
            else
                parameters += "null";

            parameters += ",'" + comboBox2.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" +
                textBox12.Text + "','" + comboBox3.Text + "',";

            if (string.IsNullOrEmpty(textBox10.Text))
                parameters += "null,";
            else
                parameters += "'" + textBox10.Text + "',";

            if (radCheckBox1.Checked)
                parameters += "'" + dateTimePicker8.Text + "'";
            else
                parameters += "null";

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("addAlone", parameters);

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
                    Fio = textBox1.Text,
                    DateRo = dateTimePicker1.Value,
                    Country = comboBox2.Text,
                    Dop = textBox10.Text,
                    SemPol = comboBox3.Text,
                    Street = textBox2.Text,
                    Phone = textBox3.Text,
                    PlaceWork = textBox12.Text
                };

                if (radioButton1.Checked)
                    _alone.Pol = 1;

                if (checkBox26.Checked)
                    _alone.DateSm = dateTimePicker2.Value;

                if (radCheckBox1.Checked)
                    _alone.DateExit = dateTimePicker8.Value;

                if (returnSqlServer[0] == "Запись успешно добавлена.")
                {
                    AddSojitel();
                    AddDopKinder();
                    AddJilUsl();
                    AddZemeln();

                    _status = false;

                    KinderLoad();
                    UpdateRelative();
                    SurveyLoad();
                    HelpLoad();
                    SocOperationLoad();
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
            var dt = commandServer.GetDataGridSet(@"select * from AloneGet(" + _keyAlone + ")").Tables[0];

            textBox1.Text = dt.Rows[0].ItemArray[0].ToString();
            _alone.Fio = dt.Rows[0].ItemArray[0].ToString();

            if (Convert.ToBoolean(dt.Rows[0].ItemArray[1].ToString()))
            {
                radioButton1.Checked = true;
                _alone.Pol = 1;
            }
            else
            {
                radioButton2.Checked = true;
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[2].ToString()))
            {
                dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0].ItemArray[2].ToString());
                _alone.DateRo = Convert.ToDateTime(dt.Rows[0].ItemArray[2].ToString());
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[3].ToString()))
            {
                dateTimePicker2.Value = Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString());
                _alone.DateSm = Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString());
                checkBox26.Checked = true;
                _statusEdit = false;
                Text = @"Дело №" + _keyAlone + " (режим просмотра)";
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[11].ToString()))
            {
                dateTimePicker8.Value = Convert.ToDateTime(dt.Rows[0].ItemArray[11].ToString());
                _alone.DateExit = Convert.ToDateTime(dt.Rows[0].ItemArray[11].ToString());
                radCheckBox1.Checked = true;
                _statusEdit = false;
                Text = @"Дело №" + _keyAlone + " (режим просмотра)";
            }

            comboBox1.SelectedIndex = comboBox1.FindStringExact(dt.Rows[0].ItemArray[4].ToString());

            comboBox2.SelectedIndex = comboBox2.FindStringExact(dt.Rows[0].ItemArray[5].ToString());
            _alone.Country = comboBox2.Text;
            textBox2.Text = dt.Rows[0].ItemArray[6].ToString();

            if (string.IsNullOrEmpty(textBox2.Text))
                textBox2.Text = @"ул. ";
            _alone.Street = textBox2.Text;
            textBox3.Text = dt.Rows[0].ItemArray[7].ToString();
            _alone.Phone = textBox3.Text;
            textBox12.Text = dt.Rows[0].ItemArray[8].ToString();
            _alone.PlaceWork = dt.Rows[0].ItemArray[8].ToString();
            comboBox3.SelectedIndex = comboBox3.FindStringExact(dt.Rows[0].ItemArray[9].ToString());
            _alone.SemPol = comboBox3.Text;
            textBox10.Text = dt.Rows[0].ItemArray[10].ToString();
            _alone.Dop = textBox10.Text;
            _dublicate = null;
        }

        private string[] AloneEdit()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            var parameters = _keyAlone + ",'" + textBox1.Text + "',";

            if (radioButton1.Checked)
                parameters += 1;
            else
                parameters += 0;

            parameters += ",'" + dateTimePicker1.Text + "',";

            if (checkBox26.Checked)
                parameters += "'" + dateTimePicker2.Text + "'";
            else
                parameters += "null";

            parameters += ",'" + comboBox2.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" +
                            textBox12.Text + "','" + comboBox3.Text + "',";

            if (string.IsNullOrEmpty(textBox10.Text))
                parameters += "null,";
            else
                parameters += "'" + textBox10.Text + "',";

            if (radCheckBox1.Checked)
                parameters += "'" + dateTimePicker8.Text + "'";
            else
                parameters += "null";

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("EditAlone", parameters);


            if (returnSqlServer[1] != "Успешно") return returnSqlServer;
            try
            {
                _alone = new StructuresAlone();

                if (radioButton1.Checked)
                    _alone.Pol = 1;

                if (checkBox26.Checked)
                    _alone.DateSm = dateTimePicker2.Value;

                if (radCheckBox1.Checked)
                    _alone.DateExit = dateTimePicker8.Value;

                _alone.Fio = textBox1.Text;
                _alone.Country = comboBox2.Text;
                _alone.DateRo = dateTimePicker1.Value;
                _alone.Street = textBox2.Text;
                _alone.Phone = textBox3.Text;
                _alone.SemPol = comboBox3.Text;
                _alone.Dop = textBox10.Text;

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

        private void AddressTranslation()
        {
            var adress = textBox2.Text.Split('.');

            if (adress.Length < 2) { }
            else
            {
                if (!string.IsNullOrEmpty(adress[1].Trim()))
                {
                    adress[1] = adress[1].Trim();
                    adress[1] = adress[1].Substring(0, 1).ToUpper() + adress[1].Substring(1, adress[1].Length - 1).ToLower();

                    if (adress.Length > 2)
                    {
                        if (adress.Length == 4)
                        {
                            if (adress[1].IndexOf(".", StringComparison.Ordinal) != -1)
                            {
                                adress[1] = adress[1].Split('.')[0] + adress[1].Split('.')[1].Trim();
                            }
                            if (adress[1].IndexOf(",", StringComparison.Ordinal) != -1)
                            {
                                adress[1] = adress[1].Split(',')[0] + adress[1].Split(',')[1].Trim();
                            }
                            textBox2.Text = adress[0].Trim() + @". " + adress[1].Trim() + @". " + adress[2].Trim() + @" " + adress[3].Trim();
                        }
                        else
                        {
                            if (adress[1].IndexOf(".", StringComparison.Ordinal) != -1)
                            {
                                adress[1] = adress[1].Split('.')[0] + adress[1].Split('.')[1].Trim();
                            }
                            if (adress[1].IndexOf(",", StringComparison.Ordinal) != -1)
                            {
                                adress[1] = adress[1].Split(',')[0] + adress[1].Split(',')[1].Trim();
                            }

                            if (adress[2].IndexOf(".", StringComparison.Ordinal) != -1)
                            {
                                adress[2] = adress[2].Split('.')[0] + " " + adress[2].Split('.')[1].Trim();
                                adress[1] += ". ";
                            }
                            if (adress[2].IndexOf(",", StringComparison.Ordinal) != -1)
                            {
                                adress[2] = adress[2].Split(',')[0] + " " + adress[2].Split(',')[1].Trim();
                                adress[1] += ". ";
                            }

                            if (adress[1].Length < 2)
                                textBox2.Text = adress[0].Trim() + @". " + adress[1].Trim() + @". " + adress[2].Trim();
                            else
                                textBox2.Text = adress[0].Trim() + @". " + adress[1].Trim() + @" " + adress[2].Trim();
                        }
                    }
                    else
                    {
                        if (adress[1].IndexOf(".", StringComparison.Ordinal) != -1)
                        {
                            adress[1] = adress[1].Split('.')[0] + adress[1].Split('.')[1].Trim();
                        }
                        if (adress[1].IndexOf(",", StringComparison.Ordinal) != -1)
                        {
                            adress[1] = adress[1].Split(',')[0] + " " + adress[1].Split(',')[1].Trim();
                        }
                        textBox2.Text = adress[0] + @". " + adress[1].Trim();
                    }
                }
            }
        }

        private void UpdateComboBoxSelsovet()
        {
            var commandServer = new CommandServer();
            comboBox1.Invoke(new MethodInvoker(delegate ()
            {
                comboBox1.DataSource = commandServer.GetComboBoxList(@"select Selsovet from Selsovet order by Selsovet", true);
            }));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxNasPunct();
        }

        private void UpdateComboBoxNasPunct()
        {
            var commandServer = new CommandServer();
            comboBox2.DataSource = commandServer.GetComboBoxList(@"select country
					from country inner join selsovet on 
						country.fk_selsovet = selsovet.key_selsovet
					where selsovet.selsovet = " + "'" + comboBox1.Text + "'", true);
        }

        private void UpdateComboBoxSemPol()
        {
            var commandServer = new CommandServer();
            comboBox3.DataSource = commandServer.GetComboBoxList(@"select sem_pol from sem_pol order by sem_pol", true);
        }

        private bool CheackCopyAlone()
        {
            var command = @"select * from AloneCopy('" + textBox1.Text + "','" + comboBox2.Text +
                             "','" + textBox2.Text + "','" + dateTimePicker1.Text + "')";
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource
            {
                DataSource = commandServer.GetDataGridSet(command).Tables[0]
            };
            return _bindingSource.Count > 0;
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            string[] s = textBox1.Text.Split(' ');
            textBox1.Text = "";

            for(int i = 0; i < s.Count(); i++)
            {
                if (!string.IsNullOrEmpty(s[i]))
                {
                    textBox1.Text += s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
                    if (i != s.Count() - 1)
                    {
                        if (!string.IsNullOrEmpty(s[i + 1]))
                            textBox1.Text += " ";
                    }
                }  
            }

            if (_status)
            {
                if(s.Count() > 2)
                {
                    new search.Family(s[0], s[1], s[2], 0, "SearchFamily", false).ShowDialog();
                }  
            }
        }

        private void checkBox26_CheckStateChanging(object sender, CheckStateChangingEventArgs args)
        {
            //int count = new CommandServer().GetDataGridSet(@"WITH CTE AS
            //(
            //    SELECT *, N=ROW_NUMBER()OVER(PARTITION BY alone.fio, alone.date_ro, country.country, alone.street 
            //    ORDER BY nad_obsl.date_operation DESC)
            //    FROM alone
            //    JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
            //    JOIN country ON alone.fk_country = country.key_country where key_alone = " + _keyAlone +
            //@")
            //SELECT CTE.fio as [ФИО], CTE.date_ro as [Дата рождения], CTE.country as [Нас. пункт], CTE.street as [Адрес], 
            //CTE.date_operation as [Дата операции]
            //FROM CTE
            //JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
            //JOIN speziolist ON speziolist.key_speziolist = CTE.fk_soc_rab
            //JOIN operation ON operation.key_operation = CTE.fk_operation
            //WHERE CTE.N=1 and operation.operation != 'снят'").Tables[0].Rows.Count;
            if(!string.IsNullOrEmpty(_endOperationSoc))
            {
                if (_endOperationSoc != "снят")
                {
                    RadMessageBox.Show("Надомное осблуживание не приостановленно. Операция не может быть выполнена.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                    args.Cancel = true;
                }
            }

            RadCheckBox cheack = sender as RadCheckBox;
            if(cheack != null)
            {
                if(cheack.Text == "Дата смерти")
                {
                    new search.Family("ListSojitel", textBox1.Text, comboBox2.Text, false).ShowDialog();
                }
            }
        }
        #endregion

        #region Члены семьи

        #region Супруг(а)
        private string[] AddSojitel()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();
            var structure = new StructuresSojitel();

            structure.Fio = textBox6.Text;

            var parameters = _keyAlone + ",'" + textBox6.Text + "',";

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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("addSojitel", parameters);

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
            var dt = commandServer.GetDataGridSet(@"select * from GetSojitel(" + _keyAlone + ")").Tables[0];

            if (dt.Rows.Count <= 0)
            {
                radButton1.Enabled = false;
                return;
            }
            radButton1.Enabled = true;
            _sojitel = new StructuresSojitel();
            textBox6.Text = dt.Rows[0].ItemArray[0].ToString();
            _sojitel.Fio = textBox6.Text;

            if (Convert.ToBoolean(dt.Rows[0].ItemArray[1].ToString()))
            {
                radioButton4.Checked = true;
                _sojitel.Pol = 1;
            }
            else
            {
                radioButton3.Checked = true;
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[2].ToString()))
            {
                dateTimePicker4.Value = Convert.ToDateTime(dt.Rows[0].ItemArray[2].ToString());
                checkBox28.Checked = true;
                _sojitel.DateRo = Convert.ToDateTime(dt.Rows[0].ItemArray[2].ToString());
            }

            if (!string.IsNullOrEmpty(dt.Rows[0].ItemArray[3].ToString()))
            {
                dateTimePicker3.Value = Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString());
                checkBox27.Checked = true;
                _sojitel.DateSm = Convert.ToDateTime(dt.Rows[0].ItemArray[3].ToString());
            }

            textBox4.Text = dt.Rows[0].ItemArray[4].ToString();
            _sojitel.Dop = textBox4.Text;
        }

        private string[] SojitelEdit()
        {
            var commandServer = new CommandServer();
            var commandClient = new CommandClient();

            var parameters = _keyAlone + ",'" + textBox6.Text + "',";

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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("editSojitel", parameters);

            if (returnSqlServer[1] != "Успешно") return returnSqlServer;
            try
            {
                _sojitel = new StructuresSojitel
                {
                    Fio = textBox6.Text,
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

        private void button5_Click(object sender, EventArgs e)
        {
            var serverCommand = new CommandServer();

            if (MessageBox.Show(@"Вы точно хотите удалить сожителя?", @"Подтверждение", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) != DialogResult.OK) return;

            serverCommand.GetServerCommandExecNoReturnServer("deleteSojitel", _keyAlone.ToString());

            AlertOperation("deleteSojitel", new string[] { "Запись успешно удалена", "1" });

            textBox6.Text = "";
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            checkBox28.Checked = false;
            checkBox27.Checked = false;
            textBox4.Text = "";
            radButton1.Enabled = false;
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            string[] s = textBox6.Text.Split(' ');
            textBox6.Text = "";

            for (int i = 0; i < s.Count(); i++)
            {
                if(!string.IsNullOrEmpty(s[i]))
                {
                    textBox6.Text += s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
                    if (i != s.Count() - 1)
                    {
                        if(!string.IsNullOrEmpty(s[i + 1]))
                            textBox6.Text += " ";
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
            _bindingRelative = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from ListRelative(" + _keyAlone + ")").Tables[0] };

            radGridView4.Invoke(new MethodInvoker(delegate ()
            {
                radGridView4.Refresh();
                radGridView4.AutoSizeRows = true;
                radGridView4.DataSource = _bindingRelative;
                radGridView4.Columns[0].IsVisible = false;
                radGridView4.Columns[1].WrapText = true;
                radGridView4.Columns[2].WrapText = true;
                radGridView4.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                radGridView4.Columns[2].BestFit();
            }));
            _loadGrid = false;
        }
        
        private void radGridView4_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();
            var line = e.Rows[0].Cells[1].Value.ToString();

            if (e.Rows[0].Cells[1].Value != null && e.Rows[0].Cells[2].Value != null)
            {
                var parameters = _keyAlone + ",'" + e.Rows[0].Cells[1].Value.ToString() + "','" + e.Rows[0].Cells[2].Value.ToString() + "'";
                var returnSqlServer = commandServer.GetServerCommandExecReturnServer("addRelatives", parameters);
                AlertGridOperation(sender, null, e, "addRelatives " + parameters, returnSqlServer);
            }
            else
            {
                RadMessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void radGridView4_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingRelative.DataSource = commandServer.GetDataGridSet(@"select * from ListRelative(" + _keyAlone + ")").Tables[0];
            radGridView4.Invoke(new MethodInvoker(delegate ()
            {
                radGridView4.DataSource = _bindingRelative;
            }));
        }

        private void radGridView4_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
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
                        parameters += "'" + e.NewValue.ToString() + "'";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(line.Cells[2].Value.ToString()))
                            parameters += "null";
                        else
                            parameters += "'" + line.Cells[2].Value.ToString() + "'";
                    }

                    if (flag)
                    {
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("editRelatives", parameters);
                        AlertGridOperation(sender, e, null, "editRelatives" + line.Cells[1].Value, returnSqlServer);
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
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("deleteRelatives", line.Cells[0].Value.ToString());
                        AlertGridOperation(sender, e, null, "deleteRelatives" + line.Cells[0].Value, returnSqlServer);
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
            _bindingSourceKinder = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from GetKinder(" + _keyAlone + ")").Tables[0] };
            radGridView1.Invoke(new MethodInvoker(delegate ()
            {
                radGridView1.Refresh();
                radGridView1.AutoSizeRows = true;
                radGridView1.DataSource = _bindingSourceKinder;
                radGridView1.Columns[0].IsVisible = false;
                radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //using (radGridView1.DeferRefresh())
                //{
                //    radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //    //foreach (GridViewDataColumn column in radGridView1.Columns)
                //    //{
                //    //    column.BestFit();
                //    //}
                //}

                radGridView1.Columns[1].WrapText = true;
               // radGridView1.Columns[1].MinWidth = 100;
                radGridView1.Columns[3].WrapText = true;
                radGridView1.Columns[3].BestFit();
                radGridView1.Columns[4].WrapText = true;
                radGridView1.Columns[4].BestFit();
            }));
            _loadGrid = false;

            
            radGridViewSurvey.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewSurvey.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //radGridViewSurvey.CellEditorInitialized += new GridViewCellEventHandler(radGridView_CellEditorInitialized);
            }));
            _loadGrid = false;
        }

        private void radGridView1_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (e.Rows[0].Cells[1].Value == null || e.Rows[0].Cells[1].Value.ToString() == "")
            {
                AlertOperation("addKinder", new string[] { "Не указано ФИО.", "1" });
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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("kinder_add", parameters);
            AlertGridOperation(sender, null, e, "kinder_add " + parameters, returnSqlServer);
        }

        private void radGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSourceKinder = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from GetKinder(" + _keyAlone + ")").Tables[0] };

            radGridView1.Invoke(new MethodInvoker(delegate ()
            {
                radGridView1.DataSource = _bindingSourceKinder.DataSource;
            }));
        }

        private void radGridView1_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
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
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("kinder_edit", parameters);
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
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("kinder_delete", line.Cells[0].Value.ToString());
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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("EditDopKinder", _keyAlone + ",'" + textBox11.Text + "'");

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
            var dt = commandServer.GetDataGridSet(command).Tables[0];

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
            var dt = commandServer.GetDataGridSet(@"select * from GetListCategory(" + _keyAlone + ")").Tables[0];
            if (dt.Rows.Count <= 0) return;

            foreach (Control control in radPageView3.Controls)
            {
                CategoryChecked(dt, control);
            }
        }

        private static void CategoryChecked(DataTable table, Control control)
        {
            foreach (Control c in control.Controls)
            {
                CategoryChecked(table, c);
            }

            var cb = control as RadCheckBox;

            if (cb == null) return;

            var controlText = cb.Text;

            var findRows = table.Select("category = '" + controlText + "'");
            if (findRows.Length > 0)
                cb.Checked = true;
        }

        public int CategoryLoadCount()
        {
            var commandServer = new CommandServer();
            var dt = commandServer.GetDataGridSet(@"select * from GetListCategory(" + _keyAlone + ")").Tables[0];
            return dt.Rows.Count;
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_load)
            {
                var checkBox = (RadCheckBox)sender;
                var text = checkBox.Text;
                var commandServer = new CommandServer();
                string[] result;
                if (radPageView3.SelectedPage == radPageViewPage10)
                {
                    if (checkBox.Checked)
                    {
                        result = commandServer.GetServerCommandExecReturnServer("addCategory", _keyAlone + ",'" + text + "','пенсионер'");
                    }
                    else
                    {
                        result = commandServer.GetServerCommandExecReturnServer("deleteCategory", _keyAlone + ",'" + text + "'");
                    }
                }
                else
                {
                    if (radPageView3.SelectedPage == radPageViewPage14)
                    {
                        if (checkBox.Checked)
                        {
                            result = commandServer.GetServerCommandExecReturnServer("addCategory", _keyAlone + ",'" + text + "','соц'");
                        }
                        else
                        {
                            result = commandServer.GetServerCommandExecReturnServer("deleteCategory", _keyAlone + ",'" + text + "'");
                        }
                    }
                    else
                    {
                        if (radPageView3.SelectedPage == radPageViewPage15)
                        {
                            if (checkBox.Checked)
                            {
                                result = commandServer.GetServerCommandExecReturnServer("addCategory", _keyAlone + ",'" + text + "','колясочник'");
                            }
                            else
                            {
                                result = commandServer.GetServerCommandExecReturnServer("deleteCategory", _keyAlone + ",'" + text + "'");
                            }
                        }
                        else
                        {
                            if (checkBox.Checked)
                            {
                                result = commandServer.GetServerCommandExecReturnServer("addCategory", _keyAlone + ",'" + text + "','не пенсионер'");
                            }
                            else
                            {
                                result = commandServer.GetServerCommandExecReturnServer("deleteCategory", _keyAlone + ",'" + text + "'");
                            }
                        }
                    }
                }
                AlertGridOperation(sender, null, null, "category", result);
            }
        }
        #endregion

        #region Обследование
        private void SurveyLoad()
        {
            _loadGrid = true;
            var commandServer = new CommandServer();
            _bindingSourceSurvey = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from AloneSurvey(" + _keyAlone + ") order by date_obsl desc").Tables[0] };

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

                GridViewComboBoxColumn comboColumn = new GridViewComboBoxColumn("Специалист");                
                comboColumn.FieldName = "ФИО";
                comboColumn.AutoCompleteMode = AutoCompleteMode.Append;
                radGridViewSurvey.Columns[1] = comboColumn;

                radGridViewSurvey.Columns[0].IsVisible = false;
                radGridViewSurvey.Columns[3].WrapText = true;
                radGridViewSurvey.Columns[1].WrapText = true;
                radGridViewSurvey.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //radGridViewSurvey.CellEditorInitialized += new GridViewCellEventHandler(radGridView_CellEditorInitialized);
            }));
            _loadGrid = false;
        }
        private void radGridView2_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();

            if (e.Rows[0].Cells[1].Value != null && e.Rows[0].Cells[2].Value != null && e.Rows[0].Cells[3].Value != null)
            {
                var parameters = _keyAlone + ",'" + e.Rows[0].Cells[1].Value.ToString() + "','" + e.Rows[0].Cells[2].Value.ToString() + "','" + e.Rows[0].Cells[3].Value.ToString() + "'";
                var returnSqlServer = commandServer.GetServerCommandExecReturnServer("addSurvey", parameters);
                AlertGridOperation(sender, null, e, "addSurvey " + parameters, returnSqlServer);
            }
            else
            {
                RadMessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
        private void radGridView2_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSourceSurvey = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from AloneSurvey(" + _keyAlone + ")").Tables[0] };

            radGridViewSurvey.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewSurvey.DataSource = _bindingSourceSurvey;
            }));
        }
        private void radGridView2_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
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

                    if (e.PropertyName == "Дата")
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
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("editSurvey", parameters);
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
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("deleteSurvey", line.Cells[0].Value.ToString());
                        AlertGridOperation(sender, e, null, "deleteSurvey" + line.Cells[0].Value, returnSqlServer);
                    }
                }
            }
        }
        private void UpdateComboBoxSpeziolist()
        {
            var commandServer = new CommandServer();

            _radSpeziolist.SelectedItemChanged += new EventHandler(lv_SelectedItemChanged);
            _radSpeziolist.DisplayMember = "fio";
            _radSpeziolist.ValueMember = "fio";
            _radSpeziolist.DataSource = commandServer.GetDataGridSet(@"select *
                from spezialistSurvey()").Tables[0];
            _radSpeziolist.EnableGrouping = true;
            _radSpeziolist.ShowGroups = true;

            GroupDescriptor group = new GroupDescriptor(new SortDescriptor[] { new SortDescriptor("cat", ListSortDirection.Descending) });
            _radSpeziolist.GroupDescriptors.Add(group);
            _radSpeziolist.AllowEdit = true;
            _radSpeziolist.CollapseAll();            
        }

        private void radGridView2_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            if(e.ActiveEditor is RadDropDownListEditor)
            {
                RadDropDownListEditor editor = e.ActiveEditor as RadDropDownListEditor;
                RadDropDownListEditorElement element = editor.EditorElement as RadDropDownListEditorElement;
                element.DropDownSizingMode = SizingMode.UpDownAndRightBottom;
                element.Popup.Controls.Add(_radSpeziolist);
                element.DropDownMinSize = new Size(300, 300);
                element.PopupOpening += new CancelEventHandler(element_PopupOpening);
            }

            RadDateTimeEditor dateTimeEditor = e.ActiveEditor as RadDateTimeEditor;
            if (dateTimeEditor != null)
            {
                dateTimeEditor.MaxValue = DateTime.Now;
                radGridViewSurvey.CellEditorInitialized -= radGridView2_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                MaskDateTimeProvider provider = editroElement.TextBoxElement.Provider as MaskDateTimeProvider;
                if (provider != null)
                    provider.AutoSelectNextPart = true;
            }
        }
        void element_PopupOpening(object sender, CancelEventArgs e)
        {
            _radSpeziolist.Size = ((RadDropDownListEditorElement)sender).Popup.Size;
            _radSpeziolist.AutoScroll = true;

        }
        private void lv_SelectedItemChanged(object sender, EventArgs e)
        {
            ListViewItemEventArgs args = (ListViewItemEventArgs)e;
            if (args.Item != null && radGridViewSurvey.CurrentCell != null)
            {
                this.radGridViewSurvey.CurrentCell.Value = args.Item.Value;
                ((DropDownPopupForm)args.ListViewElement.ElementTree.Control.Parent).ClosePopup(RadPopupCloseReason.Mouse);
                radGridViewSurvey.CancelEdit();
            }
        }

        private void radGridView_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadDropDownListEditor editor = e.ActiveEditor as RadDropDownListEditor;

            if (editor == null)
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
            _bindingSourceHelp = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from AloneHelp(" + _keyAlone + ")").Tables[0] };

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

                GridViewComboBoxColumn comboColumn = new GridViewComboBoxColumn("Тип помощи");
                comboColumn.DataSource = _bindingHelp;
                radGridViewHelp.Columns[0].IsVisible = false;
                comboColumn.FieldName = "type";
                radGridViewHelp.Columns[1] = comboColumn;
                radGridViewHelp.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                radGridViewHelp.Columns[2].MinWidth = 45;
                radGridViewHelp.Columns[3].WrapText = true;
                radGridViewHelp.Columns[1].WrapText = true;
                radGridViewHelp.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                radGridViewHelp.CellEditorInitialized += new GridViewCellEventHandler(radGridView_CellEditorInitialized);
            }));
            _loadGrid = false;
        }

        private void radGridView3_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();

            if (e.Rows[0].Cells[1].Value != null && e.Rows[0].Cells[2].Value != null && e.Rows[0].Cells[3].Value != null)
            {
                var parameters = _keyAlone + ",'" + e.Rows[0].Cells[1].Value.ToString() + "','" + e.Rows[0].Cells[2].Value.ToString() + "','" + e.Rows[0].Cells[3].Value.ToString() + "'";
                var returnSqlServer = commandServer.GetServerCommandExecReturnServer("addHelp", parameters);
                AlertGridOperation(sender, null, e, "addHelp " + parameters, returnSqlServer);
            }
            else
            {
                RadMessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void radGridView3_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSourceHelp = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from AloneHelp(" + _keyAlone + ")").Tables[0] };

            radGridViewHelp.Invoke(new MethodInvoker(delegate ()
            {
                radGridViewHelp.DataSource = _bindingSourceHelp;
            }));
        }

        private void radGridView3_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
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
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("editHelp", parameters);
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
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("deleteHelp", line.Cells[0].Value.ToString());
                        AlertGridOperation(sender, e, null, "deleteHelp" + line.Cells[0].Value, returnSqlServer);
                    }
                }
            }
        }

        private void radGridView3_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadDateTimeEditor dateTimeEditor = e.ActiveEditor as RadDateTimeEditor;
            if (dateTimeEditor != null)
            {
                dateTimeEditor.MaxValue = DateTime.Now;
                radGridViewHelp.CellEditorInitialized -= radGridView3_CellEditorInitialized;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                MaskDateTimeProvider provider = editroElement.TextBoxElement.Provider as MaskDateTimeProvider;
                if (provider != null)
                    provider.AutoSelectNextPart = true;
            }
        }
        #endregion

        #region Дополнительно
        private void Blocked()
        {
            foreach (Control control in radPageView1.Controls)
            {
                BlockedControl(control);
            }            
        }

        private void BlockedControl(Control control)
        {
            foreach (Control c in control.Controls)
            {
                BlockedControl(c);
            }

            var cb = control as RadPageViewPage;
            if (cb != null) return;
            var cd = control as RadPageView;
            if (cd != null) return;

            control.Enabled = false;
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
            DateTime dateTime = new DateTime((DateTime.Now - dateTimePicker1.Value).Ticks);
            int dat = dateTime.Year - 1;
            if (80 <= dat && dat < 90)
            {
                checkBox22.Checked = true;
            }

            if (90 <= dat && dat < 100)
            {
                checkBox22.Checked = false;
                checkBox23.Checked = true;
            }

            if (100 <= dat && dat <= 110)
            {
                checkBox24.Checked = true;
                checkBox23.Checked = false;
            }
        }

        //узнать текущую вкладку
        private void radPageView1_SelectedPageChanging(object sender, RadPageViewCancelEventArgs e)
        {
            if (!_loadGrid)
            {
                if (radPageView1.SelectedPage == radPageViewPage1)
                {
                    //добавление
                    if (_status)
                    {
                        string error = null;
                        if (string.IsNullOrEmpty(textBox1.Text))
                        {
                            error = "Не заполнено ФИО." + Environment.NewLine;
                        }
                        if (dateTimePicker1.Value.Year == DateTime.Now.Year)
                        {
                            error += "Год рождения не должен совпадать с текущим годом." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(comboBox1.Text))
                        {
                            error += "Не выбран сельский совет." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(comboBox2.Text))
                        {
                            error += "Не выбран населнный пункт." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(comboBox3.Text))
                        {
                            error += "Не выбрано семейное положение." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(error))
                        {
                            AddressTranslation();
                            if (CheackCopyAlone())
                            {
                                //if (MessageBox.Show(@"Данные ФИО уже добавлены по этому адресу.",
                                //    @"Ошибка при добавлении", MessageBoxButtons.OK, MessageBoxIcon.Information) !=
                                //    DialogResult.OK)
                                //e.Cancel = true;
                                new search.Family(_bindingSource).ShowDialog();
                                _dublicate = true;
                                Close();
                                e.Cancel = true;
                            }
                            else
                            {
                                _dublicate = false;
                                AddAloneSql();
                            }
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
                        if (string.IsNullOrEmpty(textBox1.Text))
                        {
                            error = "Не заполнено ФИО." + Environment.NewLine;
                        }

                        if (string.IsNullOrEmpty(error))
                        {
                            if (!CompareAlone())
                            {
                                string[] operation = AloneEdit();
                                AlertOperation("aloneEdit", operation);
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
                                AlertOperation("AddSojitel", operation);
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
                                        AlertOperation("editSojitel", operation);
                                        if (operation[1] != "1")
                                            e.Cancel = true;
                                        else
                                            radButton1.Enabled = true;
                                    }
                                }
                                else
                                {
                                    string[] operation = AddSojitel();
                                    AlertOperation("AddSojitel", operation);
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
                                    AlertOperation("EditDopKinder", operation);
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
                            //////////////////////////
                            if (_status)
                            {
                                //string error = null;
                                //if (string.IsNullOrEmpty(textBox1.Text))
                                //{
                                //    error = "Не заполнено ФИО." + Environment.NewLine;
                                //}
                                //if (dateTimePicker1.Value.Year == DateTime.Now.Year)
                                //{
                                //    error += "Год рождения не должен совпадать с текущим годом." + Environment.NewLine;
                                //}

                                //if (string.IsNullOrEmpty(comboBox1.Text))
                                //{
                                //    error += "Не выбран сельский совет." + Environment.NewLine;
                                //}

                                //if (string.IsNullOrEmpty(comboBox2.Text))
                                //{
                                //    error += "Не выбран населнный пункт." + Environment.NewLine;
                                //}

                                //if (string.IsNullOrEmpty(comboBox3.Text))
                                //{
                                //    error += "Не выбрано семейное положение." + Environment.NewLine;
                                //}

                                //if (string.IsNullOrEmpty(error))
                                //{
                                //    AddressTranslation();
                                //    if (CheackCopyAlone())
                                //    {
                                //        if (MessageBox.Show(@"Данные ФИО уже добавлены по этому адресу.",
                                //            @"Ошибка при добавлении", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) !=
                                //            DialogResult.OK) e.Cancel = true;
                                //        Close();
                                //        e.Cancel = true;
                                //    }
                                //    AddAloneSql();
                                //}
                                //else
                                //{
                                //    RadMessageBox.Show("Заполните обязательные поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, error);
                                //    e.Cancel = true;
                                //}
                            }
                            else
                            {
                                if (!CompareJilUsl())
                                {
                                    string[] operation = JilUslEdit();
                                    AlertOperation("JilUslEdit", operation);
                                    if (operation[1] != "1")
                                        e.Cancel = true;
                                }

                                if (!CompareZemeln())
                                {
                                    string[] operation = ZemelnEdit();
                                    AlertOperation("ZemelnEdit", operation);
                                    if (operation[1] != "1")
                                        e.Cancel = true;
                                }
                            }
                            //UpdateComboBoxWoter();
                            //UpdateComboBoxPlita();
                            //UpdateComboBoxKanal();
                            //UpdateComboBoxOtopl();
                            //JilUslLoad();
                            //ZemelnLoad();
                        }
                        else
                        {
                            if (radPageView1.SelectedPage == radPageViewPage4)
                            {
                                if (this._status)
                                {
                                    CheackDateCategory();
                                }
                            }
                            else
                            {
                                if (radPageView1.SelectedPage == radPageViewPage5)
                                {
                                    //SurveyLoad();
                                    //UpdateComboBoxSpeziolist();
                                }
                                else
                                {
                                    //HelpLoad();
                                    //UpdateComboBoxHelp();
                                    //UpdateComboBoxSocRabotnik();
                                }
                            }
                        }
                    }
                }
            }
        }

        //на которую нажали
        private void radPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (radPageView1.SelectedPage == radPageViewPage1)
            {
                //AloneLoad();
                //CategoryLoad();
            }
            else
            {
                if (radPageView1.SelectedPage == radPageViewPage2)
                {
                    if (_status)
                    {
                        if (radioButton1.Checked)
                            radioButton3.Checked = true;
                        else
                        {
                            radioButton4.Checked = true;
                        }
                    }
                    else
                    {
                        //KinderLoad();
                        //SojitelLoad();
                        //KinderOther();
                        //UpdateRelative();
                    }
                }
                else
                {
                    if (radPageView1.SelectedPage == radPageViewPage3)
                    {
                        //UpdateComboBoxWoter();
                        //UpdateComboBoxPlita();
                        //UpdateComboBoxKanal();
                        //UpdateComboBoxOtopl();
                        //JilUslLoad();
                        //ZemelnLoad();
                    }
                    else
                    {
                        if (radPageView1.SelectedPage == radPageViewPage4)
                        {
                            if (!this._status)
                            {
                                CheackDateCategory();
                            }
                        }
                        else
                        {
                            if (radPageView1.SelectedPage == radPageViewPage5)
                            {
                                //SurveyLoad();
                                //UpdateComboBoxSpeziolist();
                            }
                            else
                            {
                                //HelpLoad();
                                //UpdateComboBoxHelp();
                                //UpdateComboBoxSocRabotnik();
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Над. обслуживание
        public void SocOperationLoad()
        {
            _loadGrid = true;
            var commandServer = new CommandServer();
            _bindingSocOperationGrid = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select * from SocOperationGrid(" + _keyAlone + ") order by [Дата]").Tables[0] };


            radGridView5.Invoke(new MethodInvoker(delegate ()
            {
                radGridView5.AutoSizeRows = true;
                radGridView5.DataSource = _bindingSocOperationGrid;
                radGridView5.Columns[0].IsVisible = false;

                GridViewComboBoxColumn comboColumn_soc_rab = new GridViewComboBoxColumn("соц. работник");

                comboColumn_soc_rab.DataSource = _bindingSocRabotnik;
                radGridView5.Columns[1] = comboColumn_soc_rab;
                comboColumn_soc_rab.FieldName = "fio";


                GridViewComboBoxColumn comboColumn_operation = new GridViewComboBoxColumn("операция");
                comboColumn_operation.DataSource = _bindingSocOperation;
                comboColumn_operation.Name = "operati";
                radGridView5.Columns[3] = comboColumn_operation;
                comboColumn_operation.FieldName = "operation";
                radGridView5.Columns[3].ReadOnly = true;

                GridViewDateTimeColumn dat = new GridViewDateTimeColumn("Дата");
                dat.Name = "date5";
                dat.FormatString = "{0:dd/MM/yyyy}";
                dat.Format = DateTimePickerFormat.Custom;
                radGridView5.Columns[2] = dat;
                dat.CustomFormat = "dd.MM.yyyy";

                radGridView5.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                radGridView5.Columns[1].WrapText = true;
                radGridView5.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                if(radGridView5.RowCount != 0)
                    _endOperationSoc = radGridView5.Rows[radGridView5.RowCount - 1].Cells[3].Value.ToString();

                radGridView5.CellEditorInitialized += new GridViewCellEventHandler(radGridView5_CellEditorInitialized);
            }));
            _loadGrid = false;
        }
        private void UpdateComboBoxSocOperation()
        {
            var commandServer = new CommandServer();
            _bindingSocOperation = new BindingList<string>(commandServer.GetComboBoxList(@"select operation.operation from operation", false));
        }

        private void UpdateComboBoxSocRabotnik()
        {
            _bindingSocRabotnik = new BindingList<string>(new CommandServer().GetComboBoxList(@"select sp.ФИО as [ФИО]
	            from spec_soc s inner join spezialistView(2,0) sp on s.fk_socRabotnik = sp.key_speziolist
	            order by sp.ФИО", true));
        }

        private void radGridView5_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name == "operati")
            {
                RadDropDownListEditor editors = (RadDropDownListEditor)e.ActiveEditor;
                RadDropDownListEditorElement elements = (RadDropDownListEditorElement)editors.EditorElement;

                switch (_endOperationSoc)
                {
                    case "принят":
                        elements.Items[0].Height = 0;
                        elements.Items[1].Height = -1;
                        elements.Items[2].Height = -1;
                        elements.Items[3].Height = 0;
                        //RadMessageBox.Show("принят", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                        break;
                    case "приостановлен":
                        elements.Items[0].Height = 0;
                        elements.Items[1].Height = -1;
                        elements.Items[2].Height = 0;
                        elements.Items[3].Height = -1;
                        //RadMessageBox.Show("приостановлен", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                        break;
                    case "снят":
                        elements.Items[0].Height = -1;
                        elements.Items[1].Height = 0;
                        elements.Items[2].Height = 0;
                        elements.Items[3].Height = 0;
                        //RadMessageBox.Show("снят", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                        break;
                    case "возобновлен":
                        elements.Items[0].Height = 0;
                        elements.Items[1].Height = -1;
                        elements.Items[2].Height = -1;
                        elements.Items[3].Height = 0;
                        //RadMessageBox.Show("возобновлен", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                        break;
                }
            }

            if (e.Column.HeaderText == "соц. работник")
            {
                RadDropDownListEditor editors = (RadDropDownListEditor)e.ActiveEditor;
                RadDropDownListEditorElement elements = (RadDropDownListEditorElement)editors.EditorElement;
                if (radGridView5.RowCount != 0)
                    elements.SelectedIndex = elements.FindString(radGridView5.Rows[radGridView5.RowCount - 1].Cells[1].Value.ToString());


            }

            RadDropDownListEditor editor = e.ActiveEditor as RadDropDownListEditor;

            if (editor == null)
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

        private void radGridView5_CellEditorInitialized_1(object sender, GridViewCellEventArgs e)
        {
            RadDateTimeEditor dateTimeEditor = e.ActiveEditor as RadDateTimeEditor;
            if (dateTimeEditor != null)
            {
                dateTimeEditor.MaxValue = DateTime.Now;
                radGridView5.CellEditorInitialized -= radGridView5_CellEditorInitialized_1;
                RadDateTimeEditorElement editroElement = (RadDateTimeEditorElement)dateTimeEditor.EditorElement;
                MaskDateTimeProvider provider = editroElement.TextBoxElement.Provider as MaskDateTimeProvider;
                if (provider != null)
                    provider.AutoSelectNextPart = true;
            }
        }

        private void radGridView5_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            _bindingSocOperationGrid = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select * from SocOperationGrid(" + _keyAlone + ") order by [Дата]").Tables[0] };
            radGridView5.Invoke(new MethodInvoker(delegate ()
            {
                radGridView5.DataSource = _bindingSocOperationGrid;
            }));
        }

        private void radGridView5_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var commandServer = new CommandServer();

            if (e.Rows[0].Cells[1].Value != null && e.Rows[0].Cells[2].Value != null && e.Rows[0].Cells[3].Value != null)
            {
                var parameters = _keyAlone + ",'" + e.Rows[0].Cells[1].Value.ToString() + "','" + e.Rows[0].Cells[2].Value.ToString() + "','" + e.Rows[0].Cells[3].Value.ToString() + "'";
                var returnSqlServer = commandServer.GetServerCommandExecReturnServer("nad_add", parameters);
                AlertGridOperation(sender, null, e, "nad_add " + parameters, returnSqlServer);
                if(returnSqlServer[1] == "1")
                {
                    _endOperationSoc = e.Rows[0].Cells[3].Value.ToString();
                }
            }
            else
            {
                RadMessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void radGridView5_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
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
                    if (e.PropertyName == "fio")
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
                        parameters += e.NewValue.ToString() + "'";
                    }
                    else
                    {
                        parameters += line.Cells[2].Value.ToString() + "'";
                    }

                    if (flag)
                    {
                        var returnSqlServer = commandServer.GetServerCommandExecReturnServer("nad_edit", parameters);
                        AlertGridOperation(sender, e, null, "nad_edit" + line.Cells[1].Value, returnSqlServer);
                    }
                }
            }
        }
        #endregion

        #region UpdateComboBox

        private void UpdateComboBoxHelp()
        {
            var commandServer = new CommandServer();
            _bindingHelp = new BindingList<string>(commandServer.GetComboBoxList(@"select typeHelp from typeHelp order by typeHelp", true));
        }

        private void UpdateComboBoxWoter()
        {
            var commandServer = new CommandServer();
            comboBox6.DataSource = commandServer.GetComboBoxList(@"select statusWoter from statusWoter", true);
        }

        private void UpdateComboBoxPlita()
        {
            var commandServer = new CommandServer();
            comboBox7.DataSource = commandServer.GetComboBoxList(@"select statusPlita from statusPlita", true);
        }

        private void UpdateComboBoxKanal()
        {
            var commandServer = new CommandServer();
            comboBox8.DataSource = commandServer.GetComboBoxList(@"select statusKanal from statusKanal", true);
        }

        private void UpdateComboBoxOtopl()
        {
            var commandServer = new CommandServer();
            comboBox9.DataSource = commandServer.GetComboBoxList(@"select statusOtopl from statusOtopl", true);
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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("addJilUsl", parameters);

            //if (returnSqlServer[1] != "Успешно") return returnSqlServer;
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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("addZemeln", parameters);

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
            var dt = commandServer.GetDataGridSet(@"select * from GetJilUsl(" + _keyAlone + ")").Tables[0];
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
            var dt = commandServer.GetDataGridSet(@"select * from GetZemeln(" + _keyAlone + ")").Tables[0];
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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("editJilsl", parameters);

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

            var returnSqlServer = commandServer.GetServerCommandExecReturnServer("editZemeln", parameters);

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
        private void radCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            dateTimePicker8.Enabled = radCheckBox1.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Enabled = checkBox8.Checked;
            comboBox10.Enabled = checkBox8.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox6.Enabled = checkBox2.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox7.Enabled = checkBox1.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            comboBox9.Enabled = checkBox4.Checked;
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker3.Enabled = checkBox27.Checked;

            //string[] s = textBox1.Text.Split(' ');
            //textBox1.Text = "";

            //for (int i = 0; i < s.Count(); i++)
            //{
            //    if (!string.IsNullOrEmpty(s[i]))
            //    {
            //        textBox1.Text += s[i].Substring(0, 1).ToUpper() + s[i].Substring(1, s[i].Length - 1).ToLower();
            //        if (i != s.Count() - 1)
            //        {
            //            if (!string.IsNullOrEmpty(s[i + 1]))
            //                textBox1.Text += " ";
            //        }
            //    }
            //}

            //if (_status)
            //{
            //    if (s.Count() > 2)
            //    {
            //        new search.Family(s[0], s[1], s[2], 0, "SearchFamily", "Поиск по фамилии").ShowDialog();
            //    }
            //}
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker4.Enabled = checkBox28.Checked;
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = checkBox26.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown2.Enabled = checkBox7.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
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

        private void AlertOperation(string operation, string[] resultOperation)
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
                Country = comboBox2.Text,
                DateRo = dateTimePicker1.Value,
                Fio = textBox1.Text,
                Phone = textBox3.Text,
                Street = textBox2.Text,
                SemPol = comboBox3.Text,
                PlaceWork = textBox12.Text,
                Dop = textBox10.Text
            };

            if (checkBox26.Checked)
                structuresAlone.DateSm = dateTimePicker2.Value;

            if (radioButton1.Checked)
                structuresAlone.Pol = 1;

            if (radCheckBox1.Checked)
                structuresAlone.DateExit = dateTimePicker8.Value;
            return _alone.Equals(structuresAlone);
        }

        private bool CompareSojitel()
        {
            var structuresSojitel = new StructuresSojitel
            {
                Fio = textBox6.Text,
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

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if(!_load)
                SendKeys.Send(".");
        }
    }
}