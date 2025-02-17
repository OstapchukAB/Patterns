using FabricMethodClassLib;

namespace WinFormsApp
{


    public partial class MainForm : Form
    {
        private readonly IFileService _fileService = new FileService();


        public MainForm()
        {
            InitializeComponent();
            Logger.InitLogger(txtLog); // ������� TextBox ��� �����

        }

        // ����� �����
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = dialog.SelectedPath;
                Logger.LogInfo($"������� �����: {dialog.SelectedPath}");
                LoadFiles();
            }
        }

        // �������� ������ � ListBox
        private void LoadFiles()
        {
            lstFiles.Items.Clear();
            try
            {
                string[] files = _fileService.GetFiles(txtDirectory.Text);
                lstFiles.Items.AddRange(files);
                Logger.LogInfo($"��������� {files.Length} ������ �� {txtDirectory.Text}");
                lblStatus.Text = "����� ���������.";
            }
            catch (Exception ex)
            {
                Logger.LogError("������ ��� �������� ������", ex);
                lblStatus.Text = "������: " + ex.Message;
            }
        }

        // ������ �����
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string filePath = lstFiles.SelectedItem.ToString();

            try
            {
                txtContent.Text = _fileService.ReadFile(filePath);
                Logger.LogInfo($"������ ����: {filePath}");
                lblStatus.Text = "���� ������.";
            }
            catch (Exception ex)
            {
                Logger.LogError($"������ ��� ������ �����: {filePath}", ex);
                lblStatus.Text = "������: " + ex.Message;
            }
        }

        // �������� �����
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string filePath = lstFiles.SelectedItem.ToString();

            try
            {
                _fileService.DeleteFile(filePath);
                Logger.LogInfo($"����� ����: {filePath}");
                LoadFiles();
                txtContent.Clear();
                lblStatus.Text = "���� �����.";
            }
            catch (Exception ex)
            {
                Logger.LogError($"������ ��� �������� �����: {filePath}", ex);
                lblStatus.Text = "������: " + ex.Message;
            }
        }


        private void buttonTestError_Click(object sender, EventArgs e)
        {
            throw new Exception("�������� ������ � WinForms!");
        }
    }
}

