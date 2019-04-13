namespace AlienRhyme
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
      var dictionariesByTailLength = new Dictionary<int, Dictionary<string, List<string>>>();
      var pairCount = new List<string>();
      for (int i = 1; i < input.LongestWord + 1; i++)
      {
        dictionariesByTailLength.Add(i, CreateDictionary(i));
      }

      for (int rhymeLength = input.LongestWord; rhymeLength >= 0; rhymeLength--)
      {
        Dictionary<string, List<string>> dictionaryByTail;
        if (!dictionariesByTailLength.TryGetValue(rhymeLength, out dictionaryByTail))
        {
          continue;
        }

        if (!dictionaryByTail.Any()) { continue; }
        foreach (var tailGroupContentsToConsider in dictionaryByTail.Values.ToList())
        {
          while (tailGroupContentsToConsider.Count > 1)
          {
            var rhymeTail = new string(tailGroupContentsToConsider[0].ToCharArray().Take(rhymeLength).ToArray());
            if (pairCount.Contains(rhymeTail))
            {
              tailGroupContentsToConsider.RemoveAt(0);
              continue;
            }
            PurgeFirst(tailGroupContentsToConsider[0], tailGroupContentsToConsider, dictionariesByTailLength);
            PurgeFirst(tailGroupContentsToConsider[0], tailGroupContentsToConsider, dictionariesByTailLength);

            pairCount.Add(rhymeTail);
          }
        }
      }
      return new CaseOutput(pairCount.Count*2);
    }

    private void PurgeFirst(string toRemove, List<string> currentIteration, Dictionary<int, Dictionary<string, List<string>>> dictionariesByTailLength , int? purgeMatchingRhymeLength = null)
    {
      currentIteration.Remove(toRemove);
      foreach (var dictionaryByTailLengthKVP in dictionariesByTailLength.ToList())
      {
        var length = dictionaryByTailLengthKVP.Key;
        if(length > toRemove.Length) { continue; }
        var dictionaryByTail = dictionaryByTailLengthKVP.Value;
        var purgeTargetTail = new string(toRemove.ToCharArray().Take(length).ToArray());
        var matchingTailList = dictionaryByTail[purgeTargetTail];
        matchingTailList.Remove(toRemove);

        if (!matchingTailList.Any()) { dictionaryByTail.Remove(purgeTargetTail); }
        if (!dictionaryByTail.Any()) { dictionariesByTailLength.Remove(length); }
      }

      if (purgeMatchingRhymeLength != null)
      {
        var rhymeTail = new string(toRemove.ToCharArray().Take(purgeMatchingRhymeLength.Value).ToArray());
        Dictionary<string, List<string>> matchSegmentLengthDictionary;
        if (dictionariesByTailLength.TryGetValue(purgeMatchingRhymeLength.Value, out matchSegmentLengthDictionary))
        {
          List<string> exactRhymeMatches;
          if (matchSegmentLengthDictionary.TryGetValue(rhymeTail, out exactRhymeMatches))
          {
            foreach (var exactRhymeMatch in exactRhymeMatches.ToList())
            {
              PurgeFirst(exactRhymeMatch, currentIteration, dictionariesByTailLength);
            }
          }
        }
      }
    }

    private Dictionary<string, List<string>> CreateDictionary(int length)
    {
      return input.ReversedCharArrays.Where(arr => arr.Length >= length).ToLookup(
        arr => new String(arr.Take(length).ToArray()),
        arr => new string(arr)
      ).ToDictionary(l => l.Key, l => l.ToList());
    }

  }
}
