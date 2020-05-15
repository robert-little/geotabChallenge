using JokeGenerator;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JokeGenerator.Tests
{
    [TestClass]
    public class ChuckNorrisApiServiceShould
    {
        private readonly ChuckNorrisApiService apiService;

        public ChuckNorrisApiServiceShould()
        {
            // I know I should be using a mocked version of HttpClient but I ran out of time :(, also it looks like it is pretty complicated to mock based on a quick google
            // So this is more of an integration test then a unit test, but at least it is something! 
            apiService = new ChuckNorrisApiService(new HttpClient());
        }

        // Obviously all of these tests are pretty brittle. I am making them based off of quanities know to me based on looking at the results I am getting at the time of writing the tests
        // If I had mocked the HttpClient then we could pass back whatever we wanted for the call, making these test actually good, but again, ran out of time

        [TestMethod]
        public async Task ChuckNorrisApiServiceShould_GetCategoriesAsync()
        {
            string[] result = await apiService.GetCategoriesAsync();
            
            Assert.IsTrue(result.Length == 16, "Category length did not match");
            Assert.IsTrue(result[0] == "animal", "Category length did not match");
        }

        [TestMethod]
        public async Task ChuckNorrisApiServiceShould_SearchJokeAsync()
        {
            // kitty was chosen as the search term because it only has 2 results (at the time of writing)
            string[] result = await apiService.SearchJokeAsync("kitty");
            
            Assert.IsTrue(result.Length == 2, "Search did not return proper match");
        }

        [TestMethod]
        public async Task ChuckNorrisApiServiceShould_SearchJokeAsyncMaxResults()
        {
            // kitty was chosen as the search term because it only has 2 results (at the time of writing)
            string[] result = await apiService.SearchJokeAsync("kitty", 1);
            
            Assert.IsTrue(result.Length == 1, "Search with max results failed");
        }

        [TestMethod]
        public async Task ChuckNorrisApiServiceShould_GetRandomJokesAsync1Joke()
        {
            string[] result = await apiService.GetRandomJokesAsync(null, null, 1);
            
            Assert.IsTrue(result.Length == 1, "Random joke was not returned");
            Assert.IsTrue(result[0].Contains("Chuck Norris"), "Joke in incorrect format");
        }

        [TestMethod]
        public async Task ChuckNorrisApiServiceShould_GetRandomJokesAsyncMultiJoke()
        {
            string[] result = await apiService.GetRandomJokesAsync(null, null, 3);
            
            Assert.IsTrue(result.Length == 3, "Wrong number of jokes returned");
            Assert.IsTrue(result[0].Contains("Chuck Norris"), "Joke in incorrect format");
        }

        [TestMethod]
        public async Task ChuckNorrisApiServiceShould_GetRandomJokesAsyncPersonalized()
        {
            string[] result = await apiService.GetRandomJokesAsync("Rob Little", null, 1);
            
            Assert.IsTrue(result.Length == 1, "Random joke was not returned");
            Assert.IsTrue(result[0].Contains("Rob Little"), "Joke in incorrect format");
        }

        [TestMethod]
        public async Task ChuckNorrisApiServiceShould_GetRandomJokesAsyncCategory()
        {
            // If I was mocking I would check for the url that was being sent to the api to ensure it had the proper query string
            string[] result = await apiService.GetRandomJokesAsync(null, "animal", 1);
            
            Assert.IsTrue(result.Length == 1, "Random joke was not returned");
        }
    }
}
