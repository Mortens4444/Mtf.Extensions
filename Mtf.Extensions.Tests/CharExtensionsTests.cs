namespace Mtf.Extensions.Tests;

public class CharExtensionsTests
{
    [Test]
    public void CharToBase64Code_InvalidCharacter_ThrowsInsteadOfSilentlyReturningZero()
    {
        Ensure.Throws<ArgumentException>(() => '!'.CharToBase64Code());
    }

    [Test]
    public void CharToBase64Code_ValidCharacters_ReturnCorrectCodes()
    {
        Assert.That('A'.CharToBase64Code(), Is.EqualTo((byte)0));
        Assert.That('a'.CharToBase64Code(), Is.EqualTo((byte)26));
        Assert.That('0'.CharToBase64Code(), Is.EqualTo((byte)52));
        Assert.That('+'.CharToBase64Code(), Is.EqualTo((byte)62));
        Assert.That('/'.CharToBase64Code(), Is.EqualTo((byte)63));
    }
}
