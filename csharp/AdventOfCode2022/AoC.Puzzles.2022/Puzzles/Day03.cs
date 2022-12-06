using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Collections;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 3, "Backpacks")]
public class Day03 : IPuzzle<string[]>
{
    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine);
    }

    public string Part1(string[] input)
    {
        var total = 0;

        // 5 extra bits for characters between 'Z' and 'a' ¯\_(ツ)_/¯
        var items = new BitArray(58);

        foreach (var line in input)
        {
            if (line.Length < 2)
            {
                continue;
            }

            items.SetAll(false);
            var mid = line.Length / 2;
            var compartmentA = line.AsSpan(0, mid);
            var compartmentB = line.AsSpan(mid);

            foreach (var item in compartmentA)
            {
                var idx = item - 65;
                items[idx] = true;
            }

            foreach (var item in compartmentB)
            {
                var idx = item - 65;
                if (items[idx])
                {
                    total += GetPriority(item);
                    break;
                }
            }
        }

        return total.ToString();
    }

    public string Part2(string[] input)
    {
        var total = 0;

        // 5 extra bits for characters between 'Z' and 'a' ¯\_(ツ)_/¯
        var itemGroup = new BitArray[2];
        itemGroup[0] = new BitArray(58);
        itemGroup[1] = new BitArray(58);
        int counter = 0;

        foreach (var line in input)
        {
            if (line.Length < 2)
            {
                continue;
            }

            if (counter == 3)
            {
                // New group
                foreach (var bitArray in itemGroup)
                {
                    bitArray.SetAll(false);
                }
                counter = 0;
            }

            if (counter < 2)
            {
                foreach (var item in line)
                {
                    var idx = item - 65;
                    itemGroup[counter][idx] = true;
                }
            }

            if (counter == 2)
            {
                // Last in group.
                foreach (var item in line)
                {
                    var idx = item - 65;

                    if (itemGroup[0][idx] && itemGroup[1][idx])
                    {
                        total += GetPriority(item);
                        break;
                    }
                }
            }

            counter++;
        }

        return total.ToString();
    }

    int GetPriority(char c)
    {
        return char.IsLower(c) ? c - 96 : c - 38;
    }
}