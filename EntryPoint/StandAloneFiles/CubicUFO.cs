//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static System.Math;

//namespace GoogleCodeJam2018_4
//{
//  class Solution
//  {
//    static void Main()
//    {
//      CubicUFO.CaseSolver.Run();
//    }
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

//namespace CubicUFO
//{
//  class CaseInput
//  {
//    internal CaseInput(string line)
//    {
//      var split = line.Split(' ');
//      Area = double.Parse(split[0]);
//    }

//    internal double Area;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(double theta, double phi = 0)
//    {
//      Theta = theta;
//      Phi = phi;
//    }

//    internal double Theta;
//    internal double Phi;

//    internal double X;
//    internal double Y;

//    public override string ToString()
//    {
//      if (Phi != 0)
//      {
//        throw new InvalidOperationException();
//      }

//      X = Cos(Theta) / 2;
//      Y = Sin(Theta) / 2;

//      var coord1 = $"{-X} {Y} 0";
//      var coord2 = $"{Y} {X} 0";
//      var coord3 = "0 0 0.5";

//      return Environment.NewLine + coord1 + Environment.NewLine + coord2 + Environment.NewLine + coord3;
//    }
//  }

//}

//namespace CubicUFO
//{
//using Common;
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
//      var cases = lines.ToArray();
//      var results = new List<string>();

//      for (int ii = 0; ii < numberOfCases; ii++)
//      {
//        var parsedCase = new CaseInput(cases[ii]);
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
//      if (input.Area <= Sqrt(2) + Pow(10, -12))
//      {
//        var areaRatio = input.Area / Sqrt(2);
//        if (areaRatio > 1)
//        {
//          areaRatio = 1;
//        }
//        var theta = PI / 4 - Acos(areaRatio);
//        return new CaseOutput(theta);
//      }
//      throw new InvalidOperationException();
//    }

//  }
//}
