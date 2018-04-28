namespace CruiseControl
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(IEnumerable<string> lines)
    {
      var DNline = lines.First();
      var split = DNline.Split(' ');
      D = int.Parse(split[0]);
      N = int.Parse(split[1]);

      HorsePositions = new long[N];
      HorseSpeeds = new long[N];

      var horseData = lines.Skip(1).Select(line =>
        line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()
      ).ToArray();

      for (int nn = 0; nn < N; nn++)
      {
        HorsePositions[nn] = horseData[nn][0];
        HorseSpeeds[nn] = horseData[nn][1];
      }
    }

    internal long D;
    internal long N;
    internal long[] HorsePositions;
    internal long[] HorseSpeeds;
  }

  class CaseOutput
  {
    internal CaseOutput(decimal speed)
    {
      Speed = speed;
    }

    internal decimal Speed;

    public override string ToString()
    {
      return Speed.ToString("0.#######");
    }
  }

}
