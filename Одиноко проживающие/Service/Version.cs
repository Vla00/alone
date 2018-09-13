using System;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class Version : RadForm
    {
        public Version()
        {
            InitializeComponent();
        }

        private void Version_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
