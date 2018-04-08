using System;
using System.Collections.Generic;
using System.Linq;

namespace RainbowUnicorns
{
  class CaseInput
  {
    internal CaseInput(string line)
    {
      var split = line.Split(' ');
      N = int.Parse(split[0]);
      R = int.Parse(split[1]);
      O = int.Parse(split[2]);
      Y = int.Parse(split[3]);
      G = int.Parse(split[4]);
      B = int.Parse(split[5]);
      V = int.Parse(split[6]);
    }

    internal CaseInput(CaseInput baseCase)
    {
      N = baseCase.N;
      R = baseCase.R;
      O = baseCase.O;
      Y = baseCase.Y;
      G = baseCase.G;
      B = baseCase.B;
      V = baseCase.V;
    }

    internal int N;
    internal int R;
    internal int O;
    internal int Y;
    internal int G;
    internal int B;
    internal int V;
  }

  class CaseOutput
  {
    internal CaseOutput()
    {
      Arrangement = null;
    }

    internal string Arrangement = null;

    public override string ToString()
    {
      return Arrangement ?? "IMPOSSIBLE";
    }
  }

}
