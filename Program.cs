using Education_Practice;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Education_practice
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            SplashScreen splash = new SplashScreen();
            splash.Show();
            Application.DoEvents(); // ќбновл€ем окно
            Thread.Sleep(3000); // ∆дЄм 3 секунды
            splash.Close();

            Application.Run(new MainWindow());
        }
    }
}