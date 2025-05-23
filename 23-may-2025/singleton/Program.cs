
class progrmam
{
    static void Main(string[] args)
    {
      while (true)
        {
            FileWriter fileWriter = FileWriter.GetInstance();
            Console.WriteLine("Enter a message to write to the file (or 'exit' to quit):");
            string input = Console.ReadLine();
            if (input?.ToLower() == "exit")
            {
            break;
            }
            fileWriter.Write(input);
        }
    }
}