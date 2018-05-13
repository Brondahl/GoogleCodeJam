namespace MysteriousRoadSigns
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      S = int.Parse(lines[0]);

      Signs = lines.Skip(1).Select(line => new Sign(line)).ToArray();
    }

    internal int S;
    internal Sign[] Signs;
  }

  public class Sign
  {
    public override string ToString()
    {
      return $"Sign = {M},{N}";
    }

    public Sign(string line)
    {
      var array = line.Split(' ').Select(int.Parse).ToArray();

      D = array[0];
      A = array[1];
      B = array[2];

      M = D + A;
      N = D - B;
    }

    public Sign(int d, int a, int b)
    {
      D = d;
      A = a;
      B = b;

      M = D + A;
      N = D - B;
    }

    public int D;

    public int A;
    public int B;

    public int M;
    public int N;
  }

  class CaseOutput
  {
    internal CaseOutput(int largest, int count)
    {
      this.largest = largest;
      this.count = count;
    }

    internal int largest;
    internal int count;

    public override string ToString()
    {
      return $"{largest} {count}";
    }
  }

}
