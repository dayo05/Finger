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
#if ENABLE_BACKEND
            analyzed.Add(Backend.Analyze("C:\\Users\\dayo\\finger\\test1"));
            analyzed.Add(Backend.Analyze("C:\\Users\\dayo\\finger\\test2"));
#endif
            Application.Run(new Form2());
        }
    }
}
