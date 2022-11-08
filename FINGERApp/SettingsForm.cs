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
        private IEnumerable<string> targets = Enumerable.Repeat("", 1);
        public SettingsForm()
        {
            InitializeComponent();
            checkedListBox1.Items.Clear();
            this.targets = File.ReadAllLines("TargetFolderPath.txt");
            foreach (string target in targets)
            {
                checkedListBox1.Items.Add(target);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            foreach (string target in checkedListBox1.Items)
            {
                File.AppendAllText("TargetFolderPath.txt", target + "\n");
            }
            GlobalSettings.setCore(textBox1.Text);
            GlobalSettings.setmode(checkBox1.Checked);
            this.targets = File.ReadAllLines("TargetFolderPath.txt");
            foreach(string target in this.targets)
            {
                GlobalSettings.AddPathToAnalyze(target);
            }
            GlobalSettings.resetAnalyzeList();
            GlobalSettings.Reanalyze();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            bool bo = checkBox2.Checked;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, bo);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            var t = checkedListBox1.CheckedItems;
            foreach (var item in t)
            {
                checkedListBox1.Items.Remove(item);
            }
            */
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--) if (checkedListBox1.GetItemChecked(i)) checkedListBox1.Items.RemoveAt(i);
            File.WriteAllText("TargetFolderPath.txt", "");
            foreach (string target in this.checkedListBox1.Items)
            {
                File.AppendAllText("TargetFolderPath.txt", target + "\n");
            }
            this.targets = File.ReadAllLines("TargetFolderPath.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                checkedListBox1.Items.Add(textBox2.Text);
                textBox2.Text = "";
            }
            File.WriteAllText("TargetFolderPath.txt", "");
            foreach (string target in this.checkedListBox1.Items)
            {
                File.AppendAllText("TargetFolderPath.txt", target + "\n");
            }
            this.targets = File.ReadAllLines("TargetFolderPath.txt");
        }
    }

    public static class GlobalSettings
    {
        public static void setCore(string core) => MaxTaskCount = Convert.ToInt32(core);
        public static int MaxTaskCount { get; private set; } = 4;

        public static void setmode(bool mode) => DecompressDirectory = mode;
        public static bool DecompressDirectory { get; private set; } = false;

        public static List<string> AnalyzePath { get; private set; } = new List<string>();
        public static void AddPathToAnalyze(string path)
        {
            AnalyzePath.Add(path);
        }
        public static void resetAnalyzeList()=>AnalyzePath=new List<string>();
        public static void Reanalyze() => Program.Reanalyze();
    }
}
