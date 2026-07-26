namespace Mtf.Extensions.Tests;

public class CharExtensionsMoreTests
{
    [Test]
    public void CharToHexRepresentation_ReturnsUppercaseHex()
    {
        Assert.That('A'.CharToHexRepresentation(), Is.EqualTo("41"));
    }

    [TestCase('a', true)]
    [TestCase('A', true)]
    [TestCase('í', true)]
    [TestCase('b', false)]
    public void IsVowel_ReturnsExpectedResult(char ch, bool expected)
    {
        Assert.That(ch.IsVowel(), Is.EqualTo(expected));
    }

    [TestCase('0', true)]
    [TestCase('9', true)]
    [TestCase('a', true)]
    [TestCase('f', true)]
    [TestCase('A', true)]
    [TestCase('F', true)]
    [TestCase('g', false)]
    [TestCase('!', false)]
    public void IsHexadecimalDigit_ReturnsExpectedResult(char ch, bool expected)
    {
        Assert.That(ch.IsHexadecimalDigit(), Is.EqualTo(expected));
    }

    [TestCase('b', true)]
    [TestCase('B', true)]
    [TestCase('a', false)]
    public void IsConsonant_ReturnsExpectedResult(char ch, bool expected)
    {
        Assert.That(ch.IsConsonant(), Is.EqualTo(expected));
    }

    [TestCase('0', '0')]
    [TestCase('1', 'l')]
    [TestCase('3', 'e')]
    [TestCase('4', 'a')]
    [TestCase('5', 's')]
    [TestCase('a', '4')]
    [TestCase('A', '4')]
    [TestCase('o', '0')]
    [TestCase('O', '0')]
    [TestCase('D', '0')]
    [TestCase('E', '3')]
    [TestCase('e', '3')]
    [TestCase('i', '!')]
    [TestCase('I', '1')]
    [TestCase('l', '1')]
    [TestCase('s', '5')]
    [TestCase('S', '5')]
    [TestCase('x', 'x')]
    public void GetCodedChar_ReturnsExpectedLeetSpeakSubstitution(char input, char expected)
    {
        Assert.That(input.GetCodedChar(), Is.EqualTo(expected));
    }

    [TestCase('0', true)]
    [TestCase('O', true)]
    [TestCase('I', true)]
    [TestCase('l', true)]
    [TestCase('a', false)]
    public void IsBadPasswordChar_ReturnsExpectedResult(char ch, bool expected)
    {
        Assert.That(ch.IsBadPasswordChar(), Is.EqualTo(expected));
    }

    [Test]
    public void IsDigit_DelegatesToCharIsDigit()
    {
        Assert.That('5'.IsDigit(), Is.True);
        Assert.That('a'.IsDigit(), Is.False);
    }

    [Test]
    public void IsControl_DelegatesToCharIsControl()
    {
        Assert.That('\n'.IsControl(), Is.True);
        Assert.That('a'.IsControl(), Is.False);
    }

    [Test]
    public void IsLetter_DelegatesToCharIsLetter()
    {
        Assert.That('a'.IsLetter(), Is.True);
        Assert.That('5'.IsLetter(), Is.False);
    }

    [Test]
    public void IsLetterOrDigit_DelegatesToCharIsLetterOrDigit()
    {
        Assert.That('a'.IsLetterOrDigit(), Is.True);
        Assert.That('!'.IsLetterOrDigit(), Is.False);
    }

    [Test]
    public void IsLower_DelegatesToCharIsLower()
    {
        Assert.That('a'.IsLower(), Is.True);
        Assert.That('A'.IsLower(), Is.False);
    }

    [Test]
    public void IsUpper_DelegatesToCharIsUpper()
    {
        Assert.That('A'.IsUpper(), Is.True);
        Assert.That('a'.IsUpper(), Is.False);
    }

    [Test]
    public void IsNumber_DelegatesToCharIsNumber()
    {
        Assert.That('5'.IsNumber(), Is.True);
        Assert.That('a'.IsNumber(), Is.False);
    }

    [Test]
    public void IsPunctuation_DelegatesToCharIsPunctuation()
    {
        Assert.That('.'.IsPunctuation(), Is.True);
        Assert.That('a'.IsPunctuation(), Is.False);
    }

    [Test]
    public void IsSeparator_DelegatesToCharIsSeparator()
    {
        Assert.That(' '.IsSeparator(), Is.True);
        Assert.That('a'.IsSeparator(), Is.False);
    }

    [Test]
    public void IsSymbol_DelegatesToCharIsSymbol()
    {
        Assert.That('+'.IsSymbol(), Is.True);
        Assert.That('a'.IsSymbol(), Is.False);
    }

    [Test]
    public void IsWhiteSpace_DelegatesToCharIsWhiteSpace()
    {
        Assert.That(' '.IsWhiteSpace(), Is.True);
        Assert.That('a'.IsWhiteSpace(), Is.False);
    }

    [Test]
    public void IsHighSurrogate_DelegatesToCharIsHighSurrogate()
    {
        var surrogatePair = char.ConvertFromUtf32(0x1F600).ToCharArray();
        Assert.That(surrogatePair[0].IsHighSurrogate(), Is.True);
        Assert.That('a'.IsHighSurrogate(), Is.False);
    }

    [Test]
    public void IsLowSurrogate_DelegatesToCharIsLowSurrogate()
    {
        var surrogatePair = char.ConvertFromUtf32(0x1F600).ToCharArray();
        Assert.That(surrogatePair[1].IsLowSurrogate(), Is.True);
        Assert.That('a'.IsLowSurrogate(), Is.False);
    }

    [Test]
    public void IsSurrogate_DelegatesToCharIsSurrogate()
    {
        var surrogatePair = char.ConvertFromUtf32(0x1F600).ToCharArray();
        Assert.That(surrogatePair[0].IsSurrogate(), Is.True);
        Assert.That('a'.IsSurrogate(), Is.False);
    }
}
