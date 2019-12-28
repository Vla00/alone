using System.Windows.Forms;

namespace Одиноко_проживающие
{
    public partial class Load : Form
    {
        public Load(string text)
        {
            InitializeComponent();
            textBox1.Text = text;
        }

        public void CloseLoadingForm()
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}