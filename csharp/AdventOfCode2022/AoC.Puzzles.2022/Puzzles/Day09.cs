using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using System.Xml.Linq;

namespace AoC.Puzzles._2022.Puzzles;

record struct Coords
{
    public int X;
    public int Y;
}

class RopeNode
{
    public HashSet<Coords> Visited { get; set; } = new();

    public RopeNode? Next { get; set; }
    public Coords Position { get; set; }
    public string Label { get; set; }

    public RopeNode(string label, RopeNode? next, Coords initialPosition)
    {
        Label = label;
        Next = next;
        Position = initialPosition;
        Visited.Add(Position);
    }

    public void UpdatePosition()
    {
        if (Next == null)
        {
            return;
        }

        int ydiff = Next.Position.Y - Position.Y;
        int xdiff = Next.Position.X - Position.X;
        var position = Position;

        if ((ydiff >= 1 && xdiff <= -2) || (ydiff >= 2 && xdiff <= -1))
        {
            // Diagonal up right
            position.Y++;
            position.X--;
        }
        else if ((ydiff >= 1 && xdiff >= 2) || (ydiff >= 2 && xdiff >= 1))
        {
            // Diagonal down right.
            position.Y++;
            position.X++;
        }
        else if ((ydiff <= -1 && xdiff <= -2) || (ydiff <= -2 && xdiff <= -1))
        {
            // Diagnoal up left
            position.Y--;
            position.X--;
        }
        else if ((ydiff <= -1 && xdiff >= 2) || (ydiff <= -2 && xdiff >= 1))
        {
            // Diagnonal down left
            position.Y--;
            position.X++;
        }
        else if (ydiff == 2)
        {
            // Move right
            position.Y++;
        }
        else if (ydiff == -2)
        {
            // Move left
            position.Y--;
        }
        else if (xdiff == 2)
        {
            // Move down
            position.X++;
        }
        else if (xdiff == -2)
        {
            // Move up
            position.X--;
        }

        Position = position;
        Visited.Add(Position);
    }
}

[Puzzle(2022, 9, "Rope Bridge")]
public class Day09 : IPuzzle<string[]>
{
    public Day09()
    {
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine).Where(x => x.Length > 0).ToArray();
    }

    public string Part1(string[] input)
    {
        var headNode = new RopeNode("H", null, new Coords { X = 0, Y = 0 });
        var tailNode = new RopeNode("T", headNode, headNode.Position);

        foreach (var str in input)
        {
            (char dir, int count) = Decode(str);
            var newpos = headNode.Position;

            for (int i = 0; i < count; i++)
            {
                switch (dir)
                {
                    case 'U':
                        newpos.X--;
                        break;
                    case 'D':
                        newpos.X++;
                        break;
                    case 'L':
                        newpos.Y--;
                        break;
                    case 'R':
                        newpos.Y++;
                        break;
                }

                headNode.Position = newpos;
                tailNode.UpdatePosition();
            }
        }

        return tailNode.Visited.Count.ToString();
    }

    public string Part2(string[] input)
    {
        var headNode = new RopeNode("H", null, new Coords { X = 16, Y = 16 });

        var lastNode = headNode;
        var nodes = new List<RopeNode>();
        for (int i = 1; i <= 9; i++)
        {
            var node = new RopeNode(i.ToString(), lastNode, lastNode.Position);
            nodes.Add(node);
            lastNode = node;
        }

        foreach (var str in input)
        {
            (char dir, int count) = Decode(str);
            var newpos = headNode.Position;

            for (int i = 0; i < count; i++)
            {
                switch (dir)
                {
                    case 'U':
                        newpos.X--;
                        break;
                    case 'D':
                        newpos.X++;
                        break;
                    case 'L':
                        newpos.Y--;
                        break;
                    case 'R':
                        newpos.Y++;
                        break;
                }

                headNode.Position = newpos;
                DrawNodes(nodes, headNode);

                foreach (var node in nodes)
                {
                    node.UpdatePosition();
                    var copy = nodes.ToList();
                    DrawNodes(copy, headNode);
                }

                DrawNodes(nodes, headNode);
            }
        }

        return nodes[nodes.Count - 1].Visited.Count.ToString();
    }

    void DrawNodes(List<RopeNode> nodes, RopeNode headNode)
    {
        if (Environment.GetEnvironmentVariable("DRAW") is  null)
        {
            return;
        }

        Console.Clear();
        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine(".................................................................................");
        }
        nodes.Reverse();
        for (int j = 0; j < nodes.Count; j++)
        {
            var node = nodes[j];
            Console.SetCursorPosition(node.Position.Y, node.Position.X);
            Console.Write(node.Label);

        }

        Console.SetCursorPosition(headNode.Position.Y, headNode.Position.X);
        Console.Write(headNode.Label);

        nodes.Reverse();
        Console.ReadKey();
    }

    static (char dir, int count) Decode(string input)
    {
        var dir = input[0];
        int count = int.Parse(input.Substring(2));
        return (dir, count);
    }
}