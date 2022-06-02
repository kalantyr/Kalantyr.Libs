using NUnit.Framework;

namespace Kalantyr.Web.Tests
{
    public class HttpClientBase_Tests
    {
        [Test]
        public void Test()
        {
            var result = HttpClientBase.Serialize(new TestClass
            {
                Prop1 = 123,
                Prop2 = "dfghxdfg",
                Prop3 = TestEnum.M2
            });
            
            Assert.IsFalse(result.Contains(nameof(TestClass.Int)));
            Assert.IsFalse(result.Contains(nameof(TestClass.Str)));
            Assert.IsFalse(result.Contains(nameof(TestClass.Enum)));

            Assert.IsTrue(result.Contains(nameof(TestClass.Prop1)));
            Assert.IsTrue(result.Contains(nameof(TestClass.Prop2)));
            Assert.IsTrue(result.Contains(nameof(TestClass.Prop3)));
        }
    }

    public class TestClass
    {
        public int Int { get; set; }

        public int Prop1 { get; set; }

        public string Str { get; set; }

        public string Prop2 { get; set; }

        public TestEnum Enum { get; set; }

        public TestEnum Prop3 { get; set; }
    }

    public enum TestEnum
    {
        M1,
        M2
    }
}
