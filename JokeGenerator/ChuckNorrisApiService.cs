using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JokeGenerator
{
    public class ChuckNorrisApiService
    {
		private readonly HttpClient client;
        public ChuckNorrisApiService(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri("https://api.chucknorris.io/");
        }
        
        public async Task<string[]> GetCategoriesAsync()
		{
            Task<string> getJoke = client.GetStringAsync("jokes/categories");
            Console.WriteLine("Loading Categories...");
            string categoryResponse = await getJoke;

            return JsonSerializer.Deserialize<string[]>(categoryResponse);
			
		}

        public async Task<string[]> SearchJokeAsync(string query, int maxResults=9)
		{
            string encodedQuery = WebUtility.UrlEncode(query);

            Task<string> getSearch = client.GetStringAsync($"jokes/search?query={encodedQuery}");
            Console.WriteLine("Loading Search Results...");
            string searchResponse = await getSearch;

            var searchResults = JsonSerializer.Deserialize<SearchResult>(searchResponse);

            if (searchResults.total == 0)
            {
                return new string[] {"No results found for search."};
            }

            if (searchResults.total < maxResults)
            {
                return searchResults.GetResultsAsStringArray();
            }

            return searchResults.GetResultsAsStringArray(maxResults);			
		}

		public async Task<string[]> GetRandomJokesAsync(string name, string category, int numberOfJokes)
		{
			StringBuilder endpoint = new StringBuilder("jokes/random");

            // If a category or a name is provided we add it to the endpoint string as a query param
            // Looking a little deeper into the doc I found that adding a name as a query param does the personalization for you
            if (category != null && name != null)
            {
                endpoint.Append($"?category={category}&name={WebUtility.UrlEncode(name)}");
            } 
            else
            {
                if (category != null)
                {
                    endpoint.Append($"?category={category}");
                }
                if (name != null) 
                {
                    endpoint.Append($"?name={WebUtility.UrlEncode(name)}");
                }
            }

            // Because the user has the option to ask for multiple jokes, we send off all the joke reqs, then wait for them to come back and put them into the output array
            var allJokes = new List<Task>();
            string[] jokes = new string[numberOfJokes];

            for(int i=0; i<numberOfJokes; i++) 
            {
                allJokes.Add(client.GetStringAsync(endpoint.ToString()));
            }

            Console.Write("Loading Jokes...");

            int cnt = 0;
            while(allJokes.Count != 0) 
            {
                // Have to cast the generic task to a string task to get to the result
                Task<string> finshedJoke = await Task.WhenAny(allJokes) as Task<string>;

                float percent = (float)(cnt+1)/numberOfJokes * 100;

                Console.Write("\rLoading Jokes... {0}%", (int)percent);

                jokes[cnt] = JsonSerializer.Deserialize<Joke>(finshedJoke.Result).value;

                allJokes.Remove(finshedJoke);
                cnt++;
            }

            Console.WriteLine();

            return jokes;            
        }
    }
}
