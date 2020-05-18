using System.Windows.Forms;

namespace Одиноко_проживающие
{
    public partial class Wait : Form
    {
        public Wait(bool top)
        {
            InitializeComponent();

            TopMost = top;
        }

        public string Message
        {
            set { label1.Text = value; }
        }

        public int ProgressVal
        {
            set { radProgressBar1.Value1 = value; }
        }
    }
}