//using System;
//using System.Collections.Generic;
//using System.Linq;

//class Solution
//{
//  static void Main()
//  {
//    TroubleSort.CaseSolver.Run();
//  }
//}

//namespace Common
//{
//  public abstract class CommonBase
//  {
//    protected abstract string folderPath { get; }
//    public abstract string[] ReadStringInput(out int numberOfCases);
//    public abstract void WriteOutput(IEnumerable<string> lines);

//    public int[] ReadIntInput(out int numberOfCases)
//    {
//      var textLines = ReadStringInput(out numberOfCases);
//      return textLines.Select(int.Parse).ToArray();
//    }

//    public long[] ReadLongInput(out int numberOfCases)
//    {
//      var textLines = ReadStringInput(out numberOfCases);
//      return textLines.Select(long.Parse).ToArray();
//    }

//    /// <summary>
//    /// These assume that the first line of each case will be a sequence of longs,
//    /// from which you can deduce the number of other lines in the case.
//    /// The first line gets parsed, and some of the 
//    /// </summary>
//    /// <param name="lines"></param>
//    /// <returns></returns>
//    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromFirstVal(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, (NArray) => NArray.First() + 1);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, (NArray) => NArray.First() + 2);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, (NArray) => NArray.Skip(1).First() + 1);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, long numberOfLinesInACase)
//    {
//      return CaseLineSplitter(lines, (args) => numberOfLinesInACase);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, Func<long[], long> numberOfLinesInACase)
//    {
//      return CaseLineSplitter(lines, (lineCount, args) => { return lineCount < numberOfLinesInACase(args) - 1; });
//    }

//    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, Func<long, long[], bool> continueTest)
//    {
//      var caseSet = new List<string>();
//      long[] continueTestArgs = null;
//      var currentLineCount = 0;

//      foreach (var line in lines)
//      {
//        caseSet.Add(line);

//        if (continueTestArgs == null)
//        {
//          continueTestArgs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
//        }

//        // e.g. when N is 3, you'll want to return lines 0, 1, 2 & 3.
//        // Thus you keep 'continue' on when count = 2, and stop when count = 3
//        if (continueTest(currentLineCount, continueTestArgs))
//        {
//          currentLineCount++;
//          continue;
//        }

//        // We've reached the end of the CaseSet.
//        // Return this caseSet and reset.
//        yield return caseSet.ToList();
//        caseSet = new List<string>();
//        continueTestArgs = null;
//        currentLineCount = 0;
//      }

//    }

//  }
//}

//namespace Common
//{
//  public class Common2018 : CommonBase
//  {
//    protected override string folderPath => @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\GoogleCodeJam2018\";

//    public override string[] ReadStringInput(out int numberOfCases)
//    {
//      List<string> lines = new List<string>();

//      while (true)
//      {
//        var line = Console.ReadLine();
//        if (string.IsNullOrEmpty(line)) { break; }
//        lines.Add(line);
//      }
//      numberOfCases = int.Parse(lines.First());
//      return lines.Skip(1).ToArray();
//    }

//    public override void WriteOutput(IEnumerable<string> lines)
//    {
//      foreach (var line in lines)
//      {
//        Console.WriteLine(line);
//      }
//    }
//  }
//}

//namespace TroubleSort
//{
//  class CaseInput
//  {
//    internal CaseInput(string[] lines)
//    {
//      N = int.Parse(lines[0]);
//      LongestSubList = (N + 1) / 2;
//      IsEven = (N % 2 == 0);

//      var V = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

//      if (N == 0)
//      {
//        EvenV = new int[0];
//        OddV = new int[0];
//        LongestSubList = 0;
//        IsEven = true;
//        return;
//      }

//      if (N == 1)
//      {
//        EvenV = new int[] { V.Single() };
//        OddV = new int[0];
//        LongestSubList = 1;
//        IsEven = false;
//        return;
//      }

//      var evens = new List<int>();
//      var odds = new List<int>();
//      for (int i = 0; i < N - 1; i++)
//      {
//        evens.Add(V[i]);
//        i++;
//        odds.Add(V[i]);
//      }

//      if (!IsEven)
//      {
//        evens.Add(V.Last());
//      }

//      EvenV = evens.OrderBy(x => x).ToArray();
//      OddV = odds.OrderBy(x => x).ToArray();
//    }

//    internal int N;
//    internal bool IsEven;
//    internal int LongestSubList;
//    internal int[] EvenV;
//    internal int[] OddV;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(int errorIndex)
//    {
//      ErrorIndex = errorIndex;
//    }

//    internal int ErrorIndex;

//    public override string ToString()
//    {
//      return ErrorIndex == -1 ? "OK" : ErrorIndex.ToString();
//    }
//  }

//}

//namespace TroubleSort
//{
//  using Common;
//  /*
//   * TODO:
//   *   - Namespace
//   *   - Copy in/out files.
//   *   - References to Common and testing Frameworks
//   *   - CodeJam Reference to here.
//   *   - Program redirect to here.
//   */
//  public class CaseSolver
//  {
//    private static int numberOfCases;
//    private static CommonBase Common = new Common2018();
//    public static void Run()
//    {
//      var lines = Common.ReadStringInput(out numberOfCases).ToList();
//      var cases = Common.CaseLineSplitter(lines, 2).ToArray();
//      var results = new List<string>();

//      for (int ii = 0; ii < numberOfCases; ii++)
//      {
//        var parsedCase = new CaseInput(cases[ii].ToArray());
//        var solver = new CaseSolver(parsedCase);
//        var result = solver.Solve();

//        var resultText = result.ToString();

//        results.Add($"Case #{ii + 1}: {resultText}");
//      }

//      Common.WriteOutput(results);
//    }



//    private CaseInput input;

//    internal CaseSolver(CaseInput inputCase)
//    {
//      input = inputCase;
//    }

//    internal CaseOutput Solve()
//    {
//      if (input.N == 0) { return new CaseOutput(-1); }
//      if (input.N == 1) { return new CaseOutput(-1); }

//      if (input.N == 2)
//      {
//        var onlyEven = input.EvenV[0];
//        var onlyOdd = input.OddV[0];
//        if (onlyOdd < onlyEven)
//        {
//          return new CaseOutput(0);
//        }
//        else
//        {
//          return new CaseOutput(-1);
//        }
//      }

//      for (int i = 0; i < input.LongestSubList - 1; i++)
//      {
//        var evenV_i = input.EvenV[i];
//        var oddV_i = input.OddV[i];
//        var evenV_i_plus_one = input.EvenV[i + 1];

//        // Even comes first, so Odd_i should be >= Even_i.
//        // If Odd_i is < Even_i we have a bug, starting at Odd_i
//        // Assume EvenV[i] is valid.
//        if (oddV_i < evenV_i)
//        {
//          return new CaseOutput(2 * i);
//        }

//        //Check Even_i+1 >= Odd_i
//        // If Even_i+1 is < Odd_i we have a bug, starting at Even_i+1
//        if (evenV_i_plus_one < oddV_i)
//        {
//          return new CaseOutput((2 * i) + 1);
//        }
//      }

//      if (input.IsEven)
//      {
//        //We checked up to Odd[Penultimate] v Even[Last]. Just one more check of Odd[last]
//        if (input.OddV[input.LongestSubList - 1] < input.EvenV[input.LongestSubList - 1])
//        {
//          return new CaseOutput(input.N - 2);
//        }
//      }
//      else
//      {
//        //We checked up to Odd[Penultimate] v Even[Last]. Nothing left to check.
//      }
//      return new CaseOutput(-1);
//    }

//  }
//}
