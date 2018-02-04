using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Windows.Forms;
using static DataMaker.Utils;

namespace DataMakerTests
{
    [TestFixture]
    public class UtilsTests
    {
        // Tests in this class doesn't need arrange.
        // Just act and then assert.
        #region Lang
        [Test]
        public void Lang_ValidKey_ReturnsTranslation()
        {
            var result = Lang("global_block");

            Assert.AreEqual("方块", result);
        }

        [Test]
        public void Lang_InvalidKey_Throws()
        {
            var ex = Assert.Catch<Exception>(() => Lang("#@$(*&"));

            StringAssert.Contains("No Lang", ex.Message);
        }
        #endregion
        #region IsNameLegal
        [Test]
        public void IsNameLegal_LegalName_ReturnsTrue()
        {
            var result = IsNameLegal("spgoding-test_file.json");

            Assert.IsTrue(result);
        }

        [Test]
        public void IsNameLegal_UpcaseName_ReturnsFalse()
        {
            var result = IsNameLegal("UPCASE");

            Assert.IsFalse(result);
        }

        [Test]
        public void IsNameLegal_ChineseName_ReturnsFalse()
        {
            var result = IsNameLegal("哈哈");

            Assert.IsFalse(result);
        }
        #endregion
        #region GetJsonPreffix
        [Test]
        public void GetJsonPreffix_NoKeyNoBrackets_ReturnsEmpty()
        {
            var result = GetJsonPreffix("%NoKey%NoBrackets%", "{");

            Assert.AreEqual("", result);
        }

        [Test]
        public void GetJsonPreffix_NoKey_ReturnsBrackets()
        {
            var result = GetJsonPreffix("%NoKey%", "{");

            Assert.AreEqual("{", result);
        }

        [Test]
        public void GetJsonPreffix_NoBrackets_Throws()
        {
            var ex =
                Assert.Catch<ArgumentException>(() => GetJsonPreffix("%NoBrackets%", "{"));

            StringAssert.Contains("Just has a key", ex.Message);
        }

        [Test]
        public void GetJsonPreffix_AllHave_ReturnsKeyAndBrackets()
        {
            var result = GetJsonPreffix("test", "{");

            Assert.AreEqual("\"test\":{", result);
        }
        #endregion
        #region GetJsonPreffix
        [Test]
        public void GetJsonSuffix_NoBrackets_ReturnsEmpty()
        {
            var result = GetJsonSuffix("%NoKey%NoBrackets%", "}");

            Assert.AreEqual("", result);
        }

        [Test]
        public void GetJsonSuffix_HaveBrackets_ReturnsBrackets()
        {
            var result = GetJsonSuffix("%NoKey%", "}");

            Assert.AreEqual("}", result);
        }
        #endregion
    }
}
