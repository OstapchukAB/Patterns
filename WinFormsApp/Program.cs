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

        // �� �������� ������ �������� ��� false � Application.ThreadException += (sender, e) => HandleException(e.Exception, false);
        // ������ ��� ����������, �������������� ���� ������������, �� ��������� ������������ ��� ����� ����������.
        //����������:
        //Application.ThreadException: ���� ���������� ������������ ��� ��������� ����������, ������� ���������� � ������� Windows Forms.
        //����� ���������� ����� ���� ������� �������� � ���������������� ���������� ��� ������ ������ ����������, ���������� � �������� ������.
        //	false ��� isFatal: �������� false ��������� �� ��, ��� ��� ���������� �� �������� ������������ � �� ������� ������������ ���������� ����������.
        //	��� ��������� ���������� ���������� ������ ����� ��������� ����������.
        //������:
        //        ���� ������������ �������� ������, � � ����������� ������� ���������� ����������, ��� ���������� ����� ���������� Application.ThreadException,
        //        � ���������� ������ ���������� ������ ����� ����������� ��������� �� ������.
        //�������� � AppDomain.CurrentDomain.UnhandledException:
        //	AppDomain.CurrentDomain.UnhandledException: ���� ���������� ������������ ��� ��������� �������������� ����������, ������� ���������� � ����� ������ ���������� �
        //	�� ���� ������� ������� �������������. ����� ���������� ��������� ������������.
        //	true ��� isFatal: �������� true ��������� �� ��, ��� ��� ���������� �������� ������������ � ������� ������������ ���������� ����������.
        //������:
        //        ���� ���������� ����������, ������� �� ���� ������� �� ����� ������������, ��� ����� ���������� AppDomain.CurrentDomain.UnhandledException,
        //        � ���������� ���������� ����� ����������� ��������� �� ������.




        // ���������� ��������� �������������� ���������� ��� ������ ����������
        AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException((Exception)e.ExceptionObject, true);

        // ������������� ������������ ����������
        ApplicationConfiguration.Initialize();
        // ������ ������� ����� ����������
        try
        {
            Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
            HandleException(ex, true);
        }
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
            Environment.Exit(1); // ������������� ��������� ����������
        }
    }
}
