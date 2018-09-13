using System.Windows.Forms;

namespace Одиноко_проживающие
{
    internal class MyDataGridView : DataGridView
    {
        public MyDataGridView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }

    public class MyForm : Form
    {
        public MyForm()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
