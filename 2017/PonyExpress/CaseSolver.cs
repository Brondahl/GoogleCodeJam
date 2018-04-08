﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Common;

namespace PonyExpress
{
  /*
   * TODO:
   *   - Namespace
   *   - Copy in/out files.
   *   - References to Common and testing Frameworks
   *   - CodeJam Reference to here.
   *   - Program redirect to here.
   */
  public class CaseSolver
  {
    private static string subFolderName = @"PonyExpress";
    private static int numberOfCases;
    private static CommonBase Common = new Common2017(subFolderName);
    public static void Run()
    {
      var lines = Common.ReadStringInput(out numberOfCases).ToList();
      var cases = Common.CaseLineSplitter(lines, (lineCount, args) => { return lineCount < 2*args[0] + args[1]; }).ToArray();
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new CaseInput(cases[ii]);
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();

        var resultText = result.ToString();

        results.Add(string.Format("Case #{0}: {1}", ii + 1, resultText));
      }

      Common.WriteOutput(results);
    }



    private CaseInput input;

    internal CaseSolver(CaseInput inputCase)
    {
      input = inputCase;
    }

    internal CaseOutput Solve()
    {
      decimal[] bestCompletionTimeStartingFromCity = new decimal[input.N];
      bestCompletionTimeStartingFromCity[input.N - 1] = 0;

      for (int nn = input.N-2; nn >=0; nn--)
      {
        var localHorseSpeed = input.HorseSpeeds[nn];
        var localHorseRange = input.HorseRanges[nn];

        var citiesViable = GetViableCities(nn, localHorseRange);

        decimal bestTime = decimal.MaxValue;
        foreach (var targetCityWithDistance in citiesViable)
        {
          var timeToTarget = (decimal)targetCityWithDistance.Item2/localHorseSpeed;
          var timeFromTarget = bestCompletionTimeStartingFromCity[targetCityWithDistance.Item1];
          var completionViaCity = timeToTarget + timeFromTarget;
          bestTime = Math.Min(bestTime, completionViaCity);
        }
        bestCompletionTimeStartingFromCity[nn] = bestTime;
      }

      return new CaseOutput(bestCompletionTimeStartingFromCity[0]);

    }

    //city n, distance.
    internal List<Tuple<int, long>> GetViableCities(int start, long range)
    {
      var ret = new List<Tuple<int, long>>();
      long distance = 0;
      for (int nn = start; nn < input.N-1; nn++)
      {
        distance += input.LinearCityDistances[nn];
        if (distance <= range)
        {
          ret.Add(Tuple.Create(nn+1, distance));
        }
        else
        {
          return ret;
        }
      }
      return ret;
    }
    
    //internal void MassFlipStartingAtIndex(int index)
    //{
    //  for (int jj = 0; jj < input.FlipSize; jj++)
    //  {
    //    SingleBitFlip(index + jj);
    //  }
    //}

    //internal void SingleBitFlip(int index)
    //{
    //  input.Sequence[index] = !input.Sequence[index];
    //}

  }
}
