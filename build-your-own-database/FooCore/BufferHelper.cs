using System;

namespace FooCore
{
    /// <summary>
    /// Helper class contains static methods that read and write numeric values
    /// into and from a byte array in little-endian byte order.
    /// </summary>
    public static class BufferHelper
    {
        public static long ReadBufferInt64(byte[] buffer, int bufferOffset)
        {
            var longBuffer = new byte[8];
            Buffer.BlockCopy(buffer, bufferOffset, longBuffer, 0, 8);
            return LittleEndianByteOrder.GetInt64(longBuffer);
        }

        public static void WriteBuffer(double value, byte[] buffer, int bufferOffset)
        {
            Buffer.BlockCopy(LittleEndianByteOrder.GetBytes(value), 0, buffer, bufferOffset, 8);
        }

        public static void WriteBuffer(uint value, byte[] buffer, int bufferOffset)
        {
            Buffer.BlockCopy(LittleEndianByteOrder.GetBytes(value), 0, buffer, bufferOffset, 4);
        }

        public static void WriteBuffer(long value, byte[] buffer, int bufferOffset)
        {
            Buffer.BlockCopy(LittleEndianByteOrder.GetBytes(value), 0, buffer, bufferOffset, 8);
        }

        public static void WriteBuffer(int value, byte[] buffer, int bufferOffset)
        {
            Buffer.BlockCopy(LittleEndianByteOrder.GetBytes(value), 0, buffer, bufferOffset, 4);
        }
    }
}
