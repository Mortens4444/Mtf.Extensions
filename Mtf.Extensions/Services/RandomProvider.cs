using System;
using System.Security.Cryptography;

namespace Mtf.Extensions.Services
{
    public static class RandomProvider
    {
        public static byte GetSecureRandomByte()
        {
            return GetRandomBytes(1)[0];
        }

        public static int GetSecureRandomInt()
        {
            return BitConverter.ToInt32(GetRandomBytes(4), 0);
        }

        public static uint GetSecureRandomUInt()
        {
            return BitConverter.ToUInt32(GetRandomBytes(4), 0);
        }

        public static byte GetSecureRandomUByteInt(byte min, byte max)
        {
            if (min >= max)
            {
                throw new ArgumentException("min must be less than max");
            }

            var range = max - min;
            var limit = Byte.MaxValue - (Byte.MaxValue % range);
            byte result;
            do
            {
                result = GetRandomBytes(1)[0];
            }
            while (result >= limit);

            return (byte)((result % range) + min);
        }

        public static uint GetSecureRandomUInt(uint min, uint max)
        {
            if (min >= max)
            {
                throw new ArgumentException("min must be less than max");
            }

            var range = max - min;
            var limit = UInt32.MaxValue - (UInt32.MaxValue % range);
            uint result;
            do
            {
                result = BitConverter.ToUInt32(GetRandomBytes(4), 0);
            }
            while (result >= limit);

            return (result % range) + min;
        }

        public static long GetSecureRandomInt64()
        {
            return BitConverter.ToInt64(GetRandomBytes(8), 0);
        }

        public static ulong GetSecureRandomUInt64()
        {
            return BitConverter.ToUInt64(GetRandomBytes(8), 0);
        }

        public static double GetSecureRandomDouble()
        {
            return BitConverter.ToDouble(GetRandomBytes(8), 0);
        }

        /// <summary>
        /// Get a secure random probability
        /// </summary>
        /// <returns>[0.0, 1.0)</returns>
        public static double GetSecureRandomProbability()
        {
            return (double)GetSecureRandomUInt64() / UInt64.MaxValue;
        }

        private static byte[] GetRandomBytes(int count)
        {
            var bytes = new byte[count];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }
    }
}
