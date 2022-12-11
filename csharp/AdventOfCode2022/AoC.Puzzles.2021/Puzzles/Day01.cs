using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2021.Puzzles;

[Puzzle(2021, 1, "Sonar Sweep")]
public class Day01 : IPuzzle<string[]>
{
    public string[] Parse(string inputText)
    {
        var lines = inputText.Split(Environment.NewLine).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        return lines;
    }

    public string Part1(string[] depths)
    {
        var prev = -1;
        var increasedCount = 0;

        foreach (var depth in depths)
        {
            var current = int.Parse(depth);
            if (prev >= 0 && current > prev)
            {
                increasedCount++;
            }

            prev = current;
        }

        return increasedCount.ToString();
    }

    public string Part2(string[] depths)
    {
        var prev = -1;
        var increasedCount = 0;
        int windowStart = 0;

        while (true)
        {
            var window = depths.AsSpan(windowStart, 3);
            var current = SumWindow(window);

            if (prev >= 0 && current > prev)
            {
                increasedCount++;
            }

            prev = current;
            windowStart += 1;

            if (windowStart + 1 >= depths.Length - 1)
            {
                break;
            }
        }

        return increasedCount.ToString();
    }

    int SumWindow(Span<string> window)
    {
        int total = 0;

        foreach (var val in window)
        {
            total += int.Parse(val);
        }

        return total;
    }
}