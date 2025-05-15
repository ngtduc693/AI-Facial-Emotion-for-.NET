namespace Example.WindowsApp
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnChooseFile = new Button();
            lblResult = new Label();
            pbSelectedImage = new PictureBox();
            openFileDialog = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)pbSelectedImage).BeginInit();
            SuspendLayout();
            // 
            // btnChooseFile
            // 
            btnChooseFile.Location = new Point(12, 12);
            btnChooseFile.Name = "btnChooseFile";
            btnChooseFile.Size = new Size(126, 35);
            btnChooseFile.TabIndex = 0;
            btnChooseFile.Text = "Choose file";
            btnChooseFile.UseVisualStyleBackColor = true;
            btnChooseFile.Click += btnChooseFile_Click;
            // 
            // lblResult
            // 
            lblResult.AutoSize = true;
            lblResult.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            lblResult.Location = new Point(144, 13);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(85, 30);
            lblResult.TabIndex = 1;
            lblResult.Text = "Result: ";
            // 
            // pbSelectedImage
            // 
            pbSelectedImage.Location = new Point(12, 55);
            pbSelectedImage.Name = "pbSelectedImage";
            pbSelectedImage.Size = new Size(126, 114);
            pbSelectedImage.SizeMode = PictureBoxSizeMode.StretchImage;
            pbSelectedImage.TabIndex = 2;
            pbSelectedImage.TabStop = false;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(453, 179);
            Controls.Add(pbSelectedImage);
            Controls.Add(lblResult);
            Controls.Add(btnChooseFile);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AI Facial Emotion in .NET 10";
            ((System.ComponentModel.ISupportInitialize)pbSelectedImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnChooseFile;
        private Label lblResult;
        private PictureBox pbSelectedImage;
        private OpenFileDialog openFileDialog;
    }
}
