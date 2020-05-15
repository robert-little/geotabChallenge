using JokeGenerator;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JokeGenerator.Tests
{
    [TestClass]
    public class NamesApiServiceShould
    {
        private readonly NamesApiService namesApi;

        public NamesApiServiceShould()
        {
            // I know I should be using a mocked version of HttpClient but I ran out of time :(, also it looks like it is pretty complicated to mock based on a quick google
            // So this is more of an integration test then a unit test, but at least it is something! 
            namesApi = new NamesApiService(new HttpClient());
        }

        [TestMethod]
        public async Task NamesApiService_GetNamesAsync()
        {
            Name result = await namesApi.GetNamesAsync();

            Assert.IsTrue(result.name != null, "Name was not set");
        }
    }
}
