using System.ComponentModel;
namespace WinForms;

public partial class frmMain : Form
{
    private readonly EmotionAnalyzer _emotionAnalyzer;
    public frmMain()
    {
        InitializeComponent();
        _emotionAnalyzer = new EmotionAnalyzer();
    }

    private void openFileDialog_FileOk(object sender, CancelEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private async void btnChooseFile_Click(object sender, EventArgs e)
    {
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
                    var result = await _emotionAnalyzer.AnalyzeEmotionFromStreamAsync(stream);
                    lblResult.Text = $"Emotion: {result.Emotion}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}