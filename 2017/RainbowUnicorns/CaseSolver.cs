using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.RegularExpressions;
using Common;

namespace RainbowUnicorns
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
    private static string subFolderName = @"RainbowUnicorns";
    private static int numberOfCases;
    private static CommonBase Common = new Common2017(subFolderName);
    public static void Run()
    {
      var lines = Common.ReadStringInput(out numberOfCases).ToList();
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new CaseInput(lines[ii]);
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
      var ret = new CaseOutput();

      if (input.O + input.G + input.V == 0)
      {
        ret.Arrangement = SingleColourSolve();
      }
      else
      {
        ret.Arrangement = MultiColourSolve();
      }

      return ret;
    }

    internal string MultiColourSolve()
    {
      var adjustedInput = new CaseInput(input);

      if (input.O == input.B && (input.G + input.R + input.V + input.Y == 0)) { return CrossPairSolve("OB", input.O); }
      if (input.G == input.R && (input.O + input.B + input.V + input.Y == 0)) { return CrossPairSolve("GR", input.G); }
      if (input.V == input.Y && (input.G + input.R + input.O + input.B == 0)) { return CrossPairSolve("VY", input.V); }

      if (input.O >= input.B) { return null; }
      if (input.G >= input.R) { return null; }
      if (input.V >= input.Y) { return null; }

      adjustedInput.B = input.B - input.O;
      adjustedInput.R = input.R - input.G;
      adjustedInput.Y = input.Y - input.V;

      var partialSolver = new CaseSolver(adjustedInput);
      var partial = partialSolver.SingleColourSolve();
      if (partial == null)
      {
        return null;
      }

      for (int i = 0; i < input.O; i++)
      {
        partial = SingleReplace(partial, "B", "BOB");
      }

      for (int i = 0; i < input.G; i++)
      {
        partial = SingleReplace(partial, "R", "RGR");
      }

      for (int i = 0; i < input.V; i++)
      {
        partial = SingleReplace(partial, "Y", "YVY");
      }

      return partial;
    }

    internal string SingleReplace(string target, string old, string @new)
    {
      var regex = new Regex(Regex.Escape(old));
      return regex.Replace(target, @new, 1);
    }

    internal string CrossPairSolve(string pair, int count)
    {
      return string.Join("", Enumerable.Repeat(pair, count));
    }

    internal string SingleColourSolve()
    {
      var maxRYB = Math.Max(input.R, Math.Max(input.Y, input.B));
      var RisMax = (input.R == maxRYB);
      var YisMax = (input.Y == maxRYB);
      var BisMax = (input.B == maxRYB);

      var maxMatchCount = (RisMax ? 1 : 0) + (YisMax ? 1 : 0) + (BisMax ? 1 : 0);

      //3-way tie.
      if (maxMatchCount == 3)
      {
        return string.Join("", Enumerable.Repeat("RYB", maxRYB));
      }

      //2-way tie.
      if (RisMax && YisMax)
      {
        return TwoMaxSolve("RY", "B", maxRYB, input.B);
      }

      if (BisMax && YisMax)
      {
        return TwoMaxSolve("BY", "R", maxRYB, input.R);
      }

      if (RisMax && BisMax)
      {
        return TwoMaxSolve("RB", "Y", maxRYB, input.Y);
      }

      //no tie.
      if (RisMax)
      {
        if (input.Y > input.B)
        {
          return DistinctSolve("R", input.R, "Y", input.Y, "B", input.B);
        }
        else
        {
          return DistinctSolve("R", input.R, "B", input.B, "Y", input.Y);
        }
      }
      if (YisMax)
      {
        if (input.R > input.B)
        {
          return DistinctSolve("Y", input.Y, "R", input.R, "B", input.B);
        }
        else
        {
          return DistinctSolve("Y", input.Y, "B", input.B, "R", input.R);
        }
      }
      if (BisMax)
      {
        if (input.Y > input.R)
        {
          return DistinctSolve("B", input.B, "Y", input.Y, "R", input.R);
        }
        else
        {
          return DistinctSolve("B", input.B, "R", input.R, "Y", input.Y);
        }
      }

      throw new Exception();
    }

    private string TwoMaxSolve(string pair, string single, int max, int min)
    {
      var firstPass = string.Join("", Enumerable.Repeat(pair, max));
      var solution = firstPass;
      var originalStringLength = firstPass.Length;//OUTside the for loop.
      for (int i = 0; i < min; i++)
      {
        solution = solution.Insert(originalStringLength - 1 - i, single);
      }
      return solution;
    }

    private string DistinctSolve(string top, int topCount, string middle, int middleCount, string bottom, int bottomCount)
    {
      if (topCount > middleCount + bottomCount)
      {
        return null;
      }

      var firstPass = string.Join("", Enumerable.Repeat(top, topCount));
      var solution = firstPass;
      var originalStringLength = firstPass.Length;//OUTside the for loop.
      for (int i = 0; i < middleCount; i++)
      {
        solution = solution.Insert(originalStringLength - i, middle);
      }

      for (int i = middleCount; i < originalStringLength; i++)
      {
        solution = solution.Insert(originalStringLength - i, bottom);
      }

      var bottomRemaining = (bottomCount + middleCount) - topCount;
      var interimStringLength = solution.Length;//OUTside the for loop.
      for (int i = 0; i < bottomRemaining; i++)
      {
        solution = solution.Insert(interimStringLength - i, bottom);
      }

      return solution;
    }


  }
}
