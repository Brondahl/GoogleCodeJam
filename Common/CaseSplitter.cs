using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
  public class CaseSplitter
  {
    /// <summary>
    /// These assume that the first line of each case will be a sequence of longs,
    /// from which you can deduce the number of other lines in the case.
    /// The first line gets parsed, and some of the 
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstVal(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First());
    }

    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 1);
    }

    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusTwo(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 2);
    }

    public IEnumerable<List<string>> GetCaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.Skip(1).First());
    }

    public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, 1).Select(caseLines => caseLines.Single());
    }

    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines, long numberOfLinesInACase)
    {
      return GetCaseLines(lines, args => numberOfLinesInACase, false);
    }

    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines, Func<long[], long> totalNumberOfLinesInACase, bool parseArgsLineAsLongs = true)
    {
      var caseSet = new List<string>();
      long[] continueTestArgs = null;
      var currentLineCount = 0;
      long numberOfLinesToPutInCurrentCase = 0;

      foreach (var line in lines)
      {
        caseSet.Add(line);
        currentLineCount++;

        if (continueTestArgs == null)
        {
          continueTestArgs = parseArgsLineAsLongs ? line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray() : null;
          numberOfLinesToPutInCurrentCase = totalNumberOfLinesInACase(continueTestArgs);
        }

        if (currentLineCount < numberOfLinesToPutInCurrentCase)
        {
          continue;
        }

        // We've reached the end of the CaseSet.
        // Return this caseSet.
        yield return caseSet.ToList();

        // And reset our counters
        caseSet = new List<string>();
        continueTestArgs = null;
        currentLineCount = 0;
        numberOfLinesToPutInCurrentCase = 0;
      }

    }
  }
}