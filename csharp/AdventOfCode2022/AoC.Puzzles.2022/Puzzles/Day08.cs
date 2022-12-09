using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Runtime.Versioning;

namespace AoC.Puzzles._2022.Puzzles;

struct Coords
{
    public int X;
    public int Y;
}

class Rope
{
    HashSet<Coords> _tailVisited = new HashSet<Coords>();

    public Coords Head { get; set; } = new Coords { X = 20, Y = 30 };
    public Coords Tail { get; set; } = new Coords { X = 20, Y = 30 };

    public Rope()
    {
        _tailVisited.Add(Tail);
    }

    public void Move(char direction, int amount)
    {
        switch (direction)
        {
            case 'R':
                for (int i = 0; i < amount; i++)
                {
                    MoveRight();
                }
                break;
            case 'L':
                for (int i = 0; i < amount; i++)
                {
                    MoveLeft();
                }
                break;
            case 'U':
                for (int i = 0; i < amount; i++)
                {
                    MoveUp();
                }
                break;
            case 'D':
                for (int i = 0; i < amount; i++)
                {
                    MoveDown();
                }
                break;
            default:
                break;
        }
    }

    public int GetTailVisitedCount()
    {
        return _tailVisited.Count;
    }

    void MoveRight()
    {
        var head = Head;
        Clear();
        head.Y++;
        Head = head;
        Draw();
        Clear();
        UpdateTail();
        Draw();
    }

    void MoveLeft()
    {
        Clear();
        var head = Head;
        head.Y--;
        Head = head;
        Draw();
        Clear();
        UpdateTail();
        Draw();
    }

    void MoveUp()
    {
        Clear();
        var head = Head;
        head.X--;
        Head = head;
        Draw();
        Clear();
        UpdateTail();
        Draw();
    }

    void MoveDown()
    {
        var head = Head;
        Clear();
        head.X++;
        Head = head;
        Draw();
        Clear();
        UpdateTail();
        Draw();
    }

    void UpdateTail()
    {
        int ydiff = Head.Y - Tail.Y;
        int xdiff = Head.X - Tail.X;

        var tail = Tail;

        if ((ydiff == 1 && xdiff == -2) || (ydiff == 2 && xdiff == -1))
        {
            // Diagonal up right
            tail.Y++;
            tail.X--;
        }
        else if ((ydiff == 1 && xdiff == 2) || (ydiff == 2 && xdiff == 1))
        {
            // Diagonal down right.
            tail.Y++;
            tail.X++;
        }
        else if ((ydiff == -1 && xdiff == -2) || (ydiff == -2 && xdiff == -1))
        {
            // Diagnoal up left
            tail.Y--;
            tail.X--;
        }
        else if ((ydiff == -1 && xdiff == 2) || (ydiff == -2 && xdiff == 1))
        {
            // Diagnonal down left
            tail.Y--;
            tail.X++;
        }
        else if (ydiff == 2 )
        {
            // Move right
            tail.Y++;
        }
        else if (ydiff == -2)
        {
            // Move left
            tail.Y--;
        }
        else if (xdiff == 2)
        {
            // Move down
            tail.X++;
        }
        else if (xdiff == -2)
        {
            // Move up
            tail.X--;
        }

        Tail = tail;

        _tailVisited.Add(tail);
    }

    public void Draw()
    {
        if (Environment.GetEnvironmentVariable("DRAW") == null)
        {
            return;
        }

        foreach (var coord in _tailVisited)
        {
            Console.SetCursorPosition(coord.Y, coord.X);
            Console.Write("#");
        }

        Console.SetCursorPosition(Tail.Y, Tail.X);
        Console.Write("T");

        Console.SetCursorPosition(Head.Y, Head.X);
        Console.Write("H");

        Thread.Sleep(50);
    }

    public void Clear()
    {
        if (Environment.GetEnvironmentVariable("DRAW") == null)
        {
            return;
        }

        foreach (var coord in _tailVisited)
        {
            Console.SetCursorPosition(coord.Y, coord.X);
            Console.Write(" ");
        }

        Console.SetCursorPosition(Head.Y, Head.X);
        Console.Write(" ");

        Console.SetCursorPosition(Tail.Y, Tail.X);
        Console.Write(" ");
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

    [SupportedOSPlatform("windows")]
    public string Part1(string[] input)
    {
        if (Environment.GetEnvironmentVariable("DRAW") == null)
        {
            Console.SetBufferSize(10000, 10000);
            Console.Clear();
        }

        var rope = new Rope();
        rope.Draw();

        foreach (var str in input)
        {
            (char dir, int count) = Decode(str);
            rope.Move(dir, count);
        }

        return rope.GetTailVisitedCount().ToString();
    }

    public string Part2(string[] input)
    {
        return "";
    }

    static (char dir, int count) Decode(string input)
    {
        var dir = input[0];
        int count = int.Parse(input.Substring(2));
        return (dir, count);
    }
}