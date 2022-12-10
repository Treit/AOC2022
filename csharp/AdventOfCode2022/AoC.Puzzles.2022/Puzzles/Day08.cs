using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Text;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 8, "Treetop Tree House")]
public class Day08 : IPuzzle<string[]>
{
    // Coordinates and height
    Dictionary<(int, int), int> _visibleTrees;

    public Day08()
    {
        _visibleTrees = new Dictionary<(int, int), int>();
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine).Where(x => x.Length > 0).ToArray();
    }

    public string Part1(string[] input)
    {
        CountVisibleRight(input);
        CountVisibleLeft(input);
        CountVisibleDown(input);
        CountVisibleUp(input);

        return _visibleTrees.Count.ToString();
    }

    public string Part2(string[] input)
    {
        int maxScenicScore = 0;

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                var scenicScore = GetScenicScore(input, (i, j));

                maxScenicScore = Math.Max(maxScenicScore, scenicScore);
            }
        }

        return maxScenicScore.ToString();
    }

    void CountVisibleRight(string[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            int maxHeight = (int)char.GetNumericValue(input[i][0]);

            for (int j = 0; j < input[i].Length; j++)
            {
                int height = (int)char.GetNumericValue(input[i][j]);

                if (j == 0)
                {
                    // Left edge.
                    _visibleTrees[(i, j)] = height;
                    continue;
                }

                if (height > maxHeight)
                {
                    maxHeight = height;
                    _visibleTrees[(i, j)] = height;
                }
            }
        }
    }

    void CountVisibleLeft(string[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            int maxHeight = (int)char.GetNumericValue(input[i][input[i].Length - 1]);

            for (int j = input[i].Length - 1; j >= 0; j--)
            {
                int height = (int)char.GetNumericValue(input[i][j]);

                if (j == input[i].Length - 1)
                {
                    // Right edge.
                    _visibleTrees[(i, j)] = height;
                    continue;
                }

                if (height > maxHeight)
                {
                    maxHeight = height;
                    _visibleTrees[(i, j)] = height;
                }
            }
        }
    }

    void CountVisibleDown(string[] input)
    {
        for (int j = 0; j < input[0].Length; j++)
        {
            int maxHeight = (int)char.GetNumericValue(input[0][j]);

            for (int i = 0; i < input.Length; i++)
            {
                int height = (int)char.GetNumericValue(input[i][j]);

                if (i == 0)
                {
                    // Top edge.
                    _visibleTrees[(i, j)] = height;
                    continue;
                }

                if (height > maxHeight)
                {
                    maxHeight = height;
                    _visibleTrees[(i, j)] = height;
                }
            }
        }
    }

    void CountVisibleUp(string[] input)
    {
        for (int j = 0; j < input[0].Length; j++)
        {
            int maxHeight = (int)char.GetNumericValue(input[input.Length - 1][j]);

            for (int i = input.Length - 1; i >= 0; i--)
            {
                int height = (int)char.GetNumericValue(input[i][j]);

                if (i == input.Length - 1)
                {
                    // Bottom edge.
                    _visibleTrees[(i, j)] = height;
                    continue;
                }

                if (height > maxHeight)
                {
                    maxHeight = height;
                    _visibleTrees[(i, j)] = height;
                }
            }
        }
    }

    int GetScenicScore(string[] input, (int X, int Y) coords)
    {
        int up = ScenicScoreUp(input, coords);
        int left = ScenicScoreLeft(input, coords);
        int right = ScenicScoreRight(input, coords);
        int down = ScenicScoreDown(input, coords);

        int score = right * left * down * up;

        return score;
    }

    int ScenicScoreRight(string[] input, (int X, int Y) coords)
    {
        int score = 0;
        var current = input[coords.X];
        var height = (int)char.GetNumericValue(current[coords.Y]);

        for (int j = coords.Y + 1; j < current.Length; j++)
        {
            var otherHeight = (int)char.GetNumericValue(current[j]);

            score++;

            if (otherHeight >= height)
            {
                break;
            }
        }

        return score;
    }

    int ScenicScoreLeft(string[] input, (int X, int Y) coords)
    {
        int score = 0;
        var current = input[coords.X];
        var height = (int)char.GetNumericValue(current[coords.Y]);

        for (int j = coords.Y - 1; j >= 0; j--)
        {
            var otherHeight = (int)char.GetNumericValue(current[j]);

            score++;

            if (otherHeight >= height)
            {
                break;
            }

        }

        return score;
    }

    int ScenicScoreDown(string[] input, (int X, int Y) coords)
    {
        int score = 0;
        var current = input[coords.X];
        var height = (int)char.GetNumericValue(current[coords.Y]);
        for (int i = coords.X + 1; i < input.Length; i++)
        {
            var otherHeight = (int)char.GetNumericValue(input[i][coords.Y]);

            score++;

            if (otherHeight >= height)
            {
                break;
            }
        }

        return score;
    }

    int ScenicScoreUp(string[] input, (int X, int Y) coords)
    {
        int score = 0;
        var current = input[coords.X];
        var height = (int)char.GetNumericValue(current[coords.Y]);
        for (int i = coords.X - 1; i >= 0; i--)
        {
            var otherHeight = (int)char.GetNumericValue(input[i][coords.Y]);

            score++;

            if (otherHeight >= height)
            {
                break;
            }
        }

        return score;
    }

}