using System;

namespace FooCore
{
    /// <summary>
    /// Helper class contains static methods to read and write
    /// numeric values in little-endian byte order.
    /// </summary>
    public static class LittleEndianByteOrder
    {
        public static byte[] GetBytes(int value)
        {
            var bytes = BitConverter.GetBytes(value);

            if (false == BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        public static byte[] GetBytes(long value)
        {
            var bytes = BitConverter.GetBytes(value);

            if (false == BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        public static byte[] GetBytes(uint value)
        {
            var bytes = BitConverter.GetBytes(value);

            if (false == BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        public static byte[] GetBytes(float value)
        {
            var bytes = BitConverter.GetBytes(value);

            if (false == BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        public static byte[] GetBytes(double value)
        {
            var bytes = BitConverter.GetBytes(value);

            if (false == BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        public static long GetInt64(byte[] bytes)
        {
            // Given bytes are little-endian
            // If this computer is big endian, then result need to be reversed
            if (false == BitConverter.IsLittleEndian)
            {
                var bytesClone = new byte[bytes.Length];
                bytes.CopyTo(bytesClone, 0);
                Array.Reverse(bytesClone);
                return BitConverter.ToInt64(bytesClone, 0);
            }
            else
            {
                return BitConverter.ToInt64(bytes, 0);
            }
        }
    }
}
