namespace RobotProgrammingStrategy
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  class CaseInput
  {
    internal CaseInput(List<string> inputLines)
    {
      N = int.Parse(inputLines[0]);

      Lines = inputLines.Skip(1).ToArray();
      LinesLengths = Lines.Select(line => line.Length).ToArray();
      MaxSingleLength = LinesLengths.Max();
    }

    internal int N;
    internal string[] Lines;
    internal int[] LinesLengths;
    internal int MaxSingleLength;
  }

  class CaseOutput
  {
    internal CaseOutput(bool possible)
    {
      if (possible) { Thrower.TriggerMemLimit(); }
      IsPossible = possible;
    }

    internal CaseOutput(string solution)
    {
      IsPossible = true;
      Solution = solution;
    }

    internal bool IsPossible;
    internal string Solution;

    public override string ToString()
    {
      return !IsPossible ? "IMPOSSIBLE" : Solution;
    }
  }

}
