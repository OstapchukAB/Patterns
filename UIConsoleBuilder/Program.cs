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

        Console.WriteLine("Выберите формат отчёта (text/csv):");
        string format = Console.ReadLine()?.ToLower();

        IReportBuilder builder = format switch
        {
            "text" => new TextReportBuilder(),
            "csv" => new CsvReportBuilder(),
            _ => throw new ArgumentException("Неподдерживаемый формат!")
        };

        var director = new ReportDirector(builder);
        director.Construct("Отчёт о продажах", reportData);

        string report = director.GetReport();
        Console.WriteLine(report);
    }
}

