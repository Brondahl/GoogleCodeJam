namespace AntStack
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.InteropServices;
  using Common;

  public class CaseSolver
  {
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2018Communicator();
    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases);
      var cases = new CaseSplitter().GetCaseLines(lines, 2);
      var results = new List<string>();
      var caseNumber = 0;

      foreach (var caseLines in cases)
      {
        caseNumber++; //1-indexed.
        var parsedCase = new CaseInput(caseLines);
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();

        var resultText = result.ToString();

        results.Add($"Case #{caseNumber}: {resultText}");
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
      //We're going to use 1-indexing through-out!!
      var countsWeights = new List<long>();
      countsWeights.Add(0);

      for (int i = 0; i < input.N; i++)
      {
        var currentAnt = input.AntWeights[i];
        if (i == 0)
        {
          countsWeights.Add(currentAnt);
          continue;
        }
        else
        {
          var lengthOfLongestAntStack = countsWeights.Count - 1;
          var weightOfLongestAntStack = countsWeights[lengthOfLongestAntStack];
          if (currentAnt.CanCarry(weightOfLongestAntStack))
          {
            countsWeights.Add(weightOfLongestAntStack + currentAnt);
          }

          for (int newFinalStackLengthToAttempt = lengthOfLongestAntStack/*!! -1 ... we've already done longest, above!!*/; newFinalStackLengthToAttempt >= 1 /*!! Use the empty Stack!!*/; newFinalStackLengthToAttempt--)
          {
            var lengthOfAntStackToAddTo = newFinalStackLengthToAttempt - 1;
            var weightOfAntStackToAddTo = countsWeights[lengthOfAntStackToAddTo];
            if (currentAnt.CanCarry(weightOfAntStackToAddTo))
            {
              var impliedNewWeight = weightOfAntStackToAddTo + currentAnt;
              var currentBestWeightForThisStackLenght = countsWeights[newFinalStackLengthToAttempt];
              if (impliedNewWeight < currentBestWeightForThisStackLenght)
              {
                countsWeights[newFinalStackLengthToAttempt] = impliedNewWeight;
              }
            }
          }
        }
      }
      //We're using 1-indexing!!
      return new CaseOutput(countsWeights.Count-1);
    }

  }

  public static class AntExtension
  {
    public static bool CanCarry(this long thisAnt, long someWeight)
    {
      return someWeight <= thisAnt * 6;
    }
  }
}
