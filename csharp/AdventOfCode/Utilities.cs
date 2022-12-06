namespace AdventOfCode
{
    internal class Utilities
    {
        public static IEnumerable<string> GetInput(BaseDay day)
        {
            using var sr = new StreamReader(day.InputFilePath);

            while (sr.ReadLine() is string line)
            {
                yield return line;
            }

        }
    }
}
