using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace JokeGenerator
{
    class Program
    {
        // Used to save the categories after they have been retirved so we dont need to always fetch them
        static private string[] categories;

        static public async Task Main(string[] args)
        {
            // Setting up the services and providing them a httpClient
            var services = new ServiceCollection();

            services.AddHttpClient<ChuckNorrisApiService>();
            services.AddHttpClient<NamesApiService>();

            var serviceProvider = services.BuildServiceProvider();

            ChuckNorrisApiService jokeApiService = serviceProvider.GetRequiredService<ChuckNorrisApiService>();
            NamesApiService nameApiService = serviceProvider.GetRequiredService<NamesApiService>();

            // Vars we will always need or ones we want to maintain (keeping categories out here lets us avoid making a call to categories too often)

            // Null char val
            char topLevelControl = '\0';
            categories = null;

            Console.WriteLine("Press ? to get instructions.");

            while (topLevelControl != 'q') {
                char[] controlChars = {'?', 'c', 'r', 'q'};
                topLevelControl = LimitedCharSelection("", controlChars);

                // Display instructions
                if (topLevelControl == '?') 
                {
                    Console.WriteLine("Press c to get categories");
                    Console.WriteLine("Press r to get random jokes");
                    Console.WriteLine("Press q to quit");
                } 
                // Display Categories
                else if (topLevelControl == 'c')
                {
                    await(GetAndDisplayCats(jokeApiService));
                }
                // Initiate random joke selection
                else if (topLevelControl == 'r')
                {
                    // Vars only needed when getting a random joke
                    string category = null;
                    Name name = null;

                    // Getting and setting a random name from the api if yes
                    string selectionMessage = "Want to use a random name? y/n";
                    char[] yesNo = {'y', 'n'};
                    if (LimitedCharSelection(selectionMessage, yesNo) == 'y') {
                        name = await(nameApiService.GetNamesAsync());
                    }


                    // Letting the user enter a category
                    selectionMessage = "Want to specify a category? y/n";
                    if (LimitedCharSelection(selectionMessage, yesNo) == 'y')
                    {
                        await(GetAndDisplayCats(jokeApiService));
                        Console.WriteLine("Enter a category from the list above: ");
                        category = Console.ReadLine();
                    }
                    
                    selectionMessage = "How many jokes do you want? (1-9)";
                    char[] digits = {'1', '2', '3', '4', '5', '6', '7', '8', '9'};
                    // Getting the key for how many jokes the user wants and converting it to an int
                    int numberOfJokes = (int)Char.GetNumericValue(LimitedCharSelection(selectionMessage, digits));
                    
                    string[] jokeResults = await(jokeApiService.GetRandomJokesAsync(name, category, numberOfJokes));
                    PrintResults(jokeResults);
                }
            }
        }

        static private void PrintResults(string[] results)
        {
            for (int i=0; i<results.Length; i++) 
            {
                Console.WriteLine($"{i+1}. {results[i]}");
            }
        }

        // Only allows users to enter chars that are valid for the current selection
        static private char LimitedCharSelection(string message, char[] validSelection) {
            Console.WriteLine(message);
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            while(Array.IndexOf(validSelection, key) == -1)
            {
                Console.WriteLine("Invaild selection.\n");
                if (message != "") {
                    Console.WriteLine(message);
                }
                key = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }

            return key;
        }

        static private async Task GetAndDisplayCats(ChuckNorrisApiService jokeApiService) {
            if (categories == null) {
                categories = await(jokeApiService.GetCategoriesAsync());
            }
            PrintResults(categories);
        }
    }
}
