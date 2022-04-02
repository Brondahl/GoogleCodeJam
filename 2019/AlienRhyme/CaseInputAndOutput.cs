namespace AlienRhyme
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      N = int.Parse(lines[0]);

      //ReversedWords = lines.Skip(1).Select(word => new string(word.Reverse().ToArray())).ToArray();
      ReversedCharArrays = lines.Skip(1).Select(word => word.ToCharArray().Reverse().ToArray()).ToArray();
      var lengths = ReversedCharArrays.Select(arr => arr.Length).ToList();
      ShortestWord = lengths.Min();
      LongestWord = lengths.Max();
    }

    internal int N;
    internal int ShortestWord;
    internal int LongestWord;
    internal char[][] ReversedCharArrays;
    //internal string[] ReversedWords;
  }

  class CaseOutput
  {
    internal CaseOutput(int pairs)
    {
      Pairs = pairs;
    }

    internal int Pairs;

    public override string ToString()
    {
      return Pairs.ToString();
    }
  }

}
