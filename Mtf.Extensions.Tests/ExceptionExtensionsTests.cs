namespace Mtf.Extensions.Tests;

public class ExceptionExtensionsTests
{
    [Test]
    public void GetErrorCode_ReturnsNonZeroHResult()
    {
        var ex = new InvalidOperationException("boom");

        var code = ex.GetErrorCode();

        Assert.That(code, Is.Not.EqualTo(0));
    }

    [Test]
    public void GetDetails_NullException_ThrowsArgumentNullException()
    {
        Exception ex = null;

        Ensure.Throws<ArgumentNullException>(() => ex.GetDetails());
    }

    [Test]
    public void GetDetails_SingleException_ContainsTypeAndMessage()
    {
        var ex = new InvalidOperationException("boom");

        var details = ex.GetDetails();

        Assert.That(details, Does.Contain("InvalidOperationException"));
        Assert.That(details, Does.Contain("boom"));
    }

    [Test]
    public void GetDetails_WithInnerExceptions_IncludesAllLevels()
    {
        var inner = new ArgumentException("inner message");
        var outer = new InvalidOperationException("outer message", inner);

        var details = outer.GetDetails();

        Assert.That(details, Does.Contain("outer message"));
        Assert.That(details, Does.Contain("inner message"));
        Assert.That(details, Does.Contain("InvalidOperationException"));
        Assert.That(details, Does.Contain("ArgumentException"));
    }

    [Test]
    public void GetDetails_CustomTitle_UsesProvidedTitle()
    {
        var ex = new InvalidOperationException("boom");

        var details = ex.GetDetails("CustomTitle");

        Assert.That(details, Does.Contain("CustomTitle"));
    }

    [Test]
    public void GetLastInnerExceptionMessage_NoInnerException_ReturnsOwnMessage()
    {
        var ex = new InvalidOperationException("top level");

        Assert.That(ex.GetLastInnerExceptionMessage(), Is.EqualTo("top level"));
    }

    [Test]
    public void GetLastInnerExceptionMessage_WithInnerExceptions_ReturnsDeepestMessage()
    {
        var innermost = new ArgumentException("deepest message");
        var middle = new InvalidOperationException("middle message", innermost);
        var outer = new Exception("outer message", middle);

        Assert.That(outer.GetLastInnerExceptionMessage(), Is.EqualTo("deepest message"));
    }
}
