//using System.Collections.Generic;
//using System.Linq;
//using System;

//namespace GoogleCodeJam2018_1
//{
//  class Solution
//  {
//    static void Main()
//    {
//      SavingTheUniverseAgain.CaseSolver.Run();
//    }
//  }
//}

//namespace SavingTheUniverseAgain
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
//    private long currentMaxChargeDamage = 1;
//    private long totalDamageDealt = 0;

//    internal CaseSolver(CaseInput inputCase)
//    {
//      input = inputCase;
//    }

//    internal CaseOutput Solve()
//    {
//      if (input.ShotCount > input.D) { return new CaseOutput(-1); }
//      ProcessSequence(input.Shots);

//      long hacks = 0;
//      while (totalDamageDealt > input.D)
//      {
//        var damageReduction = currentMaxChargeDamage / 2;

//        //Move Shot down one level.
//        input.Shots[input.MaxChargeFired]--;
//        input.Shots[input.MaxChargeFired - 1]++;

//        //Reduce Damage Tracker.
//        totalDamageDealt -= damageReduction;

//        //Record Hack
//        hacks++;

//        if (input.Shots[input.MaxChargeFired] == 0)
//        {
//          input.MaxChargeFired--;
//          currentMaxChargeDamage = damageReduction;
//        }
//      }
//      return new CaseOutput(hacks);
//    }

//    private long ProcessSequence(int[] sequence)
//    {
//      for (int charge = 0; charge < input.MaxChargeFired + 1; charge++)
//      {
//        totalDamageDealt += currentMaxChargeDamage * sequence[charge];
//        currentMaxChargeDamage *= 2;
//      }
//      currentMaxChargeDamage /= 2;
//      return totalDamageDealt;
//    }
//  }

//  class CaseInput
//  {
//    internal CaseInput(string line)
//    {
//      var split = line.Split(' ');
//      D = int.Parse(split[0]);
//      var P = split[1].ToCharArray();
//      Shots = new int[30];

//      var shotIndex = 0;
//      foreach (var character in P)
//      {
//        if (character == 'C')
//        {
//          shotIndex++;
//        }
//        else
//        {
//          Shots[shotIndex]++;
//          ShotCount++;
//          MaxChargeFired = shotIndex;
//        }
//      }
//    }

//    internal long D;
//    internal int[] Shots;
//    internal int ShotCount;
//    internal int MaxChargeFired;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(long hacks)
//    {
//      Hacks = hacks;
//    }

//    internal long Hacks;

//    public override string ToString()
//    {
//      return Hacks == -1 ? "IMPOSSIBLE" : Hacks.ToString();
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

//    public IEnumerable<IEnumerable<string>> CaseLineForNFromFirstVal(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, "0 -1", (NArray) => NArray.Single() + 1);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLineForNFromFirstValPlusOne(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, "0 -1", (NArray) => NArray.Single() + 2);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLineForNFromSecondVal(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, "-1 0", (NArray) => NArray.Single() + 1);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, string firstLineFormat, Func<long[], long> numberOfLinesInACase)
//    {
//      return CaseLineSplitter(lines, firstLineFormat, (lineCount, args) => { return lineCount < numberOfLinesInACase(args) - 1; });
//    }

//    public IEnumerable<IEnumerable<string>> CaseLineSplitter(IEnumerable<string> lines, string firstLineFormat, Func<long, long[], bool> continueTest)
//    {
//      var firstLineFormatComponents = firstLineFormat.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
//      var caseSet = new List<string>();
//      long[] continueTestArgs = null;
//      var currentLineCount = 0;

//      foreach (var line in lines)
//      {
//        caseSet.Add(line);

//        if (continueTestArgs == null)
//        {
//          var currentFirstLineValues = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
//          var zippedValues = Enumerable.Zip(firstLineFormatComponents, currentFirstLineValues, (format, actual) => new KeyValuePair<long, long>(format, actual));
//          continueTestArgs = zippedValues.Where(pair => pair.Key != -1).OrderBy(pair => pair.Key).Select(pair => pair.Value).ToArray();
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
