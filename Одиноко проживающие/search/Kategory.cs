using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CheckBox = System.Windows.Forms.CheckBox;

namespace Одиноко_проживающие.search
{
    public partial class Kategory : Form
    {
        private const string HeadOne =
            @"select max(alone.key_alone), max(selsovet.selsovet) as selsovet, max(country.country) as country, 
	max(alone.fio) as [ФИО], max(alone.date_ro) as [дата рождения], max(country.country) + ' ' + max(alone.street) as [адрес],
	max(protivopojar.ApiDate) as [АПИ], max(protivopojar.SzuDate) as [СЗУ]";
        private const string HeadOneNo =
            @"select alone.key_alone, selsovet.selsovet as selsovet, country.country as country, 
	alone.fio as [ФИО], alone.date_ro as [дата рождения], country.country + ' ' + alone.street as [адрес],
	protivopojar.ApiDate as [АПИ], protivopojar.SzuDate as [СЗУ], category.category as [категория]";
        private string _requestGroup = @" from alone inner join category on alone.key_alone = category.fk_alone 
	        inner join country on alone.fk_country = country.key_country 
	        inner join selsovet on selsovet.key_selsovet = country.fk_selsovet 
	        left join protivopojar on protivopojar.fk_alone = alone.key_alone";
        private const string RequestWhereParam = " join category c";
        private const string Where = @" where alone.date_sm is null";
        private const string Group = @" group by alone.key_alone, selsovet.selsovet, country.country, alone.fio, alone.date_ro, alone.street, 
protivopojar.ApiDate, protivopojar.SzuDate ";
        private int _count;
        private int _countTwo;
        private readonly List<KategoryNumber> _list = new List<KategoryNumber>();
        private bool _flagDetail; //true - подробнее (показать категории)
        private readonly bool _statusLoad;
        private int _operation;


        private const string Head = @"select max(t.key_alone), max(t.selsovet) as selsovet, max(t.country) as country, 
		max(t.fio) as [ФИО], max(t.date_ro) as [дата рождения], max(t.countrys) as [адрес], 
		max(t.ApiDate) as [АПИ], max(t.SzuDate) as [СЗУ] from (";
        private const string HeadTwo = @"select t.key_alone, t.selsovet as selsovet, t.country as country, 
		t.fio as [ФИО], t.date_ro as [дата рождения], t.countrys as [адрес], 
		t.ApiDate as [АПИ], t.SzuDate as [СЗУ], t.category as [категория] from (";
        private const string Body = @"select alone.key_alone, selsovet.selsovet, country.country, alone.fio, alone.date_ro, 
		(country.country + ' ' + alone.street) as countrys, protivopojar.ApiDate, protivopojar.SzuDate, category.category
	from alone inner join category on alone.key_alone = category.fk_alone 
		inner join country on alone.fk_country = country.key_country 
		inner join selsovet on selsovet.key_selsovet = country.fk_selsovet 
		left join protivopojar on protivopojar.fk_alone = alone.key_alone 
	where alone.date_sm is null and category.category = '";
        private static string _bodyResult;
        private const string Union = @" union all ";
        private const string Footer = @") as t ";
        private const string Groups = @" group by t.key_alone, t.selsovet, t.countrys, t.fio, t.date_ro, t.country, 
	t.ApiDate, t.SzuDate  
order by t.fio";

        public Kategory()
        {
            InitializeComponent();
            dataGridView1.DataSource = null;
            _statusLoad = true;
            UpdateDateRoSerer();
            UpdateComboBoxSelsovet();
            _operation = 1;

            toolStripStatusLabel1.Text = "";
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            _statusLoad = false;
        }

        private static void UpdateDateRoSerer()
        {
            var commandServer = new CommandServer();
            commandServer.GetServerCommandExecNoReturnServer("UpdateDateRo", "");
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            _operation = 1;
            string param = null;
            var checkBox = (CheckBox) sender;

            if (checkBox.Checked)
            {
                _count++;
                _list.Add(new KategoryNumber(checkBox.Text, _count));
                param = RequestWhereParam + _count + @" on alone.key_alone = c" + _count + @".fk_alone and c" + _count + @".category = '" + checkBox.Text + "' ";
            }

            if (checkBox.Checked)
            {
                _requestGroup += param;
            }
            else
            {
                var number = _list.Find(kategoryNumber => kategoryNumber.Name == checkBox.Text);
                _list.Remove(number);
                var find = RequestWhereParam + number.Number + @" on alone.key_alone = c" + number.Number + @".fk_alone and c" + number.Number + @".category = '" + checkBox.Text + "' ";
                var n = _requestGroup.IndexOf(find, StringComparison.Ordinal);
                _requestGroup = _requestGroup.Remove(n, find.Length);
            }
            
            UpdateDateGrid(_requestGroup);
        }

        private void checkBox_Checked(object sender, EventArgs e)
        {
            _operation = 2;
            var checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                _countTwo++;
                if (_countTwo == 1)
                    _bodyResult = Body + checkBox.Text + "'";
                else
                    _bodyResult += Union + Body + checkBox.Text + "'";
            }else{
                _countTwo--;

                if (_countTwo == 0)
                {
                    _operation = 1;
                    UpdateDateGrid(_requestGroup);
                    return;
                }
                    
                var n = _bodyResult.IndexOf(Union + Body + checkBox.Text + "'", StringComparison.Ordinal);
                var len = (Union + Body + checkBox.Text + "'").Length;

                if (n == -1)
                {
                    len = (Body + checkBox.Text + "'" + Union).Length;
                    n = _bodyResult.IndexOf(Body + checkBox.Text + "'" + Union, StringComparison.Ordinal);

                    if (n == -1)
                    {
                        len = (Body + checkBox.Text + "'").Length;
                        n = _bodyResult.IndexOf(Body + checkBox.Text + "'", StringComparison.Ordinal);
                        _bodyResult = _bodyResult.Remove(n, len);
                    }else
                        _bodyResult = _bodyResult.Remove(n, len);
                }
                else
                {
                    _bodyResult = _bodyResult.Remove(n, len);
                }
            }

            UpdateDateGridTwo(_bodyResult);
        }

        private void UpdateDateGridTwo(string request)
        {
            Cursor = Cursors.AppStarting;
            string commandReq;

            if (!_flagDetail)
            {
                commandReq = Head + request + Footer;

                if (radioButton1.Checked)
                    commandReq = commandReq + @"where t.ApiDate is not null";
                else
                {
                    if (radioButton2.Checked)
                        commandReq = commandReq + @"where t.SzuDate is not null";
                }
                commandReq = commandReq + Groups;
            }
            else
            {
                commandReq = HeadTwo + request + Footer;
                if (radioButton1.Checked)
                    commandReq = commandReq + @"where t.ApiDate is not null";
                else
                {
                    if (radioButton2.Checked)
                        commandReq = commandReq + @"where t.SzuDate is not null";
                }
                commandReq += " order by t.fio";
            }

            var commandServer = new CommandServer();
            var bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(commandReq).Tables[0] };

            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                if (!string.IsNullOrEmpty(comboBox1.Text))
                {
                    bindingSource.Filter = string.Format("selsovet = '{0}'", comboBox1.Text);
                }
            }
            else
            {
                bindingSource.Filter = string.Format("country = '{0}'", comboBox2.Text);
            }

            dataGridView1.DataSource = bindingSource;
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;

                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                if(dataGridView1.ColumnCount == 9)
                    dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            toolStripStatusLabel1.Text = @"Записей: " + dataGridView1.Rows.Count;
            Cursor = Cursors.Arrow;
        }

        private void UpdateDateGrid(string request)
        {
            Cursor = Cursors.AppStarting;
            if (string.IsNullOrEmpty(request))
                request = _requestGroup;
            string commandReq = request;

            if (!_flagDetail)
            {
                commandReq = HeadOne + commandReq;
                //commandReq = commandReq.Replace(@", category.category as [категория]", @"/*, category.category as [категория]*/");
                if(radioButton1.Checked)
                    commandReq = commandReq + Where + @" and protivopojar.ApiDate is not null" + Group +
                              @" order by alone.fio";
                else
                {
                    if (radioButton2.Checked)
                        commandReq = commandReq + Where + @" and protivopojar.SzuDate is not null" + Group +
                                  @"order by alone.fio";
                    else
                        commandReq = commandReq + Where + Group + @" order by alone.fio";
                }
            }
            else
            {
                //commandReq = commandReq.Replace("max(", "");
                //commandReq = commandReq.Replace(")", "");
                commandReq = HeadOneNo + commandReq;
                //commandReq = commandReq.Replace(@"/*, category.category as [категория]*/", ", category.category as [категория]");
                if (radioButton1.Checked)
                    commandReq = commandReq + @" and protivopojar.ApiDate is not null order by alone.fio";
                else
                {
                    if (radioButton2.Checked)
                        commandReq = commandReq + @" and protivopojar.SzuDate is not null order by alone.fio";
                    else
                        commandReq = commandReq + @" order by alone.fio";
                }
            }

            var commandServer = new CommandServer();
            var bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(commandReq).Tables[0] };

            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                if (!string.IsNullOrEmpty(comboBox1.Text))
                {
                    bindingSource.Filter = string.Format("selsovet = '{0}'", comboBox1.Text);
                }
            }
            else
            {
                bindingSource.Filter = string.Format("country = '{0}'", comboBox2.Text);
            }

            dataGridView1.DataSource = bindingSource;
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;

                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                if (dataGridView1.ColumnCount == 9)
                    dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
           
            toolStripStatusLabel1.Text = @"Записей: " + dataGridView1.Rows.Count;
            Cursor = Cursors.Arrow;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            Hide();
            new AddAlone(false, Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value)).ShowDialog();
            Show();
        }

        private void UpdateComboBoxSelsovet()
        {
            var commandServer = new CommandServer();
            comboBox1.DataSource = commandServer.GetComboBoxList(@"select Selsovet from Selsovet order by Selsovet");
        }

        private void UpdateComboBoxNasPunct()
        {
            var commandServer = new CommandServer();

            comboBox2.DataSource = commandServer.GetComboBoxList(@"select country
                    from country inner join selsovet on 
                        country.fk_selsovet = selsovet.key_selsovet
                    where selsovet.selsovet = " + "'" + comboBox1.Text + "'");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxNasPunct();
            if (_statusLoad) return;
            if (_operation == 1)
                UpdateDateGrid(_requestGroup);
            else
                UpdateDateGridTwo(_bodyResult);
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_statusLoad) return;
            if (_operation == 1)
                UpdateDateGrid(_requestGroup);
            else
                UpdateDateGridTwo(_bodyResult);
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            Cursor = Cursors.AppStarting;
            Microsoft.Office.Interop.Excel.Application excelApp = new ApplicationClass();
            try
            {
                excelApp.Application.Workbooks.Add(Type.Missing);
                excelApp.Cells[1, 1] = "№ п/п";
                excelApp.Cells[1, 2] = "ФИО";
                excelApp.Cells[1, 3] = "Дата рождения";
                excelApp.Cells[1, 4] = "Адрес";
                excelApp.Cells[1, 5] = "АПИ";
                excelApp.Cells[1, 6] = "СЗУ";
                if(_flagDetail)
                    excelApp.Cells[1, 7] = "Категория";

                for (var i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (i == 1 || i == 2) continue;
                    for (var j = 0; j < dataGridView1.RowCount; j++)
                    {
                        
                        switch (i)
                        {
                            case 0:
                                excelApp.Cells[j + 2, i + 1] = (j + 1).ToString();
                                break;
                            case 2:
                                excelApp.Cells[j + 2, i - 1] = dataGridView1[i, j].Value.ToString();
                                break;
                            case 4:
                                excelApp.Cells[j + 2, i - 1] = dataGridView1[i, j].Value.ToString().Split(' ')[0];
                                break;
                            default:
                                excelApp.Cells[j + 2, i - 1] = dataGridView1[i, j].Value.ToString();
                                break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"Ошибка", @"Произошла ошибка при заполнении данных в таблицу.", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                new CommandClient().WriteFileError(exception, null);
            }
            finally
            {
                excelApp.Columns.EntireColumn.AutoFit();
                excelApp.Visible = true;
                Cursor = Cursors.Arrow;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (_operation == 1)
                UpdateDateGrid(_requestGroup);
            else
                UpdateDateGridTwo(_bodyResult);
        }

        private void показатьКатегорииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_flagDetail)
            {
                _flagDetail = false;
                показатьКатегорииToolStripMenuItem.Text = @"Показать категории";
            }
            else
            {
                _flagDetail = true;
                показатьКатегорииToolStripMenuItem.Text = @"Скрыть категории";
            }

            if(_operation == 1)
                UpdateDateGrid(_requestGroup);
            else
                UpdateDateGridTwo(_bodyResult);
        }
    }

    public class KategoryNumber
    {
        public string Name;
        public int Number;

        public KategoryNumber(string name, int number)
        {
            Name = name;
            Number = number;
        }
    }
}
