using System.Collections.Generic;
using System.Text;
using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 6, "Tuning Trouble")]
public class Day06 : IPuzzle<string>
{
    HashSet<char> _set = new HashSet<char>();

    public string Parse(string inputText) => inputText;

    public string Part1(string input)
    {
        var window = input.AsSpan(0, 4);
        int count = 0;

        while (true)
        {
            if (AllDifferent(window))
            {
                count += 4;
                break;
            }

            count++;

            window = input.AsSpan(count, 4);
        }

        return count.ToString();
    }

    public string Part2(string input)
    {
        var window = input.AsSpan(0, 14);
        int count = 0;

        while (true)
        {
            if (AllDifferent(window))
            {
                count += 14;
                break;
            }

            count++;

            window = input.AsSpan(count, 14);
        }

        return count.ToString();
    }

    bool AllDifferent(ReadOnlySpan<char> span)
    {
        _set.Clear();

        foreach (char c in span)
        {
            _set.Add(c);
        }

        return _set.Count == span.Length;
    }
}