using System;
using System.Collections.Generic;
using System.Linq;

namespace PonyExpress
{
  class CaseInput
  {
    internal CaseInput(IEnumerable<string> linesIn)
    {
      var lines = linesIn.ToList();
      var NQline = lines.First();
      var split = NQline.Split(' ');
      N = int.Parse(split[0]);
      Q = int.Parse(split[1]);

      HorseRanges = new long[N];
      HorseSpeeds = new long[N];

      var nextNLines = lines.Skip(1).Take(N).ToArray();
      var horseData = nextNLines.Select(line =>
        line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()
      ).ToArray();

      for (int i = 0; i < N; i++)
      {
        HorseRanges[i] = horseData[i][0];
        HorseSpeeds[i] = horseData[i][1];
      }


      CityDistances = new long[N][];
      LinearCityDistances = new long[N-1];


      nextNLines = lines.Skip(N+1).Take(N).ToArray();
      var cityData = nextNLines.Select(line =>
        line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()
      ).ToArray();

      for (int i = 0; i < N; i++)
      {
        CityDistances[i] = cityData[i];
        if (i != N - 1)
        {
          LinearCityDistances[i] = CityDistances[i][i + 1];
        }
      }

      //ParseQ stuff!
    }

    internal int N;
    internal int Q;
    internal long[] HorseRanges;
    internal long[] HorseSpeeds;
    internal long[][] CityDistances;
    internal long[] LinearCityDistances; //Entry N Stores distance from N to N+1
  }

  class CaseOutput
  {
    internal CaseOutput(decimal time)
    {
      Time = time;
    }

    internal decimal Time;

    public override string ToString()
    {
      return Time.ToString("0.#######");
    }
  }

}
