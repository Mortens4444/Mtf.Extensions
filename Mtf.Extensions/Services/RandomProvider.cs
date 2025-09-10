using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace Mtf.Extensions.Services
{
    public static class RandomProvider
    {
        public static T GetSecureRandom<T>(T min, T max) where T : struct, IComparable<T>
        {
            if (Comparer<T>.Default.Compare(min, max) >= 0)
            {
                throw new ArgumentException("min must be less than max");
            }

            var range = Subtract(max, min);
            var limit = ulong.MaxValue - (ulong.MaxValue % Convert.ToUInt64(range, CultureInfo.InvariantCulture));

            ulong result;
            do
            {
                result = GetRandomUInt64(typeof(T));
            }
            while (result >= limit);

            var value = (result % Convert.ToUInt64(range, CultureInfo.InvariantCulture)) + Convert.ToUInt64(min, CultureInfo.InvariantCulture);
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        public static byte GetSecureRandomByte(byte min, byte max) => GetSecureRandom(min, max);

        public static short GetSecureRandomShort(short min, short max) => GetSecureRandom(min, max);

        public static int GetSecureRandomInt(int min, int max) => GetSecureRandom(min, max);

        public static uint GetSecureRandomUInt(uint min, uint max) => GetSecureRandom(min, max);

        public static long GetSecureRandomInt64(long min, long max) => GetSecureRandom(min, max);
        
        public static ulong GetSecureRandomUInt64(ulong min, ulong max) => GetSecureRandom(min, max);

        private static ulong Subtract<T>(T max, T min)
        {
            var a = Convert.ToUInt64(max, CultureInfo.InvariantCulture);
            var b = Convert.ToUInt64(min, CultureInfo.InvariantCulture);
            return a - b;
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

        private static ulong GetRandomUInt64(Type type)
        {
            int size;
            switch (type.Name)
            {
                case "Byte":
                case "SByte":
                    size = 1;
                    break;
                case "Int16":
                case "UInt16":
                    size = 2;
                    break;
                case "Int32":
                case "UInt32":
                    size = 4;
                    break;
                case "Int64":
                case "UInt64":
                    size = 8;
                    break;
                default:
                    throw new NotSupportedException("Unsupported type provided.");
            }

            var bytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            switch (size)
            {
                case 1:
                    return bytes[0];
                case 2:
                    return BitConverter.ToUInt16(bytes, 0);
                case 4:
                    return BitConverter.ToUInt32(bytes, 0);
                case 8:
                    return BitConverter.ToUInt64(bytes, 0);
                default:
                    return 0;
            }
        }
    }
}
