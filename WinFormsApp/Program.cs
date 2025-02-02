namespace WinFormsApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        // �������� ���������� ����� ��� ����������
        Application.EnableVisualStyles();
        // ������������� ������������� � ����������� ������ �� ���������
        Application.SetCompatibleTextRenderingDefault(false);

        // ���������� ��������� ���������� ��� ������� Windows Forms
        Application.ThreadException += (sender, e) => HandleException(e.Exception, false);
        // ���������� ��������� �������������� ���������� ��� ������ ����������
        AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException((Exception)e.ExceptionObject, true);

        // ������������� ������������ ����������
        ApplicationConfiguration.Initialize();
        // ������ ������� ����� ����������
        Application.Run(new MainForm());
    }

    // ����� ��� ��������� ����������
    private static void HandleException(Exception ex, bool isFatal)
    {
        // ��������� �� ������ � ����������� �� �����������
        string message = isFatal ? "����������� ������ ����������!" : "��������� ������.";
        // ����������� ����������� ������
        Logger.LogFatal(message, ex);

        // ����� ��������� �� ������ ������������
        MessageBox.Show($"{message}\n\n{ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // ��������� ���������� ��� ����������� ������
        if (isFatal)
        {
            Application.Exit();
        }
    }
}
