namespace JokeGenerator 
{
    public class Name 
    {
        public string name {get;set;}
        public string surname {get;set;}

        public override string ToString() 
        {
            return $"{name} {surname}";
        }
    }
}