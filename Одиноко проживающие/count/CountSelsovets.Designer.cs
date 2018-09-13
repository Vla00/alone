namespace Одиноко_проживающие.count
{
    partial class CountSelsovets
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CountSelsovets));
            this.radLayoutControl1 = new Telerik.WinControls.UI.RadLayoutControl();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.layoutControlLabelItem1 = new Telerik.WinControls.UI.LayoutControlLabelItem();
            this.layoutControlItem1 = new Telerik.WinControls.UI.LayoutControlItem();
            this.layoutControlItem2 = new Telerik.WinControls.UI.LayoutControlItem();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).BeginInit();
            this.radLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLayoutControl1
            // 
            this.radLayoutControl1.Controls.Add(this.radStatusStrip1);
            this.radLayoutControl1.Controls.Add(this.radGridView1);
            this.radLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLayoutControl1.HiddenItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.layoutControlLabelItem1});
            this.radLayoutControl1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.radLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.radLayoutControl1.Name = "radLayoutControl1";
            this.radLayoutControl1.Size = new System.Drawing.Size(757, 350);
            this.radLayoutControl1.TabIndex = 0;
            this.radLayoutControl1.Text = "radLayoutControl1";
            this.radLayoutControl1.ThemeName = "TelerikMetro";
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radLabelElement1});
            this.radStatusStrip1.Location = new System.Drawing.Point(3, 2);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(751, 25);
            this.radStatusStrip1.TabIndex = 3;
            this.radStatusStrip1.Text = "radStatusStrip1";
            this.radStatusStrip1.ThemeName = "TelerikMetro";
            // 
            // radLabelElement1
            // 
            this.radLabelElement1.Name = "radLabelElement1";
            this.radStatusStrip1.SetSpring(this.radLabelElement1, false);
            this.radLabelElement1.Text = "radLabelElement1";
            this.radLabelElement1.TextWrap = true;
            // 
            // radGridView1
            // 
            this.radGridView1.Location = new System.Drawing.Point(3, 31);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.PageSize = 50;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowGroupPanel = false;
            this.radGridView1.Size = new System.Drawing.Size(751, 316);
            this.radGridView1.TabIndex = 4;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ThemeName = "TelerikMetro";
            this.radGridView1.FilterChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.radGridView1_FilterChanged);
            // 
            // layoutControlLabelItem1
            // 
            this.layoutControlLabelItem1.Bounds = new System.Drawing.Rectangle(0, 176, 559, 150);
            this.layoutControlLabelItem1.DrawText = false;
            this.layoutControlLabelItem1.Name = "layoutControlLabelItem1";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AssociatedControl = this.radStatusStrip1;
            this.layoutControlItem1.Bounds = new System.Drawing.Rectangle(0, 0, 757, 28);
            this.layoutControlItem1.ControlVerticalAlignment = Telerik.WinControls.UI.RadVerticalAlignment.Center;
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "layoutControlItem1";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AssociatedControl = this.radGridView1;
            this.layoutControlItem2.Bounds = new System.Drawing.Rectangle(0, 28, 757, 322);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "layoutControlItem2";
            // 
            // CountSelsovets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 350);
            this.Controls.Add(this.radLayoutControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CountSelsovets";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Общее количество";
            this.ThemeName = "TelerikMetro";
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).EndInit();
            this.radLayoutControl1.ResumeLayout(false);
            this.radLayoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLayoutControl radLayoutControl1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.LayoutControlLabelItem layoutControlLabelItem1;
        private Telerik.WinControls.UI.LayoutControlItem layoutControlItem1;
        private Telerik.WinControls.UI.LayoutControlItem layoutControlItem2;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement1;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
    }
}