using System;
using System.Data;
using System.Windows.Forms;
using Telerik.Charting;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Statistic : RadForm
    {
        private BindingSource _bindingSource;
        //private BindingSource _bindingSourceLine;

        public Statistic()
        {
            InitializeComponent();
            radChartView1.ShowSmartLabels = true;
            LinesLoad();
        }

        private void PieSeriesLoad()
        {
            var commandServer = new CommandServer();
            PieSeries series = new PieSeries();
            _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select *
                from socPersonelViewColor()  order by [ФИО]").Tables[0] };
        }

        private void LinesLoad()
        {
            var commandServer = new CommandServer();
            BarSeries series = new BarSeries();
            DataSet ds = commandServer.GetDataGridSet(@"select max(c.y) as [возраст], count(*) as [количество]
                from (select floor(convert(int, getdate() - alone.date_ro)/365.25) as [y] 
	                from alone
	                where alone.date_exit is null and alone.date_sm is null) as c
                group by c.y
                order by [возраст]");

            foreach(DataRow v in ds.Tables[0].Rows)
            {
                series.DataPoints.Add(new CategoricalDataPoint(Convert.ToDouble(v.ItemArray[1]), v.ItemArray[0]));
            }
            radChartView1.Series.Add(series);
            //radChartView1.ShowToolTip = true;
            ChartTooltipController tooltipController = new ChartTooltipController();
            tooltipController.DataPointTooltipTextNeeded += tooltipController_DataPointTooltipTextNeeded;
            this.radChartView1.Controllers.Add(tooltipController);
            //radChartView1.Controllers.Add(new SmartLabelsController());

        }

        private void tooltipController_DataPointTooltipTextNeeded(object sender, DataPointTooltipTextNeededEventArgs e)
        {
            e.Text = e.Text.Replace("Category", "Возраст");
            e.Text = e.Text.Replace("Value", "Количество");
        }

        private void radChartView1_LabelFormatting(object sender, ChartViewLabelFormattingEventArgs e)
        {
            e.LabelElement.Text = "qeqweqweqewqweqeq";
        }
    }
}
