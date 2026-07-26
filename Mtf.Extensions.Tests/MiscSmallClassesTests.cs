using Mtf.Extensions.Exceptions;
using System.Net;

namespace Mtf.Extensions.Tests;

public class DateTimeExtensionsMoreTests
{
    [Test]
    public void SqlMinValue_MatchesExpectedDate()
    {
        Assert.That(DateTimeExtensions.SqlMinValue, Is.EqualTo(new DateTime(1753, 1, 1, 12, 0, 0)));
    }

    [Test]
    public void ToFriendlyString_UsesFixedInvariantFormat()
    {
        var date = new DateTime(2012, 11, 15, 14, 2, 35);

        Assert.That(date.ToFriendlyString(), Is.EqualTo("2012.11.15 14:02:35"));
    }
}

public class LocalizedExceptionTests
{
    [TearDown]
    public void ResetTranslator()
    {
        LocalizedException.Translator = null;
    }

    [Test]
    public void LocalizedMessage_NoTranslatorSet_ReturnsRawMessage()
    {
        var ex = new LocalizedException("Something went wrong: {0}", "detail");

        Assert.That(ex.LocalizedMessage, Is.EqualTo("Something went wrong: {0}"));
    }

    [Test]
    public void LocalizedMessage_TranslatorSet_UsesTranslatorOutput()
    {
        LocalizedException.Translator = (id, index, args) => $"translated:{id}";

        var ex = new LocalizedException("original message");

        Assert.That(ex.LocalizedMessage, Is.EqualTo("translated:original message"));
    }

    [Test]
    public void LocalizedMessage_WithInnerException_UsesInnermostMessage()
    {
        var inner = new InvalidOperationException("inner message");
        var ex = new LocalizedException("outer message", inner);

        Assert.That(ex.LocalizedMessage, Is.EqualTo("inner message"));
    }
}

public class ObjectArrayExtensionsMoreTests
{
    [Test]
    public void ConvertToString_NullArray_ReturnsEmptyString()
    {
        object[] elements = null;

        Assert.That(elements.ConvertToString(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ConvertToString_UsesTabSeparator()
    {
        var elements = new object[] { 1, 2, 3 };

        Assert.That(elements.ConvertToString(), Is.EqualTo("1\t2\t3"));
    }

    [Test]
    public void ToArrayString_NoArgOverload_NullArray_ReturnsEmptyString()
    {
        object[] elements = null;

        Assert.That(elements.ToArrayString(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ToArrayString_NoArgOverload_UsesSpaceSeparator()
    {
        var elements = new object[] { 1, 2, 3 };

        Assert.That(elements.ToArrayString(), Is.EqualTo("1 2 3"));
    }
}

public class EndPointExtensionsMoreTests
{
    private sealed class TextEndPoint : EndPoint
    {
        private readonly string text;

        public TextEndPoint(string text) => this.text = text;

        public override string ToString() => text;
    }

    [Test]
    public void GetEndPointInfo_NullEndpoint_ReturnsEmptyString()
    {
        EndPoint endpoint = null;

        Assert.That(endpoint.GetEndPointInfo(_ => new[] { "x" }), Is.EqualTo(string.Empty));
    }

    [Test]
    public void GetEndPointInfo_SpecificAddress_ReturnsEndpointTextUnchanged()
    {
        var endpoint = new IPEndPoint(IPAddress.Loopback, 8080);

        var result = endpoint.GetEndPointInfo(_ => new[] { "should-not-be-used" });

        Assert.That(result, Is.EqualTo("127.0.0.1:8080"));
    }

    [Test]
    public void GetEndPointInfo_AnyAddress_AppendsCallbackResults()
    {
        var endpoint = new TextEndPoint("0.0.0.0:8080");

        var result = endpoint.GetEndPointInfo(_ => new[] { "192.168.1.1" }, "|");

        Assert.That(result, Is.EqualTo("0.0.0.0:8080 192.168.1.1"));
    }

    [Test]
    public void GetIpAddress_NullEndpoint_ReturnsEmptyString()
    {
        EndPoint endpoint = null;

        Assert.That(endpoint.GetIpAddress(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void GetIpAddress_ValidEndpoint_ReturnsAddressPortion()
    {
        var endpoint = new IPEndPoint(IPAddress.Loopback, 8080);

        Assert.That(endpoint.GetIpAddress(), Is.EqualTo("127.0.0.1"));
    }
}
