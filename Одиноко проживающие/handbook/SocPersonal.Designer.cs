namespace Одиноко_проживающие
{
    partial class SocPersonal
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition4 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SocPersonal));
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage3 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGridViewSoc = new Telerik.WinControls.UI.RadGridView();
            this.radPageViewPage2 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radSplitContainer1 = new Telerik.WinControls.UI.RadSplitContainer();
            this.splitPanel1 = new Telerik.WinControls.UI.SplitPanel();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.splitPanel2 = new Telerik.WinControls.UI.SplitPanel();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGridView3 = new Telerik.WinControls.UI.RadGridView();
            this.radDesktopAlert1 = new Telerik.WinControls.UI.RadDesktopAlert(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSoc.MasterTemplate)).BeginInit();
            this.radPageViewPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainer1)).BeginInit();
            this.radSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).BeginInit();
            this.splitPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel2)).BeginInit();
            this.splitPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            this.radPageViewPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewPage3);
            this.radPageView1.Controls.Add(this.radPageViewPage2);
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage3;
            this.radPageView1.Size = new System.Drawing.Size(731, 371);
            this.radPageView1.TabIndex = 29;
            this.radPageView1.Text = "radPageView1";
            this.radPageView1.ThemeName = "TelerikMetro";
            ((Telerik.WinControls.UI.StripViewButtonsPanel)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // radPageViewPage3
            // 
            this.radPageViewPage3.Controls.Add(this.radGridViewSoc);
            this.radPageViewPage3.ItemSize = new System.Drawing.SizeF(89F, 28F);
            this.radPageViewPage3.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage3.Name = "radPageViewPage3";
            this.radPageViewPage3.Size = new System.Drawing.Size(710, 323);
            this.radPageViewPage3.Text = "Действующие";
            // 
            // radGridViewSoc
            // 
            this.radGridViewSoc.AllowDrop = true;
            this.radGridViewSoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewSoc.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridViewSoc.MasterTemplate.AllowCellContextMenu = false;
            this.radGridViewSoc.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridViewSoc.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewSoc.MasterTemplate.PageSize = 50;
            this.radGridViewSoc.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridViewSoc.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewSoc.Name = "radGridViewSoc";
            this.radGridViewSoc.ShowGroupPanel = false;
            this.radGridViewSoc.Size = new System.Drawing.Size(710, 323);
            this.radGridViewSoc.TabIndex = 38;
            this.radGridViewSoc.Text = "radGridView4";
            this.radGridViewSoc.ThemeName = "TelerikMetro";
            this.radGridViewSoc.UserAddingRow += new Telerik.WinControls.UI.GridViewRowCancelEventHandler(this.radGridViewSoc_UserAddingRow);
            this.radGridViewSoc.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.radGridViewSoc_UserAddedRow);
            this.radGridViewSoc.RowsChanging += new Telerik.WinControls.UI.GridViewCollectionChangingEventHandler(this.radGridViewSoc_RowsChanging);
            // 
            // radPageViewPage2
            // 
            this.radPageViewPage2.Controls.Add(this.radSplitContainer1);
            this.radPageViewPage2.ItemSize = new System.Drawing.SizeF(146F, 28F);
            this.radPageViewPage2.Location = new System.Drawing.Point(5, 31);
            this.radPageViewPage2.Name = "radPageViewPage2";
            this.radPageViewPage2.Size = new System.Drawing.Size(721, 335);
            this.radPageViewPage2.Text = "Привязка к инспекторам";
            // 
            // radSplitContainer1
            // 
            this.radSplitContainer1.Controls.Add(this.splitPanel1);
            this.radSplitContainer1.Controls.Add(this.splitPanel2);
            this.radSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.radSplitContainer1.Name = "radSplitContainer1";
            // 
            // 
            // 
            this.radSplitContainer1.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.radSplitContainer1.Size = new System.Drawing.Size(721, 335);
            this.radSplitContainer1.TabIndex = 39;
            this.radSplitContainer1.TabStop = false;
            this.radSplitContainer1.Text = "radSplitContainer1";
            // 
            // splitPanel1
            // 
            this.splitPanel1.Controls.Add(this.radGridView1);
            this.splitPanel1.Location = new System.Drawing.Point(0, 0);
            this.splitPanel1.Name = "splitPanel1";
            // 
            // 
            // 
            this.splitPanel1.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.splitPanel1.Size = new System.Drawing.Size(347, 335);
            this.splitPanel1.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(-0.01649928F, 0F);
            this.splitPanel1.SizeInfo.SplitterCorrection = new System.Drawing.Size(-11, 0);
            this.splitPanel1.TabIndex = 0;
            this.splitPanel1.TabStop = false;
            this.splitPanel1.Text = "splitPanel1";
            // 
            // radGridView1
            // 
            this.radGridView1.AllowDrop = true;
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.PageSize = 50;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowGroupPanel = false;
            this.radGridView1.Size = new System.Drawing.Size(347, 335);
            this.radGridView1.TabIndex = 38;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ThemeName = "TelerikMetro";
            this.radGridView1.Click += new System.EventHandler(this.radGridView1_Click);
            // 
            // splitPanel2
            // 
            this.splitPanel2.Controls.Add(this.radGridView2);
            this.splitPanel2.Location = new System.Drawing.Point(351, 0);
            this.splitPanel2.Name = "splitPanel2";
            // 
            // 
            // 
            this.splitPanel2.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.splitPanel2.Size = new System.Drawing.Size(370, 335);
            this.splitPanel2.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(0.01649928F, 0F);
            this.splitPanel2.SizeInfo.SplitterCorrection = new System.Drawing.Size(11, 0);
            this.splitPanel2.TabIndex = 1;
            this.splitPanel2.TabStop = false;
            this.splitPanel2.Text = "splitPanel2";
            // 
            // radGridView2
            // 
            this.radGridView2.AllowDrop = true;
            this.radGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView2.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView2.MasterTemplate.AllowAddNewRow = false;
            this.radGridView2.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView2.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView2.MasterTemplate.AllowDeleteRow = false;
            this.radGridView2.MasterTemplate.PageSize = 50;
            this.radGridView2.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.ShowGroupPanel = false;
            this.radGridView2.Size = new System.Drawing.Size(370, 335);
            this.radGridView2.TabIndex = 37;
            this.radGridView2.Text = "radGridView2";
            this.radGridView2.ThemeName = "TelerikMetro";
            this.radGridView2.ValueChanged += new System.EventHandler(this.radGridView2_ValueChanged);
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.Controls.Add(this.radGridView3);
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(68F, 28F);
            this.radPageViewPage1.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(710, 323);
            this.radPageViewPage1.Text = "Уволеные";
            // 
            // radGridView3
            // 
            this.radGridView3.AllowDrop = true;
            this.radGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView3.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView3.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView3.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView3.MasterTemplate.AllowDeleteRow = false;
            this.radGridView3.MasterTemplate.PageSize = 50;
            this.radGridView3.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView3.MasterTemplate.ViewDefinition = tableViewDefinition4;
            this.radGridView3.Name = "radGridView3";
            this.radGridView3.ShowGroupPanel = false;
            this.radGridView3.Size = new System.Drawing.Size(710, 323);
            this.radGridView3.TabIndex = 39;
            this.radGridView3.Text = "radGridView4";
            this.radGridView3.ThemeName = "TelerikMetro";
            // 
            // radDesktopAlert1
            // 
            this.radDesktopAlert1.AutoCloseDelay = 3;
            this.radDesktopAlert1.PopupAnimationFrames = 25;
            this.radDesktopAlert1.ThemeName = "TelerikMetro";
            // 
            // SocPersonal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(731, 371);
            this.Controls.Add(this.radPageView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SocPersonal";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Социальные работники";
            this.ThemeName = "TelerikMetro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SocPersonal_FormClosing);
            this.Load += new System.EventHandler(this.SocPersonal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSoc.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewSoc)).EndInit();
            this.radPageViewPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainer1)).EndInit();
            this.radSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).EndInit();
            this.splitPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel2)).EndInit();
            this.splitPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            this.radPageViewPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage2;
        private Telerik.WinControls.UI.RadGridView radGridView2;
        private Telerik.WinControls.UI.RadDesktopAlert radDesktopAlert1;
        private Telerik.WinControls.UI.RadSplitContainer radSplitContainer1;
        private Telerik.WinControls.UI.SplitPanel splitPanel1;
        private Telerik.WinControls.UI.SplitPanel splitPanel2;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage3;
        private Telerik.WinControls.UI.RadGridView radGridViewSoc;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Telerik.WinControls.UI.RadGridView radGridView3;
    }
}