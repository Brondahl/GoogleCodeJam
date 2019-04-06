namespace Cryptopangrams
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      N = int.Parse(lines[0]);
      LongestSubList = (N+1) / 2;
      IsEven = (N % 2 == 0);

      var V = lines[1].Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

      if (N == 0)
      {
        EvenV = new int[0];
        OddV = new int[0];
        LongestSubList = 0;
        IsEven = true;
        return;
      }

      if (N == 1)
      {
        EvenV = new int[]{V.Single()};
        OddV = new int[0];
        LongestSubList = 1;
        IsEven = false;
        return;
      }

      var evens = new List<int>();
      var odds = new List<int>();
      for (int i = 0; i < N - 1; i++)
      {
        evens.Add(V[i]);
        i++;
        odds.Add(V[i]);
      }

      if (!IsEven)
      {
        evens.Add(V.Last());
      }

      EvenV = evens.OrderBy(x => x).ToArray();
      OddV = odds.OrderBy(x => x).ToArray();
    }

    internal int N;
    internal bool IsEven;
    internal int LongestSubList;
    internal int[] EvenV;
    internal int[] OddV;
  }

  class CaseOutput
  {
    internal CaseOutput(int errorIndex)
    {
      ErrorIndex = errorIndex;
    }

    internal int ErrorIndex;

    public override string ToString()
    {
      return ErrorIndex == -1 ? "OK" : ErrorIndex.ToString();
    }
  }

}
