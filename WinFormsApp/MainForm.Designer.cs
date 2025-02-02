namespace WinFormsApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label lblStatus;
        //TextBox для логов 
        private System.Windows.Forms.RichTextBox txtLog;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtDirectory = new TextBox();
            btnBrowse = new Button();
            lstFiles = new ListBox();
            btnRead = new Button();
            btnDelete = new Button();
            txtContent = new TextBox();
            lblStatus = new Label();
            txtLog = new RichTextBox();
            buttonTestError = new Button();
            SuspendLayout();
            // 
            // txtDirectory
            // 
            txtDirectory.Location = new Point(12, 12);
            txtDirectory.Name = "txtDirectory";
            txtDirectory.Size = new Size(320, 23);
            txtDirectory.TabIndex = 0;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(340, 10);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 25);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "Обзор";
            btnBrowse.Click += btnBrowse_Click;
            // 
            // lstFiles
            // 
            lstFiles.Location = new Point(12, 45);
            lstFiles.Name = "lstFiles";
            lstFiles.Size = new Size(320, 139);
            lstFiles.TabIndex = 2;
            // 
            // btnRead
            // 
            btnRead.Location = new Point(340, 45);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(75, 25);
            btnRead.TabIndex = 3;
            btnRead.Text = "Открыть";
            btnRead.Click += btnRead_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(340, 80);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 25);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Удалить";
            btnDelete.Click += btnDelete_Click;
            // 
            // txtContent
            // 
            txtContent.Location = new Point(12, 200);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.ScrollBars = ScrollBars.Vertical;
            txtContent.Size = new Size(400, 150);
            txtContent.TabIndex = 5;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(12, 360);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(400, 23);
            lblStatus.TabIndex = 6;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 390);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(400, 114);
            txtLog.TabIndex = 7;
            txtLog.Text = "";
            // 
            // buttonTestError
            // 
            buttonTestError.Location = new Point(340, 141);
            buttonTestError.Name = "buttonTestError";
            buttonTestError.Size = new Size(75, 23);
            buttonTestError.TabIndex = 8;
            buttonTestError.Text = "TestError";
            buttonTestError.UseVisualStyleBackColor = true;
            buttonTestError.Click += buttonTestError_Click;
            // 
            // MainForm
            // 
            ClientSize = new Size(428, 518);
            Controls.Add(buttonTestError);
            Controls.Add(txtDirectory);
            Controls.Add(btnBrowse);
            Controls.Add(lstFiles);
            Controls.Add(btnRead);
            Controls.Add(btnDelete);
            Controls.Add(txtContent);
            Controls.Add(lblStatus);
            Controls.Add(txtLog);
            Name = "MainForm";
            Text = "Файловый менеджер";
            ResumeLayout(false);
            PerformLayout();
        }

        private Button buttonTestError;
    }
}
