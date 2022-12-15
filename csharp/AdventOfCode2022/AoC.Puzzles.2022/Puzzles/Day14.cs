using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.ComponentModel.Design;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

namespace AoC.Puzzles._2022.Puzzles;

[Puzzle(2022, 14, "Regolith Reservoir")]
[SupportedOSPlatform("windows")]
public class Day14 : IPuzzle<string[]>
{
    char[,] _buffer = new char[5000, 5000];

    public Day14()
    {
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split($"{Environment.NewLine}").Where(x => x.Length > 0).ToArray();
    }

    public string Part1(string[] input)
    {
        ClearBuffer();
        int sandUnits = 0;
        var sandpos = (row: 0, col: 500);

        _buffer[sandpos.row, sandpos.col] = 'x';

        foreach (var line in input)
        {
            var tokens = line.Split(" -> ");
            var currentStart = tokens[0];

            foreach (var token in tokens)
            {
                if (token == currentStart)
                {
                    continue;
                }

                var parts = currentStart.Split(",");
                (int scol, int srow) = (int.Parse(parts[0]), int.Parse(parts[1]));

                parts = token.Split(",");
                (int dcol, int drow) = (int.Parse(parts[0]), int.Parse(parts[1]));

                DrawLine((scol, srow), (dcol, drow));
                int maxrow = Math.Max(srow, drow);
                currentStart = token;
            }
        }

        while (true)
        {
            //Render();

            if (sandpos.row + 1 >= _buffer.GetLength(0))
            {
                break;
            }

            char below = _buffer[sandpos.row + 1, sandpos.col];
            char belowLeft = _buffer[sandpos.row + 1, sandpos.col - 1];
            char belowRight = _buffer[sandpos.row + 1, sandpos.col + 1];
            if (below == '.')
            {
                sandpos = (sandpos.row + 1, sandpos.col);

                _buffer[sandpos.row - 1, sandpos.col] = '.';
                _buffer[sandpos.row, sandpos.col] = 'x';
            }
            else if (belowLeft == '.')
            {
                sandpos = (sandpos.row + 1, sandpos.col - 1);
                _buffer[sandpos.row - 1, sandpos.col + 1] = '.';
                _buffer[sandpos.row, sandpos.col] = 'x';

            }
            else if (belowRight == '.')
            {
                sandpos = (sandpos.row + 1, sandpos.col + 1);
                _buffer[sandpos.row -1, sandpos.col - 1] = '.';
                _buffer[sandpos.row, sandpos.col] = 'x';
            }
            else
            {
                _buffer[sandpos.row, sandpos.col] = 'o';
                sandUnits++;
                sandpos = (row: 0, col: 500);
                _buffer[sandpos.row, sandpos.col] = 'x';
            }
        }

        return sandUnits.ToString();
    }

    public string Part2(string[] input)
    {
        ClearBuffer();
        int sandUnits = 0;
        var sandpos = (row: 0, col: 500);

        _buffer[sandpos.row, sandpos.col] = 'x';
        int maxfloor = 0;

        foreach (var line in input)
        {
            var tokens = line.Split(" -> ");
            var currentStart = tokens[0];

            foreach (var token in tokens)
            {
                if (token == currentStart)
                {
                    continue;
                }

                var parts = currentStart.Split(",");
                (int scol, int srow) = (int.Parse(parts[0]), int.Parse(parts[1]));

                parts = token.Split(",");
                (int dcol, int drow) = (int.Parse(parts[0]), int.Parse(parts[1]));

                DrawLine((scol, srow), (dcol, drow));
                int maxrow = Math.Max(srow, drow);
                maxfloor = Math.Max(maxfloor, maxrow);
                currentStart = token;
            }
        }

        maxfloor += 2;

        DrawLine((0, maxfloor), (_buffer.GetLength(1) - 1, maxfloor));

        while (true)
        {
            //Render();

            char below = _buffer[sandpos.row + 1, sandpos.col];
            char belowLeft = _buffer[sandpos.row + 1, sandpos.col - 1];
            char belowRight = _buffer[sandpos.row + 1, sandpos.col + 1];
            if (below == '.')
            {
                sandpos = (sandpos.row + 1, sandpos.col);

                _buffer[sandpos.row - 1, sandpos.col] = '.';
                _buffer[sandpos.row, sandpos.col] = 'x';
            }
            else if (belowLeft == '.')
            {
                sandpos = (sandpos.row + 1, sandpos.col - 1);
                _buffer[sandpos.row - 1, sandpos.col + 1] = '.';
                _buffer[sandpos.row, sandpos.col] = 'x';

            }
            else if (belowRight == '.')
            {
                sandpos = (sandpos.row + 1, sandpos.col + 1);
                _buffer[sandpos.row - 1, sandpos.col - 1] = '.';
                _buffer[sandpos.row, sandpos.col] = 'x';
            }
            else
            {
                _buffer[sandpos.row, sandpos.col] = 'o';
                sandUnits++;
                if (sandpos.col == 500 && sandpos.row == 0)
                {
                    break;
                }

                sandpos = (row: 0, col: 500);
                _buffer[sandpos.row, sandpos.col] = 'x';
            }
        }

        return sandUnits.ToString();
    }

    void DrawLine((int, int) src, (int, int) dest)
    {
        var (srcRow, srcCol) = src;
        var (destRow, destCol) = dest;

        var ydist = destRow - srcRow;
        var xdist = destCol - srcCol;

        if (Math.Abs(ydist) > 0)
        {
            for (int i = 0; i <= Math.Abs(ydist); i++)
            {
                var sign = ydist < 0 ? -1 : 1;
                _buffer[srcCol, srcRow + i * sign] = '#';
            }
        }

        if (Math.Abs(xdist) > 0)
        {
            for (int i = 0; i <= Math.Abs(xdist); i++)
            {
                var sign = xdist < 0 ? -1 : 1;
                _buffer[srcCol + i * sign, srcRow] = '#';
            }
        }
    }

    void ClearBuffer()
    {
        for (int i = 0; i <  _buffer.GetLength(0); i++)
        {
            for (int j = 0; j < _buffer.GetLength(1); j++)
            {
                _buffer[i, j] = '.';
            }
        }
    }

    void Render()
    {
        Thread.Sleep(20);
        var sb = new StringBuilder();
        for (int i = 0; i < _buffer.GetLength(1); i++)
        {
            for (int j = 0; j < _buffer.GetLength(0); j++)
            {
                if (_buffer[i, j] == 0)
                {
                    sb.Append('.');
                }
                else
                {
                    sb.Append(_buffer[i, j]);
                }
            }

            sb.Append(Environment.NewLine);
        }

        Console.Clear();
        Console.WriteLine(sb.ToString());
    }
}