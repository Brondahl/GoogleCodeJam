using static System.Math;

namespace CubicUFO
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
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2018Communicator();
    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = lines.ToArray();
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new CaseInput(cases[ii]);
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
      if (input.Area <= Sqrt(2) + Pow(10, -12))
      {
        var areaRatio = input.Area / Sqrt(2);
        if (areaRatio > 1)
        {
          areaRatio = 1;
        }
        var theta = PI / 4 - Acos(areaRatio);
        return new CaseOutput(theta);
      }
      throw new InvalidOperationException();
    }

  }
}
