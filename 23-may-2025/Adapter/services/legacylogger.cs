

namespace Adapter.services
{
public class LegacyLogger
{
    public void WriteToLog(string msg)
    {
        Console.WriteLine($"[LegacyLogger] {msg}");
    }
}
}