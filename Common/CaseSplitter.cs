namespace Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class CaseSplitter
  {
      private Func<long[], long> getTotalNumberOfLinesInACase;
      private bool shouldParseFirstLineAsLongs = true;
    /// <summary>
    /// These assume that the first line of each case will be a sequence of longs,
    /// from which you can deduce the number of other lines in the case.
    /// In general the first line gets parsed, and those values are the passed to a function that calculates how many more lines to include in the Case.
    ///
    /// **N includes the first line, which provided N**, thus GetCaseLines_TakingNFromFirstValPlusOne is the most common variant.
    /// </summary>
    public CaseSplitter Configure_TakingNFromFirstVal()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First();
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_TakingNFromFirstValPlusOne()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First()+1;
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_TakingNFromFirstValPlusTwo()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First() + 2;
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_TakingNFromSecondVal()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.Skip(1).First();
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_ConstantMultiLineCases(long numberOfLinesInACase)
    {
        getTotalNumberOfLinesInACase = _ => numberOfLinesInACase;
        shouldParseFirstLineAsLongs = false;
        return this;
    }

    public CaseSplitter Configure_CustomMap(Func<long[], long> customFuncForTotalNumberOfLinesInACase)
    {
        getTotalNumberOfLinesInACase = customFuncForTotalNumberOfLinesInACase;
        shouldParseFirstLineAsLongs = true;
        return this;
    }

        public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
    {
        Configure_ConstantMultiLineCases(1);
        return GetCaseLines(lines).Select(caseLines => caseLines.Single());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines)
    {
      var linesInCurrentCase = new List<string>();
      long numberOfLinesToPutInCurrentCase = 0;

      foreach (var line in lines)
      {
        linesInCurrentCase.Add(line);

        if (linesInCurrentCase.Count == 1)
        {
            if (shouldParseFirstLineAsLongs)
            {
                var firstLineLongs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                numberOfLinesToPutInCurrentCase = getTotalNumberOfLinesInACase(firstLineLongs);
            }
            else
            {
                numberOfLinesToPutInCurrentCase = getTotalNumberOfLinesInACase(new long[0]);
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