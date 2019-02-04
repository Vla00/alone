namespace Одиноко_проживающие.menu
{
    partial class SocRabotnik
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition4 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SocRabotnik));
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPageViewPage2 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGridView3 = new Telerik.WinControls.UI.RadGridView();
            this.radGridView5 = new Telerik.WinControls.UI.RadGridView();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage1.SuspendLayout();
            this.radPageViewPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView2
            // 
            this.radGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView2.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView2.MasterTemplate.AllowAddNewRow = false;
            this.radGridView2.MasterTemplate.AllowDeleteRow = false;
            this.radGridView2.MasterTemplate.AllowEditRow = false;
            this.radGridView2.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.ShowGroupPanel = false;
            this.radGridView2.Size = new System.Drawing.Size(735, 405);
            this.radGridView2.TabIndex = 1;
            this.radGridView2.Text = "radGridView2";
            this.radGridView2.ThemeName = "TelerikMetro";
            this.radGridView2.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView2_CellDoubleClick);
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Controls.Add(this.radPageViewPage2);
            this.radPageView1.Location = new System.Drawing.Point(476, 13);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage1;
            this.radPageView1.Size = new System.Drawing.Size(745, 441);
            this.radPageView1.TabIndex = 2;
            this.radPageView1.Text = "radPageView1";
            this.radPageView1.ThemeName = "TelerikMetro";
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.Controls.Add(this.radGridView2);
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(92F, 25F);
            this.radPageViewPage1.Location = new System.Drawing.Point(5, 31);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(735, 405);
            this.radPageViewPage1.Text = "Действующие";
            // 
            // radPageViewPage2
            // 
            this.radPageViewPage2.Controls.Add(this.radGridView3);
            this.radPageViewPage2.ItemSize = new System.Drawing.SizeF(123F, 25F);
            this.radPageViewPage2.Location = new System.Drawing.Point(5, 31);
            this.radPageViewPage2.Name = "radPageViewPage2";
            this.radPageViewPage2.Size = new System.Drawing.Size(616, 405);
            this.radPageViewPage2.Text = "Приостановленные";
            // 
            // radGridView3
            // 
            this.radGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView3.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView3.MasterTemplate.AllowAddNewRow = false;
            this.radGridView3.MasterTemplate.AllowDeleteRow = false;
            this.radGridView3.MasterTemplate.AllowEditRow = false;
            this.radGridView3.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView3.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView3.Name = "radGridView3";
            this.radGridView3.ShowGroupPanel = false;
            this.radGridView3.Size = new System.Drawing.Size(616, 405);
            this.radGridView3.TabIndex = 2;
            this.radGridView3.Text = "radGridView3";
            this.radGridView3.ThemeName = "TelerikMetro";
            // 
            // radGridView5
            // 
            this.radGridView5.Location = new System.Drawing.Point(12, 36);
            // 
            // 
            // 
            this.radGridView5.MasterTemplate.AllowAddNewRow = false;
            this.radGridView5.MasterTemplate.AllowDeleteRow = false;
            this.radGridView5.MasterTemplate.AllowEditRow = false;
            this.radGridView5.MasterTemplate.AllowRowResize = false;
            this.radGridView5.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView5.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.radGridView5.Name = "radGridView5";
            this.radGridView5.ShowGroupPanel = false;
            this.radGridView5.Size = new System.Drawing.Size(224, 418);
            this.radGridView5.TabIndex = 3;
            this.radGridView5.Text = "radGridView5";
            this.radGridView5.ThemeName = "TelerikMetro";
            this.radGridView5.SelectionChanged += new System.EventHandler(this.LoadSocRab);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(13, 12);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(223, 18);
            this.radButton1.TabIndex = 4;
            this.radButton1.Text = "Печать всех";
            this.radButton1.ThemeName = "TelerikMetro";
            this.radButton1.Click += new System.EventHandler(this.PrintAll);
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(242, 12);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(228, 18);
            this.radButton2.TabIndex = 5;
            this.radButton2.Text = "Печать специалиста";
            this.radButton2.ThemeName = "TelerikMetro";
            this.radButton2.Click += new System.EventHandler(this.PrintSpec);
            // 
            // radGridView1
            // 
            this.radGridView1.Location = new System.Drawing.Point(242, 36);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AllowRowResize = false;
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition4;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowGroupPanel = false;
            this.radGridView1.Size = new System.Drawing.Size(228, 418);
            this.radGridView1.TabIndex = 0;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ThemeName = "TelerikMetro";
            this.radGridView1.SelectionChanged += new System.EventHandler(this.LoadAlone);
            // 
            // SocRabotnik
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 466);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radGridView5);
            this.Controls.Add(this.radPageView1);
            this.Controls.Add(this.radGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SocRabotnik";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Закрепленные социальные работники";
            this.ThemeName = "TelerikMetro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SocRabotnik_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage1.ResumeLayout(false);
            this.radPageViewPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadGridView radGridView2;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage2;
        private Telerik.WinControls.UI.RadGridView radGridView3;
        private Telerik.WinControls.UI.RadGridView radGridView5;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadGridView radGridView1;
    }
}