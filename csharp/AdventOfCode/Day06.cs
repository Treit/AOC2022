namespace AdventOfCode;

public class Day06 : BaseDay
{
    string _input;
    HashSet<char> _set;

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);
        _set = new HashSet<char>();
    }
    public override ValueTask<string> Solve_1()
    {
        var window = _input.AsSpan(0, 4);
        int count = 0;

        while (true)
        {
            if (AllDifferent(window))
            {
                count += 4;
                break;
            }

            count++;

            window = _input.AsSpan(count, 4);
        }

        return ValueTask.FromResult(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var window = _input.AsSpan(0, 14);
        int count = 0;

        while (true)
        {
            if (AllDifferent(window))
            {
                count += 14;
                break;
            }

            count++;

            window = _input.AsSpan(count, 14);
        }

        return ValueTask.FromResult(count.ToString());
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
