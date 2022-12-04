using Spectre.Console;
using System.Collections;

namespace AdventOfCode;

public class Day03 : BaseDay
{
    public Day03()
    {
    }

    public override ValueTask<string> Solve_1()
    {
        using var sr = new StreamReader(InputFilePath);
        var total = 0;

        // 5 extra bits for characters between 'Z' and 'a' ¯\_(ツ)_/¯
        var items = new BitArray(58);

        while (sr.ReadLine() is string line)
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

                    // Ensure we only consider the priority of a given item type once.
                    items[idx] = false;
                    break;
                }
            }
        }

        return ValueTask.FromResult(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        using var sr = new StreamReader(InputFilePath);
        var total = 0;

        // 5 extra bits for characters between 'Z' and 'a' ¯\_(ツ)_/¯
        var itemGroup = new BitArray[2];
        itemGroup[0] = new BitArray(58);
        itemGroup[1] = new BitArray(58);
        int counter = 0;

        while (sr.ReadLine() is string line)
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

                        // Ensure we only consider the priority of a given item type once.
                        // Probalby unnecessary.
                        itemGroup[0][idx] = false;
                        itemGroup[1][idx] = false;
                        break;
                    }
                }
            }

            counter++;
        }

        return ValueTask.FromResult(total.ToString());
    }

    int GetPriority(char c)
    {
        if (char.IsLower(c))
        {
            return c - 96;
        }

        return c - 38;
    }
}
