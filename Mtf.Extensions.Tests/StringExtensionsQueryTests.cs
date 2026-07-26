namespace Mtf.Extensions.Tests;

public class StringExtensionsQueryTests
{
    [Test]
    public void NumberOfOccurrences_NullSource_ThrowsArgumentNullException()
    {
        string source = null;
        Ensure.Throws<ArgumentNullException>(() => source.NumberOfOccurrences("a"));
    }

    [Test]
    public void NumberOfOccurrences_NullWord_ThrowsArgumentNullException()
    {
        Ensure.Throws<ArgumentNullException>(() => "abc".NumberOfOccurrences(null));
    }

    [Test]
    public void NumberOfOccurrences_CountsNonOverlappingCaseInsensitiveMatches()
    {
        Assert.That("AbcABCabc".NumberOfOccurrences("abc"), Is.EqualTo(3));
    }

    [Test]
    public void NumberOfOccurrences_OverlappingPattern_CountsOnlyNonOverlapping()
    {
        Assert.That("aaaa".NumberOfOccurrences("aa"), Is.EqualTo(2));
    }

    [Test]
    public void SplitByUppercase_SplitsBeforeEachCapitalAndCapitalizesFirstWord()
    {
        Assert.That("HelloWorld".SplitByUppercase(), Is.EqualTo("Hello world"));
    }

    [Test]
    public void ChangeExpanderText_NullText_ThrowsArgumentNullException()
    {
        string text = null;
        Ensure.Throws<ArgumentNullException>(() => text.ChangeExpanderText());
    }

    [Test]
    public void ChangeExpanderText_StartsWithMinus_SwapsToPlus()
    {
        Assert.That("-Item".ChangeExpanderText(), Is.EqualTo("+Item"));
    }

    [Test]
    public void ChangeExpanderText_DoesNotStartWithMinus_SwapsToMinus()
    {
        Assert.That("+Item".ChangeExpanderText(), Is.EqualTo("-Item"));
    }

    [Test]
    public void ChangeExpanderText_EmptyString_ReturnsEmptyString()
    {
        Assert.That(string.Empty.ChangeExpanderText(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void FirstChar_NullOrEmpty_ThrowsArgumentException()
    {
        string value = null;
        Ensure.Throws<ArgumentException>(() => value.FirstChar());
        Ensure.Throws<ArgumentException>(() => string.Empty.FirstChar());
    }

    [Test]
    public void FirstChar_ReturnsFirstCharacter()
    {
        Assert.That("Hello".FirstChar(), Is.EqualTo('H'));
    }

    [Test]
    public void LastChar_NullOrEmpty_ThrowsArgumentException()
    {
        string value = null;
        Ensure.Throws<ArgumentException>(() => value.LastChar());
        Ensure.Throws<ArgumentException>(() => string.Empty.LastChar());
    }

    [Test]
    public void LastChar_ReturnsLastCharacter()
    {
        Assert.That("Hello".LastChar(), Is.EqualTo('o'));
    }

    [Test]
    public void IsStartsAndEndWith_MatchesBothEnds_ReturnsTrue()
    {
        Assert.That("[abc]".IsStartsAndEndWith('[', ']'), Is.True);
    }

    [Test]
    public void IsStartsAndEndWith_OnlyOneEndMatches_ReturnsFalse()
    {
        Assert.That("[abc)".IsStartsAndEndWith('[', ']'), Is.False);
    }

    [Test]
    public void IsStartsWith_EmptyOrNull_ReturnsFalse()
    {
        Assert.That(string.Empty.IsStartsWith('a'), Is.False);
        string value = null;
        Assert.That(value.IsStartsWith('a'), Is.False);
    }

    [Test]
    public void IsEndWith_EmptyOrNull_ReturnsFalse()
    {
        Assert.That(string.Empty.IsEndWith('a'), Is.False);
        string value = null;
        Assert.That(value.IsEndWith('a'), Is.False);
    }

    [Test]
    public void IsNumber_AllDigits_ReturnsTrue()
    {
        Assert.That("12345".IsNumber(), Is.True);
    }

    [Test]
    public void IsNumber_ContainsNonDigit_ReturnsFalse()
    {
        Assert.That("123a5".IsNumber(), Is.False);
    }

    [Test]
    public void IsNumber_NullOrWhitespace_ReturnsFalse()
    {
        string value = null;
        Assert.That(value.IsNumber(), Is.False);
        Assert.That("   ".IsNumber(), Is.False);
    }

    [Test]
    public void ToLower_NullOrEmpty_ReturnsEmptyString()
    {
        Assert.That(Mtf.Extensions.StringExtensions.ToLower(null), Is.EqualTo(string.Empty));
    }

    [Test]
    public void ToLower_MixedCase_LowersEverything()
    {
        Assert.That(Mtf.Extensions.StringExtensions.ToLower("HeLLo"), Is.EqualTo("hello"));
    }

    [Test]
    public void ToUpper_MixedCase_UppersEverything()
    {
        Assert.That(Mtf.Extensions.StringExtensions.ToUpper("HeLLo"), Is.EqualTo("HELLO"));
    }

    [Test]
    public void ToName_NullValue_ReturnsNullUnchanged()
    {
        Assert.That(Mtf.Extensions.StringExtensions.ToName(null), Is.Null);
    }

    [Test]
    public void ToName_CapitalizesFirstLetterAndLowersRest()
    {
        Assert.That(Mtf.Extensions.StringExtensions.ToName("hELLO"), Is.EqualTo("Hello"));
    }

    [TestCase("123", true)]
    [TestCase("abc", false)]
    [TestCase("a1b", true)]
    public void IsContainsNumber_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.That(value.IsContainsNumber(), Is.EqualTo(expected));
    }

    [Test]
    public void IsContainsNumber_NullOrEmpty_ReturnsFalse()
    {
        string value = null;
        Assert.That(value.IsContainsNumber(), Is.False);
    }

    [TestCase("ABC", false)]
    [TestCase("aBC", true)]
    public void IsContainsLowerLetter_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.That(value.IsContainsLowerLetter(), Is.EqualTo(expected));
    }

    [TestCase("abc", false)]
    [TestCase("abC", true)]
    public void IsContainsUpperLetter_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.That(value.IsContainsUpperLetter(), Is.EqualTo(expected));
    }

    [TestCase("123", false)]
    [TestCase("1a3", true)]
    public void IsContainsLetter_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.That(value.IsContainsLetter(), Is.EqualTo(expected));
    }

    [TestCase("abc123", false)]
    [TestCase("abc!23", true)]
    public void IsContainsSpecial_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.That(value.IsContainsSpecial(), Is.EqualTo(expected));
    }

    [Test]
    public void IsContainsSpecialLetterAndDigit_HasSpecialAndAlphanumeric_ReturnsTrue()
    {
        Assert.That("abc!23".IsContainsSpecialLetterAndDigit(), Is.True);
    }

    [Test]
    public void IsContainsSpecialLetterAndDigit_NoSpecialCharacter_ReturnsFalse()
    {
        Assert.That("abc123".IsContainsSpecialLetterAndDigit(), Is.False);
    }

    [Test]
    public void IsStrongPassword_MeetsAllCriteria_ReturnsTrue()
    {
        Assert.That("Passw0rd!".IsStrongPassword(), Is.True);
    }

    [Test]
    public void IsStrongPassword_TooShort_ReturnsFalse()
    {
        Assert.That("Pw0!".IsStrongPassword(), Is.False);
    }

    [Test]
    public void IsStrongPassword_MissingUppercase_ReturnsFalse()
    {
        Assert.That("passw0rd!".IsStrongPassword(), Is.False);
    }

    [Test]
    public void IsStrongPassword_NullOrEmpty_ReturnsFalse()
    {
        string value = null;
        Assert.That(value.IsStrongPassword(), Is.False);
    }

    [Test]
    public void IsLessThan_NullArguments_ThrowArgumentNullException()
    {
        string a = null;
        Ensure.Throws<ArgumentNullException>(() => a.IsLessThan("b"));
        Ensure.Throws<ArgumentNullException>(() => "a".IsLessThan(null));
    }

    [Test]
    public void IsLessThan_ComparesOrdinally()
    {
        Assert.That("a".IsLessThan("b"), Is.True);
        Assert.That("b".IsLessThan("a"), Is.False);
    }

    [Test]
    public void IsLessOrEqualThan_EqualStrings_ReturnsTrue()
    {
        Assert.That("a".IsLessOrEqualThan("a"), Is.True);
    }

    [Test]
    public void IsGreaterThan_ComparesOrdinally()
    {
        Assert.That("b".IsGreaterThan("a"), Is.True);
        Assert.That("a".IsGreaterThan("b"), Is.False);
    }

    [Test]
    public void IsGreaterOrEqualThan_EqualStrings_ReturnsTrue()
    {
        Assert.That("a".IsGreaterOrEqualThan("a"), Is.True);
    }

    [Test]
    public void TruncateOnChars_NullOrEmptyValue_ReturnsEmptyString()
    {
        string value = null;
        Assert.That(value.TruncateOnChars(';'), Is.EqualTo(string.Empty));
    }

    [Test]
    public void TruncateOnChars_NullChars_ReturnsValueUnchanged()
    {
        Assert.That("hello;world".TruncateOnChars(null), Is.EqualTo("hello;world"));
    }

    [Test]
    public void TruncateOnChars_TruncatesAtEarliestMatch()
    {
        Assert.That("hello;world,end".TruncateOnChars(',', ';'), Is.EqualTo("hello"));
    }

    [Test]
    public void IsEqualOneOfThis_NullOrEmptyValues_ReturnsFalse()
    {
        Assert.That("a".IsEqualOneOfThis(), Is.False);
        Assert.That("a".IsEqualOneOfThis(null), Is.False);
    }

    [Test]
    public void IsEqualOneOfThis_MatchesOneOfTheCandidates_ReturnsTrue()
    {
        Assert.That("b".IsEqualOneOfThis("a", "b", "c"), Is.True);
    }

    [Test]
    public void IsEqualOneOfThis_MatchesNone_ReturnsFalse()
    {
        Assert.That("z".IsEqualOneOfThis("a", "b", "c"), Is.False);
    }
}
