namespace Одиноко_проживающие
{
    partial class Inspector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inspector));
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGridView5 = new Telerik.WinControls.UI.RadGridView();
            this.radPageViewPage3 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.radDesktopAlert1 = new Telerik.WinControls.UI.RadDesktopAlert(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5.MasterTemplate)).BeginInit();
            this.radPageViewPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Controls.Add(this.radPageViewPage3);
            this.radPageView1.DefaultPage = this.radPageViewPage1;
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage1;
            this.radPageView1.Size = new System.Drawing.Size(540, 367);
            this.radPageView1.TabIndex = 30;
            this.radPageView1.Text = "radPageView1";
            this.radPageView1.ThemeName = "TelerikMetro";
            ((Telerik.WinControls.UI.StripViewButtonsPanel)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.Controls.Add(this.radGridView5);
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(89F, 28F);
            this.radPageViewPage1.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(519, 319);
            this.radPageViewPage1.Text = "Действующие";
            // 
            // radGridView5
            // 
            this.radGridView5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView5.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView5.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView5.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView5.MasterTemplate.AllowDeleteRow = false;
            this.radGridView5.MasterTemplate.PageSize = 50;
            this.radGridView5.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView5.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView5.Name = "radGridView5";
            this.radGridView5.ShowGroupPanel = false;
            this.radGridView5.Size = new System.Drawing.Size(519, 319);
            this.radGridView5.TabIndex = 36;
            this.radGridView5.Text = "radGridView5";
            this.radGridView5.ThemeName = "TelerikMetro";
            this.radGridView5.UserAddingRow += new Telerik.WinControls.UI.GridViewRowCancelEventHandler(this.radGridView5_UserAddingRow);
            this.radGridView5.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.radGridView5_UserAddedRow);
            this.radGridView5.RowsChanging += new Telerik.WinControls.UI.GridViewCollectionChangingEventHandler(this.radGridView5_RowsChanging);
            // 
            // radPageViewPage3
            // 
            this.radPageViewPage3.Controls.Add(this.radGridView2);
            this.radPageViewPage3.ItemSize = new System.Drawing.SizeF(74F, 28F);
            this.radPageViewPage3.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage3.Name = "radPageViewPage3";
            this.radPageViewPage3.Size = new System.Drawing.Size(519, 319);
            this.radPageViewPage3.Text = "Уволенные";
            // 
            // radGridView2
            // 
            this.radGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView2.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView2.MasterTemplate.AllowAddNewRow = false;
            this.radGridView2.MasterTemplate.AllowCellContextMenu = false;
            this.radGridView2.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView2.MasterTemplate.AllowDeleteRow = false;
            this.radGridView2.MasterTemplate.AllowEditRow = false;
            this.radGridView2.MasterTemplate.PageSize = 50;
            this.radGridView2.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.ShowGroupPanel = false;
            this.radGridView2.Size = new System.Drawing.Size(519, 319);
            this.radGridView2.TabIndex = 36;
            this.radGridView2.Text = "radGridView2";
            this.radGridView2.ThemeName = "TelerikMetro";
            // 
            // radDesktopAlert1
            // 
            this.radDesktopAlert1.ThemeName = "TelerikMetro";
            // 
            // Inspector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(540, 367);
            this.Controls.Add(this.radPageView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Inspector";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Инспекторы";
            this.ThemeName = "TelerikMetro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Inspector_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5)).EndInit();
            this.radPageViewPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage3;
        private Telerik.WinControls.UI.RadDesktopAlert radDesktopAlert1;
        private Telerik.WinControls.UI.RadGridView radGridView2;
        private Telerik.WinControls.UI.RadGridView radGridView5;
    }
}