namespace Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class CaseSplitter
  {
    /// <summary>
    /// These assume that the first line of each case will be a sequence of longs,
    /// from which you can deduce the number of other lines in the case.
    /// In general the first line gets parsed, and those values are the passed to a function that calculates how many more lines to include in the Case.
    /// </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstVal(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 1);
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusTwo(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 2);
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.Skip(1).First());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
    {
      return GetConstantMultiLineCases(lines, 1).Select(caseLines => caseLines.Single());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetConstantMultiLineCases(IEnumerable<string> lines, long numberOfLinesInACase)
    {
      return GetCaseLines(lines, args => numberOfLinesInACase, false);
    }

    /// <summary> See summary comment above </summary>
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
          continueTestArgs = parseArgsLineAsLongs ? line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray() : new long[0];
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