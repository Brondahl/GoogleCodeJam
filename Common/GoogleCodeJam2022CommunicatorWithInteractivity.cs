namespace Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class GoogleCodeJam2022CommunicatorWithInteractivity : IGoogleCodeJamCommunicator, IGoogleCodeJamInteractiveCommunicator
  {
    public IEnumerable<string> ReadStringInput(out int numberOfCases)
    {
      var lines = ReadStringInputAsIterator();

      var firstLine = lines.Take(1).Single();
      numberOfCases = int.Parse(firstLine);

      return lines;
    }

    private IEnumerable<string> ReadStringInputAsIterator()
    {
      while (true)
      {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line)) { break; }
        yield return (line);
      }
    }

    public string ReadSingleStringInput()
    {
        return Console.ReadLine();
    }

    public long ReadSingleLongInput()
    {
        return long.Parse(ReadSingleStringInput());
    }

    public List<long> ReadSingleLineOfLongsInput()
    {
        return ReadSingleStringInput().Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
    }

    public List<string> ReadNextStringInputs(int linesToRead)
    {
        return ReadNextStringInputs_Internal(linesToRead).ToList();
    }

    private IEnumerable<string> ReadNextStringInputs_Internal(int linesToRead)
    {
        for (int i = 0; i < linesToRead; i++)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) { throw new Exception("Couldn't read expected number of lines"); }
            yield return (line);
        }
    }

    public void WriteSingleInteractiveOutput(string line)
    {
        WriteInteractiveOutput(new List<string>{line});
    }

    public void WriteInteractiveOutput(List<string> lines)
    {
        WriteOutput(lines);
        Console.Out.Flush();
    }

    public void WriteOutput(IEnumerable<string> lines)
    {
      foreach (var line in lines)
      {
        Console.WriteLine(line);
       }
    }
  }
}
