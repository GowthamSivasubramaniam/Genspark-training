
class Program
{
    public static void Main(string[] args)
    {
        int attempts = 0;
        while (true)
        {
            Console.WriteLine("Enter a word");
            string? word = Console.ReadLine();
            if (string.IsNullOrEmpty(word))
            {
                Console.WriteLine("No word entered.");
                continue;
            }
            if (word.Length != 4)
            {
                Console.WriteLine("secret Word length is 4.");
                continue;
            }
            int bullscount = Checkbulls(word, secret: "GAME");
            if (bullscount == 4)
             {
                Console.WriteLine("You guessed the word!");
                Console.WriteLine("You used " + attempts + " attempts.");
                break;
            }
            int cowscount = Checkcows(word, secret: "GAME");
            Console.WriteLine("Bulls: " + bullscount);
            Console.WriteLine("Cows: " + cowscount);
        
            attempts++;
        }
        
    }

    private static int Checkbulls(string? word, string secret)
    {
        int count = 0;
        if (word != null && word.Length > 0)
        {
            for (int i = 0; i < word.Length; i++)
            {

                if (word[i] == secret[i])
                {
                    count++;
                }
            }
        }
        else
        {
            Console.WriteLine("No word entered.");
        }
        return count;
    }
    private static int Checkcows(string word,string secret)
    {
        int count = 0;
        if (word != null && word.Length > 0)
        {
            for (int i = 0; i < word.Length; i++)
            {
                for (int j = 0; j < secret.Length; j++)
                {
                    if (word[i] == secret[j] && i != j)
                    {
                        count++;
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("No word entered.");
        }
        return count;
    }
}