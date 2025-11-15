using System;
using System.IO;
using System.Linq;
using FooCore;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class BlockStorageTest
    {
        [Test]
        public void TestBlockStoragePersistent()
        {
            using (var ms = new MemoryStream())
            {
                var storage = new BlockStorage(ms);

                using (var firstBlock = storage.CreateNew())
                using (var secondBlock = storage.CreateNew())
                using (var thirdBlock = storage.CreateNew())
                {
                    Assert.That(firstBlock.Id, Is.EqualTo(0));
                    Assert.That(secondBlock.Id, Is.EqualTo(1));

                    secondBlock.SetHeader(1, 100);
                    secondBlock.SetHeader(2, 200);

                    Assert.That(thirdBlock.Id, Is.EqualTo(2));
                    Assert.That(storage.BlockSize * 3, Is.EqualTo(ms.Length));
                }

                // Test to make sure our creation persists
                var storage2 = new BlockStorage(ms);
                Assert.That(storage2.Find(0).Id, Is.EqualTo(0));
                Assert.That(storage2.Find(1).Id, Is.EqualTo(1));
                Assert.That(storage2.Find(2).Id, Is.EqualTo(2));

                Assert.That(storage2.Find(1).GetHeader(1), Is.EqualTo(100));
                Assert.That(storage2.Find(1).GetHeader(2), Is.EqualTo(200));
            }
        }
    }
}
