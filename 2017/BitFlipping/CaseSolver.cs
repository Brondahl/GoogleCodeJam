using System.Collections.Generic;
using System.Linq;
using Common;

namespace BitFlipping
{
  public class CaseSolver
  {
    private static string subFolderName = @"BitFlipping";
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2017Communicator(subFolderName);
    public static void Run()
    {
      var cases = InOut.ReadStringInput(out numberOfCases).ToList();
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
      var totalTestStepsCount = input.SequenceLength - input.FlipSize + 1;
      var totalFlipsCount = 0;
      for (int ii = 0; ii < totalTestStepsCount; ii++)
      {
        if (!input.Sequence[ii])
        {
          MassFlipStartingAtIndex(ii);
          totalFlipsCount++;
        }
      }

      if (input.Sequence.All(flag => flag))
      {
        return new CaseOutput(totalFlipsCount);
      }
      return new CaseOutput(-1);
    }

    internal void MassFlipStartingAtIndex(int index)
    {
      for (int jj = 0; jj < input.FlipSize; jj++)
      {
        SingleBitFlip(index + jj);
      }
    }

    internal void SingleBitFlip(int index)
    {
      input.Sequence[index] = !input.Sequence[index];
    }

  }
}
