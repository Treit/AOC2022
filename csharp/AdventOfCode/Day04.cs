using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day04 : BaseDay
{
    Regex _re = new Regex(@"^(\d+)-(\d+),(\d+)-(\d+)", RegexOptions.Compiled);

    public override ValueTask<string> Solve_1()
    {
        using var sr = new StreamReader(InputFilePath);
        var total = 0;

        while (sr.ReadLine() is string line)
        {
            var m = _re.Match(line);

            if (!m.Success)
            {
                continue;
            }

            (int start, int end) first = (int.Parse(m.Result("$1")), int.Parse(m.Result("$2")));
            (int start, int end) second = (int.Parse(m.Result("$3")), int.Parse(m.Result("$4")));

            bool fullyContained = Contains(first, second) || Contains(second, first);

            if (fullyContained)
            {
                total++;
            }
        }

        return ValueTask.FromResult(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        using var sr = new StreamReader(InputFilePath);
        var total = 0;

        while (sr.ReadLine() is string line)
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

        return ValueTask.FromResult(total.ToString());
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
