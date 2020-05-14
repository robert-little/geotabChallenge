namespace JokeGenerator 
{
    public class SearchResult 
    {
        public int total {get;set;}
        public Joke[] result {get;set;}

        public string[] GetResultsAsStringArray(int maxResults = -1) 
        {
            if (maxResults == -1) 
            {
                maxResults = total;
            }
            var tempArray = new string[maxResults];
            for(var i=0; i<maxResults; i++)
            {
                tempArray[i] = result[i].value;
            }

            return tempArray;
        }
    }
}