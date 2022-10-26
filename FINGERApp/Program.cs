using FingerBackend;

namespace FINGERApp
{
    internal static class Program
    {
        public static IBackend Backend { get; }
        static Program() {
            Backend = BackendLoader.LoadAssembly();
        }

        public static List<Analyzed> analyzed = new();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            analyzed.Add(Backend.Analyze("C:\\Users\\maxma\\Downloads\\test2"));
            analyzed.Add(Backend.Analyze("C:\\Users\\maxma\\Downloads\\test3"));

            Application.Run(new Form2());
        }
    }
}
