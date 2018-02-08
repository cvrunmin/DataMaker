using DataMaker;
using DataMaker.Parsers;
using NUnit.Framework;

namespace DataMakerTests
{
    [TestFixture]
    public class ConditionTests
    {
        [Test]
        public void IsTrue_EmptyCondition_ReturnsTrue()
        {
            var c = Condition.Parse("", new FrameParser());

            var result = c.IsTrue();

            Assert.IsTrue(result);
        }

        //[Test]
        //public void IsTrue_TrueCondition_ReturnsTrue()
        //{
        //    var c = Condition.Parse("", new FrameParser());


        //    var result = c.IsTrue();

        //    Assert.IsTrue(result);
        //}
    }
}
