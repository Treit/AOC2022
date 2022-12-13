using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Text.RegularExpressions;

namespace AoC.Puzzles._2022.Puzzles;

class Monkey
{
    Queue<long> _items = new Queue<long>();
    static long _maxSeen = 0;

    public Monkey()
    {
        Op = "";
        OpValue = "";
    }

    public int Id { get; set; }

    public void ReceiveItem(long item)
    {
        _items.Enqueue(item);
    }

    public bool HasItems()
    {
        return _items.Count > 0;
    }

    public string Op
    {
        get;
        set;
    }

    public string OpValue
    {
        get;
        set;
    }

    public int Divisor
    {
        get;
        set;
    }

    public int TrueMonkeyTarget
    {
        get;
        set;
    }

    public int FalseMonkeyTarget
    {
        get;
        set;
    }

    public long Inspections
    {
        get;
        set;
    }

    public void PlayRound(List<Monkey> monkeys, bool superAnxious)
    {
        // To avoid the numbers getting way too huge, mod by all of the divisors multiplied
        // together. (Had to look at other people's answers to figure this out.)
        // So for our puzzle input it was 5 * 11 * 2 * 13 * 7 * 3 * 17 * 19 -> 9699690.
        // (This apparently works because all of the divisors are primes, otherwise you
        // would need to calculate the GCD, which when everything is prime is just all of
        // the values multiplied together.)
        int modby = monkeys.Aggregate(1, (accum, m) => accum *= m.Divisor);

        int startCount = _items.Count;

        for (int i = 0; i < startCount; i++)
        {
            var item = _items.Dequeue();

            Inspections++;

            if (Op == "+")
            {
                if (OpValue == "old")
                {
                    item = item + item;
                }
                else
                {
                    item = item + int.Parse(OpValue);
                }
            }
            else if (Op == "*")
            {
                if (OpValue == "old")
                {
                    item = item * item;
                }
                else
                {
                    item = item * int.Parse(OpValue);
                }
            }

            if (item > _maxSeen)
            {
                _maxSeen = item;
                Console.WriteLine(_maxSeen);
            }

            item = item % modby;

            if (!superAnxious)
            {
                item = item / 3;
            }

            if (item % Divisor == 0)
            {
                monkeys[TrueMonkeyTarget].ReceiveItem(item);
            }
            else
            {
                monkeys[FalseMonkeyTarget].ReceiveItem(item);
            }
        }
    }
}

[Puzzle(2022, 11, "Monkey in the Middle")]
public class Day11 : IPuzzle<string[]>
{
    public Day11()
    {
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine).Where(x => x.Length > 0).ToArray();
    }

    public string Part1(string[] input)
    {
        var monkeys = GetMonkeys(input);

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                var monkey = monkeys[j];
                monkey.PlayRound(monkeys, false);
            }
        }

        var sorted = monkeys.OrderByDescending(x => x.Inspections).ToArray();

        return (sorted[0].Inspections * sorted[1].Inspections).ToString();
    }

    public string Part2(string[] input)
    {
        var monkeys = GetMonkeys(input);

        for (int i = 0; i < 10_000; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                var monkey = monkeys[j];
                monkey.PlayRound(monkeys, superAnxious: true);
            }
        }

        var sorted = monkeys.OrderByDescending(x => x.Inspections).ToArray();

        return ((long)sorted[0].Inspections * (long)sorted[1].Inspections).ToString();
    }

    List<Monkey> GetMonkeys(string[] input)
    {
        var monkeys = new List<Monkey>();
        int currentMonkey = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(input[i]))
            {
                continue;
            }

            var m = Regex.Match(input[i], @"Monkey (\d+):");

            if (m.Success)
            {
                var monkey = new Monkey { Id = int.Parse(m.Result("$1")) };
                currentMonkey = monkey.Id;
                monkeys.Add(monkey);
                continue;
            }

            m = Regex.Match(input[i], "Starting items: (.+)$");
            if (m.Success)
            {
                foreach (var item in m.Result("$1").Replace(" ", "").Split(","))
                {
                    monkeys[currentMonkey].ReceiveItem(int.Parse(item));
                }

                continue;
            }

            m = Regex.Match(input[i], @"Operation: new = old ([*+]) (\d+|old)$");
            if (m.Success)
            {
                monkeys[currentMonkey].Op = m.Result("$1");
                monkeys[currentMonkey].OpValue = m.Result("$2");
                continue;
            }

            m = Regex.Match(input[i], @"divisible by (\d+)");
            if (m.Success)
            {
                monkeys[currentMonkey].Divisor = int.Parse(m.Result("$1"));
                continue;
            }

            m = Regex.Match(input[i], @"true: throw to monkey (\d+)");
            if (m.Success)
            {
                monkeys[currentMonkey].TrueMonkeyTarget = int.Parse(m.Result("$1"));
                continue;
            }

            m = Regex.Match(input[i], @"false: throw to monkey (\d+)");
            if (m.Success)
            {
                monkeys[currentMonkey].FalseMonkeyTarget = int.Parse(m.Result("$1"));
                continue;
            }
        }

        return monkeys;
    }
}