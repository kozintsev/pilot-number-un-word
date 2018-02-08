using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NumberInWords.Test
{
    [TestClass]
    public class TestConvert
    {
        [TestMethod]
        public void Test1()
        {
            var convert = new RoubleToStringProvider(true, true, true);
            var m = new Money(1000000);
            var s = convert.MoneyToString(m);

        }

        [TestMethod]
        public void Test2()
        {
            const int n = 1402368;
            var s = RusNumber.Str(n);
            Assert.IsTrue(s.Contains("четыреста"));
            const double d = 1402368.5;
            var c = d.ToString("C");
            var a = d.ToString("N");
        }

        [TestMethod]
        public void Test3()
        {
            const int n = 51200;
            var s = RusNumber.Str(n);
            Assert.IsTrue(s.Contains("двести"));
            var c = n.ToString("C");
            var a = n.ToString("N");
        }

    }
}
