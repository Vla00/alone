namespace Одиноко_проживающие.handbook
{
    partial class Powered
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Powered));
            this.radGridView5 = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5.MasterTemplate)).BeginInit();
            this.SuspendLayout();
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
            this.radGridView5.Size = new System.Drawing.Size(591, 317);
            this.radGridView5.TabIndex = 36;
            this.radGridView5.Text = "radGridView5";
            this.radGridView5.ThemeName = "TelerikMetro";
            this.radGridView5.UserAddingRow += new Telerik.WinControls.UI.GridViewRowCancelEventHandler(this.radGridView5_UserAddingRow);
            this.radGridView5.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.radGridView5_UserAddedRow);
            this.radGridView5.RowsChanging += new Telerik.WinControls.UI.GridViewCollectionChangingEventHandler(this.radGridView5_RowsChanging);
            // 
            // Powered
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 317);
            this.Controls.Add(this.radGridView5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Powered";
            this.Text = "Диагноз";
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadGridView radGridView5;
    }
}