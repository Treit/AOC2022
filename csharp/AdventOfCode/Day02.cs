using Spectre.Console;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    public Day02()
    {
    }

    public override ValueTask<string> Solve_1()
    {
        using var sr = new StreamReader(InputFilePath);
        var total = 0;

        while (sr.ReadLine() is string line)
        {
            if (line.Length < 3)
            {
                continue;
            }

            var theirs = line[0];
            var ours = line[2];

            total += ShapeScore(ours) + PlayRound(theirs, ours);
        }

        return ValueTask.FromResult(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        using var sr = new StreamReader(InputFilePath);
        var total = 0;

        while (sr.ReadLine() is string line)
        {
            if (line.Length < 3)
            {
                continue;
            }

            var theirs = line[0];
            var desiredOutcome = line[2];
            var getPlay = GetCorrectPlay(desiredOutcome);
            var ours = getPlay(theirs);

            total += ShapeScore(ours) + PlayRound(theirs, ours);
        }

        return ValueTask.FromResult(total.ToString());
    }

    private int ShapeScore(char shape)
    {
        return shape switch
        {
            'X' => 1,
            'Y' => 2,
            'Z' => 3,
            _ => throw new ArgumentOutOfRangeException("shape")
        };
    }

    private int PlayRound(char theirShape, char ourShape)
    {
        return (theirShape, ourShape) switch
        {
            ('A', 'X') => 3,
            ('A', 'Y') => 6,
            ('A', 'Z') => 0,
            ('B', 'X') => 0,
            ('B', 'Y') => 3,
            ('B', 'Z') => 6,
            ('C', 'X') => 6,
            ('C', 'Y') => 0,
            ('C', 'Z') => 3,
            _ => throw new ArgumentException($"Unexpected input: {(theirShape, ourShape)}")
        };
    }

    private char Lose(char theirShape)
    {
        return theirShape switch
        {
            'A' => 'Z',
            'B' => 'X',
            'C' => 'Y',
            _ => throw new ArgumentException($"Unexpected input: {theirShape}")
        };
    }

    private char Draw(char theirShape)
    {
        return theirShape switch
        {
            'A' => 'X',
            'B' => 'Y',
            'C' => 'Z',
            _ => throw new ArgumentException($"Unexpected input: {theirShape}")
        };
    }

    private char Win(char theirShape)
    {
        return theirShape switch
        {
            'A' => 'Y',
            'B' => 'Z',
            'C' => 'X',
            _ => throw new ArgumentException($"Unexpected input: {theirShape}")
        };
    }

    private Func<char, char> GetCorrectPlay(char desiredOutcome)
    {
        return desiredOutcome switch
        {
            'X' => Lose,
            'Y' => Draw,
            'Z' => Win,
            _ => throw new ArgumentException($"Unexpected input: {desiredOutcome}")
        };
    }
}
