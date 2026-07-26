using Mtf.Extensions.Services;
using System.Collections;

namespace Mtf.Extensions.Tests;

public class CombinatoricsTests
{
    [Test]
    public void BruteForce_CharArrayOverload_NullBasicPassword_ThrowsArgumentNullException()
    {
        string password = null;

        Assert.Throws<ArgumentNullException>(() => Combinatorics.BruteForce(ref password, new[] { 'a', 'b' }));
    }

    [Test]
    public void BruteForce_CharArrayOverload_NullChars_ThrowsArgumentNullException()
    {
        var password = "a";

        Assert.Throws<ArgumentNullException>(() => Combinatorics.BruteForce(ref password, null));
    }

    [Test]
    public void BruteForce_CharArrayOverload_IncrementsLastCharacter()
    {
        var password = "aa";
        var chars = new[] { 'a', 'b', 'c' };

        var result = Combinatorics.BruteForce(ref password, chars);

        Assert.That(result, Is.EqualTo("ab"));
    }

    [Test]
    public void BruteForce_CharArrayOverload_RollsOverToFirstCharWhenLastReached()
    {
        var password = "ac";
        var chars = new[] { 'a', 'b', 'c' };

        var result = Combinatorics.BruteForce(ref password, chars);

        Assert.That(result, Is.EqualTo("ba"));
    }

    [Test]
    public void BruteForce_IntCodeOverload_NullBasicPassword_ReturnsEmptyString()
    {
        string password = null;

        var result = Combinatorics.BruteForce(ref password, 48, 57);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void BruteForce_IntCodeOverload_IncrementsLastDigit()
    {
        var password = "12";

        var result = Combinatorics.BruteForce(ref password, (int)'0', (int)'9');

        Assert.That(result, Is.EqualTo("13"));
    }

    [Test]
    public void BruteForce_IntCodeOverload_SwapsReversedCodes()
    {
        var password = "12";

        var result = Combinatorics.BruteForce(ref password, (int)'9', (int)'0');

        Assert.That(result, Is.EqualTo("13"));
    }

    [Test]
    public void BruteForce_CharOverload_DelegatesToIntCodeOverload()
    {
        var password = "12";

        var result = Combinatorics.BruteForce(ref password, '0', '9');

        Assert.That(result, Is.EqualTo("13"));
    }

    [Test]
    public void GetPermutations_NullLetters_ThrowsArgumentNullException()
    {
        string letters = null;
        var result = new ArrayList();

        Assert.Throws<ArgumentNullException>(() => Combinatorics.GetPermutations(letters, ref result, string.Empty));
    }

    [Test]
    public void GetPermutations_NullResult_ThrowsArgumentNullException()
    {
        ArrayList result = null;

        Assert.Throws<ArgumentNullException>(() => Combinatorics.GetPermutations("ab", ref result, string.Empty));
    }

    [Test]
    public void GetPermutations_ThreeDistinctLetters_ProducesAllSixPermutations()
    {
        var result = new ArrayList();

        Combinatorics.GetPermutations("ABC", ref result, string.Empty);

        var permutations = result.Cast<string>().ToList();
        Assert.That(permutations, Has.Count.EqualTo(6));
        Assert.That(permutations, Is.EquivalentTo(new[] { "ABC", "ACB", "BCA", "BAC", "CAB", "CBA" }));
    }
}
