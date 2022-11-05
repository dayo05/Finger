using System.Data;

namespace FINGERApp
{
    public partial class Form2 : Form
    {
        private IEnumerable<string> history = Enumerable.Repeat("", 1);
        public Form2()
        {
            InitializeComponent();
            listBox1.Visible = false;
            if (File.Exists("PathHistory.txt"))
            {
                this.history = File.ReadAllLines("PathHistory.txt").Where(x => !string.IsNullOrWhiteSpace(x));
            }
        }

        private void findFolder(object sender, EventArgs e)
        {
            folderBrowserDialog1.Reset();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) 
                ApplyDirectory(folderBrowserDialog1.SelectedPath);
        }

        private void ApplyDirectory(string directory)
        {
            label6.Text = directory;
            label8.Text = "";
            var dir = Directory.GetFiles(label6.Text);
            foreach (var f in dir)
                label8.Text += Path.GetFileName(f) + "\n";
            addHistory(label6.Text);
        }

        private void addHistory(string newPath)
        {
            listBox1.Items.Clear();
            var temp = this.history.Where(str => str != newPath).ToList();

            label6.Text = newPath;
            foreach (string hist in temp)
                listBox1.Items.Add(hist);

            File.WriteAllText("PathHistory.txt", newPath + "\n");
            int i = 0;
            foreach (string hist in temp)
            {
                File.AppendAllText("PathHistory.txt", hist + "\n");
                i++; if (i == 10) break;
            }
            this.history = File.ReadAllLines("PathHistory.txt").Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private void cleanFolder(object sender, EventArgs e)
        {
            if (label6.Text == "현재 경로")
                MessageBox.Show("시작하기 전에 경로를 설정해 주세요!");
            else
            {
                int totalFiles = Directory.GetFiles(label6.Text).Length;
                if(totalFiles == 0)
                {
                    MessageBox.Show("정리할 파일이 없습니다.");
                    return;
                }
                actionProgress.Maximum = totalFiles;
                workingFiles = totalFiles;
                actionProgress.Value = 0;
                actionProgress.Step = 1;
                tableLayoutPanel4.Enabled = false;
                foreach (var s in Directory.GetFiles(label6.Text))
                    BatchAsync(s);
            }
        }

        private int workingFiles = 0;
        private void BatchAsync(string s)
            => new Task(() =>
            {
                Program.Backend.BatchFile(s, Program.analyzed);
                Invoke(() => {
                    actionProgress.PerformStep();
                    ApplyDirectory(label6.Text);
                    if(--workingFiles == 0)
                    {
                        MessageBox.Show("완료되었습니다!");
                        tableLayoutPanel4.Enabled = true;
                    }
                });
            }).Start();

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
                label8.Location = new Point(label8.Location.X, 200);
                label8.Size -= new Size(0, 90);
            }
            else {
                label8.Location = new Point(label8.Location.X, 110);
                label8.Size += new Size(0, 90);
            }
        }

        private void changed(object sender, EventArgs e)
        {
            ApplyDirectory(listBox1.SelectedItem.ToString());
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (history.Count() != 0)
                ApplyDirectory(history.First());
        }
    }
}
