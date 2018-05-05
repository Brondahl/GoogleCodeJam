namespace AntStack
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput()
    {
    }

    internal CaseInput(List<string> lines)
    {
      N = int.Parse(lines[0]);
      AntWeights = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
    }

    internal int N;
    internal long[] AntWeights;
  }

  class CaseOutput
  {
    internal CaseOutput(int maxCount)
    {
      this.maxCount = maxCount;
    }

    internal int maxCount;

    public override string ToString()
    {
      return maxCount == -1 ? "OK" : maxCount.ToString();
    }
  }

}
