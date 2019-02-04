using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие.menu
{
    public partial class SocRabotnik : RadForm
    {
        #region переменные
        private BindingSource _bindingSourceSoc;
        private BindingSource _bindingSourcePer;
        private bool _load;
        private static string queryHead = @"WITH CTE AS
                (
                 SELECT alone.key_alone, family.family + ' ' + family.name + ' ' + family.surname as [FIO],
                    alone.date_ro, country.country,
                    (every_set.name + ' ' + adres.street +
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

                    end) as [adres], country.fk_selsovet, nad_obsl.fk_soc_rab, nad_obsl.fk_operation,
					nad_obsl.date_operation, N=ROW_NUMBER()OVER(PARTITION BY alone.key_alone, family.family, family.name, family.surname, alone.date_ro, country.country, alone.street
                    ORDER BY nad_obsl.date_operation DESC)
                 FROM alone
                 join family on family.fk_alone = alone.key_alone
                 JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
                 JOIN country ON alone.fk_country = country.key_country
                 inner join adres on adres.fk_alone = alone.key_alone
                 inner join every_set on every_set.key_every = adres.fk_every
                ) ";
        private static string queryTail = @"
                FROM CTE
                JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
                JOIN speziolist soc ON soc.key_speziolist = CTE.fk_soc_rab
                JOIN operation ON operation.key_operation = CTE.fk_operation
                join spec_soc on spec_soc.fk_socRabotnik = soc.key_speziolist
                join speziolist spez on spez.key_speziolist = spec_soc.fk_speciolist";

        private static string gridHead = @"WITH CTE AS
	            (
		            SELECT *, N=ROW_NUMBER()OVER(PARTITION BY alone.key_alone 
		            ORDER BY nad_obsl.date_operation DESC)
		            FROM alone JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
			            JOIN country ON alone.fk_country = country.key_country
	            )
	            SELECT CTE.fk_alone, (family.family + ' ' + family.name + ' ' + family.surname) as [ФИО], 
		            CTE.date_ro as [Дата рожд.], CTE.country as [Нас. пункт], 
		            (every_set.name + ' ' + adres.street +
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
			            end) as [Адрес], 
		            CTE.date_operation as [Дата опер.]
	            FROM CTE
	            JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
	            JOIN speziolist ON speziolist.key_speziolist = CTE.fk_soc_rab
	            JOIN operation ON operation.key_operation = CTE.fk_operation
	            inner join family on family.fk_alone = CTE.key_alone
	            join adres on adres.fk_alone = CTE.key_alone
	            join every_set on key_every = fk_every";

        #endregion

        public SocRabotnik()
        {
            InitializeComponent();
            //radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView1.ClearSelection();
            HandleCreated += Form_HandleCreated;
        }

        private void Form_HandleCreated(object sender, EventArgs e)
        {
            var thread = new Thread(LoadSpezialist);
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            thread.IsBackground = true;
            thread.Start();
        }

        private void LoadSpezialist()
        {
            _load = true;
            var commandServer = new CommandServer();
            try
            {
                radGridView5.Invoke(new MethodInvoker(delegate
                {
                    _bindingSourcePer = new BindingSource { DataSource = commandServer.DataGridSet(@"select key_speziolist, fio as [ФИО специалист]
                        from spec_soc inner join speziolist on spec_soc.fk_speciolist = speziolist.key_speziolist
                        group by key_speziolist, fio order by fio").Tables[0] };

                    radGridView5.DataSource = _bindingSourcePer;

                    if (radGridView5.Columns.Count > 0)
                    {
                        radGridView5.Columns[0].IsVisible = false;
                    }
                }));
            }
            catch (Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
            _load = false;
        }

        /*Загрузка дел*/
        private void LoadAlone(object sender, EventArgs e)
        {
            if (_load) return;
            var commandServer = new CommandServer();
            var bindingSourceRun = new BindingSource { DataSource = commandServer.DataGridSet(gridHead + @"
                WHERE CTE.N=1 and speziolist.fio = '" + radGridView1.CurrentRow.Cells[1].Value + "' and (operation.operation = 'принят' or operation.operation = 'возобновлен')").Tables[0] };

            radGridView2.DataSource = bindingSourceRun;

            if(radGridView2.Rows.Count != 0)
            {
                radGridView2.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                radGridView2.Columns[5].FormatString = "{0:dd/MM/yyyy}";
                radPageViewPage1.Text = "Действующие (" + radGridView2.RowCount + ")";
            }else
            {
                radPageViewPage1.Text = "Действующие";
            }
            radGridView2.Columns[0].IsVisible = false;
            radGridView2.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView2.Columns["ФИО"].BestFit();
            radGridView2.Columns["Дата рожд."].BestFit();
            radGridView2.Columns["Дата опер."].BestFit();
            radGridView2.Columns["Нас. пункт"].BestFit();

            var bindingSourcePause = new BindingSource { DataSource = commandServer.DataGridSet(gridHead + @"
                WHERE CTE.N=1 and speziolist.fio = '" + radGridView1.CurrentRow.Cells[1].Value + "' and operation.operation = 'приостановлен'").Tables[0] };

            radGridView3.DataSource = bindingSourcePause;

            if (radGridView3.Rows.Count != 0)
            {
                radGridView3.Columns[2].FormatString = "{0:dd/MM/yyyy}";
                radGridView3.Columns[5].FormatString = "{0:dd/MM/yyyy}";
                radPageViewPage2.Text = "Приостановленные (" + radGridView3.RowCount + ")";
            }
            else
            {
                radPageViewPage2.Text = "Приостановленные";
            }
            radGridView3.Columns[0].IsVisible = false;
            radGridView3.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView3.Columns["ФИО"].BestFit();
            radGridView3.Columns["Дата рожд."].BestFit();
            radGridView3.Columns["Дата опер."].BestFit();
            radGridView3.Columns["Нас. пункт"].BestFit();
        }

        /*Загрузка социальных работников*/
        private void LoadSocRab(object sender, EventArgs e)
        {            
            if(radGridView5.CurrentRow != null)
            {
                _bindingSourceSoc = new BindingSource
                {
                    DataSource = new CommandServer().DataGridSet(@"select key_speziolist, fio as [ФИО соц. раб.]
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
        private void PrintAll(object sender, EventArgs e)
        {
            var bindingSourceStop = new BindingSource { DataSource = new CommandServer().DataGridSet(queryHead +
                @"SELECT spez.fio, soc.fio, CTE.fio, CTE.date_ro, CTE.country, CTE.adres, 
                    operation.operation , CTE.date_operation" + queryTail + @"
                WHERE CTE.N=1 and (operation.operation = 'принят' or operation.operation = 'приостановлен' or operation.operation = 'возобновлен')
                order by spez.fio, soc.fio, CTE.fio").Tables[0] };

            new Print().ExcelAllSoc(bindingSourceStop);
        }

        //специалиста
        private void PrintSpec(object sender, EventArgs e)
        {
            var bindingSourceStop = new BindingSource { DataSource = new CommandServer().DataGridSet(queryHead +
                @"SELECT soc.fio, CTE.fio, CTE.date_ro, CTE.country, CTE.adres, 
                    operation.operation , CTE.date_operation" + queryTail + @"
                WHERE CTE.N=1 and (operation.operation = 'принят' or operation.operation = 'приостановлен' or operation.operation = 'возобновлен')  and spez.fio = '" +
                radGridView5.CurrentRow.Cells[1].Value + "' order by soc.fio, CTE.fio").Tables[0] };

            new Print().ExcelSoc(radGridView5.CurrentRow.Cells[1].Value.ToString(), bindingSourceStop);
        }

        private void radGridView2_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1) return;
            Hide();
            try
            {
                new Alone(false, Convert.ToInt32(e.Row.Cells[0].Value)).ShowDialog();
            }
            catch (Exception ex)
            {
                var commandClient = new CommandClient();
                commandClient.WriteFileError(ex, null);
            }
            Show();
        }
    }
}
