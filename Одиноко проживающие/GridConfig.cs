using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public class GridConfigSurveyCellElement : GridGroupContentCellElement
    {
        private RadDropDownListElement dropDown;
        private LightVisualElement textElement;
        private LightVisualElement aggregateElement;
        private StackLayoutElement stack;
        public GridConfigSurveyCellElement(GridViewColumn column, GridRowElement row) : base(column, row)
        {
        }

        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(GridGroupContentCellElement);
            }
        }

        protected override void CreateChildElements()
        {
            base.CreateChildElements();
            stack = new StackLayoutElement();
            stack.Orientation = Orientation.Horizontal;
            stack.StretchHorizontally = true;
            stack.Margin = new Padding(0, 2, 5, 0);
            textElement = new LightVisualElement();
            dropDown = new RadDropDownListElement();
            dropDown.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            aggregateElement = new LightVisualElement();
            dropDown.SelectedIndexChanged += SelectedIndexChanged;
            dropDown.Items.AddRange(new List<string>(new string[]
            {
            "Sum",
            "Avg",
            "Min",
            "Max",
            "Count",
            "Last"
            }));
            stack.Children.Add(textElement);
            stack.Children.Add(dropDown);
            stack.Children.Add(aggregateElement);
            this.Children.Add(stack);
        }

        public override void SetContent()
        {
            base.SetContent();
            GridViewGroupRowInfo row = this.RowInfo as GridViewGroupRowInfo;
            if (row != null)
            {
                this.textElement.Text = row.HeaderText;
                if (row.Tag != null)
                {
                    dropDown.SelectedIndexChanged -= SelectedIndexChanged;
                    dropDown.SelectedIndex = (int)row.Tag;
                    dropDown.SelectedIndexChanged += SelectedIndexChanged;
                }
                else
                {
                    dropDown.SelectedIndexChanged -= SelectedIndexChanged;
                    dropDown.SelectedIndex = -1;
                    dropDown.SelectedIndexChanged += SelectedIndexChanged;
                }
            }
            switch (dropDown.Text)
            {
                case "Sum":
                    this.aggregateElement.Text = GetSum(row).ToString();
                    break;
                case "Avg":
                    this.aggregateElement.Text = (GetSum(row) / row.ChildRows.Count).ToString();
                    break;
                case "Min":
                    this.aggregateElement.Text = GetMin(row).ToString();
                    break;
                case "Max":
                    this.aggregateElement.Text = GetMax(row).ToString();
                    break;
                case "Count":
                    this.aggregateElement.Text = row.ChildRows.Count.ToString();
                    break;
                case "Last":
                    this.aggregateElement.Text = row.ChildRows.Last().Cells["ProductName"].Value.ToString();
                    break;
                default:
                    this.aggregateElement.Text = "No aggregate function";
                    break;
            }
            this.Text = string.Empty;
        }
        private void SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            this.RowInfo.Tag = dropDown.SelectedIndex;
        }
        private decimal GetSum(GridViewGroupRowInfo row)
        {
            decimal sum = 0;
            foreach (GridViewRowInfo childRow in row.ChildRows)
            {
                sum = sum + (decimal)childRow.Cells["UnitPrice"].Value;
            }
            return sum;
        }
        private decimal GetMin(GridViewGroupRowInfo row)
        {
            decimal min = decimal.MaxValue;
            foreach (GridViewRowInfo childRow in row.ChildRows)
            {
                if ((decimal)childRow.Cells["UnitPrice"].Value < min)
                {
                    min = (decimal)childRow.Cells["UnitPrice"].Value;
                }
            }
            return min;
        }
        private decimal GetMax(GridViewGroupRowInfo row)
        {
            decimal max = decimal.MinValue;
            foreach (GridViewRowInfo childRow in row.ChildRows)
            {
                if ((decimal)childRow.Cells["UnitPrice"].Value > max)
                {
                    max = (decimal)childRow.Cells["UnitPrice"].Value;
                }
            }
            return max;
        }
    }
}
