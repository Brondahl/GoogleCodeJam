namespace PancakeStack
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;

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
    private static string subFolderName = @"PancakeStack";
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2017Communicator(subFolderName);

    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = new CaseSplitter().GetCaseLines_TakingNFromFirstVal(lines).ToArray();
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new CaseInput(cases[ii]);
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();

        var resultText = result.ToString();

        results.Add(string.Format("Case #{0}: {1}", ii + 1, resultText));
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
      double maxArea = 0;
      for (int n = 0; n < input.N; n++)
      {
        var area = MaxAreaIfBasedAtN(n);
        if (area > maxArea)
        {
          maxArea = area;
        }
      }

      return new CaseOutput(maxArea);
    }

    internal double MaxAreaIfBasedAtN(int n)
    {
      var surfaceAreaOverPi = input.PancakeRadii[n]*input.PancakeRadii[n];
      var baseSidesOverPi = input.PancakeSideAreaOverPi[n];

      var bestOtherSidesOverPi =
        input.PancakeSideAreaOverPi.Where((prod, index) => index != n)
          .OrderByDescending(prod => prod)
          .Take((int) input.K - 1)
          .Sum();

      return (surfaceAreaOverPi + baseSidesOverPi + bestOtherSidesOverPi)*Math.PI;
    }
  }
}