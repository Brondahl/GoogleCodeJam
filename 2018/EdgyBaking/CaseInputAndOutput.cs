using static System.Math;

namespace EdgyBaking
{
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(string[] lines)
    {
      var NPline = lines[0].Split(' ');
      N = int.Parse(NPline[0]);
      P = int.Parse(NPline[1]);

      Cookies = new Cookie[N];

      for (int i = 0; i < N; i++)
      {
        Cookies[i] = new Cookie(lines[i+1]);
      }
    }

    internal int N;
    internal int P;
    internal Cookie[] Cookies;
  }

  class Cookie
  {
    public Cookie(string cookieLine)
    {
      var dimensions = cookieLine.Split(' ').Select(int.Parse).ToArray();
      W = dimensions[0];
      H = dimensions[1];
      basicPerimeter = 2 * (W + H);
      minAdditionalCutPerimeter = 2 * (Min(W, H));
      maxAdditionalCutPerimeter = 2*(Sqrt(W * W + H * H));
    }

    public int W;
    public int H;
    public int basicPerimeter;
    public int minAdditionalCutPerimeter;
    public double maxAdditionalCutPerimeter;
  }

  class CaseOutput
  {
    internal CaseOutput(int closestValue)
    {
      Closest = closestValue;
    }

    internal CaseOutput(double closestValue)
    {
      Closest = closestValue;
    }

    internal double Closest;

    public override string ToString()
    {
      return Closest.ToString();
    }
  }

}
