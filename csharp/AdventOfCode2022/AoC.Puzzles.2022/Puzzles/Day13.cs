using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Security.AccessControl;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 13, "Distress Signal")]
public class Day13 : IPuzzle<string[]>
{
    public Day13()
    {
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split($"{Environment.NewLine}{Environment.NewLine}").ToArray();
    }

    public string Part1(string[] input)
    {
        var goodIndexes = new List<int>();

        for (int i = 0; i < input.Length; i++)
        {
            var packetPair = input[i];
            var loc = packetPair.IndexOf(Environment.NewLine);
            var left = packetPair.Substring(0, loc);
            var right = packetPair.Substring(loc + Environment.NewLine.Length);
            var leftPacket = DecodePacket(left);
            var rightPacket = DecodePacket(right);

            if (Compare(leftPacket, rightPacket) <= 0)
            {
                goodIndexes.Add(i + 1);
            }
        }

        return goodIndexes.Sum().ToString();
    }

    public string Part2(string[] input)
    {
        var packets = new List<string>();

        for (int i = 0; i < input.Length; i++)
        {
            var packetPair = input[i];
            var loc = packetPair.IndexOf(Environment.NewLine);
            var left = packetPair.Substring(0, loc);
            var right = packetPair.Substring(loc + Environment.NewLine.Length);

            packets.Add(left);
            packets.Add(right);
        }

        packets.Add("[[2]]");
        packets.Add("[[6]]");

        packets = packets.OrderBy(x => x, Comparer<string>.Create((x, y) =>
        {
            return Compare(DecodePacket(x), DecodePacket(y));
        })).ToList();

        foreach (var  packet in packets)
        {
            Console.WriteLine(packet);
        }

        var idxA = packets.IndexOf("[[2]]") + 1;
        var idxB = packets.IndexOf("[[6]]") + 1;

        return (idxA * idxB).ToString();
    }

    public List<object> DecodePacket(string packet)
    {
        var result = new List<object>(); // :(
        var listStack = new Stack<(int, List<object>)>();
        var listId = -1;

        var currentInt = "";
        List<object>? currentList = null;

        foreach (char c in packet)
        {
            if (c == '[')
            {
                listId++;
                var newList = new List<object>();

                if (currentList != null)
                {
                    currentList.Add(newList);
                }

                currentList = newList;

                listStack.Push((listId, currentList));
            }

            if (c == ']')
            {
                if (currentInt != "")
                {
                    var val = int.Parse(currentInt);
                    currentList!.Add(val);
                    currentInt = "";
                }

                if (listStack.Count > 1)
                {
                    listStack.Pop();
                }

                (listId, currentList) = listStack.Peek();
            }

            if (c == ',')
            {
                if (currentInt != "")
                {
                    var val = int.Parse(currentInt);
                    currentList!.Add(val);
                    currentInt = "";
                }
            }

            if (char.IsDigit(c))
            {
                currentInt += c;
            }
        }

        return currentList!;
    }

    int Compare(object left, object right)
    {
        return (left, right) switch
        {
            (int l, int r) => l.CompareTo(r),
            (List<object> l, List<object> r) => CompareLists(l, r),
            (List<object> l, int r) => CompareLists(l, ListFromInt(r)),
            (int l, List<object> r) => CompareLists(ListFromInt(l), r),
            _ => 0
        };

        List<object> ListFromInt(int x)
        {
            var result = new List<object>();
            result.Add(x);
            return result;
        }
    }

    int CompareLists(List<object> leftPacket, List<object> rightPacket)
    {
        for (int i = 0; i < leftPacket.Count; i++)
        {
            if (i >= rightPacket.Count)
            {
                break;
            }

            var tmp = Compare(leftPacket[i], rightPacket[i]);

            if (tmp != 0)
            {
                return tmp;
            }
        }

        return leftPacket.Count.CompareTo(rightPacket.Count);
    }
}