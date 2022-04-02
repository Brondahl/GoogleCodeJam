namespace ManhattanCrepeCart
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  public class CaseSolver
  {
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut;
    public static void Run(IGoogleCodeJamCommunicator io = null)
    {
      InOut = io ?? new GoogleCodeJam2018Communicator();
      var lines = InOut.ReadStringInput(out numberOfCases);
      var cases = new CaseSplitter().GetCaseLines_TakingNFromFirstValPlusOne(lines);
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
      var nCumulative = SetCumulative(input.NPeople, input.Q);
      var eCumulative = SetCumulative(input.EPeople, input.Q);
      var sCumulative = SetCumulative(input.SPeople, input.Q);
      var wCumulative = SetCumulative(input.WPeople, input.Q);

      var bestPopularity = 0;
      var bestLocation = Tuple.Create(0,0);

      for (int x = 0; x < input.Q+1; x++)
      {
        for (int y = 0; y < input.Q+1; y++)
        {
          var pop = Popularity(nCumulative, sCumulative, eCumulative, wCumulative, x, y, input.Q);
          if (pop > bestPopularity)
          {
            bestLocation = Tuple.Create(x,y);
            bestPopularity = pop;
          }
        }
      }
      return new CaseOutput(bestLocation.Item1, bestLocation.Item2);
    }

    public static int Popularity(
      Dictionary<int, int> nPeopleDict,
      Dictionary<int, int> sPeopleDict,
      Dictionary<int, int> ePeopleDict,
      Dictionary<int, int> wPeopleDict,
      int x, int y,
      int q)
    {
      return Popularity(ePeopleDict, wPeopleDict, x, q) + Popularity(nPeopleDict, sPeopleDict, y, q);
    }

    public static int Popularity(
      Dictionary<int, int> posPeopleDict,
      Dictionary<int, int> negPeopleDict,
      int coord,
      int q)
    {
      var totalPos = posPeopleDict[q];
      var totalNeg = negPeopleDict[q];

      // set for 0;
      if (coord == 0)
      {
        return totalNeg;
      }

      // set for q;
      if (coord == q)
      {
        return totalPos;
      }

      //set for 1 ... q-1
      var posTowards = posPeopleDict[coord - 1];
      var negTowards = totalNeg - negPeopleDict[coord];
      return posTowards + negTowards;
    }

    public static Dictionary<int, int> SetCumulative(List<int> inputPeople, int max)
    {
      var cumDict = new Dictionary<int, int>();
      var cumCount = 0;
      foreach (var x in inputPeople)
      {
        cumCount++;
        cumDict[x] = cumCount;
      }
      FillDictionary(cumDict, max);
      return cumDict;
    }

    public static void FillDictionary(Dictionary<int, int> sparseDict, int max)
    {
      var dictLatest = 0;
      for (int i = 0; i < max+1; i++)
      {
        if (sparseDict.ContainsKey(i))
        {
          dictLatest = sparseDict[i];
        }
        else
        {
          sparseDict[i] = dictLatest;
        }
      }
    }

  }
}
