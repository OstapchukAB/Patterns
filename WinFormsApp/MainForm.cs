using System;
using System.IO;
using System.Windows.Forms;
using MyClassLib;

namespace WinFormsApp
{
    public partial class MainForm : Form
    {
        private readonly IFileService _fileService = new FileService();

        public MainForm()
        {
            InitializeComponent();
        }

        // Выбор папки
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = dialog.SelectedPath;
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
                lblStatus.Text = "Файлы загружены.";
            }
            catch (Exception ex)
            {
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
                lblStatus.Text = "Файл открыт.";
            }
            catch (Exception ex)
            {
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
                LoadFiles();
                txtContent.Clear();
                lblStatus.Text = "Файл удалён.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Ошибка: " + ex.Message;
            }
        }
    }
}

