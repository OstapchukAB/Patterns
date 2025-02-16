#region Задание: Реализация паттерна "Шаблонный метод" для генерации отчетов
/*
 Цель:
Создать систему генерации отчетов, 
где общий алгоритм (сбор данных → форматирование → сохранение) стандартизирован,
но каждый шаг может быть кастомизирован для разных форматов (PDF, CSV).
 */
#endregion

namespace UITemplateMethodExample;
public abstract class ReportGenerator
{

    public void Generate()
    {
        PrintReportHeader();
        CollectData();
        FormatData();
        SaveData();
    }

    protected virtual void PrintReportHeader()=>Console.WriteLine($"-----Генерация {GetReportType()} отчета----");

    void CollectData()
    {
        Console.WriteLine($"Сбор данных для {GetReportType()} отчета");
    }
    void SaveData()
    {
        Console.WriteLine($"Сохранение {GetReportType()} отчета");
    }
    protected abstract void FormatData();
    protected abstract string GetReportType();
}

public class PdfReportGenerate : ReportGenerator
{
    protected override void FormatData()
    {
        Console.WriteLine($"Форматирование данных для {GetReportType()} отчета");
    }

    protected override string GetReportType()
    {
        return "PDF";
    }
}
public class ExcelReportGenerate : ReportGenerator
{
    protected override void FormatData()
    {
        Console.WriteLine($"Форматирование данных для {GetReportType()} отчета");
    }

    protected override string GetReportType()
    {
        return "Excel";
    }
}

//class Program
//{
//    static void Main()
//    {
//        ReportGenerator pdfReport = new PdfReportGenerate();
//        pdfReport.Generate();
//        ReportGenerator excelReport = new ExcelReportGenerate();
//        excelReport.Generate();
//    }
//}