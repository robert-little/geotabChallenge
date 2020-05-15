using System;
using System.Threading.Tasks;

namespace JokeGenerator
{
    class JokeGenerator
    {
        // Used to save the categories after they have been retirved so we dont need to always fetch them
        private string[] categories;
        public ChuckNorrisApiService jokeApiService;
        public NamesApiService nameApiService;

        public JokeGenerator(ChuckNorrisApiService jokeApiService, NamesApiService nameApiService) 
        {
            this.jokeApiService = jokeApiService;
            this.nameApiService = nameApiService;
        }

        public async Task GenerateJokes()
        {
            // Null char val
            char topLevelControl = '\0';
            categories = null;

            Console.Clear();
            Console.WriteLine("Welcome to the Chuck Norris Joke Generator");
            PrintFist();

            while (topLevelControl != 'q') 
            {
                var selectionMessage = "";
                char[] yesNo = {'y', 'n'};
                char[] controlChars = {'?', 'c', 'j', 'q', 's'};
                char[] digits = {'1', '2', '3', '4', '5', '6', '7', '8', '9'};
                topLevelControl = LimitedCharSelection("", controlChars);

                // Display instructions
                if (topLevelControl == '?') 
                {
                    Console.Clear();
                    Console.WriteLine("Press c to get a Chuck Norris joke");
                    Console.WriteLine("Press j to get custom jokes");
                    Console.WriteLine("Press s to search for jokes");
                    Console.WriteLine("Press q to quit");
                } 
                // Just a random Chuck Norris joke
                else if (topLevelControl == 'c')
                {
                    try
                    {
                        PrintResults(await jokeApiService.GetRandomJokesAsync(null, null, 1));
                        PrintFist();
                    }
                    catch
                    {
                        PrintError("An error occured when attempting to retrieve a joke. Please try again later.");
                        continue;
                    }
                    
                }
                // Getting a more specific joke, allows for category choice and personalization  
                else if (topLevelControl == 'j')
                {
                    Console.Clear();

                    string category = null;
                    string name = null;

                    selectionMessage = "Want to use a different name? y/n";
                    if (LimitedCharSelection(selectionMessage, yesNo) == 'y') 
                    {
                        selectionMessage = "Would you like to enter and name? (Selecting n will use a random name) y/n";
                        if (LimitedCharSelection(selectionMessage, yesNo) == 'n') 
                        {
                            try
                            {
                                Name nameResult = await nameApiService.GetNamesAsync();
                                name = nameResult.ToString();
                            }
                            catch
                            {
                                PrintError("An error occured when attempting to retrieve a random name. Please try again later.");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Enter name:");
                            name = Console.ReadLine();
                        }
                    }

                    // Letting the user enter a category
                    selectionMessage = "Want to specify a category? y/n";
                    if (LimitedCharSelection(selectionMessage, yesNo) == 'y')
                    {
                        try
                        {
                            if (categories == null) 
                            {

                                categories = await jokeApiService.GetCategoriesAsync();
                            }
                            PrintResults(categories);
                            category = LimitedStringSelection("Enter a category from the list above: ", categories);
                        }
                        catch
                        {
                            PrintError("An error occured when attempting to retrieve categories. Please try again later.");
                            continue;
                        }
                    }
                    
                    selectionMessage = "How many jokes do you want? (1-9)";
                    // Getting the key for how many jokes the user wants and converting it to an int
                    int numberOfJokes = (int)Char.GetNumericValue(LimitedCharSelection(selectionMessage, digits));

                    try
                    {
                        PrintResults(await jokeApiService.GetRandomJokesAsync(name, category, numberOfJokes));
                        PrintFist();   
                    }
                    catch
                    {
                        PrintError("An error occured when attempting to retrieve joke(s). Please try again later.");
                        continue;
                    }
                }
                // Joke search 
                else if (topLevelControl == 's')
                {
                    Console.Clear();

                    Console.WriteLine("Enter search string:");
                    var query = Console.ReadLine();

                    selectionMessage = "How many results would you like? (1-9)";
                    var numberOfResults = (int)Char.GetNumericValue(LimitedCharSelection(selectionMessage, digits));

                    try
                    {
                        PrintResults(await jokeApiService.SearchJokeAsync(query, numberOfResults));
                        PrintFist();
                    }
                    catch
                    {
                        PrintError("An error occured when attempting to search. Please try again later.");
                        continue;
                    }
                }
            }

            return;
        }

        private void PrintFist() 
        {
            Console.WriteLine("\n        _    ,-,    _");
            Console.WriteLine(" ,--, /: :\\/': :`\\/: :\\");
            Console.WriteLine("|`;  ' `,'   `.;    `: |");
            Console.WriteLine("|    |     |  '  |     |.");
            Console.WriteLine("| :  |   Chuck Norris  ||");
            Console.WriteLine("| :. |  :  |  :  |  :  | \\");
            Console.WriteLine(" \\__/: :.. : :.. | :.. |  )");
            Console.WriteLine("      `---',\\___/,\\___/ /'");
            Console.WriteLine("           `==._ .. . /'");
            Console.WriteLine("                `-::-'");
            Console.WriteLine("Press ? to get instructions.");
        }

        private void PrintResults(string[] results)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            for (int i=0; i<results.Length; i++) 
            {
                Console.WriteLine($"{i+1}. {results[i]}");
            }
            Console.ResetColor();
        }

        private void PrintError(string error) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
            PrintFist();

        }

        // Only allows users to enter chars that are valid for the current selection
        private char LimitedCharSelection(string message, char[] validSelection) 
        {
            Console.WriteLine(message);
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            while(Array.IndexOf(validSelection, key) == -1)
            {
                Console.WriteLine("Invaild selection.\n");
                if (message != "") 
                {
                    Console.WriteLine(message);
                }
                key = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }

            return key;
        }

        private string LimitedStringSelection(string message, string[] validSelection) 
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            Console.WriteLine();
            while(Array.IndexOf(validSelection, input) == -1)
            {
                Console.WriteLine("Invaild selection.\n");
                if (message != "") 
                {
                    Console.WriteLine(message);
                }
                input = Console.ReadLine();
                Console.WriteLine();
            }

            return input;
        }
    }
}
