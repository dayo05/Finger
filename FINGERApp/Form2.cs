using FingerBackend;
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

            AllowDrop = true;
            DragEnter += (s, e) =>
            {
                dragDropImage.Visible = true;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Move;
            };
            DragDrop += (s, e) =>
            {
                dragDropImage.Visible = false;
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    AddTasks((e.Data.GetData(DataFormats.FileDrop) as string[]).Select<string, Action?>(x => File.Exists(x) ? () => Batch(x, Program.Backend.BatchFile) : Directory.Exists(x) ? () => Batch(x, Program.Backend.BatchDirectory) : null));
                    BatchAll();
                }
            };
            DragLeave += (s, e) =>
            {
                dragDropImage.Visible = false;
            };
        }

        private void FindFolder(object sender, EventArgs e)
        {
            folderBrowserDialog1.Reset();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) 
                ApplyDirectory(folderBrowserDialog1.SelectedPath);
        }

        private void ApplyDirectory(string directory)
        {
            ReloadInformation(directory);
            AddHistory(directory);
        }

        private void ReloadInformation(string? directory = null)
        {
            directory ??= label6.Text;
            if (directory == "현재 경로") return;
            label8.Text = "";
            if (Directory.Exists(directory))
            {
                var dir = Directory.GetFiles(directory);
                foreach (var f in dir)
                    label8.Text += Path.GetFileName(f) + "\n";
            }
        }

        private void AddHistory(string newPath)
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

        private void CleanFolder(object sender, EventArgs e)
        {
            if (label6.Text == "현재 경로")
                MessageBox.Show("시작하기 전에 경로를 설정해 주세요!");
            else
            {
                AddTasksFromDirectory(label6.Text);
                BatchAll();
            }
        }

        private void AddTasksFromDirectory(string dir)
        {
            AddTasks(Directory.GetFiles(dir).ToList().Select<string, Action>(x => () => Batch(x, Program.Backend.BatchFile)));

            if (!GlobalSettings.DecompressDirectory)
                AddTasks(Directory.GetDirectories(dir).ToList().Select<string, Action>(x => () => Batch(x, Program.Backend.BatchDirectory)));
            else foreach (var x in Directory.GetDirectories(dir)) AddTasksFromDirectory(x);
        }

        List<Action> tasks = new();
        private void AddTask(Action act)
        {
            if (workingFiles != 0) throw new InvalidOperationException("Task is already running");
            tasks.Add(act);
        }
        private void AddTasks(IEnumerable<Action> act)
        {
            if (workingFiles != 0) throw new InvalidOperationException("Task is already running");
            tasks.AddRange(act);
        }
        private void BatchAll()
        {
            if (workingFiles != 0) return;
            int totalActions = tasks.Count;
            if(totalActions == 0)
            {
                MessageBox.Show("정리할 파일이 없습니다.");
                return;
            }
            actionProgress.Maximum = totalActions;
            actionProgress.Value = 0;
            actionProgress.Step = 1;
            tableLayoutPanel4.Enabled = false;
            new Task(() =>
            {
                foreach(var x in tasks)
                {
                    while(workingFiles >= GlobalSettings.MaxTaskCount) Task.Yield();
                    Interlocked.Increment(ref workingFiles);
                    new Task(() =>
                    {
                        x();
                        Interlocked.Decrement(ref workingFiles);
                    }).Start();
                }
                while(workingFiles > 0) Task.Yield();
                MessageBox.Show("완료되었습니다!");
                Invoke(() =>
                {
                    tasks.Clear();
                    tableLayoutPanel4.Enabled = true;
                });
            }).Start();
        }

        private int workingFiles = 0;
        private void Batch(string s, Action<string, IEnumerable<Analyzed>> action)
        {
            action.Batch(s);
            Invoke(() =>
            {
                actionProgress.PerformStep();
            });
        }

        private void OpenHistory(object sender, EventArgs e)
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

        private void Changed(object sender, EventArgs e)
        {
            ApplyDirectory(listBox1.SelectedItem.ToString());
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (history.Count() != 0)
                ApplyDirectory(history.First());
            dragDropImage.Visible = false;

            timer1.Interval = 500;
            timer1.Tick += (s, e) => ReloadInformation();
            timer1.Enabled = true;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }
    }
}
