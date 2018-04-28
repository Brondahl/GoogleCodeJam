using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Common;

namespace CruiseControl
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
    private static string subFolderName = @"CruiseControl";
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2017Communicator(subFolderName);

    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = new CaseSplitter().GetCaseLines_TakingNFromSecondVal(lines).ToArray();
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
      decimal completionTime = 0;
      for (int nn = 0; nn < input.N; nn++)
      {
        var thisHorseDistance = input.D - input.HorsePositions[nn];
        var thisHorseCompletionTime = (decimal)thisHorseDistance / input.HorseSpeeds[nn];
        completionTime = Math.Max(completionTime, thisHorseCompletionTime);
      }

      var targetSpeed = input.D/completionTime;

      return new CaseOutput(targetSpeed);
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
