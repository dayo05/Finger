#define ENABLE_BACKEND
using FingerBackend;

namespace FINGERApp
{
    internal static class Program
    {
        public static IBackend Backend { get; }
        static Program() {
            BackendLoader.BiasPath = "bias.txt";
#if ENABLE_BACKEND
            Backend = BackendLoader.LoadAssembly();
#endif
        }

        public static void Reanalyze()
        {
            analyzed.Clear();
            analyzed.AddRange(GlobalSettings.AnalyzePath.Select(Backend.Analyze));
        }

        private static List<Analyzed> analyzed = new();
        public static void Batch(this Action<string, IEnumerable<Analyzed>> action, string s)
        {
            action(s, analyzed);
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
#if ENABLE_BACKEND
            GlobalSettings.AddPathToAnalyze("C:\\Users\\maxma\\Downloads\\test1");
            GlobalSettings.AddPathToAnalyze("C:\\Users\\maxma\\Downloads\\test2");

            GlobalSettings.Reanalyze();
#endif
            Application.Run(new Form2());
        }
    }
}
