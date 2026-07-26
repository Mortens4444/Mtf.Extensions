using Mtf.Extensions.Services;

namespace Mtf.Extensions.Tests;

public class EnglishGrammarExtensionsTests
{
    [TestCase('a')]
    [TestCase('A')]
    [TestCase('e')]
    [TestCase('E')]
    [TestCase('i')]
    [TestCase('I')]
    [TestCase('o')]
    [TestCase('O')]
    [TestCase('u')]
    [TestCase('U')]
    public void IsEnglishVowel_VowelCharacters_ReturnsTrue(char ch)
    {
        Assert.That(ch.IsEnglishVowel(), Is.True);
    }

    [TestCase('b')]
    [TestCase('z')]
    [TestCase('5')]
    public void IsEnglishVowel_NonVowelCharacters_ReturnsFalse(char ch)
    {
        Assert.That(ch.IsEnglishVowel(), Is.False);
    }

    [TestCase('b')]
    [TestCase('B')]
    [TestCase('z')]
    [TestCase('Z')]
    public void IsEnglishConsonant_ConsonantCharacters_ReturnsTrue(char ch)
    {
        Assert.That(ch.IsEnglishConsonant(), Is.True);
    }

    [TestCase('a')]
    [TestCase('5')]
    public void IsEnglishConsonant_NonConsonantCharacters_ReturnsFalse(char ch)
    {
        Assert.That(ch.IsEnglishConsonant(), Is.False);
    }
}
