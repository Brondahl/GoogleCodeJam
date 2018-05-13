namespace AWholeNewWord
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      var NLLine = lines[0];
      var NLArray = NLLine.Split(' ').Select(int.Parse).ToArray();
      NumberOfWords = NLArray[0];
      WordLength = NLArray[1];

      var Words = lines.Skip(1).ToArray();
      CharArrays = Words.Select(word => word.ToCharArray()).ToArray();
    }

    internal int NumberOfWords;
    internal int WordLength;
    internal char[][] CharArrays;
  }

  class CaseOutput
  {
    internal CaseOutput(string wordFound)
    {
      this.wordFound = wordFound;
    }

    internal string wordFound;

    public override string ToString()
    {
      return wordFound ?? "-";
    }
  }

}