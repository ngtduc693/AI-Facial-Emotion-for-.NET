using AI.Facial.Emotion;
using System.Diagnostics;

namespace Example.WindowsApp
{
    public partial class frmMain : Form
    {
        private readonly EmotionAnalyzer _emotionAnalyzer;
        public frmMain()
        {
            InitializeComponent();
            _emotionAnalyzer = new EmotionAnalyzer();
        }

        private async void btnChooseFile_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    pbSelectedImage.Image = System.Drawing.Image.FromFile(filePath);

                    try
                    {
                        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        stopwatch.Start();
                        var result = await _emotionAnalyzer.AnalyzeEmotionFromStreamAsync(stream);
                        stopwatch.Stop();
                        lblResult.Text = $"Emotion: {result.Emotion} in {stopwatch.ElapsedMilliseconds / 1000.0} s";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
