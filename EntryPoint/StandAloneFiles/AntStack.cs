//namespace AntStack
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  class CaseInput
//  {
//    internal CaseInput()
//    {
//    }

//    internal CaseInput(List<string> lines)
//    {
//      N = int.Parse(lines[0]);
//      AntWeights = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
//    }

//    internal int N;
//    internal long[] AntWeights;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(int maxCount)
//    {
//      this.maxCount = maxCount;
//    }

//    internal int maxCount;

//    public override string ToString()
//    {
//      return maxCount == -1 ? "OK" : maxCount.ToString();
//    }
//  }

//}
//namespace AntStack
//{
//  using System.Collections.Generic;
//  using System.Linq;
//  using System.Runtime.InteropServices;
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

//    internal CaseOutput Solve()
//    {
//      //We're going to use 1-indexing through-out!!
//      var countsWeights = new List<long>();
//      countsWeights.Add(0);

//      for (int i = 0; i < input.N; i++)
//      {
//        var currentAnt = input.AntWeights[i];
//        if (i == 0)
//        {
//          countsWeights.Add(currentAnt);
//          continue;
//        }
//        else
//        {
//          var lengthOfLongestAntStack = countsWeights.Count - 1;
//          var weightOfLongestAntStack = countsWeights[lengthOfLongestAntStack];
//          if (currentAnt.CanCarry(weightOfLongestAntStack))
//          {
//            countsWeights.Add(weightOfLongestAntStack + currentAnt);
//          }

//          for (int newFinalStackLengthToAttempt = lengthOfLongestAntStack/*!! -1 ... we've already done longest, above!!*/; newFinalStackLengthToAttempt >= 1 /*!! Use the empty Stack!!*/; newFinalStackLengthToAttempt--)
//          {
//            var lengthOfAntStackToAddTo = newFinalStackLengthToAttempt - 1;
//            var weightOfAntStackToAddTo = countsWeights[lengthOfAntStackToAddTo];
//            if (currentAnt.CanCarry(weightOfAntStackToAddTo))
//            {
//              var impliedNewWeight = weightOfAntStackToAddTo + currentAnt;
//              var currentBestWeightForThisStackLenght = countsWeights[newFinalStackLengthToAttempt];
//              if (impliedNewWeight < currentBestWeightForThisStackLenght)
//              {
//                countsWeights[newFinalStackLengthToAttempt] = impliedNewWeight;
//              }
//            }
//          }
//        }
//      }
//      //We're using 1-indexing!!
//      return new CaseOutput(countsWeights.Count - 1);
//    }

//  }

//  public static class AntExtension
//  {
//    public static bool CanCarry(this long thisAnt, long someWeight)
//    {
//      return someWeight <= thisAnt * 6;
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
//  using System;
//  using System.Collections.Generic;
//  using System.IO;
//  using System.Linq;
//  using AntStack;

//  class Program
//  {
//    static void Main(string[] args)
//    {
//      CaseSolver.Run();
//    }

//  }
//}

