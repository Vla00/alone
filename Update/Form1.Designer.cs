namespace Update
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
            this.segmentedRingWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radWaitingBar1
            // 
            this.radWaitingBar1.Location = new System.Drawing.Point(-4, 0);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.Size = new System.Drawing.Size(124, 98);
            this.radWaitingBar1.TabIndex = 6;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.WaitingIndicators.Add(this.segmentedRingWaitingBarIndicatorElement1);
            this.radWaitingBar1.WaitingSpeed = 20;
            this.radWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.SegmentedRing;
            // 
            // segmentedRingWaitingBarIndicatorElement1
            // 
            this.segmentedRingWaitingBarIndicatorElement1.AutoSize = true;
            this.segmentedRingWaitingBarIndicatorElement1.ElementColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(131)))), ((int)(((byte)(18)))));
            this.segmentedRingWaitingBarIndicatorElement1.ElementColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(14)))), ((int)(((byte)(131)))), ((int)(((byte)(37)))));
            this.segmentedRingWaitingBarIndicatorElement1.ElementCount = 20;
            this.segmentedRingWaitingBarIndicatorElement1.Name = "segmentedRingWaitingBarIndicatorElement1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(132, 94);
            this.Controls.Add(this.radWaitingBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Обновление";
            this.ThemeName = "TelerikMetro";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
        private Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement segmentedRingWaitingBarIndicatorElement1;
    }
}

