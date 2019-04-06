namespace Cryptopangrams
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      N = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).First();
      Numbers = lines[1].Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
    }

    internal int N;
    internal long[] Numbers;
  }

  class CaseOutput
  {
    public readonly string Decryption;

    internal CaseOutput(string decryption)
    {
      Decryption = decryption;
    }

    public override string ToString()
    {
      return Decryption;
    }
  }

}
