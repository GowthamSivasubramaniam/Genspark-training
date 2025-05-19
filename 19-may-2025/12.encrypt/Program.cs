class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a word");
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("No word entered.");
            return;
        }
        Encrypt(input);
        Decrypt(input);

    }

    static void Encrypt(string input)
    {

        char[] charArray = input.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i];
            if (char.IsLetter(c))
            {
                char newChar = (char)(c + 3);
                if (char.IsLower(c) && newChar > 'z' || char.IsUpper(c) && newChar > 'Z')
                {
                    newChar = (char)(newChar - 26);
                }
                charArray[i] = newChar;
            }
            else
            {
                Console.WriteLine("Invalid character in input. Only letters are allowed.");
                return;
            }
        }
        Console.WriteLine("Encrypted word: " + new string(charArray));
    }
    static void Decrypt(string input)
    {
        char[] charArray = input.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            char c = charArray[i];
            if (char.IsLetter(c))
            {
                char newChar = (char)(c - 3);
                if (char.IsLower(c) && newChar < 'a' || char.IsUpper(c) && newChar < 'A')
                {
                    newChar = (char)(newChar + 26);
                }
                charArray[i] = newChar;
            }
            else
            {
                Console.WriteLine("Invalid character in input. Only letters are allowed.");
                return;
            }
        }
        Console.WriteLine("Decrypted word: " + new string(charArray));
    }
}
