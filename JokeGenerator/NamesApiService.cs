using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace JokeGenerator
{
    class NamesApiService
    {
		private readonly HttpClient _client;
        public NamesApiService (HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://names.privserv.com/api/");
        }

        public async Task<Name> GetNamesAsync()
		{
            Console.WriteLine("Generating Name...");
			string result = await(_client.GetStringAsync(""));
			return JsonSerializer.Deserialize<Name>(result);
		}
    }
}
