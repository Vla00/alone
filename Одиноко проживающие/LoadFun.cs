using System.Threading;

namespace Одиноко_проживающие
{
    public class LoadFun
    {
        Load load;
        Thread thread;

        public void Show(string text)
        {
            thread = new Thread(new ParameterizedThreadStart(Start));
            thread.Start(text);
        }

        public void Close()
        {
            if(load != null)
            {
                load.BeginInvoke(new ThreadStart(load.CloseLoadingForm));
                load = null;
                thread = null;
            }
        }

        private void Start(object text)
        {
            load = new Load((string)text);
            load.ShowDialog();
        }
    }
}
