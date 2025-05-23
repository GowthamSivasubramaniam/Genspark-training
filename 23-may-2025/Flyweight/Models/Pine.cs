using Flyweight.Interfaces;
namespace Flyweight.Models
{
public class PineTree : ITree
{
    private readonly string _texture = "Needle Leaf Texture";
    private readonly string _bark = "Scaly Bark";

    public void Display(int x, int y)
    {
        Console.WriteLine($"Displaying Pine Tree at ({x}, {y}) with {_texture} and {_bark}");
    }
}
}