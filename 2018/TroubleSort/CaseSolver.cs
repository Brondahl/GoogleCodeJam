﻿namespace TroubleSort
{
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  public class CaseSolver
  {
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2018Communicator();
    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = new CaseSplitter().GetCaseLines(lines, 2).ToArray();
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new CaseInput(cases[ii].ToArray());
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();

        var resultText = result.ToString();

        results.Add($"Case #{ii + 1}: {resultText}");
      }

      InOut.WriteOutput(results);
    }



    private CaseInput input;

    internal CaseSolver(CaseInput inputCase)
    {
      input = inputCase;
    }

    internal CaseOutput Solve()
    {
      if (input.N == 0) { return new CaseOutput(-1); }
      if (input.N == 1) { return new CaseOutput(-1); }

      if (input.N == 2)
      {
        var onlyEven = input.EvenV[0];
        var onlyOdd = input.OddV[0];
        if (onlyOdd < onlyEven)
        {
          return new CaseOutput(0);
        }
        else
        {
          return new CaseOutput(-1);
        }
      }

      for (int i = 0; i < input.LongestSubList-1; i++)
      {
        var evenV_i = input.EvenV[i];
        var oddV_i = input.OddV[i];
        var evenV_i_plus_one = input.EvenV[i+1];

        // Even comes first, so Odd_i should be >= Even_i.
        // If Odd_i is < Even_i we have a bug, starting at Odd_i
        // Assume EvenV[i] is valid.
        if (oddV_i < evenV_i)
        {
          return new CaseOutput(2 * i);
        }

        //Check Even_i+1 >= Odd_i
        // If Even_i+1 is < Odd_i we have a bug, starting at Even_i+1
        if (evenV_i_plus_one < oddV_i)
        {
          return new CaseOutput((2 * i) + 1);
        }
      }

      if (input.IsEven)
      {
        //We checked up to Odd[Penultimate] v Even[Last]. Just one more check of Odd[last]
        if (input.OddV[input.LongestSubList-1] < input.EvenV[input.LongestSubList-1])
        {
          return new CaseOutput(input.N-2);
        }
      }
      else
      {
        //We checked up to Odd[Penultimate] v Even[Last]. Nothing left to check.
      }
      return new CaseOutput(-1);
    }

  }
}
