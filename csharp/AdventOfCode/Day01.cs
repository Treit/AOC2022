using Spectre.Console;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split(Environment.NewLine);
        int max = 0;
        int current = 0;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (current > max)
                {
                    max = current;
                }

                current = 0;
                continue;
            }

            current += int.Parse(line);
        }

        return ValueTask.FromResult(max.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split(Environment.NewLine);
        var topThree = new PriorityQueue<int, int>();
        int current = 0;

        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                current += int.Parse(line);
                continue;
            }

            topThree.Enqueue(current, current);
            current = 0;

            if (topThree.Count < 4)
            {
                continue;
            }

            // Throw away the smallest.
            _ = topThree.Dequeue();
        }

        var total = (topThree.Dequeue() + topThree.Dequeue() + topThree.Dequeue());

        return ValueTask.FromResult(total.ToString());

    }
}
