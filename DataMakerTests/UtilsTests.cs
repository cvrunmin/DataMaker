using DataMaker;
using NUnit.Framework;
using System;

namespace DataMakerTests
{
    [TestFixture]
    public class UtilsTests
    {
        // Tests in this class doesn't need arrange.
        // Just act and then assert.

        [Test]
        public void Lang_ValidKey_Returns()
        {
            var result = Utils.Lang("global_block");

            Assert.AreEqual("方块", result);
        }

        [Test]
        public void Lang_InvalidKey_Throws()
        {
            var ex = Assert.Catch<Exception>(() => Utils.Lang("#@$(*&"));

            StringAssert.Contains("No Lang", ex.Message);
        }


    }
}
