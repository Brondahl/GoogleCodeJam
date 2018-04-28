namespace Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class GoogleCodeJam2018Communicator : IGoogleCodeJamCommunicator
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

    public void WriteOutput(IEnumerable<string> lines)
    {
      foreach (var line in lines)
      {
        Console.WriteLine(line);
      }
    }
  }
}
