//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static System.Math;

//namespace GoogleCodeJam2018_1A_1
//{
//  class Program
//  {
//    static void Main(string[] args)
//    {
//      EdgyBaking.CaseSolver.Run();
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
//      return CaseLineSplitter(lines, (NArray) => NArray.First());
//    }

//    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, (NArray) => NArray.First() + 1);
//    }

//    public IEnumerable<IEnumerable<string>> CaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
//    {
//      return CaseLineSplitter(lines, (NArray) => NArray.Skip(1).First());
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
//  using System.Collections.Generic;
//  public interface IGoogleCodeJamCommunicator
//  {
//    IEnumerable<string> ReadStringInput(out int numberOfCases);
//    void WriteOutput(IEnumerable<string> lines);
//  }
//}
//namespace Common
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  public class GoogleCodeJam2018Communicator : IGoogleCodeJamCommunicator
//  {
//    public IEnumerable<string> ReadStringInput(out int numberOfCases)
//    {
//      var lines = ReadStringInputAsIterator();

//      var firstLine = lines.Take(1).Single();
//      numberOfCases = int.Parse(firstLine);

//      return lines;
//    }

//    private IEnumerable<string> ReadStringInputAsIterator()
//    {
//      while (true)
//      {
//        var line = Console.ReadLine();
//        if (string.IsNullOrEmpty(line)) { break; }
//        yield return (line);
//      }
//    }

//    public void WriteOutput(IEnumerable<string> lines)
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
//  using System.Collections.Generic;
//  using System.IO;
//  using System.Linq;

//  public class GoogleCodeJam2017Communicator : IGoogleCodeJamCommunicator
//  {
//    private readonly string folderPath = @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\GoogleCodeJam2017\";
//    private readonly string inputFileName;
//    private readonly string inputFilePath;
//    private readonly string outputFilePath;

//    public GoogleCodeJam2017Communicator(string subFolderName, string fileName = null)
//    {
//      inputFileName = fileName ?? @"Data.in";
//      inputFilePath = Path.Combine(folderPath, subFolderName, inputFileName);
//      outputFilePath = Path.Combine(folderPath, subFolderName, "Data.out");
//    }

//    public IEnumerable<string> ReadStringInput(out int numberOfCases)
//    {
//      var lines = File.ReadAllLines(inputFilePath);
//      numberOfCases = int.Parse(lines.First());
//      return lines.Skip(1).ToArray();
//    }

//    public void WriteOutput(IEnumerable<string> lines)
//    {
//      File.WriteAllLines(outputFilePath, lines.ToArray());
//    }
//  }
//}

//namespace Common
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  public class CaseSplitter
//  {
//    /// <summary>
//    /// These assume that the first line of each case will be a sequence of longs,
//    /// from which you can deduce the number of other lines in the case.
//    /// The first line gets parsed, and some of the 
//    /// </summary>
//    /// <param name="lines"></param>
//    /// <returns></returns>
//    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstVal(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.First());
//    }

//    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 1);
//    }

//    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusTwo(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 2);
//    }

//    public IEnumerable<List<string>> GetCaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.Skip(1).First());
//    }

//    public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, 1).Select(caseLines => caseLines.Single());
//    }

//    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines, long numberOfLinesInACase)
//    {
//      return GetCaseLines(lines, args => numberOfLinesInACase, false);
//    }

//    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines, Func<long[], long> totalNumberOfLinesInACase, bool parseArgsLineAsLongs = true)
//    {
//      var caseSet = new List<string>();
//      long[] continueTestArgs = null;
//      var currentLineCount = 0;
//      long numberOfLinesToPutInCurrentCase = 0;

//      foreach (var line in lines)
//      {
//        caseSet.Add(line);
//        currentLineCount++;

//        if (continueTestArgs == null)
//        {
//          continueTestArgs = parseArgsLineAsLongs ? line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray() : null;
//          numberOfLinesToPutInCurrentCase = totalNumberOfLinesInACase(continueTestArgs);
//        }

//        if (currentLineCount < numberOfLinesToPutInCurrentCase)
//        {
//          continue;
//        }

//        // We've reached the end of the CaseSet.
//        // Return this caseSet.
//        yield return caseSet.ToList();

//        // And reset our counters
//        caseSet = new List<string>();
//        continueTestArgs = null;
//        currentLineCount = 0;
//        numberOfLinesToPutInCurrentCase = 0;
//      }

//    }
//  }
//}

//namespace EdgyBaking
//{
//  using Common;
//  public class CaseSolver
//  {
//    private static int numberOfCases;
//    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2018Communicator();

//    public static void Run()
//    {
//      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
//      string[][] cases = new CaseSplitter().Configure_TakingNFromFirstValPlusOne().GetCaseLines(lines).Select(caseLineSet => caseLineSet.ToArray()).ToArray();
//      var results = new List<string>();

//      for (int ii = 0; ii < numberOfCases - 1; ii++)
//      {
//        string[] caseArray = null;
//        CaseInput parsedCase = null;
//        CaseSolver solver = null;

//        caseArray = cases[ii];

//        parsedCase = new CaseInput(caseArray);
//        solver = new CaseSolver(parsedCase);

//        CaseOutput result = null;
//        result = solver.Solve();
//        //var result = solver.Solve();

//        var resultText = result.ToString();

//        results.Add($"Case #{ii + 1}: {resultText}");

//      }

//      for (int ii = numberOfCases - 1; ii < numberOfCases; ii++)
//      {
//        string[] caseArray = null;
//        CaseInput parsedCase = null;
//        CaseSolver solver = null;

//        try
//        {
//          caseArray = cases[ii];
//        }
//        catch (Exception)
//        {
//          System.Threading.Thread.Sleep(17 * 100 * 1000);
//        }

//        parsedCase = new CaseInput(caseArray);
//        solver = new CaseSolver(parsedCase);

//        CaseOutput result = null;
//        result = solver.Solve();
//        //var result = solver.Solve();

//        var resultText = result.ToString();

//        results.Add($"Case #{ii + 1}: {resultText}");

//      }

//      InOut.WriteOutput(results);
//    }



//    private CaseInput input;

//    internal CaseSolver(CaseInput inputCase)
//    {
//      input = inputCase;
//    }

//    private double currentMin = 0;
//    private double currentMax = 0;
//    private double currentBestAttempt = 0;
//    internal CaseOutput Solve()
//    {
//      var minArea = input.Cookies.Sum(c => c.basicPerimeter);
//      var maxArea = input.Cookies.Sum(c => c.basicPerimeter + c.additionalBasicCutPerimeter + c.maxAdditionalCutPerimeter);

//      if (maxArea < input.P)
//      {
//        return new CaseOutput(maxArea);
//      }

//      currentMin = minArea;
//      currentMax = minArea;
//      currentBestAttempt = minArea;

//      bool[] currentCuts = new bool[input.N];

//      try
//      {
//        TryNext(currentCuts, 0);
//      }
//      catch (ArgumentException) //SignalForPerfectSolution
//      {
//        return new CaseOutput(input.P);
//      }

//      return new CaseOutput(currentBestAttempt);
//    }

//    private void TryNext(bool[] currentCuts, int minUsableI)
//    {
//      for (int i = minUsableI; i < input.N; i++)
//      {
//        if (currentCuts[i]) { continue; }
//        var minAdded = input.Cookies[i].additionalBasicCutPerimeter;
//        var maxAdded = minAdded + input.Cookies[i].maxAdditionalCutPerimeter;

//        currentCuts[i] = true;
//        currentMin += minAdded;
//        currentMax += maxAdded;

//        if (currentMin > input.P)
//        {
//          // Illegal. Revert and continue.
//          currentCuts[i] = false;
//          currentMin -= minAdded;
//          currentMax -= maxAdded;
//          continue;
//        }
//        else if (currentMax > input.P)
//        {
//          //Perfect solution available.
//          throw new ArgumentException(); //signal for perfect solution.
//        }
//        else
//        {
//          //Fair Try, keep going.
//          if (currentMax > currentBestAttempt)
//          {
//            currentBestAttempt = currentMax;
//          }
//          TryNext(currentCuts, i + 1);
//        }
//      }
//    }
//  }
//}

//namespace EdgyBaking
//{
//  class CaseInput
//  {
//    internal CaseInput(string[] lines)
//    {
//      try
//      {
//        var NPline = lines[0].Split(' ');
//        N = int.Parse(NPline[0]);
//        P = int.Parse(NPline[1]);

//        Cookies = new Cookie[N];

//        for (int i = 0; i < N; i++)
//        {
//          Cookies[i] = new Cookie(lines[i + 1]);
//        }
//      }
//      catch (Exception)
//      {
//        System.Threading.Thread.Sleep(17 * 100 * 1000);
//      }

//    }

//    internal int N;
//    internal int P;
//    internal Cookie[] Cookies;
//  }

//  class Cookie
//  {
//    public Cookie(string cookieLine)
//    {
//      var dimensions = cookieLine.Split(' ').Select(int.Parse).ToArray();
//      W = dimensions[0];
//      H = dimensions[1];
//      basicPerimeter = 2 * (W + H);
//      additionalBasicCutPerimeter = 2 * (Min(W, H));
//      maxAdditionalCutPerimeter = 2 * (Sqrt(W * W + H * H)) - additionalBasicCutPerimeter;
//    }

//    public int W;
//    public int H;
//    public int basicPerimeter;
//    public int additionalBasicCutPerimeter;
//    public double maxAdditionalCutPerimeter;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(int closestValue)
//    {
//      Closest = closestValue;
//    }

//    internal CaseOutput(double closestValue)
//    {
//      Closest = closestValue;
//    }

//    internal double Closest;

//    public override string ToString()
//    {
//      return Closest.ToString();
//    }
//  }

//}
