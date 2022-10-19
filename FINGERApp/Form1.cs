namespace FINGERApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void startClean(object sender, EventArgs e)
        {

        }

        private void recentFolder(object sender, EventArgs e)
        {

        }

        private void findFolder(object sender, EventArgs e)
        {
            //folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void dragDropFolder(object sender, DragEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            findFolder(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("asdf");
            findFolder(sender, e);
        }
    }
}