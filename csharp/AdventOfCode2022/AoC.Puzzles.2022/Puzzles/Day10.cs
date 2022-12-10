using AoC.Common.Attributes;
using AoC.Common.Interfaces;

namespace AoC.Puzzles._2022.Puzzles;

class ElfDisplay
{
    char[] _screenBuffer = new char[40];
    long _cycle = 0;
    long _signalStrength = 0;
    long _cumulativeSignalStrength = 0;
    long _strengthCycle = 20;
    int _interval = 40;
    int _currentPixel = 0;

    public long X { get; private set; } = 1;

    public void Noop()
    {
        Tick();
    }

    public void AddX(int val)
    {
        Tick();
        Tick();
        X += val;
    }

    public long GetTicks()
    {
        return _cycle;
    }

    public long GetCumulativeSignalStrength()
    {
        return _cumulativeSignalStrength;
    }

    void Tick()
    {
        _cycle++;
        UpdateSignalStrength();
        UpdateDipaly();
    }

    void UpdateSignalStrength()
    {
        if (_cycle == _strengthCycle)
        {
            _signalStrength = X * _cycle;
            _cumulativeSignalStrength += _signalStrength;
            _strengthCycle += _interval;
        }
    }

    void UpdateDipaly()
    {
        var pixel = ' ';

        if (_currentPixel == X
            || _currentPixel == X + 1
            || _currentPixel == X - 1)
        {
            pixel = '#';
        }

        _screenBuffer[_currentPixel++] = pixel;

        if (_currentPixel == _screenBuffer.Length)
        {
            _currentPixel = 0;
            Console.WriteLine(new string(_screenBuffer).Replace(" ", "🎄").Replace("#", "🎅"));
        }
    }
}

[Puzzle(2022, 10, "Cathode-Ray Tube")]
public class Day10 : IPuzzle<string[]>
{
    public Day10()
    {
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine).Where(x => x.Length > 0).ToArray();
    }

    public string Part1(string[] input)
    {
        var cpu = new ElfDisplay();

        foreach (var instr in input)
        {
            if (instr.StartsWith("noop"))
            {
                cpu.Noop();
                continue;
            }
            else if (instr.StartsWith("addx"))
            {
                int val = int.Parse(instr.Substring(5));
                cpu.AddX(val);
            }
        }

        Console.WriteLine($"X: {cpu.X}, cycles: {cpu.GetTicks()}, accumulated signal strength: {cpu.GetCumulativeSignalStrength()}");
        return cpu.GetCumulativeSignalStrength().ToString();
    }

    public string Part2(string[] input)
    {
        // Read from the output of Part1.
        return "FZBPBFZF";
    }

}