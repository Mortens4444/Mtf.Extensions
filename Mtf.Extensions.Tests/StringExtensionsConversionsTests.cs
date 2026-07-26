namespace Mtf.Extensions.Tests;

public class StringExtensionsConversionsTests
{
    [Test]
    public void ConvertBinaryToText_RoundTripsWithConvertTextToBinary()
    {
        Assert.That("01000001".ConvertBinaryToText(), Is.EqualTo("A"));
        Assert.That("A".ConvertTextToBinary(), Is.EqualTo("01000001"));
    }

    [Test]
    public void ConvertBinaryToText_EmptyString_ReturnsEmptyString()
    {
        Assert.That(string.Empty.ConvertBinaryToText(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void BinaryToDecimalByte_ParsesBinaryDigits()
    {
        Assert.That(Mtf.Extensions.StringExtensions.BinaryToDecimalByte("00000101"), Is.EqualTo((byte)5));
    }

    [Test]
    public void ConvertHexToText_RoundTripsWithConvertTextToHex()
    {
        Assert.That("41".ConvertHexToText(), Is.EqualTo("A"));
        Assert.That("A".ConvertTextToHex(), Is.EqualTo("41"));
    }

    [Test]
    public void ConvertHexToText_EmptyString_ReturnsEmptyString()
    {
        Assert.That(string.Empty.ConvertHexToText(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void HexaToDecimal_ParsesHexToDecimalString()
    {
        Assert.That("FF".HexaToDecimal(), Is.EqualTo("255"));
    }

    [Test]
    public void HexaStringToASCII_RoundTripsWithASCIIToHexaString()
    {
        Assert.That("48656C6C6F".HexaStringToASCII(), Is.EqualTo("Hello"));
        Assert.That("Hello".ASCIIToHexaString(), Is.EqualTo("48656C6C6F"));
    }

    [Test]
    public void HexaStringToASCII_OddLength_ThrowsArgumentException()
    {
        Ensure.Throws<ArgumentException>(() => "ABC".HexaStringToASCII());
    }

    [Test]
    public void HexaStringToASCII_EmptyString_ReturnsEmptyString()
    {
        Assert.That(string.Empty.HexaStringToASCII(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void Base64Encode_RoundTripsWithBase64Decode()
    {
        var encoded = "Hello, world!".Base64Encode();

        Assert.That(encoded.Base64Decode(), Is.EqualTo("Hello, world!"));
    }

    [Test]
    public void UrlEncode_RoundTripsWithUrlDecode()
    {
        var encoded = "a b&c=d".UrlEncode();

        Assert.That(encoded.UrlDecode(), Is.EqualTo("a b&c=d"));
        Assert.That(encoded, Does.Not.Contain(" "));
    }

    [Test]
    public void HtmlEncode_RoundTripsWithHtmlDecode()
    {
        var encoded = "<a>&b</a>".HtmlEncode();

        Assert.That(encoded.HtmlDecode(), Is.EqualTo("<a>&b</a>"));
        Assert.That(encoded, Does.Not.Contain("<"));
    }

    [Test]
    public void EscapeString_EscapesXmlSpecialCharacters()
    {
        var result = "<tag attr=\"a&b\">".EscapeString();

        Assert.That(result, Does.Not.Contain("<tag"));
        Assert.That(result, Does.Contain("&lt;"));
    }

    [Test]
    public void GetCodedString_AppliesLeetSpeakSubstitutionToEveryCharacter()
    {
        Assert.That("sale".GetCodedString(), Is.EqualTo("5413"));
    }
}
