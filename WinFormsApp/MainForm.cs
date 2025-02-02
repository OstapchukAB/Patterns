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

        // ����� �����
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = dialog.SelectedPath;
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
                lblStatus.Text = "����� ���������.";
            }
            catch (Exception ex)
            {
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
                lblStatus.Text = "���� ������.";
            }
            catch (Exception ex)
            {
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
                LoadFiles();
                txtContent.Clear();
                lblStatus.Text = "���� �����.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "������: " + ex.Message;
            }
        }
    }
}

