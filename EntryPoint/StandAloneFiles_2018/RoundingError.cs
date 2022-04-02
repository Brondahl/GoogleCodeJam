//namespace RoundingError
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  class CaseInput
//  {
//    internal CaseInput(List<string> lines)
//    {
//      var NLLine = lines[0];
//      var NLArray = NLLine.Split(' ').Select(int.Parse).ToArray();
//      SurveySize = NLArray[0];
//      InitialLangCount = NLArray[1];

//      var CurrentCountsLine = lines[1];
//      InitialVoteCounts = CurrentCountsLine.Split(' ').Select(int.Parse).ToArray();

//      VoteValue = 100.0 / SurveySize;
//      VoteFractionalValue = VoteValue - Math.Floor(VoteValue);
//      RemainingVotes = SurveySize - InitialVoteCounts.Sum();
//    }

//    internal int SurveySize;
//    internal int InitialLangCount;
//    internal int[] InitialVoteCounts;

//    internal double VoteValue;
//    internal double VoteFractionalValue;
//    internal int RemainingVotes;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(int maxSum)
//    {
//      MaxSum = maxSum;
//    }

//    internal int MaxSum;

//    public override string ToString()
//    {
//      return MaxSum.ToString();
//    }
//  }

//}
//namespace RoundingError
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;
//  using Common;

//  public class CaseSolver
//  {
//    private static int numberOfCases;
//    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2018Communicator();
//    public static void Run()
//    {
//      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
//      var cases = new CaseSplitter().GetCaseLines(lines, 2).ToArray();
//      var results = new List<string>();
//      var caseNumber = 0;

//      foreach (var caseLines in cases)
//      {
//        caseNumber++; //1-indexed.
//        var parsedCase = new CaseInput(caseLines);
//        var solver = new CaseSolver(parsedCase);
//        var result = solver.Solve();

//        var resultText = result.ToString();

//        results.Add($"Case #{caseNumber}: {resultText}");
//      }

//      InOut.WriteOutput(results);
//    }

//    private CaseInput input;

//    internal CaseSolver(CaseInput inputCase)
//    {
//      input = inputCase;
//    }

//    private List<double> currentPercentages;
//    private int currentVotesRemaining;

//    internal CaseOutput Solve()
//    {
//      currentVotesRemaining = input.RemainingVotes;
//      currentPercentages = CalculateCurrentPercentages();

//      if (input.RemainingVotes == 0)
//      {
//        var total = CalculateTotalImpliedByPercentages(currentPercentages);
//        return new CaseOutput(total);
//      }

//      if (100 % input.SurveySize == 0)
//      {
//        return new CaseOutput(100);
//      }

//      var fractionalOffsetsFromHalfWithIndex = currentPercentages.Select((perc, index) => new { PercOffset = (perc - Math.Floor(perc) - 0.5), Index = index });
//      foreach (var offsetObject in fractionalOffsetsFromHalfWithIndex.OrderByDescending(offset => offset.PercOffset))
//      {
//        var offset = offsetObject.PercOffset;
//        if (offset > 0)
//        {
//          continue; //nothing to do here.
//        }

//        var votesToAssign = CalculateVotesToAssignToSurpassTarget(offset);
//        AssignVotes(votesToAssign, offsetObject.Index);
//      }

//      var votesToAssignToANewLanguage = CalculateVotesToAssignToSurpassTarget(0.5);
//      while (currentVotesRemaining > 0)
//      {
//        currentPercentages.Add(0);
//        AssignVotes(votesToAssignToANewLanguage, currentPercentages.Count - 1);
//      }

//      var modifiedTotal = CalculateTotalImpliedByPercentages(currentPercentages);
//      return new CaseOutput(modifiedTotal);

//    }

//    private int CalculateVotesToAssignToSurpassTarget(double target)
//    {
//      var votesRequiredToGetPastOffset = (int)(Math.Ceiling(Math.Abs(target) / input.VoteFractionalValue));
//      return votesRequiredToGetPastOffset;
//    }

//    private void AssignVotes(int desiredVotesToAssign, int index)
//    {
//      var votesToActuallyAssign = Math.Min(desiredVotesToAssign, currentVotesRemaining);
//      currentPercentages[index] += votesToActuallyAssign * input.VoteValue;
//      currentVotesRemaining -= votesToActuallyAssign;
//    }

//    private List<double> CalculateCurrentPercentages()
//    {
//      return input.InitialVoteCounts.Select(count => count * input.VoteValue).ToList();
//    }

//    private int CalculateTotalImpliedByPercentages(List<double> percentages)
//    {
//      return percentages.Select(doublePercentage => ((int)(Math.Round(doublePercentage, MidpointRounding.AwayFromZero)))).Sum();
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
//    IEnumerable<string> ReadStringInput(out int numberOfCases);
//    void WriteOutput(IEnumerable<string> lines);
//  }
//}
//namespace GoogleCodeJam
//{
//  using System.Linq;

//  class Program
//  {
//    static void Main(string[] args)
//    {
//      RoundingError.CaseSolver.Run();
//    }
//  }
//}
