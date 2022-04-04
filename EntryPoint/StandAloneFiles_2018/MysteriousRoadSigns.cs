//namespace GoogleCodeJam
//{
//  using MysteriousRoadSigns;

//  class Program
//  {
//    static void Main(string[] args)
//    {
//      CaseSolver.Run();
//    }
//  }
//}

//namespace MysteriousRoadSigns
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  class CaseInput
//  {
//    internal CaseInput(List<string> lines)
//    {
//      S = int.Parse(lines[0]);

//      Signs = lines.Skip(1).Select(line => new Sign(line)).ToArray();
//    }

//    internal int S;
//    internal Sign[] Signs;
//  }

//  public class Sign
//  {
//    public override string ToString()
//    {
//      return $"Sign = {M},{N}";
//    }

//    public Sign(string line)
//    {
//      var array = line.Split(' ').Select(int.Parse).ToArray();

//      D = array[0];
//      A = array[1];
//      B = array[2];

//      M = D + A;
//      N = D - B;
//    }

//    public Sign(int d, int a, int b)
//    {
//      D = d;
//      A = a;
//      B = b;

//      M = D + A;
//      N = D - B;
//    }

//    public int D;

//    public int A;
//    public int B;

//    public int M;
//    public int N;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(int largest, int count)
//    {
//      this.largest = largest;
//      this.count = count;
//    }

//    internal int largest;
//    internal int count;

//    public override string ToString()
//    {
//      return $"{largest} {count}";
//    }
//  }

//}
//namespace MysteriousRoadSigns
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Diagnostics;
//  using System.IO;
//  using System.Linq;
//  using Common;

//  public class CaseSolver
//  {
//    public static void GenerateAndSolveNewTestData()
//    {
//      GenerateTestData.GenerateData();
//      CaseSolver.Run(true);
//    }

//    private static int numberOfCases;
//    private static IGoogleCodeJamCommunicator testFileInOut = new GoogleCodeJam2017Communicator(true, @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\2018\MysteriousRoadSigns\TestData", "data.txt");
//    private static IGoogleCodeJamCommunicator liveInOut = new GoogleCodeJam2018Communicator();
//    public static void Run(bool testingMode = false)
//    {
//      var activeIO = testingMode ? testFileInOut : liveInOut;
//      var lines = activeIO.ReadStringInput(out numberOfCases);
//      var cases = new CaseSplitter().Configure_TakingNFromFirstValPlusOne().GetCaseLines(lines);
//      var results = new List<string>();
//      var caseNumber = 0;
//      var stopWatch = new Stopwatch();

//      foreach (var caseLines in cases)
//      {
//        caseNumber++; //1-indexed.
//        stopWatch.Reset();
//        stopWatch.Start();

//        var parsedCase = new CaseInput(caseLines);
//        var solver = new CaseSolver(parsedCase);
//        var result = solver.Solve();
//        var resultText = result.ToString();

//        stopWatch.Stop();
//        results.Add($"Case #{caseNumber}: {resultText}");
//        if (testingMode)
//        {
//          Console.WriteLine(stopWatch.ElapsedMilliseconds / 1000.0);
//      }
//      }

//      activeIO.WriteOutput(results);
//    }

//    private CaseInput input;

//    internal CaseSolver(CaseInput inputCase)
//    {
//      input = inputCase;
//    }

//    internal CaseOutput Solve()
//    {
//      var earliestSetStillActive = 0;
//      var setLengthStartingAtI = new int[input.S];
//      var rulesForSetStartingAtI = new SetRuleOptions[input.S];
//      var setWithStartPointIIsActive = new bool[input.S];
//      for (int i = 0; i < input.S; i++)
//      {
//        var sign_I = input.Signs[i];
//        setLengthStartingAtI[i] = 1;
//        rulesForSetStartingAtI[i] = new SetRuleOptions(sign_I);
//        setWithStartPointIIsActive[i] = true;

//        for (int j = earliestSetStillActive; j < i; j++)
//        {
//          if (setWithStartPointIIsActive[j])
//          {
//            var existingRules = rulesForSetStartingAtI[j];
//            var newRules = existingRules.AttemptIncludeSign(sign_I);

//            if (newRules.IsBroken)
//            {
//              setWithStartPointIIsActive[j] = false;
//              if (j == earliestSetStillActive)
//              {
//                earliestSetStillActive++;
//              }
//            }
//            else
//            {
//              setLengthStartingAtI[j]++;
//              rulesForSetStartingAtI[j] = newRules;
//            }
//          }
//        }
//      }

//      var maxSetLength = setLengthStartingAtI.Max();
//      var numberOfSuchSets = setLengthStartingAtI.Count(x => x == maxSetLength);

//      return new CaseOutput(maxSetLength, numberOfSuchSets);
//    }

//    class SetRuleOptions
//    {
//      public readonly SetRule Rule1 = new SetRule();
//      public readonly SetRule Rule2 = new SetRule();
//      public readonly bool Considers2Rules = true;
//      public readonly bool IsBroken = false;

//      public override string ToString()
//      {
//        var rule1Text = $"Rule1 = {Rule1}";
//        var rule2Text = $"Rule2 = {Rule2}";
//        return IsBroken ? "Broken" : Considers2Rules ? $"{rule1Text} | {rule2Text}" : rule1Text;
//      }

//      public SetRuleOptions(Sign initialSign)
//      {
//        Rule1 = new SetRule (initialSign.M, null);
//        Rule2 = new SetRule (null, initialSign.N);
//      }

//      private SetRuleOptions(SetRule rule)
//      {
//        Considers2Rules = false;
//        Rule1 = rule;
//      }

//      private SetRuleOptions(SetRule rule1, SetRule rule2)
//      {
//        Considers2Rules = true;
//        Rule1 = rule1;
//        Rule2 = rule2;
//      }

//      private SetRuleOptions()
//      {
//        IsBroken = true;
//      }

//      private static SetRuleOptions BrokenRule => new SetRuleOptions();

//      public SetRuleOptions AttemptIncludeSign(Sign sign)
//      {
//        if (Considers2Rules)
//        {
//          var rule1Compat = Rule1.IsCompatibleWith(sign);
//          var rule1Support = Rule1.ActivelySupports(sign);

//          var rule2Compat = Rule2.IsCompatibleWith(sign);
//          var rule2Support = Rule2.ActivelySupports(sign);

//          if (!Rule1.IsFixed)
//          {
//            if (rule1Support && rule2Support)
//            {
//              return this;
//            }
//            if (rule1Support)
//            {
//              return new SetRuleOptions (Rule1);
//            }
//            if (rule2Support)
//            {
//              return new SetRuleOptions (Rule2);
//            }
//            return new SetRuleOptions(
//              new SetRule (Rule1.Left, sign.N),
//              new SetRule (sign.M, Rule2.Right)
//            );
//          }
//          else
//          {
//            if (rule1Compat && rule2Compat)
//            {
//              return this;
//            }

//            if (rule1Compat)
//            {
//              return new SetRuleOptions(Rule1);
//            }

//            if (rule2Compat)
//            {
//              return new SetRuleOptions(Rule2);
//            }
//            return BrokenRule;
//          }
//        }
//        else
//        {
//          var rule1Compat = Rule1.IsCompatibleWith(sign);
//          var rule1Support = Rule1.ActivelySupports(sign);
//          if (!rule1Compat)
//          {
//            return BrokenRule;
//          }
//          if (rule1Support)
//          {
//            return this;
//          }
//          else
//          {
//            return new SetRuleOptions(Rule1.ExpandToSupport(sign));
//          }
//        }
//      }
//    }

//    public class SetRule
//    {
//      public readonly int? Left;
//      public readonly int? Right;
//      public bool IsFixed => Left.HasValue && Right.HasValue;

//      public SetRule(int? left, int? right)
//      {
//        Left = left;
//        Right = right;
//      }

//      public SetRule() : this(null, null) {}

//      public override string ToString()
//      {
        
//        return $"Rule = {StringOrQuestionMark(Left)},{StringOrQuestionMark(Right)}";
//      }

//      private string StringOrQuestionMark(int? val)
//      {
//        return val.HasValue ? val.ToString() : "?";
//      }

//      public bool ActivelySupports(Sign sign)
//      {
//        var supportedOnLeft = Left.HasValue && Left == sign.M;
//        var supportedOnRight = Right.HasValue && Right == sign.N;

//        return supportedOnLeft || supportedOnRight;
//      }

//      public bool IsCompatibleWith(Sign sign)
//      {
//        var compatibleOnLeft = Left == null || Left == sign.M;
//        var compatibleOnRight = Right == null || Right == sign.N;

//        return compatibleOnLeft || compatibleOnRight;
//      }

//      public SetRule ExpandToSupport(Sign sign)
//      {
//        if(Left == null && Right == null)
//        {
//          throw new InvalidOperationException("This should never have happened! - This method shouldn't get called in that scenario.");
//        }

//        if (!IsCompatibleWith(sign))
//        {
//          throw new InvalidOperationException("This should never have happened! - This method shouldn't get called in that scenario.");
//        }

//        if (ActivelySupports(sign))
//        {
//          return this;
//        }

//        //The only possibility remaining, is that exactly one of Left and Right is null, and we're setting that value accordingly.
//        var setLeft = (Left == null);

//        var resultingLeft = setLeft ? sign.M : Left;
//        var resultingRight = setLeft ? Right : sign.N;
//        return new SetRule (resultingLeft, resultingRight);
//      }

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
//          continueTestArgs = parseArgsLineAsLongs ? line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray() : new long[0];
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

//    public GoogleCodeJam2017Communicator(bool indicator, string fullFolderOverride, string fileName = null)
//    {
//      inputFileName = fileName ?? @"Data.in";
//      inputFilePath = Path.Combine(fullFolderOverride, inputFileName);
//      outputFilePath = Path.Combine(fullFolderOverride, "Data.out");
//    }

//    public GoogleCodeJam2017Communicator(string subFolderName, string fileName = null)
//    {
//      inputFileName = fileName ?? @"Data.in";
//      inputFilePath = Path.Combine(folderPath, subFolderName, inputFileName);
//      outputFilePath = Path.Combine(folderPath, subFolderName, "Data.out");
//    }

//    public IEnumerable<string> ReadStringInput(out int numberOfCases)
//    {
//      var lines = File.ReadLines(inputFilePath);
//      numberOfCases = int.Parse(lines.First());
//      return lines.Skip(1);
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

//  public interface IGoogleCodeJamCommunicator
//  {
//     IEnumerable<string> ReadStringInput(out int numberOfCases);
//     void WriteOutput(IEnumerable<string> lines);
//  }
//}
