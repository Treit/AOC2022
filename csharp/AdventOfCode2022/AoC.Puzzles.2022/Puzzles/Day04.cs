using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Text.RegularExpressions;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 4, "Camp cleanup")]
public class Day04 : IPuzzle<string[]>
{
    Regex _re = new Regex(@"^(\d+)-(\d+),(\d+)-(\d+)", RegexOptions.Compiled);

    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine);
    }

    public string Part1(string[] input)
    {
        var total = 0;

        foreach (var line in input)
        {
            var m = _re.Match(line);

            if (!m.Success)
            {
                continue;
            }

            (int Start, int End) first = (int.Parse(m.Result("$1")), int.Parse(m.Result("$2")));
            (int Start, int End) second = (int.Parse(m.Result("$3")), int.Parse(m.Result("$4")));

            bool fullyContained = Contains(first, second) || Contains(second, first);

            if (fullyContained)
            {
                total++;
            }
        }

        return total.ToString();
    }

    public string Part2(string[] input)
    {
        var total = 0;

        foreach (var line in input)
        {
            var m = _re.Match(line);

            if (!m.Success)
            {
                continue;
            }

            (int Start, int End) first = (int.Parse(m.Result("$1")), int.Parse(m.Result("$2")));
            (int Start, int End) second = (int.Parse(m.Result("$3")), int.Parse(m.Result("$4")));

            if (Overlaps(first, second) || Overlaps(second, first))
            {
                total++;
            }
        }

        return total.ToString();
    }
    private bool Overlaps((int Start, int End) first, (int Start, int End) second)
    {
        if (second.Start > first.End || (second.Start < first.Start && second.End < first.Start))
        {
            return false;
        }

        return true;
    }

    bool Contains((int Start, int End) first, (int Start, int End) second)
    {
        if (second.Start >= first.Start
            && second.Start <= first.End
            && second.End >= first.Start
            && second.End <= first.End)
        {
            return true;
        }

        return false;
    }
}