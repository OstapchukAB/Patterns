using FabricMethodClassLib;

namespace WinFormsApp
{


    public partial class MainForm : Form
    {
        private readonly IFileService _fileService = new FileService();


        public MainForm()
        {
            InitializeComponent();
            Logger.InitLogger(txtLog); // Передаём TextBox для логов

        }

        // Выбор папки
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = dialog.SelectedPath;
                Logger.LogInfo($"Выбрана папка: {dialog.SelectedPath}");
                LoadFiles();
            }
        }

        // Загрузка файлов в ListBox
        private void LoadFiles()
        {
            lstFiles.Items.Clear();
            try
            {
                string[] files = _fileService.GetFiles(txtDirectory.Text);
                lstFiles.Items.AddRange(files);
                Logger.LogInfo($"Загружено {files.Length} файлов из {txtDirectory.Text}");
                lblStatus.Text = "Файлы загружены.";
            }
            catch (Exception ex)
            {
                Logger.LogError("Ошибка при загрузке файлов", ex);
                lblStatus.Text = "Ошибка: " + ex.Message;
            }
        }

        // Чтение файла
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string filePath = lstFiles.SelectedItem.ToString();

            try
            {
                txtContent.Text = _fileService.ReadFile(filePath);
                Logger.LogInfo($"Открыт файл: {filePath}");
                lblStatus.Text = "Файл открыт.";
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при чтении файла: {filePath}", ex);
                lblStatus.Text = "Ошибка: " + ex.Message;
            }
        }

        // Удаление файла
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem == null) return;
            string filePath = lstFiles.SelectedItem.ToString();

            try
            {
                _fileService.DeleteFile(filePath);
                Logger.LogInfo($"Удалён файл: {filePath}");
                LoadFiles();
                txtContent.Clear();
                lblStatus.Text = "Файл удалён.";
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при удалении файла: {filePath}", ex);
                lblStatus.Text = "Ошибка: " + ex.Message;
            }
        }


        private void buttonTestError_Click(object sender, EventArgs e)
        {
            throw new Exception("Тестовая ошибка в WinForms!");
        }
    }
}

