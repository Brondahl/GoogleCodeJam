namespace PancakeStack
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(IEnumerable<string> linesIn)
    {
      var lines = linesIn.ToList();
      var NKline = lines.First();
      var split = NKline.Split(' ');
      N = long.Parse(split[0]);
      K = long.Parse(split[1]);

      PancakeHeights = new long[N];
      PancakeRadii = new long[N];
      PancakeSideAreaOverPi = new long[N];

      var nextNLines = lines.Skip(1).Take((int)N).ToArray();
      var pancakeData = nextNLines.Select(line =>
        line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()
      ).ToArray();

      for (int i = 0; i < N; i++)
      {
        PancakeRadii[i] = pancakeData[i][0];
        PancakeHeights[i] = pancakeData[i][1];
        PancakeSideAreaOverPi[i] = 2 * PancakeRadii[i] * PancakeHeights[i];
      }
    }

    //internal CaseInput(CaseInput baseCase)
    //{
    //  N = baseCase.N;
    //  R = baseCase.R;
    //  O = baseCase.O;
    //  Y = baseCase.Y;
    //  G = baseCase.G;
    //  B = baseCase.B;
    //  V = baseCase.V;
    //}

    internal long N;
    internal long K;
    internal long[] PancakeHeights;
    internal long[] PancakeRadii;
    internal long[] PancakeSideAreaOverPi;

  }

  class CaseOutput
  {
    internal CaseOutput(double area)
    {
      Area = area;
    }

    internal double Area;

    public override string ToString()
    {
      return Area.ToString("0.#######");
    }
  }

}
