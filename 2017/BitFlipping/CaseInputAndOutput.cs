using System.Linq;

namespace BitFlipping
{
  class CaseInput
  {
    internal CaseInput(string line)
    {
      var split = line.Split(' ');
      SequenceLength = split[0].Length;
      Sequence = split[0].ToCharArray().Select(symbol => symbol == '+').ToArray();
      FlipSize = int.Parse(split[1]);
    }

    internal int SequenceLength;
    internal int FlipSize;
    internal bool[] Sequence;
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
