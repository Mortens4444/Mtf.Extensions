using Mtf.Extensions.Exceptions;

namespace Mtf.Extensions.Tests;

public class StringExtensionsTests
{
    [Test]
    public void IndexOfAny_NullKeywords_ReturnsMinusOneInsteadOfThrowing()
    {
        IEnumerable<string> keywords = null;
        Assert.That("abc".IndexOfAny(keywords), Is.EqualTo(-1));
    }

    [Test]
    public void IndexOfAny_EmptyKeywords_ReturnsMinusOne()
    {
        Assert.That("abc".IndexOfAny(Array.Empty<string>()), Is.EqualTo(-1));
    }

    [Test]
    public void IndexOfAny_MatchingKeyword_ReturnsEarliestIndex()
    {
        Assert.That("hello world".IndexOfAny(new[] { "world", "hello" }), Is.EqualTo(0));
    }

    [Test]
    public void GetIpAddressAndPortFromEndPoint_MissingPort_ThrowsLocalizedException()
    {
        Ensure.Throws<LocalizedException>(() => "192.168.1.1".GetIpAddressAndPortFromEndPoint());
    }

    [Test]
    public void GetIpAddressAndPortFromEndPoint_ValidInput_ParsesCorrectly()
    {
        var result = "192.168.1.1:8080".GetIpAddressAndPortFromEndPoint();

        Assert.That(result.Item1, Is.EqualTo("192.168.1.1"));
        Assert.That(result.Item2, Is.EqualTo((ushort)8080));
    }

    [Test]
    public void GetPortFromEndPoint_MissingPort_ThrowsLocalizedException()
    {
        Ensure.Throws<LocalizedException>(() => "192.168.1.1".GetPortFromEndPoint());
    }

    [Test]
    public void GetPortFromEndPoint_ValidInput_ReturnsPort()
    {
        Assert.That("192.168.1.1:8080".GetPortFromEndPoint(), Is.EqualTo((ushort)8080));
    }

    [Test]
    public void Substring_CommaImmediatelyAfterFirstElement_ReturnsEmptyString()
    {
        Assert.That("Name:,End".Substring("Name:"), Is.EqualTo(string.Empty));
    }

    [Test]
    public void Substring_NormalCase_ReturnsTextBetweenFirstElementAndComma()
    {
        Assert.That("Name:John,End".Substring("Name:"), Is.EqualTo("John"));
    }

    [Test]
    public void ExtractBetween_EmptyContentBetweenDelimiters_ReturnsEmptyStringNotNull()
    {
        Assert.That("<a></a>".ExtractBetween("<a>", "</a>"), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ExtractBetween_NormalCase_ReturnsContentBetweenDelimiters()
    {
        Assert.That("<a>hello</a>".ExtractBetween("<a>", "</a>"), Is.EqualTo("hello"));
    }

    [Test]
    public void ExtractBetween_DelimitersNotFound_ReturnsNull()
    {
        Assert.That("no delimiters here".ExtractBetween("<a>", "</a>"), Is.Null);
    }

    [Test]
    public void HexaToInteger_InvalidCharacter_ThrowsFormatExceptionInsteadOfSilentlyCorrupting()
    {
        Ensure.Throws<FormatException>(() => "1G3".HexaToInteger());
    }

    [Test]
    public void HexaToInteger_ValidHex_ReturnsCorrectValue()
    {
        Assert.That("1F".HexaToInteger(), Is.EqualTo(31));
    }

    [Test]
    public void ReplaceHtmlCharacterEntities_RsaquoWithSemicolon_IsReplaced()
    {
        Assert.That("a&rsaquo;b".ReplaceHtmlCharacterEntities(), Is.EqualTo("a›b"));
    }

    [Test]
    public void ReplaceHtmlCharacterEntities_RsaquoWithoutSemicolon_IsLeftUnchanged()
    {
        Assert.That("a&rsaquo b".ReplaceHtmlCharacterEntities(), Is.EqualTo("a&rsaquo b"));
    }

    [Test]
    public void GetSpecialStringWithoutAccent_MicroSign_PassesThroughUnchanged()
    {
        Assert.That("5µm".GetSpecialStringWithoutAccent(), Is.EqualTo("5µm"));
    }

    [Test]
    public void GetSpecialStringWithoutAccent_AccentedLetters_AreNormalized()
    {
        Assert.That("café".GetSpecialStringWithoutAccent(), Is.EqualTo("cafe"));
    }
}
