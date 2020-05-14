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

            var serviceProvider = services.BuildServiceProvider();

            ChuckNorrisApiService jokeApiService = serviceProvider.GetRequiredService<ChuckNorrisApiService>();
            NamesApiService nameApiService = serviceProvider.GetRequiredService<NamesApiService>();

            JokeGenerator jokeGenerator = new JokeGenerator(jokeApiService, nameApiService);
            await jokeGenerator.GenerateJokes();
        }
    }
}
