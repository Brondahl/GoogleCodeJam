using System;
using System.Collections.Generic;
using System.Linq;

namespace Ratatouille
{
  class CaseInput
  {
    internal static string[][] SubDivideInput(IEnumerator<string> lines)
    {
      var output = new List<string[]>();

      while (lines.MoveNext())
      {
        var thisCase = new List<string>();

        var NPLine = lines.Current;
        var split = NPLine.Split(' ');
        var N = long.Parse(split[0]);

        thisCase.Add(NPLine);

        for (int i = 0; i < N+1; i++)
        {
          lines.MoveNext();
          thisCase.Add(lines.Current);
        }

        output.Add(thisCase.ToArray());
      }

      return output.ToArray();
    }

    internal CaseInput(IEnumerable<string> lines)
    {
      var NPLine = lines.First();
      var split = NPLine.Split(' ');
      N = int.Parse(split[0]);
      P = int.Parse(split[1]);

      Recipe = lines.Skip(1).First().Split(' ').Select(int.Parse).ToArray();
      Packages = lines.Skip(2).Select(row => row.Split(' ').Select(int.Parse).ToArray()).ToArray();
    }

    internal int N;
    internal int P;
    internal int[] Recipe; //N ingredients,
    internal int[][] Packages; //N collections of P packages
  }

  class CaseOutput
  {
    internal CaseOutput(int result)
    {
      Result = result;
    }

    internal int Result;

    public override string ToString()
    {
      return Result == -1 ? "IMPOSSIBLE" : Result.ToString();
    }
  }

}
