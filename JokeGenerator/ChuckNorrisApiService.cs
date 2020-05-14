using System;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JokeGenerator
{
    class ChuckNorrisApiService
    {
		private readonly HttpClient _client;
        public ChuckNorrisApiService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.chucknorris.io/");
        }
        
        public async Task<string[]> GetCategoriesAsync()
		{
			Task<string> getJoke = _client.GetStringAsync("jokes/categories");
            Console.WriteLine("Loading Categories...");
            string categoryResponse = await(getJoke);

			return JsonSerializer.Deserialize<string[]>(categoryResponse);
		}

		public async Task<string[]> GetRandomJokesAsync(Name name, string category, int numberOfJokes)
		{
			StringBuilder endpoint = new StringBuilder("jokes/random");

            // If a category is provided we add it to the endpoint string as a query param
			if (category != null)
			{
                endpoint.Append($"?category={category}");
			}

            // Because the user has the option to ask for multiple jokes, we send off all the joke reqs, then wait for them to come back and put them into the output array
            var allJokes = new List<Task>();
            string[] jokes = new string[numberOfJokes];

            for(int i=0; i<numberOfJokes; i++) 
            {
                allJokes.Add(_client.GetStringAsync(endpoint.ToString()));
            }

            Console.Write("Loading Jokes...");

            int cnt = 0;
            while(allJokes.Count != 0) 
            {
                // Have to cast the generic task to a string task to get to the result
                Task<string> finshedJoke = await Task.WhenAny(allJokes) as Task<string>;

                float percent = (float)(cnt+1)/numberOfJokes * 100;

                Console.Write("\rLoading Jokes... {0}%", (int)percent);

                string tempJoke = JsonSerializer.Deserialize<Joke>(finshedJoke.Result).value;

                if (name != null)
                {
                    StringBuilder namedJoke = new StringBuilder(tempJoke);
                    namedJoke.Replace("Chuck Norris", $"{name.name} {name.surname}");
                    tempJoke = namedJoke.ToString();
                }

                jokes[cnt] = tempJoke;

                allJokes.Remove(finshedJoke);
                cnt++;
            }

            Console.WriteLine();

            return jokes;
        }
    }
}
