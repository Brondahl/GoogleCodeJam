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
    ///
    /// **N includes the first line, which provided N**, thus GetCaseLines_TakingNFromFirstValPlusOne is the most common variant.
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
      return GetCaseLines(lines, _ => numberOfLinesInACase, false);
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines, Func<long[], long> totalNumberOfLinesInACase, bool parseFirstLineAsLongs = true)
    {
      var linesInCurrentCase = new List<string>();
      long numberOfLinesToPutInCurrentCase = 0;

      foreach (var line in lines)
      {
        linesInCurrentCase.Add(line);

        if (linesInCurrentCase.Count == 1)
        {
            if (parseFirstLineAsLongs)
            {
                var firstLineLongs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                numberOfLinesToPutInCurrentCase = totalNumberOfLinesInACase(firstLineLongs);
            }
            else
            {
                numberOfLinesToPutInCurrentCase = totalNumberOfLinesInACase(new long[0]);
            }
        }

        if (linesInCurrentCase.Count < numberOfLinesToPutInCurrentCase)
        {
          continue;
        }

        // We've reached the end of the CaseSet.
        // Return this caseSet.
        yield return linesInCurrentCase.ToList();

        // And reset our counters
        linesInCurrentCase = new List<string>();
        numberOfLinesToPutInCurrentCase = 0;
      }
    }
  }
}