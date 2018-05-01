namespace EdgyBaking
{
  using System;
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
      var cases = new CaseSplitter().GetCaseLines_TakingNFromFirstValPlusOne(lines).ToArray();
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

    private double currentMin = 0;
    private double currentMax = 0;
    private double currentBestAttempt = 0;
    internal CaseOutput Solve()
    {
      var minArea = input.Cookies.Sum(c => c.basicPerimeter);
      var maxArea = input.Cookies.Sum(c => c.basicPerimeter + c.maxAdditionalCutPerimeter);

      if (maxArea < input.P)
      {
        return new CaseOutput(maxArea);
      }

      currentMin = minArea;
      currentMax = minArea;
      currentBestAttempt = minArea;

      bool[] currentCuts = new bool[input.N];

      try
      {
        TryNext(currentCuts,0);
      }
      catch (ArgumentException) //SignalForPerfectSolution
      {
        return new CaseOutput(input.P);
      }

      return new CaseOutput(currentBestAttempt);
    }

    private void TryNext(bool[] currentCuts, int minUsableI)
    {
      for (int i = minUsableI; i < input.N; i++)
      {
        if(currentCuts[i]) { continue; }
        var minAdded = input.Cookies[i].minAdditionalCutPerimeter;
        var maxAdded = input.Cookies[i].maxAdditionalCutPerimeter;

        currentCuts[i] = true;
        currentMin += minAdded;
        currentMax += maxAdded;

        if (currentMin > input.P)
        {
          // Illegal. Revert and continue.
          currentCuts[i] = false;
          currentMin -= minAdded;
          currentMax -= maxAdded;
          continue;
        }
        else if (currentMax > input.P)
        {
          //Perfect solution available.
          throw new ArgumentException(); //signal for perfect solution.
        }
        else
        {
          //Fair Try, keep going.
          if (currentMax > currentBestAttempt)
          {
            currentBestAttempt = currentMax;
          }
          TryNext(currentCuts, i+1);
        }
      }
    }
  }
}
