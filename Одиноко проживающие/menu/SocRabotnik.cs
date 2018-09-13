using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.menu
{
    public partial class SocRabotnik : RadForm
    {
        private BindingSource _bindingSourceSoc;
        private BindingSource _bindingSourcePer;
        private bool _load;

        public SocRabotnik()
        {
            InitializeComponent();
            //radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView1.ClearSelection();
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            Thread _thread = new Thread(new ThreadStart(fillTheDataGridSoc));
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void fillTheDataGridSoc()
        {
            _load = true;
            var commandServer = new CommandServer();
            try
            {
                radGridView5.Invoke(new MethodInvoker(delegate ()
                {
                    _bindingSourcePer = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select key_speziolist, fio as [ФИО специалист]
                        from spec_soc inner join speziolist on spec_soc.fk_speciolist = speziolist.key_speziolist
                        group by key_speziolist, fio order by fio").Tables[0] };

                    radGridView5.DataSource = _bindingSourcePer;

                    if (radGridView5.Columns.Count > 0)
                    {
                        radGridView5.Columns[0].IsVisible = false;
                        //radGridView5.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        //radGridView5.SelectionMode = GridViewSelectionMode.FullRowSelect;
                    }

                    //radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //radGridView1.SelectionMode = GridViewSelectionMode.FullRowSelect;
                }));
            }
            catch (Exception ex)
            {
                CommandClient commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
            _load = false;
        }

        private void radGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if(!_load)
            {
                var commandServer = new CommandServer();
                BindingSource _bindingSourceRun = new BindingSource { DataSource = commandServer.GetDataGridSet(@"WITH CTE AS
                (
                 SELECT *, N=ROW_NUMBER()OVER(PARTITION BY alone.fio, alone.date_ro, country.country, alone.street 
                    ORDER BY nad_obsl.date_operation DESC)
                 FROM alone
                 JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
                 JOIN country ON alone.fk_country = country.key_country
                )
                SELECT CTE.fio as [ФИО], CTE.date_ro as [Дата рождения], CTE.country as [Нас. пункт], CTE.street as [Адрес], 
                CTE.date_operation as [Дата операции]
                FROM CTE
                JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
                JOIN speziolist ON speziolist.key_speziolist = CTE.fk_soc_rab
                JOIN operation ON operation.key_operation = CTE.fk_operation
                WHERE CTE.N=1 and speziolist.fio = '" + radGridView1.CurrentRow.Cells[1].Value + "' and (operation.operation = 'принят' or operation.operation = 'возобновлен')").Tables[0] };

                radGridView2.DataSource = _bindingSourceRun;

                if(radGridView2.Rows.Count != 0)
                {
                    radGridView2.Columns[1].FormatString = "{0:dd/MM/yyyy}";
                    radGridView2.Columns[4].FormatString = "{0:dd/MM/yyyy}";
                    radPageViewPage1.Text = "Действующие (" + radGridView2.RowCount + ")";
                }else
                {
                    radPageViewPage1.Text = "Действующие";
                }

                BindingSource _bindingSourcePause = new BindingSource { DataSource = commandServer.GetDataGridSet(@"WITH CTE AS
                (
                 SELECT *, N=ROW_NUMBER()OVER(PARTITION BY alone.fio, alone.date_ro, country.country, alone.street 
                    ORDER BY nad_obsl.date_operation DESC)
                 FROM alone
                 JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
                 JOIN country ON alone.fk_country = country.key_country
                )
                SELECT CTE.fio as [ФИО], CTE.date_ro as [Дата рождения], CTE.country as [Нас. пункт], CTE.street as [Адрес], 
                CTE.date_operation as [Дата операции]
                FROM CTE
                JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
                JOIN speziolist ON speziolist.key_speziolist = CTE.fk_soc_rab
                JOIN operation ON operation.key_operation = CTE.fk_operation
                WHERE CTE.N=1 and speziolist.fio = '" + radGridView1.CurrentRow.Cells[1].Value + "' and operation.operation = 'приостановлен'").Tables[0] };

                radGridView3.DataSource = _bindingSourcePause;

                if (radGridView3.Rows.Count != 0)
                {
                    radGridView3.Columns[1].FormatString = "{0:dd/MM/yyyy}";
                    radGridView3.Columns[4].FormatString = "{0:dd/MM/yyyy}";
                    radPageViewPage2.Text = "Приостановленные (" + radGridView3.RowCount + ")";
                }
                else
                {
                    radPageViewPage2.Text = "Приостановленные";
                }

                BindingSource _bindingSourceStop = new BindingSource { DataSource = commandServer.GetDataGridSet(@"WITH CTE AS
                (
                 SELECT *, N=ROW_NUMBER()OVER(PARTITION BY alone.fio, alone.date_ro, country.country, alone.street 
                    ORDER BY nad_obsl.date_operation DESC)
                 FROM alone
                 JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
                 JOIN country ON alone.fk_country = country.key_country
                )
                SELECT CTE.fio as [ФИО], CTE.date_ro as [Дата рождения], CTE.country as [Нас. пункт], CTE.street as [Адрес], 
                CTE.date_operation as [Дата операции]
                FROM CTE
                JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
                JOIN speziolist ON speziolist.key_speziolist = CTE.fk_soc_rab
                JOIN operation ON operation.key_operation = CTE.fk_operation
                WHERE CTE.N=1 and speziolist.fio = '" + radGridView1.CurrentRow.Cells[1].Value + "' and operation.operation = 'снят'").Tables[0] };

                radGridView4.DataSource = _bindingSourceStop;

                if (radGridView4.Rows.Count != 0)
                {
                    radGridView4.Columns[1].FormatString = "{0:dd/MM/yyyy}";
                    radGridView4.Columns[4].FormatString = "{0:dd/MM/yyyy}";
                    radPageViewPage3.Text = "Завершенные (" + radGridView4.RowCount + ")";
                }
                else
                {
                    radPageViewPage3.Text = "Завершенные";
                }
            }
        }

        private void radGridView5_SelectionChanged(object sender, EventArgs e)
        {            
            if(radGridView5.CurrentRow != null)
            {
                _bindingSourceSoc = new BindingSource
                {
                    DataSource = new CommandServer().GetDataGridSet(@"select key_speziolist, fio as [ФИО соц. раб.]
                    from spec_soc inner join speziolist on spec_soc.fk_socRabotnik = speziolist.key_speziolist
                    where fk_speciolist = " + radGridView5.CurrentRow.Cells[0].Value +
                    " order by fio").Tables[0]
                };

                radGridView1.DataSource = _bindingSourceSoc;

                if (radGridView1.Columns.Count > 0)
                {
                    radGridView1.Columns[0].IsVisible = false;
                    
                }
            }            
        }

        private void SocRabotnik_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //всех
        private void radButton1_Click(object sender, EventArgs e)
        {
            BindingSource _bindingSourceStop = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"WITH CTE AS
            (
             SELECT *, N=ROW_NUMBER()OVER(PARTITION BY alone.fio, alone.date_ro, country.country, alone.street 
                ORDER BY nad_obsl.date_operation DESC)
             FROM alone
             JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
             JOIN country ON alone.fk_country = country.key_country
            )
            SELECT spez.fio, soc.fio, CTE.fio, CTE.date_ro, CTE.country, CTE.street, 
            operation.operation , CTE.date_operation
            FROM CTE
            JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
            JOIN speziolist soc ON soc.key_speziolist = CTE.fk_soc_rab
            JOIN operation ON operation.key_operation = CTE.fk_operation
            join spec_soc on spec_soc.fk_socRabotnik = soc.key_speziolist
            join speziolist spez on spez.key_speziolist = spec_soc.fk_speciolist
            WHERE CTE.N=1 and (operation.operation = 'принят' or operation.operation = 'приостановлен' or operation.operation = 'возобновлен')
            order by spez.fio, soc.fio, CTE.fio").Tables[0] };

            new Print().ExcelAllSoc(_bindingSourceStop);
        }

        //специалиста
        private void radButton2_Click(object sender, EventArgs e)
        {
            BindingSource _bindingSourceStop = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"WITH CTE AS
            (
             SELECT *, N=ROW_NUMBER()OVER(PARTITION BY alone.fio, alone.date_ro, country.country, alone.street 
                ORDER BY nad_obsl.date_operation DESC)
             FROM alone
             JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
             JOIN country ON alone.fk_country = country.key_country
            )
            SELECT soc.fio, CTE.fio, CTE.date_ro, CTE.country, CTE.street, 
            operation.operation , CTE.date_operation
            FROM CTE
            JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
            JOIN speziolist soc ON soc.key_speziolist = CTE.fk_soc_rab
            JOIN operation ON operation.key_operation = CTE.fk_operation
            join spec_soc on spec_soc.fk_socRabotnik = soc.key_speziolist
            join speziolist spez on spez.key_speziolist = spec_soc.fk_speciolist
            WHERE CTE.N=1 and (operation.operation = 'принят' or operation.operation = 'приостановлен' or operation.operation = 'возобновлен')  and spez.fio = '" +
            radGridView5.CurrentRow.Cells[1].Value + "' order by soc.fio, CTE.fio").Tables[0] };

            new Print().ExcelSoc(radGridView5.CurrentRow.Cells[1].Value.ToString(), _bindingSourceStop);
        }
    }
}
