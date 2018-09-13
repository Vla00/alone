namespace Одиноко_проживающие
{
    partial class Statistic
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.CartesianArea cartesianArea1 = new Telerik.WinControls.UI.CartesianArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Statistic));
            this.radChartView1 = new Telerik.WinControls.UI.RadChartView();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radChartView1
            // 
            cartesianArea1.GridDesign.DrawHorizontalStripes = false;
            cartesianArea1.GridDesign.DrawVerticalFills = false;
            cartesianArea1.ShowGrid = true;
            this.radChartView1.AreaDesign = cartesianArea1;
            this.radChartView1.Location = new System.Drawing.Point(12, 12);
            this.radChartView1.Name = "radChartView1";
            this.radChartView1.ShowPanZoom = true;
            this.radChartView1.ShowTitle = true;
            this.radChartView1.ShowToolTip = true;
            this.radChartView1.ShowTrackBall = true;
            this.radChartView1.Size = new System.Drawing.Size(1285, 354);
            this.radChartView1.TabIndex = 0;
            this.radChartView1.Text = "radChartView1";
            this.radChartView1.ThemeName = "TelerikMetro";
            this.radChartView1.LabelFormatting += new Telerik.WinControls.UI.ChartViewLabelFormattingEventHandler(this.radChartView1_LabelFormatting);
            // 
            // Statistic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1329, 379);
            this.Controls.Add(this.radChartView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Statistic";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Статистика";
            this.ThemeName = "TelerikMetro";
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadChartView radChartView1;
    }
}
