using Mtf.Extensions.Services;

namespace Mtf.Extensions.Tests;

public class PasswordTests
{
    [Test]
    public void MixedAlphabet_IsLowercaseFollowedByUppercase()
    {
        Assert.That(Password.MixedAlphabet, Is.EqualTo(Password.LowercaseAlphabet.Concat(Password.UppercaseAlphabet)));
    }

    [Test]
    public void Alphanumeric_IsMixedAlphabetFollowedByDigits()
    {
        Assert.That(Password.Alphanumeric, Is.EqualTo(Password.MixedAlphabet.Concat(Password.DecimalNumbers)));
    }

    [Test]
    public void Consonants_ContainNoVowels()
    {
        Assert.That(Password.Consonants.Intersect(Password.Vowels), Is.Empty);
    }

    [Test]
    public void PasswordGeneratorCharacters_ExcludeKnownBadPasswordChars()
    {
        foreach (var badChar in Password.BadPasswordChars)
        {
            Assert.That(Password.PasswordGeneratorCharacters, Does.Not.Contain(badChar));
        }
    }

    [Test]
    public void HexadecimalNumbersMixed_IsDigitsThenLowercaseThenUppercase()
    {
        var expected = Password.DecimalNumbers
            .Concat(Password.HexadecimalNumbersLower.Skip(Password.DecimalNumbers.Length))
            .Concat(Password.HexadecimalNumbersUpper.Skip(Password.DecimalNumbers.Length));

        Assert.That(Password.HexadecimalNumbersMixed, Is.EqualTo(expected));
    }
}
