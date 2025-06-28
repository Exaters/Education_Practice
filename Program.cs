using Education_Practice;

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
            Application.DoEvents();
            Thread.Sleep(3000);
            splash.Close();

            Application.Run(new MainWindow());
        }
    }
}