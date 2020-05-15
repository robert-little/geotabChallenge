using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace JokeGenerator
{
    public class NamesApiService
    {
		private readonly HttpClient client;
        public NamesApiService (HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri("https://names.privserv.com/api/");
        }

        public async Task<Name> GetNamesAsync()
		{
            Console.WriteLine("Generating Name...");
			string result = await client.GetStringAsync("");
			return JsonSerializer.Deserialize<Name>(result);
		}
    }
}
