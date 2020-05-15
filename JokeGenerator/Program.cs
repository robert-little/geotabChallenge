using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace JokeGenerator
{
    class Program
    {
        static public async Task Main(string[] args)
        {
            // Setting up the services and providing them a httpClient
            var services = new ServiceCollection();

            services.AddHttpClient<ChuckNorrisApiService>();
            services.AddHttpClient<NamesApiService>();
            services.AddSingleton<JokeGenerator>();

            var serviceProvider = services.BuildServiceProvider();

            var jokeGenerator = serviceProvider.GetRequiredService<JokeGenerator>();
            await jokeGenerator.GenerateJokes();
        }
    }
}
