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
        public Form2()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("asdf");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void findFolder(object sender, EventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                label6.Text=folderBrowserDialog1.SelectedPath;
        }

        private void cleanFolder(object sender, EventArgs e)
        {
            foreach(var s in Directory.GetFiles("C:\\Users\\maxma\\Downloads\\test1"))
                Program.Backend.BatchFile(s, Program.analyzed);
            return;
            string path = label6.Text;
            DirectoryInfo dir = new DirectoryInfo(path);
            var arr=dir.GetFiles().Select(x => x.Name);
            foreach(string targetFile in arr)
            {
                /*
                string initialFile = Path.Combine(path, targetFile);
                string targetPath = @"C:\Users\maxma\Downloads\test1";//FingerBackend.Analyze(initialFile);
                try
                {
                    File.Copy(initialFile, Path.Combine(targetPath, targetFile), true);
                }
                catch (IOException ee)
                {
                    MessageBox.Show(ee.Message);
                }
                File.Delete(initialFile);
                */
            }
            MessageBox.Show("정리 완료");
        }

        private void dragDropFolder(object sender, DragEventArgs e)
        {

        }
    }
}
