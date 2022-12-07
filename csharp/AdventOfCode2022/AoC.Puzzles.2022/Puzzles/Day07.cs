using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Text.RegularExpressions;

namespace AoC.Puzzles._2022.Puzzles;

record File(string Name, int Size);

record Directory(string Name, IList<File> Files, IList<Directory> Directories);

record DirStat(Directory Directory, long TotalSize);

[Puzzle(2022, 7, "No Space Left On Device")]
public class Day07 : IPuzzle<string[]>
{
    public string[] Parse(string inputText)
    {
        return inputText.Split(Environment.NewLine);
    }

    public string Part1(string[] input)
    {
        var total = 0L;
        var root = BuildFileSystemHierarchy(input);
        var dirstats = GetDirStats(root);

        foreach (var stat in dirstats)
        {
            if (stat.TotalSize < 100_000)
            {
                total += stat.TotalSize;
            }
        }

        return total.ToString();
    }

    public string Part2(string[] input)
    {
        var totalDiskSize = 70_000_000;
        var required = 30_000_000;
        var root = BuildFileSystemHierarchy(input);
        var currentFree = totalDiskSize - TotalFileSize(root);
        var needed = required - currentFree;

        var answer = GetDirStats(root)
            .Where(x => x.TotalSize > needed)
            .OrderBy(x => x.TotalSize)
            .First();

        return answer.TotalSize.ToString();
    }

    static long TotalFileSize(Directory dir)
    {
        var total = 0L;

        foreach (var file in dir.Files)
        {
            total += file.Size;
        }

        foreach (var subdir in dir.Directories)
        {
            total += TotalFileSize(subdir);
        }

        return total;
    }

    static Directory BuildFileSystemHierarchy(string[] input)
    {
        var rootDir = new Directory("/", new List<File>(), new List<Directory>());
        var currentDir = rootDir;

        var dirstack = new Stack<Directory>();
        dirstack.Push(currentDir);

        foreach (var str in input)
        {
            if (str.StartsWith("$ cd "))
            {
                var dirName = str.Substring(5);
                if (dirName == "..")
                {
                    currentDir = dirstack.Pop();
                }
                else
                {
                    if (dirName == "/")
                    {
                        continue;
                    }

                    var matchingDir = currentDir!.Directories.First(x => x.Name == dirName);

                    dirstack.Push(currentDir!);
                    currentDir = matchingDir;
                }
            }
            else if (str == "$ ls")
            {
                continue;
            }
            else if (str.StartsWith("dir "))
            {
                var subdirName = str.Substring(4);
                var subdir = new Directory(subdirName, new List<File>(), new List<Directory>());
                currentDir?.Directories.Add(subdir);
            }
            else
            {
                var m = Regex.Match(str, @"^(\d+) (.+)$");
                if (!m.Success)
                {
                    continue;
                }

                var file = new File(m.Result("$2"), int.Parse(m.Result("$1")));
                currentDir?.Files.Add(file);
            }
        }

        return rootDir;
    }

    static IList<DirStat> GetDirStats(Directory root)
    {
        var dirstats = new List<DirStat>();
        var dirs = new Stack<Directory>();
        dirs.Push(root);

        while (dirs.Count > 0)
        {
            var current = dirs.Pop();
            dirstats.Add(new DirStat(current, TotalFileSize(current)));

            foreach (var subdir in current.Directories)
            {
                dirs.Push(subdir);
            }
        }

        return dirstats;
    }
}