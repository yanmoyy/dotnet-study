using FooCore;

namespace UnitTest
{
    public class LittleEndianByteOrderTest
    {
        [Test]
        public void TestGetInt64()
        {
            TestLE(0x0000000012345678L, [0x78, 0x56, 0x34, 0x12, 0x00, 0x00, 0x00, 0x00]);
            TestLE(-1L, [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF]);
            TestLE(1L, [0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]);
        }

        private static void TestLE(long expected, byte[] input)
        {
            Assert.That(LittleEndianByteOrder.GetInt64(input), Is.EqualTo(expected));
        }
    }
}
