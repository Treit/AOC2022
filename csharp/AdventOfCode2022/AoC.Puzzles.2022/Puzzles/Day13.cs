using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2022.Puzzles;

public record PacketItem
{
    public PacketItem(int id)
    {
        Id = id;
        List = new List<PacketItem>();
        Values = new List<int>();
    }
    public int Id { get; init; }
    public List<PacketItem> List { get; init; }
    public List<int> Values { get; init; }
}

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

            if (Compare(leftPacket, rightPacket) is true)
            {
                goodIndexes.Add(i + 1);
            }
        }

        return goodIndexes.Sum().ToString();
    }

    public string Part2(string[] input)
    {
        return "";
    }

    public List<object> DecodePacket(string packet)
    {
        var result = new List<object>(); // :(
        Console.WriteLine(packet);
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
                    Console.WriteLine($"Start list {listId}");
                    Console.WriteLine($"Adding list {listId} to list {listId - 1}");
                    currentList.Add(newList);
                }
                else
                {
                    Console.WriteLine($"Start list {listId}");
                }

                currentList = newList;

                listStack.Push((listId, currentList));
            }

            if (c == ']')
            {
                if (currentInt != "")
                {
                    var val = int.Parse(currentInt);
                    Console.WriteLine($"Adding {val} to list {listId}");
                    currentList!.Add(val);
                    currentInt = "";
                }

                if (listStack.Count > 1)
                {
                    listStack.Pop();
                }

                Console.WriteLine($"End list {listId}");

                (listId, currentList) = listStack.Peek();
            }

            if (c == ',')
            {
                if (currentInt != "")
                {
                    var val = int.Parse(currentInt);
                    Console.WriteLine($"Adding {val} to list {listId}");
                    currentList!.Add(val);
                    currentInt = "";
                }
            }

            if (char.IsDigit(c))
            {
                currentInt += c;
            }
        }

        Console.WriteLine("--------------------------------");

        return currentList!;
    }

    bool? Compare(object left, object right)
    {
        if (left is int x && right is int y)
        {
            if (x < y)
            {
                return true;
            }
            else if (x > y)
            {
                return false;
            }

            return null;
        }

        if (left is List<object> ll && right is List<object> lr)
        {
            return CompareLists(ll, lr);
        }

        if (left is int il && right is List<object> lr2)
        {
            return CompareLists(ListFromInt(il), lr2);
        }

        if (left is List<object> ll2 && right is int ir)
        {
            return CompareLists(ll2, ListFromInt(ir));
        }

        return null;

        List<object> ListFromInt(int x)
        {
            var result = new List<object>();
            result.Add(x);
            return result;
        }
    }

    bool? CompareLists(List<object> leftPacket, List<object> rightPacket)
    {
        if (leftPacket.Count == 0 || rightPacket.Count == 0)
        {
            if (leftPacket.Count > rightPacket.Count)
            {
                return false;
            }

            return true;
        }

        for (int i = 0; i < leftPacket.Count; i++)
        {
            var tmp = Compare(leftPacket[i], rightPacket[i]);

            if (tmp != null)
            {
                return tmp;
            }

            if (i + 1 == leftPacket.Count)
            {
                return true;
            }
            if (i + 1 == rightPacket.Count)
            {
                return false;
            }
        }

        return null;
    }

    public PacketItem DecodePacketOld(string packet)
    {
        Console.WriteLine(packet);
        var listStack = new Stack<PacketItem>();
        var lists = 0;

        var currentInt = "";
        PacketItem? currentList = null;

        foreach (char c in packet)
        {
            if (c == '[')
            {
                var newList = new PacketItem(lists++);

                if (currentList != null)
                {
                    Console.WriteLine($"Start list {newList.Id}");
                    Console.WriteLine($"Adding list {newList.Id} to list {currentList.Id}");
                    currentList.List.Add(newList);
                }
                else
                {
                    Console.WriteLine($"Start list {newList.Id}");
                }

                currentList = newList;

                listStack.Push(currentList);
            }

            if (c == ']')
            {
                if (currentInt != "")
                {
                    var val = int.Parse(currentInt);
                    Console.WriteLine($"Adding {val} to list {currentList!.Id}");
                    currentList.Values.Add(val);
                    currentInt = "";
                }

                if (listStack.Count > 1)
                {
                    listStack.Pop();
                }

                Console.WriteLine($"End list {currentList!.Id}");

                currentList = listStack.Peek();
            }

            if (c == ',')
            {
                if (currentInt != "")
                {
                    var val = int.Parse(currentInt);
                    Console.WriteLine($"Adding {val} to list {currentList!.Id}");
                    currentList.Values.Add(val);
                    currentInt = "";
                }
            }

            if (char.IsDigit(c))
            {
                currentInt += c;
            }
        }

        Console.WriteLine("--------------------------------");

        return currentList!;
    }
}