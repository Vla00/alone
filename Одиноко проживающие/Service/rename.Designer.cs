namespace Одиноко_проживающие.Service
{
    partial class rename
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rename));
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGridView3 = new Telerik.WinControls.UI.RadGridView();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.radPageViewPage2 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            this.radPageViewPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            this.SuspendLayout();
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Controls.Add(this.radPageViewPage2);
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage1;
            this.radPageView1.Size = new System.Drawing.Size(1188, 466);
            this.radPageView1.TabIndex = 0;
            this.radPageView1.Text = "radPageView1";
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.Controls.Add(this.radGridView3);
            this.radPageViewPage1.Controls.Add(this.radGridView2);
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(57F, 28F);
            this.radPageViewPage1.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(1167, 418);
            this.radPageViewPage1.Text = "Подбор";
            // 
            // radGridView3
            // 
            this.radGridView3.Location = new System.Drawing.Point(624, 4);
            // 
            // 
            // 
            this.radGridView3.MasterTemplate.AllowAddNewRow = false;
            this.radGridView3.MasterTemplate.AllowDeleteRow = false;
            this.radGridView3.MasterTemplate.AllowEditRow = false;
            this.radGridView3.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView3.Name = "radGridView3";
            this.radGridView3.Size = new System.Drawing.Size(526, 414);
            this.radGridView3.TabIndex = 1;
            this.radGridView3.Text = "radGridView3";
            // 
            // radGridView2
            // 
            this.radGridView2.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.radGridView2.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.Size = new System.Drawing.Size(615, 415);
            this.radGridView2.TabIndex = 0;
            this.radGridView2.Text = "radGridView2";
            // 
            // radPageViewPage2
            // 
            this.radPageViewPage2.Controls.Add(this.radGroupBox1);
            this.radPageViewPage2.ItemSize = new System.Drawing.SizeF(61F, 28F);
            this.radPageViewPage2.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage2.Name = "radPageViewPage2";
            this.radPageViewPage2.Size = new System.Drawing.Size(1167, 418);
            this.radPageViewPage2.Text = "Загрузка";
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.radButton2);
            this.radGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBox1.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radGroupBox1.HeaderText = "Операции (ФИО, Дата рождения, Дата сметри, сельсовет, адрес";
            this.radGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(1167, 50);
            this.radGroupBox1.TabIndex = 0;
            this.radGroupBox1.Text = "Операции (ФИО, Дата рождения, Дата сметри, сельсовет, адрес";
            this.radGroupBox1.ThemeName = "TelerikMetro";
            // 
            // radButton2
            // 
            this.radButton2.Dock = System.Windows.Forms.DockStyle.Left;
            this.radButton2.Location = new System.Drawing.Point(2, 18);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(110, 30);
            this.radButton2.TabIndex = 1;
            this.radButton2.Text = "Преобразовать";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Excel файлы(*.xls; *.xlsx)|*.xls; *xlsx";
            // 
            // rename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 466);
            this.Controls.Add(this.radPageView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "rename";
            this.Text = "Список умерших";
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            this.radPageViewPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage2;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Telerik.WinControls.UI.RadGridView radGridView3;
        private Telerik.WinControls.UI.RadGridView radGridView2;
    }
}