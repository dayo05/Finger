using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FingerBackend;

namespace FINGERApp
{
    public partial class Form2 : Form
    {
        private string[] history;
        public Form2()
        {
            InitializeComponent();
            listBox1.Visible = false;
            this.history = File.ReadAllLines("PathHistory.txt");
        }

        private void findFolder(object sender, EventArgs e)
        {
            label8.Text = "";
            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                label6.Text=folderBrowserDialog1.SelectedPath;
            var dir = Directory.GetFiles(label6.Text);
            foreach (var f in dir)
                label8.Text += Path.GetFileName(f) + "\n";
            addHistory(label6.Text);
        }

        private void addHistory(string newPath)
        {
            listBox1.Items.Clear();
            var temp = this.history.Where(str => str != newPath).ToArray();
            label6.Text = newPath;
            foreach (string hist in temp)
                listBox1.Items.Add(hist);
            File.WriteAllText("PathHistory.txt", newPath + "\n");
            int i = 0;
            foreach (string hist in temp)
            {
                File.WriteAllText("PathHistory.txt", hist + "\n");
                i++; if (i == 10) break;
            }
            this.history = File.ReadAllLines("PathHistory.txt");
        }

        private void cleanFolder(object sender, EventArgs e)
        {
            foreach(var s in Directory.GetFiles("C:\\Users\\maxma\\Downloads\\test1"))
                Program.Backend.BatchFile(s, Program.analyzed);
        }

        private void dragDropFolder(object sender, DragEventArgs e)
        {
            
        }

        private void openHistory(object sender, EventArgs e)
        {
            listBox1.Visible = !listBox1.Visible;
            if (listBox1.Visible)
            {
                foreach (string hist in this.history)
                    listBox1.Items.Add(hist);
                listBox1.Select();
            }
        }

        private void changed(object sender, EventArgs e)
        {
            addHistory(listBox1.SelectedValue.ToString());
        }
    }
}
