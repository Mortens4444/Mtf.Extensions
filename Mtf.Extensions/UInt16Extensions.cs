using System;
using System.Globalization;

namespace Mtf.Extensions
{
    public static class UInt16Extensions
    {
        public static string ToHexString(this UInt16 value)
        {
            return value.ToString("x4", CultureInfo.InvariantCulture).ToUpper(CultureInfo.CurrentCulture);
        }

        public static bool IsBitSet(this ushort value, int bitIndex)
        {
            return (ushort)(value & (ushort)Math.Pow(2, bitIndex)) > 0;
        }

        public static ushort GetSubBitConbinationValue(this ushort value, int bitIndex, int numberOfBits)
        {
            ushort result = 0;
            for (var i = bitIndex; i < bitIndex + numberOfBits; i++)
            {
                if (value.IsBitSet(i))
                {
                    result += (ushort)Math.Pow(2, i - bitIndex);
                }
            }

            return result;
        }
    }
}
