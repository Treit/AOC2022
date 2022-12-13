using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2022.Puzzles;

record GraphNode
{
    public GraphNode(char label, GraphNode? left, GraphNode? right, GraphNode? up, GraphNode? down, (int, int) coords)
    {
        Label = label;
        Left = left;
        Right = right;
        Up = up;
        Down = down;
    }

    public char Label { get; set; }
    public GraphNode? Left { get; set; }
    public GraphNode? Right { get; set; }
    public GraphNode? Up { get; set; }
    public GraphNode? Down { get; set; }
    public GraphNode? Parent { get; set; }
    public (int, int) Coords { get; set; }
}

[Puzzle(2022, 12, "Hill Climbing Algorithm")]
public class Day12 : IPuzzle<string[]>
{
    public Day12()
    {
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine).Where(x => x.Length > 0).ToArray();
    }

    public string Part1(string[] input)
    {
        var map = BuildMap(input);
        GraphNode? startNode = null;

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                if (map[i, j].Label == 'S')
                {
                    startNode = map[i, j];
                }
            }
        }

        var node = Search(startNode!);
        int steps = 0;

        while (true)
        {
            if (node.Parent == null)
            {
                break;
            }

            steps++;
            node = node.Parent;
        }

        return steps.ToString();
    }

    public string Part2(string[] input)
    {
        var map = BuildMap(input);
        var startingNodes = new List<GraphNode>();

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                if (map[i, j].Label == 'S' || map[i, j].Label == 'a')
                {
                    startingNodes.Add(map[i, j]);
                }
            }
        }

        int minsteps = int.MaxValue;

        foreach (var startNode in startingNodes)
        {
            // Clear previously set parents.
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    map[i, j].Parent = null;
                }
            }

            var node = Search(startNode!);
            int steps = 0;

            while (true)
            {
                if (node.Parent == null)
                {
                    break;
                }

                steps++;
                node = node.Parent;
            }

            if (steps > 0 && steps < minsteps)
            {
                minsteps = steps;
            }
        }

        return minsteps.ToString();
    }

    GraphNode[,] BuildMap(string[] input)
    {
        var grid = new GraphNode[input.Length, input[0].Length];

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                grid[i, j] = new GraphNode('_', null, null, null, null, (0, 0));
            }
        }

        var inputCopy = input.ToArray();
        for (int i = 0; i < inputCopy.Length; i++)
        {
            inputCopy[i] = inputCopy[i].Replace('S', 'a').Replace('E', 'z');
            for (int j = 0; j < inputCopy[i].Length; j++)
            {
                char label = input[i][j];
                char current = inputCopy[i][j];

                GraphNode? left = (j > 0 && HeightOk(inputCopy[i][j - 1], current)) ? grid[i, j - 1] : null;
                GraphNode? right = (j < inputCopy[i].Length - 1 && HeightOk(inputCopy[i][j + 1], current)) ? grid[i, j + 1] : null;
                GraphNode? up = (i > 0 && HeightOk(inputCopy[i - 1][j], current)) ? grid[i - 1, j] : null;
                GraphNode? down = (i < inputCopy.Length - 1 && HeightOk(inputCopy[i + 1][j], current)) ? grid[i + 1, j] : null;

                var currentNode = grid[i, j];
                currentNode.Label = label;
                currentNode.Left = left;
                currentNode.Right = right;
                currentNode.Up = up;
                currentNode.Down = down;
                currentNode.Coords = (i, j);
            }
        }

        return grid;
    }

    GraphNode Search(GraphNode root)
    {
        var explored = new HashSet<(int, int)>();
        var queue = new Queue<GraphNode>();
        explored.Add(root.Coords);
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var v = queue.Dequeue();

            if (v.Label == 'E')
            {
                return v;
            }

            if (v.Left != null && !explored.Contains(v.Left.Coords))
            {
                v.Left.Parent = v;
                explored.Add(v.Left.Coords);
                queue.Enqueue(v.Left);
            }

            if (v.Right != null && !explored.Contains(v.Right.Coords))
            {
                v.Right.Parent = v;
                explored.Add(v.Right.Coords);
                queue.Enqueue(v.Right);
            }

            if (v.Up != null && !explored.Contains(v.Up.Coords))
            {
                v.Up.Parent = v;
                explored.Add(v.Up.Coords);
                queue.Enqueue(v.Up);
            }

            if (v.Down != null && !explored.Contains(v.Down.Coords))
            {
                v.Down.Parent = v;
                explored.Add(v.Down.Coords);
                queue.Enqueue(v.Down);
            }
        }

        return root;
    }

    bool HeightOk(char dest, char src)
    {
        if (dest > src)
        {
            return dest - src <= 1;
        }

        return true;
    }
}
