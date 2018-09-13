using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Statistic : RadForm
    {
        //private BindingSource _bindingSource;
        TelerikMetroTheme theme = new TelerikMetroTheme();

        public Statistic()
        {
            InitializeComponent();
            ThemeResolutionService.ApplyThemeToControlTree(this, theme.ThemeName);
            YearsLoad();
            Soc();
        }

        private void PieSeriesLoad()
        {
            //var commandServer = new CommandServer();
            //PieSeries series = new PieSeries();
            //_bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
            //    from socPersonelViewColor()  order by [ФИО]").Tables[0] };
        }

        private void YearsLoad()
        {
            var commandServer = new CommandServer();
            BindingSource _bindingSourceRun = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select max(c.y) as [возраст (лет)], count(*) as [количество]
                from (select floor(convert(int, getdate() - alone.date_ro)/365.25) as [y] 
	                from alone
	                where alone.date_exit is null and alone.date_sm is null) as c
                group by c.y
                order by [возраст (лет)]").Tables[0] };
            radGridView1.DataSource = _bindingSourceRun;
        }

        private void Statistic_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void Soc()
        {
            BindingSource _bindingSourceStart = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"WITH CTE AS
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
            WHERE CTE.N=1 and (operation.operation = 'принят' or operation.operation = 'возобновлен')
            order by soc.fio").Tables[0] };

            label3.Text = _bindingSourceStart.Count.ToString();

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
            WHERE CTE.N=1 and (operation.operation = 'приостановлен')
            order by soc.fio").Tables[0] };

            label4.Text = _bindingSourceStop.Count.ToString();
        }

        private void Obsch()
        {
            BindingSource _bindingSource = new BindingSource { DataSource = new CommandServer().GetDataGridSet(@"select *
            from alone inner join category on category.fk_alone = alone.key_alone
            where podCategory = 'пенсионер'").Tables[0] };
            label6.Text = _bindingSource.Count.ToString();


        }


        //        CREATE FUNCTION Statistik()
        //RETURNS
        //@table TABLE
        //(
        //    cat nvarchar(100),
        //	name nvarchar(100),
        //	caun int
        //) 
        //AS
        //begin

        //    insert @table

        //    select*
        //    from(
        //    WITH CTE AS
        //    (
        //     SELECT*, N= ROW_NUMBER()OVER(PARTITION BY alone.fio, alone.date_ro, country.country, alone.street
        //        ORDER BY nad_obsl.date_operation DESC)
        //     FROM alone
        //     JOIN nad_obsl ON alone.key_alone = nad_obsl.fk_alone
        //     JOIN country ON alone.fk_country = country.key_country
        //    )
        //    SELECT CTE.fio as [ФИО], CTE.date_ro as [Дата рождения], CTE.country as [Нас.пункт], CTE.street as [Адрес],
        //    CTE.date_operation as [Дата операции]
        //    FROM CTE
        //    JOIN selsovet ON selsovet.key_selsovet = CTE.fk_selsovet
        //    JOIN speziolist ON speziolist.key_speziolist = CTE.fk_soc_rab
        //    JOIN operation ON operation.key_operation = CTE.fk_operation
        //    WHERE CTE.N= 1 and operation.operation = 'принят')


        //	return
        //end

    }
}
