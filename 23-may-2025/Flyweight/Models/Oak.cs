using Flyweight.Interfaces;

namespace Flyweight.Models
{
public class OakTree : ITree
{
   
    private readonly string _texture = "Oak Leaf Texture";
    private readonly string _bark = "Rough Bark";

    public void Display(int x, int y)
    {
        Console.WriteLine($"Displaying Oak Tree at ({x}, {y}) with {_texture} and {_bark}");
    }
}

}