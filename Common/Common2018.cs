using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
  public class Common2018 : CommonBase
  {
    protected override string folderPath => @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\GoogleCodeJam2018\";

    public override string[] ReadStringInput(out int numberOfCases)
    {
      List<string> lines = new List<string>();

      while (true)
      {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line)) { break; }
        lines.Add(line);
      }
      numberOfCases = int.Parse(lines.First());
      return lines.Skip(1).ToArray();
    }

    public override void WriteOutput(IEnumerable<string> lines)
    {
      foreach (var line in lines)
      {
        Console.WriteLine(line);
      }
    }
  }
}
