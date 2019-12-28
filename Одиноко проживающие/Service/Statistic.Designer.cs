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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Statistic));
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(12, 41);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(352, 24);
            this.radButton2.TabIndex = 1;
            this.radButton2.Text = "сформировать и показать на: ";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.Location = new System.Drawing.Point(12, 12);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(236, 20);
            this.radDropDownList1.TabIndex = 2;
            this.radDropDownList1.Text = "radDropDownList1";
            this.radDropDownList1.SelectedIndexChanging += new Telerik.WinControls.UI.Data.PositionChangingEventHandler(this.radDropDownList1_SelectedIndexChanging);
            // 
            // radButton3
            // 
            this.radButton3.Location = new System.Drawing.Point(254, 12);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(110, 20);
            this.radButton3.TabIndex = 3;
            this.radButton3.Text = "показать";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // Statistic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 78);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radDropDownList1);
            this.Controls.Add(this.radButton2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Statistic";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Статистика";
            this.ThemeName = "TelerikMetro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Statistic_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private Telerik.WinControls.UI.RadButton radButton3;
    }
}
