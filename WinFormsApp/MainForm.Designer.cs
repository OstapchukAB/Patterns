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
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // txtDirectory
            this.txtDirectory.Location = new System.Drawing.Point(12, 12);
            this.txtDirectory.Size = new System.Drawing.Size(320, 23);

            // btnBrowse
            this.btnBrowse.Location = new System.Drawing.Point(340, 10);
            this.btnBrowse.Size = new System.Drawing.Size(75, 25);
            this.btnBrowse.Text = "Обзор";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // lstFiles
            this.lstFiles.Location = new System.Drawing.Point(12, 45);
            this.lstFiles.Size = new System.Drawing.Size(320, 150);

            // btnRead
            this.btnRead.Location = new System.Drawing.Point(340, 45);
            this.btnRead.Size = new System.Drawing.Size(75, 25);
            this.btnRead.Text = "Открыть";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(340, 80);
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // txtContent
            this.txtContent.Location = new System.Drawing.Point(12, 200);
            this.txtContent.Size = new System.Drawing.Size(400, 150);
            this.txtContent.Multiline = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // lblStatus
            this.lblStatus.Location = new System.Drawing.Point(12, 360);
            this.lblStatus.Size = new System.Drawing.Size(400, 23);

            // MainForm
            this.ClientSize = new System.Drawing.Size(430, 400);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.lblStatus);
            this.Text = "Файловый менеджер";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
