using System.Text;

namespace Mtf.Extensions.Tests;

public class ByteArrayExtensionsMoreTests
{
    [Test]
    public void IsNullOrEmpty_NullArray_ReturnsTrue()
    {
        byte[] array = null;
        Assert.That(array.IsNullOrEmpty(), Is.True);
    }

    [Test]
    public void IsNullOrEmpty_EmptyArray_ReturnsTrue()
    {
        Assert.That(Array.Empty<byte>().IsNullOrEmpty(), Is.True);
    }

    [Test]
    public void IsNullOrEmpty_NonEmptyArray_ReturnsFalse()
    {
        Assert.That(new byte[] { 1 }.IsNullOrEmpty(), Is.False);
    }

    [Test]
    public void AppendArrays_NullFirst_ThrowsArgumentNullException()
    {
        byte[] first = null;
        Assert.Throws<ArgumentNullException>(() => first.AppendArrays(new byte[] { 1 }));
    }

    [Test]
    public void AppendArrays_NullArraysParam_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new byte[] { 1 }.AppendArrays(null));
    }

    [Test]
    public void AppendArrays_ConcatenatesInOrderSkippingNulls()
    {
        var result = new byte[] { 1, 2 }.AppendArrays(new byte[] { 3, 4 }, null, new byte[] { 5 });

        Assert.That(result, Is.EqualTo(new byte[] { 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void ToZeroByteTerminatedString_NullValue_ThrowsArgumentNullException()
    {
        byte[] value = null;
        Assert.Throws<ArgumentNullException>(() => value.ToZeroByteTerminatedString(Encoding.ASCII));
    }

    [Test]
    public void ToZeroByteTerminatedString_NullEncoding_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new byte[] { 65 }.ToZeroByteTerminatedString(null));
    }

    [Test]
    public void ToZeroByteTerminatedString_StopsAtZeroByte()
    {
        var bytes = new byte[] { 65, 66, 0, 67 };

        Assert.That(bytes.ToZeroByteTerminatedString(Encoding.ASCII), Is.EqualTo("AB"));
    }

    [Test]
    public void ToZeroByteTerminatedString_NoZeroByte_UsesFullLength()
    {
        var bytes = new byte[] { 65, 66, 67 };

        Assert.That(bytes.ToZeroByteTerminatedString(Encoding.ASCII), Is.EqualTo("ABC"));
    }

    [Test]
    public void Find_TwoArgOverload_DelegatesToThreeArgOverloadStartingAtZero()
    {
        var array = new byte[] { 1, 2, 3, 4, 5 };

        Assert.That(array.Find(new byte[] { 3, 4 }), Is.EqualTo(2));
    }

    [Test]
    public void Find_ThreeArgOverload_NullArray_ThrowsArgumentNullException()
    {
        byte[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.Find(new byte[] { 1 }, 0));
    }

    [Test]
    public void Find_ThreeArgOverload_NullOrTooLongSubArray_ReturnsNotFound()
    {
        var array = new byte[] { 1, 2, 3 };

        Assert.That(array.Find(null, 0), Is.EqualTo(-1));
        Assert.That(array.Find(new byte[] { 1, 2, 3, 4 }, 0), Is.EqualTo(-1));
    }

    [Test]
    public void Find_ThreeArgOverload_FindsSubArray()
    {
        var array = new byte[] { 10, 20, 30, 40, 50 };

        Assert.That(array.Find(new byte[] { 30, 40 }, 0), Is.EqualTo(2));
    }

    [Test]
    public void Find_ThreeArgOverload_NotFound_ReturnsMinusOne()
    {
        var array = new byte[] { 10, 20, 30 };

        Assert.That(array.Find(new byte[] { 99 }, 0), Is.EqualTo(-1));
    }

    [Test]
    public void Find_FourArgOverload_NullArray_ThrowsArgumentNullException()
    {
        byte[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.Find(new byte[] { 1 }, 0, 1));
    }

    [Test]
    public void Find_FourArgOverload_NullSubArray_ThrowsArgumentNullException()
    {
        var array = new byte[] { 1, 2, 3 };
        Assert.Throws<ArgumentNullException>(() => array.Find(null, 0, 3));
    }

    [Test]
    public void Find_FourArgOverload_FindsSubArrayWithinCount()
    {
        var array = new byte[] { 10, 20, 30, 40, 50 };

        Assert.That(array.Find(new byte[] { 30, 40 }, 0, array.Length), Is.EqualTo(2));
    }

    [Test]
    public void Find_FourArgOverload_NotFound_ReturnsMinusOne()
    {
        var array = new byte[] { 10, 20, 30 };

        Assert.That(array.Find(new byte[] { 99 }, 0, array.Length), Is.EqualTo(-1));
    }

    [Test]
    public void Replace_NullArray_ThrowsArgumentNullException()
    {
        byte[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.Replace(1, 2));
    }

    [Test]
    public void Replace_ReplacesAllMatchingBytesInPlace()
    {
        var array = new byte[] { 1, 2, 1, 3, 1 };

        array.Replace(1, 9);

        Assert.That(array, Is.EqualTo(new byte[] { 9, 2, 9, 3, 9 }));
    }

    [Test]
    public void ToArrayString_ThreeArgOverload_NullArray_ThrowsArgumentNullException()
    {
        byte[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.ToArrayString(0, 1));
    }

    [Test]
    public void ToArrayString_ThreeArgOverload_FormatsEachByteInBrackets()
    {
        var array = new byte[] { 12, 243, 124, 68 };

        Assert.That(array.ToArrayString(0, array.Length), Is.EqualTo("[12][243][124][68]"));
    }

    [Test]
    public void CreateArray_FromString_ParsesBracketedFormat()
    {
        var result = ByteArrayExtensions.CreateArray("[12][243][124][68]");

        Assert.That(result, Is.EqualTo(new byte[] { 12, 243, 124, 68 }));
    }

    [Test]
    public void ToASCIIString_DecodesAsciiBytes()
    {
        var bytes = Encoding.ASCII.GetBytes("Hello");

        Assert.That(bytes.ToASCIIString(), Is.EqualTo("Hello"));
    }

    [Test]
    public void ASCIIGetString_DecodesAsciiBytes()
    {
        var bytes = Encoding.ASCII.GetBytes("Hello");

        Assert.That(bytes.ASCIIGetString(), Is.EqualTo("Hello"));
    }

    [Test]
    public void UTF8GetString_DecodesUtf8Bytes()
    {
        var bytes = Encoding.UTF8.GetBytes("Héllo");

        Assert.That(bytes.UTF8GetString(), Is.EqualTo("Héllo"));
    }

    [Test]
    public void ToArrayString_NoArgOverload_NullArray_ThrowsArgumentNullException()
    {
        byte[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.ToArrayString());
    }

    [Test]
    public void ToArrayString_NoArgOverload_FormatsWholeArray()
    {
        var array = new byte[] { 1, 2, 3 };

        Assert.That(array.ToArrayString(), Is.EqualTo("[1][2][3]"));
    }

    [Test]
    public void ToASCIIStringZeroByteTerminated_NullArray_ThrowsArgumentNullException()
    {
        byte[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.ToASCIIStringZeroByteTerminated());
    }

    [Test]
    public void ToASCIIStringZeroByteTerminated_StopsAtZeroByte()
    {
        var array = new byte[] { 65, 66, 0, 67 };

        Assert.That(array.ToASCIIStringZeroByteTerminated(), Is.EqualTo("AB"));
    }

    [Test]
    public void SubArray_ReturnsSliceOfGivenLength()
    {
        var array = new byte[] { 1, 2, 3, 4, 5 };

        Assert.That(array.SubArray(1, 3), Is.EqualTo(new byte[] { 2, 3, 4 }));
    }

    [Test]
    public void CreateArray_NullArray_ThrowsArgumentNullException()
    {
        byte[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.CreateArray(0, 1));
    }

    [Test]
    public void CreateArray_ReturnsSliceOfGivenLength()
    {
        var array = new byte[] { 1, 2, 3, 4, 5 };

        Assert.That(array.CreateArray(1, 3), Is.EqualTo(new byte[] { 2, 3, 4 }));
    }

    [Test]
    public void EqualsPercent_DelegatesToEqualInPercent()
    {
        var array1 = new byte[] { 1, 2, 3 };
        var array2 = new byte[] { 1, 2, 3 };

        Assert.That((byte)array1.EqualsPercent(array2), Is.EqualTo((byte)100));
    }

    [Test]
    public void EqualInPercent_PartialMatch_ReturnsProportionalPercentage()
    {
        var array1 = new byte[] { 1, 2, 3, 4 };
        var array2 = new byte[] { 1, 2, 9, 9 };

        var result = (byte)ByteArrayExtensions.EqualInPercent(array1, array2);

        Assert.That(result, Is.EqualTo(50));
    }

    [Test]
    public void EqualInPercent_DifferentLengths_UsesLongerArrayAsDenominator()
    {
        var shorter = new byte[] { 1, 2 };
        var longer = new byte[] { 1, 2, 3, 4 };

        var result = (byte)ByteArrayExtensions.EqualInPercent(shorter, longer);

        Assert.That(result, Is.EqualTo(50));
    }

    [Test]
    public void IsEqual_DifferentLengths_ReturnsFalse()
    {
        Assert.That(ByteArrayExtensions.IsEqual(new byte[] { 1, 2 }, new byte[] { 1, 2, 3 }), Is.False);
    }

    [Test]
    public void IsEqual_OneNull_ReturnsFalse()
    {
        Assert.That(ByteArrayExtensions.IsEqual(null, new byte[] { 1 }), Is.False);
    }

    [Test]
    public void IsEqual_SameContent_ReturnsTrue()
    {
        Assert.That(ByteArrayExtensions.IsEqual(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }), Is.True);
    }

    [Test]
    public void IsEqual_DifferentContent_ReturnsFalse()
    {
        Assert.That(ByteArrayExtensions.IsEqual(new byte[] { 1, 2, 3 }, new byte[] { 1, 9, 3 }), Is.False);
    }
}
