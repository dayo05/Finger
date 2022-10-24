using FingerBackend;

namespace FINGERApp
{
    internal static class Program
    {
        public static IBackend Backend { get; init; }
        static Program() {
            Backend = BackendLoader.LoadAssembly();
        }
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            MessageBox.Show( Backend.Analyze("C:").Path);

            Application.Run(new Form1());
        }
    }
}
