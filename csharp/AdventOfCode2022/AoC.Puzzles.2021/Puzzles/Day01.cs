using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 1, "Calories")]
public class Day01 : IPuzzle<string[]>
{
    public string[] Parse(string inputText)
    {
        var lines = inputText.Split(Environment.NewLine);
        return lines;
    }

    public string Part1(string[] lines)
    {

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

        return max.ToString();
    }

    public string Part2(string[] lines)
    {
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

        return total.ToString();
    }
}