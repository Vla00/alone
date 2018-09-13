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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition5 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition6 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition7 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition8 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPageViewPage2 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPageViewPage3 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.radGridView3 = new Telerik.WinControls.UI.RadGridView();
            this.radGridView4 = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage1.SuspendLayout();
            this.radPageViewPage2.SuspendLayout();
            this.radPageViewPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView4.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView1
            // 
            this.radGridView1.Location = new System.Drawing.Point(13, 13);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition5;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(257, 441);
            this.radGridView1.TabIndex = 0;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ThemeName = "TelerikMetro";
            this.radGridView1.SelectionChanged += new System.EventHandler(this.radGridView1_SelectionChanged);
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Controls.Add(this.radPageViewPage2);
            this.radPageView1.Controls.Add(this.radPageViewPage3);
            this.radPageView1.Location = new System.Drawing.Point(277, 13);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage1;
            this.radPageView1.Size = new System.Drawing.Size(469, 441);
            this.radPageView1.TabIndex = 2;
            this.radPageView1.Text = "radPageView1";
            this.radPageView1.ThemeName = "TelerikMetro";
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.Controls.Add(this.radGridView3);
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(54F, 25F);
            this.radPageViewPage1.Location = new System.Drawing.Point(5, 31);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(459, 405);
            this.radPageViewPage1.Text = "Принят";
            // 
            // radPageViewPage2
            // 
            this.radPageViewPage2.Controls.Add(this.radGridView4);
            this.radPageViewPage2.ItemSize = new System.Drawing.SizeF(101F, 25F);
            this.radPageViewPage2.Location = new System.Drawing.Point(5, 31);
            this.radPageViewPage2.Name = "radPageViewPage2";
            this.radPageViewPage2.Size = new System.Drawing.Size(459, 405);
            this.radPageViewPage2.Text = "Приостановлен";
            // 
            // radPageViewPage3
            // 
            this.radPageViewPage3.Controls.Add(this.radGridView2);
            this.radPageViewPage3.ItemSize = new System.Drawing.SizeF(53F, 25F);
            this.radPageViewPage3.Location = new System.Drawing.Point(5, 31);
            this.radPageViewPage3.Name = "radPageViewPage3";
            this.radPageViewPage3.Size = new System.Drawing.Size(459, 405);
            this.radPageViewPage3.Text = "Закрыт";
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
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition6;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.Size = new System.Drawing.Size(459, 405);
            this.radGridView2.TabIndex = 2;
            this.radGridView2.Text = "radGridView2";
            this.radGridView2.ThemeName = "TelerikMetro";
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
            this.radGridView3.MasterTemplate.ViewDefinition = tableViewDefinition7;
            this.radGridView3.Name = "radGridView3";
            this.radGridView3.Size = new System.Drawing.Size(459, 405);
            this.radGridView3.TabIndex = 3;
            this.radGridView3.Text = "radGridView3";
            this.radGridView3.ThemeName = "TelerikMetro";
            // 
            // radGridView4
            // 
            this.radGridView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView4.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView4.MasterTemplate.AllowAddNewRow = false;
            this.radGridView4.MasterTemplate.AllowDeleteRow = false;
            this.radGridView4.MasterTemplate.AllowEditRow = false;
            this.radGridView4.MasterTemplate.ViewDefinition = tableViewDefinition8;
            this.radGridView4.Name = "radGridView4";
            this.radGridView4.Size = new System.Drawing.Size(459, 405);
            this.radGridView4.TabIndex = 3;
            this.radGridView4.Text = "radGridView4";
            this.radGridView4.ThemeName = "TelerikMetro";
            // 
            // SocRabotnik
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 466);
            this.Controls.Add(this.radPageView1);
            this.Controls.Add(this.radGridView1);
            this.Name = "SocRabotnik";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Закрепленные социальные работники";
            this.ThemeName = "TelerikMetro";
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage1.ResumeLayout(false);
            this.radPageViewPage2.ResumeLayout(false);
            this.radPageViewPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView4.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Telerik.WinControls.UI.RadGridView radGridView3;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage2;
        private Telerik.WinControls.UI.RadGridView radGridView4;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage3;
        private Telerik.WinControls.UI.RadGridView radGridView2;
    }
}