using Microsoft.VisualStudio.TestTools.UnitTesting;
using JokeGenerator;

namespace JokeGenerator.Tests
{
    [TestClass]
    public class SearchResultShould
    {
        private readonly SearchResult searchResult;

        public SearchResultShould()
        {
            searchResult = new SearchResult();
            searchResult.total = 2;
            var joke1 = new Joke();
            joke1.value = "This is a joke";
            var joke2 = new Joke();
            joke2.value = "This is a joke too";
            searchResult.result = new Joke[] {joke1, joke2};
        }

        [TestMethod]
        public void SearchResult_GetResultsAsStringArrayNoArg()
        {
            var expected = new string[] { "This is a joke", "This is a joke too" };
            var result = searchResult.GetResultsAsStringArray();

            Assert.IsTrue(result.Length == expected.Length, "Array lengths do not match");
            // comparing the string vals of the arrays to see if they match
            Assert.IsTrue(result.ToString() == expected.ToString(), "Array does not match expected array");
        }

        [TestMethod]
        public void SearchResult_GetResultsAsStringArrayWithMax()
        {
            var expected = new string[] { "This is a joke" };
            var result = searchResult.GetResultsAsStringArray(1);

            Assert.IsTrue(result.Length == expected.Length, "Array lengths do not match");
            Assert.IsTrue(result.ToString() == expected.ToString(), "Array does not match expected array");
        }

        [TestMethod]
        public void SearchResult_GetResultsAsStringArrayWithMaxSmallArray()
        {
            var expected = new string[] { "This is a joke", "This is a joke too" };
            var result = searchResult.GetResultsAsStringArray(5);

            Assert.IsTrue(result.Length == expected.Length, "Array lengths do not match");
            Assert.IsTrue(result.ToString() == expected.ToString(), "Array does not match expected array");
        }
    }
}
