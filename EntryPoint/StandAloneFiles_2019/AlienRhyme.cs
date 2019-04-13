//namespace GoogleCodeJam
//{
//  using AlienRhyme;
//  // See README.txt in sln root!!
//  class Program
//  {
//    static void Main(string[] args)
//    {
//      CaseSolver.Run();
//    }
//  }
//}

//namespace AlienRhyme
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  class CaseInput
//  {
//    internal CaseInput(List<string> lines)
//    {
//      N = int.Parse(lines[0]);

//      //ReversedWords = lines.Skip(1).Select(word => new string(word.Reverse().ToArray())).ToArray();
//      ReversedCharArrays = lines.Skip(1).Select(word => word.ToCharArray().Reverse().ToArray()).ToArray();
//      var lengths = ReversedCharArrays.Select(arr => arr.Length).ToList();
//      ShortestWord = lengths.Min();
//      LongestWord = lengths.Max();
//    }

//    internal int N;
//    internal int ShortestWord;
//    internal int LongestWord;
//    internal char[][] ReversedCharArrays;
//    //internal string[] ReversedWords;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(int pairs)
//    {
//      Pairs = pairs;
//    }

//    internal int Pairs;

//    public override string ToString()
//    {
//      return Pairs.ToString();
//    }
//  }

//}
//namespace AlienRhyme
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
//      var lines = InOut.ReadStringInput(out numberOfCases);
//      var cases = new CaseSplitter().GetCaseLines_TakingNFromFirstValPlusOne(lines);
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

//    internal CaseOutput Solve()
//    {
//      var dictionariesByTailLength = new Dictionary<int, Dictionary<string, List<string>>>();
//      var pairCount = new List<string>();
//      for (int i = 1; i < input.LongestWord + 1; i++)
//      {
//        dictionariesByTailLength.Add(i, CreateDictionary(i));
//      }

//      for (int rhymeLength = input.LongestWord; rhymeLength >= 0; rhymeLength--)
//      {
//        Dictionary<string, List<string>> dictionaryByTail;
//        if (!dictionariesByTailLength.TryGetValue(rhymeLength, out dictionaryByTail))
//        {
//          continue;
//        }

//        if (!dictionaryByTail.Any()) { continue; }
//        foreach (var tailGroupContentsToConsider in dictionaryByTail.Values.ToList())
//        {
//          while (tailGroupContentsToConsider.Count > 1)
//          {
//            var rhymeTail = new string(tailGroupContentsToConsider[0].ToCharArray().Take(rhymeLength).ToArray());
//            if (pairCount.Contains(rhymeTail))
//            {
//              tailGroupContentsToConsider.RemoveAt(0);
//              continue;
//            }
//            PurgeFirst(tailGroupContentsToConsider[0], tailGroupContentsToConsider, dictionariesByTailLength);
//            PurgeFirst(tailGroupContentsToConsider[0], tailGroupContentsToConsider, dictionariesByTailLength);

//            pairCount.Add(rhymeTail);
//          }
//        }
//      }
//      return new CaseOutput(pairCount.Count*2);
//    }

//    private void PurgeFirst(string toRemove, List<string> currentIteration, Dictionary<int, Dictionary<string, List<string>>> dictionariesByTailLength , int? purgeMatchingRhymeLength = null)
//    {
//      currentIteration.Remove(toRemove);
//      foreach (var dictionaryByTailLengthKVP in dictionariesByTailLength.ToList())
//      {
//        var length = dictionaryByTailLengthKVP.Key;
//        if(length > toRemove.Length) { continue; }
//        var dictionaryByTail = dictionaryByTailLengthKVP.Value;
//        var purgeTargetTail = new string(toRemove.ToCharArray().Take(length).ToArray());
//        var matchingTailList = dictionaryByTail[purgeTargetTail];
//        matchingTailList.Remove(toRemove);

//        if (!matchingTailList.Any()) { dictionaryByTail.Remove(purgeTargetTail); }
//        if (!dictionaryByTail.Any()) { dictionariesByTailLength.Remove(length); }
//      }

//      if (purgeMatchingRhymeLength != null)
//      {
//        var rhymeTail = new string(toRemove.ToCharArray().Take(purgeMatchingRhymeLength.Value).ToArray());
//        Dictionary<string, List<string>> matchSegmentLengthDictionary;
//        if (dictionariesByTailLength.TryGetValue(purgeMatchingRhymeLength.Value, out matchSegmentLengthDictionary))
//        {
//          List<string> exactRhymeMatches;
//          if (matchSegmentLengthDictionary.TryGetValue(rhymeTail, out exactRhymeMatches))
//          {
//            foreach (var exactRhymeMatch in exactRhymeMatches.ToList())
//            {
//              PurgeFirst(exactRhymeMatch, currentIteration, dictionariesByTailLength);
//            }
//          }
//        }
//      }
//    }

//    private Dictionary<string, List<string>> CreateDictionary(int length)
//    {
//      return input.ReversedCharArrays.Where(arr => arr.Length >= length).ToLookup(
//        arr => new String(arr.Take(length).ToArray()),
//        arr => new string(arr)
//      ).ToDictionary(l => l.Key, l => l.ToList());
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
//    /// In general the first line gets parsed, and those values are the passed to a function that calculates how many more lines to include in the Case.
//    /// </summary>
//    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstVal(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.First());
//    }

//    /// <summary> See summary comment above </summary>
//    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 1);
//    }

//    /// <summary> See summary comment above </summary>
//    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusTwo(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 2);
//    }

//    /// <summary> See summary comment above </summary>
//    public IEnumerable<List<string>> GetCaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
//    {
//      return GetCaseLines(lines, firstLineArray => firstLineArray.Skip(1).First());
//    }

//    /// <summary> See summary comment above </summary>
//    public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
//    {
//      return GetConstantMultiLineCases(lines, 1).Select(caseLines => caseLines.Single());
//    }

//    /// <summary> See summary comment above </summary>
//    public IEnumerable<List<string>> GetConstantMultiLineCases(IEnumerable<string> lines, long numberOfLinesInACase)
//    {
//      return GetCaseLines(lines, args => numberOfLinesInACase, false);
//    }

//    /// <summary> See summary comment above </summary>
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
//namespace Common
//{
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//  public static class Thrower
//  {
//    public enum ResponseType
//    {
//      Memory,
//      Time
//    }

//    public static void TriggerMemLimit()
//    {
//      var size = 620;
//      var newTooBigArray = new long[size, size, size];
//      //Consumes about 1.9GB RAM
//    }

//    public static void TriggerTimeLimit()
//    {
//      System.Threading.Thread.Sleep(120000);
//    }

//    public static void TriggerResponseIfErrors(ResponseType response, Action doStuff)
//    {
//      Func<int> doStuffWithDummyReturn = () =>
//      {
//        doStuff();
//        return 0;
//      };
//      TriggerResponseIfErrors(response, doStuffWithDummyReturn );
//    }

//    public static T TriggerResponseIfErrors<T>(ResponseType response, Func<T> doStuff)
//    {
//      try
//      {
//        return doStuff();
//      }
//      catch
//      {
//        if (response == ResponseType.Memory)
//        {
//          TriggerMemLimit();
//        }
//        else
//        {
//          TriggerTimeLimit();
//        }
//        throw;
//      }
//    }
//  }
//}
