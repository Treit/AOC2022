using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 2, "Rock Paper Scissors")]
public class Day02 : IPuzzle<string[]>
{
    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine);
    }

    public string Part1(string[] input)
    {
        int total = 0;

        foreach (var line in input)
        {
            if (line.Length < 3)
            {
                continue;
            }

            var theirs = line[0];
            var ours = line[2];

            total += ShapeScore(ours) + PlayRound(theirs, ours);
        }

        return total.ToString();
    }

    public string Part2(string[] input)
    {
        var total = 0;

        foreach (var line in input)
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

        return total.ToString();
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