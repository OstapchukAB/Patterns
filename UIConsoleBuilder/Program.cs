﻿using Newtonsoft.Json;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Text;

namespace BuilderPattern;
#region UML schema
/*
 +----------------------+           +----------------------+
|   IReportBuilder    |<----------|   TextReportBuilder  |
|----------------------|           |----------------------|
| void AddHeader()     |           | void AddHeader()     |
| void AddContent()    |           | void AddContent()    |
| string GetReport()   |           | string GetReport()   |
+----------------------+           +----------------------+
       | (реализация)                | (реализация)
       v                              v
+----------------------+           +----------------------+
|  CsvReportBuilder   |           |   ReportDirector    |
|----------------------|           |----------------------|
| void AddHeader()     |           | void Construct()    |
| void AddContent()    |           |                    |
| string GetReport()   |           |                    |
+----------------------+           +----------------------+

 */
#endregion

#region Интерфейс строителя
public interface IReportBuilder
{
    void AddHeader(string title);
    void AddContent(Dictionary<string, string> data);
    string GetReport();
}
#endregion

#region  Реализация строителя для текстового отчёта
public class TextReportBuilder : IReportBuilder
{
    private StringBuilder _report = new();

    public void AddHeader(string title)
    {
        _report.AppendLine($"=== {title} ===\n");
    }

    public void AddContent(Dictionary<string, string> data)
    {
        foreach (var item in data)
        {
            _report.AppendLine($"{item.Key}: {item.Value}");
        }
    }

    public string GetReport() => _report.ToString();
}
#endregion
#region  Реализация строителя для CSV-отчёта
public class CsvReportBuilder : IReportBuilder
{
    private StringBuilder _report = new();

    public void AddHeader(string title)
    {
        _report.AppendLine($"Title,{title}");
    }

    public void AddContent(Dictionary<string, string> data)
    {
        foreach (var item in data)
        {
            _report.AppendLine($"{item.Key},{item.Value}");
        }
    }

    public string GetReport() => _report.ToString();
}
#endregion

#region Реализация строителя для JSON-отчёта
public class JsonReportBuilder : IReportBuilder
{
    private string _title;
    private Dictionary<string, string> _data;

    public void AddHeader(string title)
    {
        _title = title;
    }

    public void AddContent(Dictionary<string, string> data)
    {
        _data = data;
    }

    public string GetReport()
    {
        var reportObject = new
        {
            Title = _title,
            Data = _data
        };
        return JsonConvert.SerializeObject(reportObject, Formatting.Indented);
    }
}
#endregion
#region Реализация строителя для PDF-отчёта
public class PdfReportBuilder : IReportBuilder
{

    private readonly PdfDocument _document = new();
    private PdfPage _page;
    private XGraphics _gfx;
    private readonly XFont _titleFont = new("Arial", 14, XFontStyleEx.Bold);
    private readonly XFont _contentFont = new("Arial", 12);
    private string _title;
    private Dictionary<string, string> _data;

    public void AddHeader(string title)
    {
        _title = title;
        _page = _document.AddPage();
        _gfx = XGraphics.FromPdfPage(_page);
        _gfx.DrawString(_title, _titleFont, XBrushes.Black, new XPoint(20, 20));
    }

    public void AddContent(Dictionary<string, string> data)
    {
        _data = data;
        var yPosition = 50;

        foreach (var item in _data)
        {
            _gfx.DrawString($"{item.Key}: {item.Value}",
                          _contentFont,
                          XBrushes.Black,
                          new XPoint(20, yPosition));
            yPosition += 20;
        }
    }

    public string GetReport()
    {
        using var stream = new MemoryStream();
        _document.Save(stream);
        return Convert.ToBase64String(stream.ToArray());
    }
}
#endregion



#region Директор, управляющий процессом построения отчёта
public class ReportDirector
{
    private readonly IReportBuilder _builder;

    public ReportDirector(IReportBuilder builder)
    {
        _builder = builder;
    }

    public void Construct(string title, Dictionary<string, string> data)
    {
        _builder.AddHeader(title);
        _builder.AddContent(data);
    }

    public string GetReport() => _builder.GetReport();
}
#endregion

#region Сохранение отчёта в файл
public static class ReportSaver
{
    private static readonly string DirectoryPath = "Reports";

    static ReportSaver()
    {
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
    }


    public static void SaveToFile(string report, string format)
    {
        string extension = format switch
        {
            "txt" => "txt",
            "csv" => "csv",
            "json" => "json",
            "pdf" => "pdf",
            _ => throw new ArgumentException("Неподдерживаемый формат")
        };


        string filePath = Path.Combine(DirectoryPath, $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}");
        if (format == "pdf")
        {
            var pdfBytes = Convert.FromBase64String(report);
            File.WriteAllBytes(filePath, pdfBytes);
        }
        else
        {
            File.WriteAllText(filePath, report, Encoding.UTF8);
        }

        Console.WriteLine($"Отчёт сохранён в файл: {filePath}");
    }
}
#endregion
class Program
{
    static void Main()
    {
        var reportData = new Dictionary<string, string>
        {
            { "Дата", DateTime.Now.ToString("yyyy-MM-dd") },
            { "Продажи", "5000$" },
            { "Клиенты", "35" }
        };

        Console.WriteLine("Выберите формат отчёта (text/csv/json/pdf):");
        string format = Console.ReadLine()?.ToLower();

        IReportBuilder builder = format switch
        {
            "txt" => new TextReportBuilder(),
            "csv" => new CsvReportBuilder(),
            "json" => new JsonReportBuilder(),
            "pdf" => new PdfReportBuilder(),
            _ => throw new ArgumentException("Неподдерживаемый формат!")
        };

        var director = new ReportDirector(builder);
        director.Construct("Отчёт о продажах", reportData);
        var report = director.GetReport();

        if (!format.Contains("pdf"))
        {
            Console.WriteLine("Вывести отчёт на экран? (y/n)");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                Console.WriteLine(report);
            }
        }

        ReportSaver.SaveToFile(report, format);
    }

}