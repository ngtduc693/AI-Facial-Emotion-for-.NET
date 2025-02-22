namespace WinForms;

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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnChooseFile = new System.Windows.Forms.Button();
        openFileDialog = new System.Windows.Forms.OpenFileDialog();
        lblResult = new System.Windows.Forms.Label();
        pbSelectedImage = new System.Windows.Forms.PictureBox();
        ((System.ComponentModel.ISupportInitialize)pbSelectedImage).BeginInit();
        SuspendLayout();
        // 
        // btnChooseFile
        // 
        btnChooseFile.Location = new System.Drawing.Point(12, 12);
        btnChooseFile.Name = "btnChooseFile";
        btnChooseFile.Size = new System.Drawing.Size(138, 31);
        btnChooseFile.TabIndex = 0;
        btnChooseFile.Text = "Choose file";
        btnChooseFile.UseVisualStyleBackColor = true;
        btnChooseFile.Click += btnChooseFile_Click;
        // 
        // openFileDialog
        // 
        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
        openFileDialog.FileOk += openFileDialog_FileOk;
        // 
        // lblResult
        // 
        lblResult.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
        lblResult.Location = new System.Drawing.Point(269, 13);
        lblResult.Name = "lblResult";
        lblResult.Size = new System.Drawing.Size(280, 32);
        lblResult.TabIndex = 1;
        lblResult.Text = "Result: ";
        // 
        // pbSelectedImage
        // 
        pbSelectedImage.Location = new System.Drawing.Point(17, 60);
        pbSelectedImage.Name = "pbSelectedImage";
        pbSelectedImage.Size = new System.Drawing.Size(132, 105);
        pbSelectedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        pbSelectedImage.TabIndex = 2;
        pbSelectedImage.TabStop = false;
        // 
        // frmMain
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(561, 177);
        Controls.Add(pbSelectedImage);
        Controls.Add(lblResult);
        Controls.Add(btnChooseFile);
        MaximizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "AI Facial Emotion";
        ((System.ComponentModel.ISupportInitialize)pbSelectedImage).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.PictureBox pbSelectedImage;

    private System.Windows.Forms.Label lblResult;

    private System.Windows.Forms.Button btnChooseFile;
    private System.Windows.Forms.OpenFileDialog openFileDialog;

    #endregion
}