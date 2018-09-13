namespace Одиноко_проживающие.search
{
    partial class SocAdaptacii
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SocAdaptacii));
            this.radLayoutControl1 = new Telerik.WinControls.UI.RadLayoutControl();
            this.radLayoutControl2 = new Telerik.WinControls.UI.RadLayoutControl();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.radButtonElement1 = new Telerik.WinControls.UI.RadButtonElement();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.layoutControlItem1 = new Telerik.WinControls.UI.LayoutControlItem();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).BeginInit();
            this.radLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl2)).BeginInit();
            this.radLayoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLayoutControl1
            // 
            this.radLayoutControl1.Controls.Add(this.radLayoutControl2);
            this.radLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLayoutControl1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.layoutControlItem1});
            this.radLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.radLayoutControl1.Name = "radLayoutControl1";
            this.radLayoutControl1.Size = new System.Drawing.Size(838, 370);
            this.radLayoutControl1.TabIndex = 2;
            this.radLayoutControl1.Text = "radLayoutControl1";
            this.radLayoutControl1.ThemeName = "TelerikMetro";
            // 
            // radLayoutControl2
            // 
            this.radLayoutControl2.Controls.Add(this.radStatusStrip1);
            this.radLayoutControl2.Controls.Add(this.radGridView2);
            this.radLayoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLayoutControl2.Location = new System.Drawing.Point(3, 3);
            this.radLayoutControl2.Name = "radLayoutControl2";
            this.radLayoutControl2.Size = new System.Drawing.Size(832, 364);
            this.radLayoutControl2.TabIndex = 3;
            this.radLayoutControl2.Text = "radLayoutControl2";
            this.radLayoutControl2.ThemeName = "TelerikMetro";
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radLabelElement1,
            this.radButtonElement1});
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 0);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(830, 27);
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
            // radButtonElement1
            // 
            this.radButtonElement1.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.radButtonElement1.Name = "radButtonElement1";
            this.radStatusStrip1.SetSpring(this.radButtonElement1, false);
            this.radButtonElement1.Text = "Сохранить EXCEL";
            // 
            // radGridView2
            // 
            this.radGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridView2.Location = new System.Drawing.Point(3, 29);
            // 
            // 
            // 
            this.radGridView2.MasterTemplate.AllowAddNewRow = false;
            this.radGridView2.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView2.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView2.MasterTemplate.AllowEditRow = false;
            this.radGridView2.MasterTemplate.PageSize = 50;
            this.radGridView2.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.ShowGroupPanel = false;
            this.radGridView2.Size = new System.Drawing.Size(822, 328);
            this.radGridView2.TabIndex = 4;
            this.radGridView2.Text = "radGridView2";
            this.radGridView2.ThemeName = "TelerikMetro";
            this.radGridView2.FilterChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.radGridView1_FilterChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AssociatedControl = this.radLayoutControl2;
            this.layoutControlItem1.Bounds = new System.Drawing.Rectangle(0, 0, 838, 370);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "radLayoutControl1";
            // 
            // radGridView1
            // 
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.PageSize = 50;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowGroupPanel = false;
            this.radGridView1.Size = new System.Drawing.Size(836, 368);
            this.radGridView1.TabIndex = 4;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ThemeName = "TelerikMetro";
            // 
            // SocAdaptacii
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 370);
            this.Controls.Add(this.radLayoutControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SocAdaptacii";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Список социальной адаптации и реабилитации";
            this.ThemeName = "TelerikMetro";
            this.Shown += new System.EventHandler(this.SocAdaptacii_Shown);
            this.DoubleClick += new System.EventHandler(this.radGridView1_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).EndInit();
            this.radLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl2)).EndInit();
            this.radLayoutControl2.ResumeLayout(false);
            this.radLayoutControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLayoutControl radLayoutControl1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadLayoutControl radLayoutControl2;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement1;
        private Telerik.WinControls.UI.RadButtonElement radButtonElement1;
        private Telerik.WinControls.UI.RadGridView radGridView2;
        private Telerik.WinControls.UI.LayoutControlItem layoutControlItem1;
    }
}