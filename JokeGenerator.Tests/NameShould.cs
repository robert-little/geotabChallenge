using Microsoft.VisualStudio.TestTools.UnitTesting;
using JokeGenerator;

namespace JokeGenerator.Tests
{
    [TestClass]
    public class NameShould
    {
        private readonly Name name;

        public NameShould()
        {
            name = new Name();
            name.name = "First";
            name.surname = "Last";
        }

        [TestMethod]
        public void Name_ToString()
        {
            var expected = "First Last";
            var result = name.ToString();

            Assert.IsTrue(result == expected, "Name strings do not match");
        }

        [TestMethod]
        public void Name_ToStringNoSurname()
        {
            var noFirst = new Name();
            noFirst.name = "First";
            var expected = "First ";
            var result = noFirst.ToString();

            Assert.IsTrue(result == expected, "Name strings do not match");
        }
    }
}
