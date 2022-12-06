using System.Text;
using System.Text.RegularExpressions;
using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2022.Puzzles;

record struct ElfStack(char StackId, int Column, Stack<char> Stack);
record struct MoveInstruction(int Count, int Source, int Dest);

[Puzzle(2022, 5, "Crane operator")]
public class Day05 : IPuzzle<string[]>
{
    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine);
    }

    public string Part1(string[] input)
    {
        var stacks = GetStartingStacks(input);

        PrintStacks(stacks);

        foreach (var mi in GetMoveInstructions(input))
        {
            MoveItems(mi, stacks);
        }

        var sb = new StringBuilder();
        foreach (var stack in stacks)
        {
            sb.Append(stack.Stack.Peek());
        }

        PrintStacks(stacks);

        return sb.ToString();
    }

    public string Part2(string[] input)
    {
        var stacks = GetStartingStacks(input);

        PrintStacks(stacks);

        foreach (var mi in GetMoveInstructions(input))
        {
            MoveItems(mi, stacks, together: true);
        }

        var sb = new StringBuilder();
        foreach (var stack in stacks)
        {
            sb.Append(stack.Stack.Peek());
        }

        PrintStacks(stacks);

        return sb.ToString();
    }

    IEnumerable<MoveInstruction> GetMoveInstructions(string[] input)
    {
        foreach (var line in input)
        {
            var m = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
            if (!m.Success)
            {
                continue;
            }

            yield return new MoveInstruction(
                int.Parse(m.Result("$1")),
                int.Parse(m.Result("$2")),
                int.Parse(m.Result("$3")));
        }
    }

    IList<ElfStack> GetStartingStacks(string[] input)
    {
        var result = new List<ElfStack>();
        var stackInfo = string.Empty;
        var stackLines = new Stack<string>();

        foreach (var line in input)
        {
            if (Regex.IsMatch(line, @"^\s+\d"))
            {
                stackInfo = line;
                break;
            }
            else
            {
                stackLines.Push(line);
            }
        }

        for (int i = 0; i < stackInfo.Length; i++)
        {
            if (char.IsDigit(stackInfo[i]))
            {
                result.Add(new ElfStack { StackId = stackInfo[i], Column = i, Stack = new Stack<char>() });
            }
        }

        while (stackLines.Count > 0)
        {
            var line = stackLines.Pop();
            for (int i = 0; i < result.Count; i++)
            {
                var c = line[result[i].Column];
                if (!char.IsLetterOrDigit(c))
                {
                    continue;
                }

                result[i].Stack.Push(c);
            }
        }

        return result;
    }

    static void MoveItems(MoveInstruction mi, IList<ElfStack> stacks, bool together = false)
    {
        int fromIndex = mi.Source - 1;
        int toIndex = mi.Dest - 1;

        if (together)
        {
            var tempStack = new Stack<char>();
            for (int i = 0; i < mi.Count; i++)
            {
                tempStack.Push(stacks[fromIndex].Stack.Pop());
            }

            while (tempStack.Count > 0)
            {
                stacks[toIndex].Stack.Push(tempStack.Pop());
            }
        }
        else
        {
            for (int i = 0; i < mi.Count; i++)
            {
                var c = stacks[fromIndex].Stack.Pop();
                stacks[toIndex].Stack.Push(c);
            }
        }
    }

    static IList<ElfStack> CloneElfStacks(IEnumerable<ElfStack> source)
    {
        var result = new List<ElfStack>();

        foreach (var stack in source)
        {
            var tmp = new ElfStack(stack.StackId, stack.Column, new Stack<char>(new Stack<char>(stack.Stack)));
            result.Add(tmp);
        }

        return result;
    }

    static void PrintStacks(IEnumerable<ElfStack> input)
    {
        int level = 0;

        var stacks = CloneElfStacks(input);

        foreach (var stack in stacks)
        {
            level = Math.Max(level, stack.Stack.Count);
        }

        while (level > 0)
        {
            Console.WriteLine();

            foreach (var stack in stacks)
            {
                if (stack.Stack.Count == level)
                {
                    var current = Console.GetCursorPosition();
                    Console.SetCursorPosition(stack.Column, current.Top);
                    Console.Write($"[{stack.Stack.Pop()}]");
                }
            }

            level--;
        }

        Console.WriteLine();
    }
}