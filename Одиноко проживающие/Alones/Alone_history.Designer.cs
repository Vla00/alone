namespace Одиноко_проживающие
{
    partial class Alone_history
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alone_history));
            this.layoutControlLabelItem1 = new Telerik.WinControls.UI.LayoutControlLabelItem();
            this.radLayoutControl1 = new Telerik.WinControls.UI.RadLayoutControl();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.exit_grid = new Telerik.WinControls.UI.RadGridView();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.category_grid = new Telerik.WinControls.UI.RadGridView();
            this.layoutControlGroupItem1 = new Telerik.WinControls.UI.LayoutControlGroupItem();
            this.layoutControlLabelItem2 = new Telerik.WinControls.UI.LayoutControlLabelItem();
            this.layoutControlLabelItem3 = new Telerik.WinControls.UI.LayoutControlLabelItem();
            this.layoutControlItem1 = new Telerik.WinControls.UI.LayoutControlItem();
            this.layoutControlItem2 = new Telerik.WinControls.UI.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).BeginInit();
            this.radLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exit_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exit_grid.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.category_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.category_grid.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlLabelItem1
            // 
            this.layoutControlLabelItem1.Bounds = new System.Drawing.Rectangle(0, 83, 432, 83);
            this.layoutControlLabelItem1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.layoutControlLabelItem1.DrawText = false;
            this.layoutControlLabelItem1.Name = "layoutControlLabelItem1";
            this.layoutControlLabelItem1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // radLayoutControl1
            // 
            this.radLayoutControl1.Controls.Add(this.radGroupBox1);
            this.radLayoutControl1.Controls.Add(this.radGroupBox2);
            this.radLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLayoutControl1.HiddenItems.AddRange(new Telerik.WinControls.RadItem[] {
            this.layoutControlGroupItem1,
            this.layoutControlLabelItem2,
            this.layoutControlLabelItem3});
            this.radLayoutControl1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.radLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.radLayoutControl1.Name = "radLayoutControl1";
            this.radLayoutControl1.Size = new System.Drawing.Size(434, 335);
            this.radLayoutControl1.TabIndex = 0;
            this.radLayoutControl1.Text = "radLayoutControl1";
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.exit_grid);
            this.radGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox1.HeaderText = "выезды";
            this.radGroupBox1.Location = new System.Drawing.Point(3, 216);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(428, 116);
            this.radGroupBox1.TabIndex = 3;
            this.radGroupBox1.Text = "выезды";
            // 
            // exit_grid
            // 
            this.exit_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exit_grid.Location = new System.Drawing.Point(2, 18);
            // 
            // 
            // 
            this.exit_grid.MasterTemplate.AllowAddNewRow = false;
            this.exit_grid.MasterTemplate.AllowDeleteRow = false;
            this.exit_grid.MasterTemplate.AllowDragToGroup = false;
            this.exit_grid.MasterTemplate.AllowEditRow = false;
            this.exit_grid.MasterTemplate.AllowRowResize = false;
            this.exit_grid.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.exit_grid.Name = "exit_grid";
            this.exit_grid.Size = new System.Drawing.Size(424, 96);
            this.exit_grid.TabIndex = 4;
            this.exit_grid.Text = "radGridView2";
            this.exit_grid.ThemeName = "TelerikMetro";
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Controls.Add(this.category_grid);
            this.radGroupBox2.HeaderText = "категории";
            this.radGroupBox2.Location = new System.Drawing.Point(3, 3);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Size = new System.Drawing.Size(428, 207);
            this.radGroupBox2.TabIndex = 4;
            this.radGroupBox2.Text = "категории";
            // 
            // category_grid
            // 
            this.category_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.category_grid.Location = new System.Drawing.Point(2, 18);
            // 
            // 
            // 
            this.category_grid.MasterTemplate.AllowAddNewRow = false;
            this.category_grid.MasterTemplate.AllowDeleteRow = false;
            this.category_grid.MasterTemplate.AllowDragToGroup = false;
            this.category_grid.MasterTemplate.AllowEditRow = false;
            this.category_grid.MasterTemplate.AllowRowResize = false;
            this.category_grid.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.category_grid.Name = "category_grid";
            this.category_grid.Size = new System.Drawing.Size(424, 187);
            this.category_grid.TabIndex = 4;
            this.category_grid.Text = "radGridView1";
            this.category_grid.ThemeName = "TelerikMetro";
            // 
            // layoutControlGroupItem1
            // 
            this.layoutControlGroupItem1.Bounds = new System.Drawing.Rectangle(0, 0, 434, 167);
            this.layoutControlGroupItem1.Name = "layoutControlGroupItem1";
            this.layoutControlGroupItem1.Text = "layoutControlGroupItem1";
            // 
            // layoutControlLabelItem2
            // 
            this.layoutControlLabelItem2.Bounds = new System.Drawing.Rectangle(0, 167, 217, 168);
            this.layoutControlLabelItem2.DrawText = false;
            this.layoutControlLabelItem2.Name = "layoutControlLabelItem2";
            // 
            // layoutControlLabelItem3
            // 
            this.layoutControlLabelItem3.Bounds = new System.Drawing.Rectangle(0, 0, 217, 167);
            this.layoutControlLabelItem3.DrawText = false;
            this.layoutControlLabelItem3.Name = "layoutControlLabelItem3";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AssociatedControl = this.radGroupBox1;
            this.layoutControlItem1.Bounds = new System.Drawing.Rectangle(0, 213, 434, 122);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "layoutControlItem1";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AssociatedControl = this.radGroupBox2;
            this.layoutControlItem2.Bounds = new System.Drawing.Rectangle(0, 0, 434, 213);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "layoutControlItem2";
            // 
            // Alone_history
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 335);
            this.Controls.Add(this.radLayoutControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Alone_history";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "История";
            this.ThemeName = "TelerikMetro";
            ((System.ComponentModel.ISupportInitialize)(this.radLayoutControl1)).EndInit();
            this.radLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.exit_grid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exit_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.category_grid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.category_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.LayoutControlLabelItem layoutControlLabelItem1;
        private Telerik.WinControls.UI.RadLayoutControl radLayoutControl1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private Telerik.WinControls.UI.LayoutControlGroupItem layoutControlGroupItem1;
        private Telerik.WinControls.UI.LayoutControlLabelItem layoutControlLabelItem2;
        private Telerik.WinControls.UI.LayoutControlLabelItem layoutControlLabelItem3;
        private Telerik.WinControls.UI.LayoutControlItem layoutControlItem1;
        private Telerik.WinControls.UI.LayoutControlItem layoutControlItem2;
        private Telerik.WinControls.UI.RadGridView exit_grid;
        private Telerik.WinControls.UI.RadGridView category_grid;
    }
}