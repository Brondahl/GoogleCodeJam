using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
  public abstract class CommonBase
  {
    protected abstract string folderPath { get; }
    public abstract string[] ReadStringInput(out int numberOfCases);
    public abstract void WriteOutput(IEnumerable<string> lines);

    public int[] ReadIntInput(out int numberOfCases)
    {
      var textLines = ReadStringInput(out numberOfCases);
      return textLines.Select(int.Parse).ToArray();
    }

    public long[] ReadLongInput(out int numberOfCases)
    {
      var textLines = ReadStringInput(out numberOfCases);
      return textLines.Select(long.Parse).ToArray();
    }

    /// <summary>
    /// These assume that the first line of each case will be a sequence of longs,
    /// from which you can deduce the number of other lines in the case.
    /// The first line gets parsed, and some of the 
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromFirstVal(IEnumerable<string> lines)
    {
      return CaseLineSplitter(lines, (NArray) => NArray.First() + 1);
    }

    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
    {
      return CaseLineSplitter(lines, (NArray) => NArray.First() + 2);
    }

    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
    {
      return CaseLineSplitter(lines, (NArray) => NArray.Skip(1).First() + 1);
    }

    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, long numberOfLinesInACase)
    {
      return CaseLineSplitter(lines, (args) => numberOfLinesInACase);
    }

    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, Func<long[], long> numberOfLinesInACase)
    {
      return CaseLineSplitter(lines, (lineCount, args) => { return lineCount < numberOfLinesInACase(args) - 1; });
    }

    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, Func<long, long[], bool> continueTest)
    {
      var caseSet = new List<string>();
      long[] continueTestArgs = null;
      var currentLineCount = 0;

      foreach (var line in lines)
      {
        caseSet.Add(line);

        if (continueTestArgs == null)
        {
          continueTestArgs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }

        // e.g. when N is 3, you'll want to return lines 0, 1, 2 & 3.
        // Thus you keep 'continue' on when count = 2, and stop when count = 3
        if (continueTest(currentLineCount, continueTestArgs))
        {
          currentLineCount++;
          continue;
        }

        // We've reached the end of the CaseSet.
        // Return this caseSet and reset.
        yield return caseSet.ToList();
        caseSet = new List<string>();
        continueTestArgs = null;
        currentLineCount = 0;
      }

    }

  }
}
