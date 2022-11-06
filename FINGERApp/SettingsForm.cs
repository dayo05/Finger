using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FINGERApp
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }
    }

    public static class GlobalSettings
    {
        public static int MaxTaskCount { get; private set; } = 4;
        public static bool DecompressDirectory { get; private set; } = false;
        public static List<string> AnalyzePath { get; private set; } = new List<string>();
        public static void AddPathToAnalyze(string path)
        {
            AnalyzePath.Add(path);
        }
        public static void Reanalyze() => Program.Reanalyze();
    }
}
