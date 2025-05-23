using Flyweight.Interfaces;
using Flyweight.Models;

public class TreeFactory
{
    private readonly Dictionary<string, ITree> _trees = new();

    public ITree GetTree(string type)
    {
        if (!_trees.ContainsKey(type))
        {
            _trees[type] = type switch
            {
                "Oak" => new OakTree(),
                "Pine" => new PineTree(),
                _ => throw new ArgumentException("Invalid tree type")
            };
        }

        return _trees[type];
    }
}
