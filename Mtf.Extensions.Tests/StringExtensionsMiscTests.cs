using System.Security;

namespace Mtf.Extensions.Tests;

public class StringExtensionsMiscTests
{
    [Test]
    public void GetProgramAndParameters_NullCommand_ThrowsArgumentNullException()
    {
        string command = null;
        Assert.Throws<ArgumentNullException>(() => command.GetProgramAndParameters());
    }

    [Test]
    public void GetProgramAndParameters_SimpleCommand_SplitsOnSpaces()
    {
        var result = "prog arg1 arg2".GetProgramAndParameters();

        Assert.That(result, Is.EqualTo(new[] { "prog", "arg1", "arg2" }));
    }

    [Test]
    public void GetProgramAndParameters_QuotedArgumentWithSpace_KeptTogether()
    {
        var result = "\"C:\\my dir\\prog.exe\" arg1".GetProgramAndParameters();

        Assert.That(result, Is.EqualTo(new[] { "C:\\my dir\\prog.exe", "arg1" }));
    }

    [Test]
    public void GetProgramAndParameters_UnclosedQuote_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => "\"unterminated".GetProgramAndParameters());
    }

    [Test]
    public void HungarianToEnglish_ReplacesAccentedLettersWithBaseLatinLetters()
    {
        Assert.That("árvíztűrő tükörfúrógép".HungarianToEnglish(), Is.EqualTo("arvizturo tukorfurogep"));
    }

    [Test]
    public void HungarianToEnglish_NullOrEmpty_ReturnsEmptyString()
    {
        string value = null;
        Assert.That(value.HungarianToEnglish(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void Reverse_ReversesCharacterOrder()
    {
        Assert.That("Hello".Reverse(), Is.EqualTo("olleH"));
    }

    [Test]
    public void Reverse_NullOrEmpty_ReturnsEmptyString()
    {
        string value = null;
        Assert.That(value.Reverse(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void Remove_RemovesMatchingPatternCaseInsensitively()
    {
        Assert.That("Hello World".Remove("world"), Is.EqualTo("Hello "));
    }

    [Test]
    public void ArrayStringToArray_EmptyString_ReturnsEmptyArray()
    {
        Assert.That(string.Empty.ArrayStringToArray(), Is.Empty);
    }

    [Test]
    public void ArrayStringToArray_ParsesBracketedFormat()
    {
        Assert.That("[12][243][124][68]".ArrayStringToArray(), Is.EqualTo(new byte[] { 12, 243, 124, 68 }));
    }

    [Test]
    public void SplitedWithIndex_CharSeparator_NullOrEmpty_ReturnsEmptyString()
    {
        string value = null;
        Assert.That(value.SplitedWithIndex(',', 0), Is.EqualTo(string.Empty));
    }

    [Test]
    public void SplitedWithIndex_CharSeparator_ReturnsElementAtIndex()
    {
        Assert.That("a,b,c".SplitedWithIndex(',', 1), Is.EqualTo("b"));
    }

    [Test]
    public void SplitedWithIndex_StringSeparator_ReturnsElementAtIndex()
    {
        Assert.That("a::b::c".SplitedWithIndex("::", 2), Is.EqualTo("c"));
    }

    // Note: StringExtensions.Split(this string, string) and the StringSplitOptions overload are
    // shadowed by the BCL's own string.Split(string, StringSplitOptions = None) (added in modern
    // .NET) whenever called via normal extension syntax - instance methods always win overload
    // resolution over extension methods. Calling them via explicit static syntax below is the only
    // way to actually reach Mtf.Extensions' own implementation (e.g. to exercise its null-handling).

    [Test]
    public void Split_StringSeparator_NullOrEmpty_ReturnsEmptyArray()
    {
        Assert.That(Mtf.Extensions.StringExtensions.Split(null, "::"), Is.Empty);
    }

    [Test]
    public void Split_StringSeparator_SplitsCorrectly()
    {
        Assert.That(Mtf.Extensions.StringExtensions.Split("a::b::c", "::"), Is.EqualTo(new[] { "a", "b", "c" }));
    }

    [Test]
    public void Split_StringSeparatorWithOptions_RemovesEmptyEntries()
    {
        var result = Mtf.Extensions.StringExtensions.Split("a::::b", "::", StringSplitOptions.RemoveEmptyEntries);

        Assert.That(result, Is.EqualTo(new[] { "a", "b" }));
    }

    [Test]
    public void SplitOnNewLines_SplitsOnCarriageReturnLineFeed()
    {
        Assert.That("line1\r\nline2\r\nline3".SplitOnNewLines(), Is.EqualTo(new[] { "line1", "line2", "line3" }));
    }

    [Test]
    public void SplitOnNewLines_NullOrEmpty_ReturnsEmptyArray()
    {
        string value = null;
        Assert.That(value.SplitOnNewLines(), Is.Empty);
    }

    [Test]
    public void SplitOnNewLines_WithOptions_RemovesEmptyEntries()
    {
        Assert.That("line1\r\n\r\nline2".SplitOnNewLines(StringSplitOptions.RemoveEmptyEntries), Is.EqualTo(new[] { "line1", "line2" }));
    }

    [Test]
    public void SplitOnDoubleNewLines_SplitsOnBlankLineSeparator()
    {
        Assert.That("para1\r\n\r\npara2".SplitOnDoubleNewLines(), Is.EqualTo(new[] { "para1", "para2" }));
    }

    [Test]
    public void SplitOnDoubleNewLines_NullOrEmpty_ReturnsEmptyArray()
    {
        string value = null;
        Assert.That(value.SplitOnDoubleNewLines(), Is.Empty);
    }

    [Test]
    public void GetNext_NoArgsOverload_IncrementsWithinByteRange()
    {
        var result = "\u0000".GetNext();

        Assert.That(result, Is.EqualTo("\u0001"));
    }

    [Test]
    public void GetNext_IntCodeOverload_DelegatesToCombinatorics()
    {
        Assert.That("1".GetNext((int)'0', (int)'9'), Is.EqualTo("2"));
    }

    [Test]
    public void GetNext_CharOverload_DelegatesToCombinatorics()
    {
        Assert.That("1".GetNext('0', '9'), Is.EqualTo("2"));
    }

    [Test]
    public void GetNext_CharArrayOverload_DelegatesToCombinatorics()
    {
        Assert.That("aa".GetNext(new[] { 'a', 'b', 'c' }), Is.EqualTo("ab"));
    }

    [Test]
    public void ToBool_ParsesBooleanString()
    {
        Assert.That("true".ToBool(), Is.True);
    }

    [Test]
    public void ToByte_ParsesNumericString()
    {
        Assert.That("200".ToByte(), Is.EqualTo((byte)200));
    }

    [Test]
    public void ToChar_ParsesSingleCharacterString()
    {
        Assert.That("A".ToChar(), Is.EqualTo('A'));
    }

    [Test]
    public void ToDateTime_ParsesDateString()
    {
        Assert.That("2020-01-01".ToDateTime(), Is.EqualTo(new DateTime(2020, 1, 1)));
    }

    [Test]
    public void ToInt_ParsesIntegerString()
    {
        Assert.That("12345".ToInt(), Is.EqualTo(12345));
    }

    [Test]
    public void ToInt64_ParsesLongString()
    {
        Assert.That("123456789012".ToInt64(), Is.EqualTo(123456789012L));
    }

    [Test]
    public void GetSecureString_NullText_ReturnsNull()
    {
        string text = null;
        Assert.That(text.GetSecureString(), Is.Null);
    }

    [Test]
    public void GetSecureString_ReturnsReadOnlySecureStringOfSameLength()
    {
        using SecureString secure = "password".GetSecureString();

        Assert.That(secure.Length, Is.EqualTo(8));
        Assert.That(secure.IsReadOnly(), Is.True);
    }

    [Test]
    public void GetIpAddressFromEndPoint_NullOrWhitespace_ReturnsEmptyString()
    {
        string text = null;
        Assert.That(text.GetIpAddressFromEndPoint(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void GetIpAddressFromEndPoint_ExtractsAddressBeforeColon()
    {
        Assert.That("192.168.1.1:8080".GetIpAddressFromEndPoint(), Is.EqualTo("192.168.1.1"));
    }

    [Test]
    public void GetVideoSourceInfo_NullOrWhitespace_ReturnsEmptyTuple()
    {
        string text = null;
        var result = text.GetVideoSourceInfo();

        Assert.That(result.Item1, Is.EqualTo(string.Empty));
        Assert.That(result.Item2, Is.EqualTo(string.Empty));
    }

    [Test]
    public void GetVideoSourceInfo_ValidPipeSeparatedInput_ParsesBothParts()
    {
        var result = "cam1|rtsp://example.com/stream".GetVideoSourceInfo();

        Assert.That(result.Item1, Is.EqualTo("cam1"));
        Assert.That(result.Item2, Is.EqualTo("rtsp://example.com/stream"));
    }

    [Test]
    public void GetVideoSourceInfo_MalformedInput_ThrowsLocalizedException()
    {
        Assert.Throws<Mtf.Extensions.Exceptions.LocalizedException>(() => "no-pipe-here".GetVideoSourceInfo());
    }

    [Test]
    public void Substring_ThreeArgOverload_ExtractsBetweenElements()
    {
        Assert.That("Name:John,Age:30".Substring("Name:", ","), Is.EqualTo("John"));
    }

    [Test]
    public void Substring_ThreeArgOverload_SecondElementNotFound_ReturnsRestOfString()
    {
        Assert.That("Name:John".Substring("Name:", ","), Is.EqualTo("John"));
    }

    [Test]
    public void Substring_CaseInsensitiveOverload_FindsElementRegardlessOfCase()
    {
        Assert.That("NAME:John,End".Substring("name:", ",", true), Is.EqualTo("John"));
    }

    [Test]
    public void Substring_WithStartIndexOverload_SearchesFromGivenPosition()
    {
        var text = "Name:John,Name:Jane";

        Assert.That(text.Substring("Name:", ",", 10), Is.EqualTo("Jane"));
    }

    [Test]
    public void Substring_FirstElementNotFound_ReturnsEmptyString()
    {
        Assert.That("no such marker here".Substring("Name:", ","), Is.EqualTo(string.Empty));
    }

    [Test]
    public void Substring_NullOrEmptyValue_ThrowsArgumentException()
    {
        string value = null;
        Assert.Throws<ArgumentException>(() => value.Substring("a", "b"));
    }
}
