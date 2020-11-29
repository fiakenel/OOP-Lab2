using System;
using Gtk;

namespace Lab2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow("/home/maksym/Documents/MonoDevelop/Lab2/data.xml");
            win.Show();
            Application.Run();
        }
    }
}
