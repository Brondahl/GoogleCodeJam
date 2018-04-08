using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeSplitting
{
  class CaseInput
  {
    internal static string[][] SubDivideInput(IEnumerator<string> lines)
    {
      var output = new List<string[]>();

      while (lines.MoveNext())
      {
        var thisCase = new List<string>();

        var RCline = lines.Current;
        var split = RCline.Split(' ');
        var R = long.Parse(split[0]);

        thisCase.Add(RCline);

        for (int i = 0; i < R; i++)
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
      var RCline = lines.First();
      var split = RCline.Split(' ');
      R = int.Parse(split[0]);
      C = int.Parse(split[1]);

      Grid = new char[R,C];
      LettersInRow = Enumerable.Repeat(0, R).ToArray();
      LetterLocationsInRow = new List<int>[R];

      var tempGrid = lines.Skip(1).Select(row => row.ToCharArray()).ToArray();
      for (int i = 0; i < R; i++)
      {
        LetterLocationsInRow[i] = new List<int>();
        for (int j = 0; j < C; j++)
        {
          var letter = tempGrid[i][j];
          Grid[i, j] = letter;
          if (letter != '?')
          {
            LetterLocationsInRow[i].Add(j);
          }
        }
        LettersInRow[i] = LetterLocationsInRow[i].Count();
      }
    }

    internal int R;
    internal int C;
    internal char[,] Grid;
    internal int[] LettersInRow;
    internal List<int>[] LetterLocationsInRow;
  }

  class CaseOutput
  {
    internal CaseOutput(Char[,] grid, int r, int c)
    {
      Grid = grid;
      R = r;
      C = c;
    }

    internal char[,] Grid;
    internal int R;
    internal int C;

    public override string ToString()
    {
      var ret = "";

      for (int i = 0; i < R; i++)
      {
        ret += Environment.NewLine;
        for (int j = 0; j < C; j++)
        {
          ret += Grid[i, j];
        }
      }

      return ret;
    }
  }

}
