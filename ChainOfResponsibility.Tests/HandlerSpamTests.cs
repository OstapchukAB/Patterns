using Moq;
using UIChainOfResponsibilitySpamFilter;
namespace ChainOfResponsibility.Tests;

public class HandlerSpamTests
{
    [Fact]
    public void HandleRequest_TextContainsSpamKeyword_MarksAsSpam()
    {
        var handler = new HandlerSpam();
        var email = new Email(1, "test@test.com", "бесплатные товары");

        handler.HandleRequest(email);

        Assert.Equal(EmailType.SPAM, email.PathEmail);
    }

    [Fact]
    public void HandleRequest_NoSpamKeywords_CallsNextHandler()
    {
        var handler = new HandlerSpam();
        var mockNextHandler = new Mock<EmailFilterHandler>();
        handler.Successor = mockNextHandler.Object;
        var email = new Email(2, "test@test.com", "обычное письмо");

        handler.HandleRequest(email);

        mockNextHandler.Verify(h => h.HandleRequest(email), Times.Once);
    }
}
public class HandlerImportantTests
{
    [Fact]
    public void HandleRequest_SenderContainsDirector_MarksAsImportant()
    {
        var handler = new HandlerImportant();
        var email = new Email(1, "director@company.com", "срочное задание");

        handler.HandleRequest(email);

        Assert.Equal(EmailType.IMPORTANT, email.PathEmail);
    }
}
public class HandlerCoworkerTests
{
    [Fact]
    public void HandleRequest_SenderWithCorpDomain_MarksAsCoworker()
    {
        var handler = new HandlerCoworker();
        var email = new Email(1, "user@corp.com", "отчет");

        handler.HandleRequest(email);

        Assert.Equal(EmailType.COWORKER, email.PathEmail);
    }
}
public class ChainIntegrationTests
{
    private EmailFilterHandler CreateChain()
    {
        var chain = new HandlerSpam();
        chain.Successor = new HandlerImportant();
        chain.Successor.Successor = new HandlerCoworker();
        chain.Successor.Successor.Successor = new HandlerKeysWord();
        chain.Successor.Successor.Successor.Successor = new HandlerDefault();
        return chain;
    }

    [Fact]
    public void ProcessEmail_MultipleConditions_FirstValidHandlerProcesses()
    {
        var chain = CreateChain();
        var email = new Email(1, "director@corp.com", "распродажа и пароль");

        chain.HandleRequest(email);

        Assert.Equal(EmailType.SPAM, email.PathEmail);
    }
}
public class EmailSorterTests
{
    [Theory]
    [InlineData(EmailType.SPAM, "Spam")]
    [InlineData(EmailType.IMPORTANT, "Important")]
    public void SortEmail_EmailType_OutputsCorrectFolder(EmailType type, string expectedFolder)
    {
        var sorter = new EmailSorter();
        var email = new Email(1, "test", "text") { PathEmail = type };
        using var sw = new StringWriter();
        Console.SetOut(sw);

        sorter.SortEmail(email);

        Assert.Contains(expectedFolder, sw.ToString());
    }
}
public class FullChainTests
{
    [Fact]
    public void FullChain_ProcessSampleEmails_CorrectlyMarksAll()
    {
        var chain = new HandlerSpam();
        chain.Successor = new HandlerImportant();
        chain.Successor.Successor = new HandlerCoworker();
        chain.Successor.Successor.Successor = new HandlerKeysWord();
        chain.Successor.Successor.Successor.Successor = new HandlerDefault();

        var emails = new[] {
            new Email(1, "spammer", "купите бесплатно"),
            new Email(2, "boss", "важное"),
            new Email(3, "hr@corp.com", "документы"),
            new Email(4, "unknown", "пароль: 123")
        };

        foreach (var email in emails)
        {
            chain.HandleRequest(email);
        }

        Assert.Equal(EmailType.SPAM, emails[0].PathEmail);
        Assert.Equal(EmailType.IMPORTANT, emails[1].PathEmail);
        Assert.Equal(EmailType.COWORKER, emails[2].PathEmail);
        Assert.Equal(EmailType.KEYSWORD, emails[3].PathEmail);
    }
}