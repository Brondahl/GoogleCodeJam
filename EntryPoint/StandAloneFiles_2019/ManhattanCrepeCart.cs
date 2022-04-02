//namespace GoogleCodeJam
//{
//  using ManhattanCrepeCart;
//  using Common;
//  // See README.txt in sln root!!

//  // Remember to add the new csproj,
//  // and to add the proj ref and the
//  // one-off fild to the EP project.
//  class Program
//  {
//    static void Main(string[] args)
//    {
//      CaseSolver.Run();
//    }
//  }
//}

//namespace ManhattanCrepeCart
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  class CaseInput
//  {
//    internal CaseInput(List<string> lines)
//    {
//      var line1 = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
//      P = line1.First();
//      Q = line1.Last();

//      var people = lines.Skip(1).Select(person => person.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();

//      foreach (var person in people)
//      {
//        switch (person[2].ToCharArray().Single())
//        {
//          case 'N':
//            NPeople.Add(int.Parse(person[1]));
//            break;
//          case 'E':
//            EPeople.Add(int.Parse(person[0]));
//            break;
//          case 'S':
//            SPeople.Add(int.Parse(person[1]));
//            break;
//          case 'W':
//            WPeople.Add(int.Parse(person[0]));
//            break;
//        }
//      }
//      NPeople = NPeople.OrderBy(x => x).ToList();
//      EPeople = EPeople.OrderBy(x => x).ToList();
//      SPeople = SPeople.OrderBy(x => x).ToList();
//      WPeople = WPeople.OrderBy(x => x).ToList();
//    }

//    internal int P;
//    internal int Q;
//    internal List<int> NPeople = new List<int>();
//    internal List<int> EPeople = new List<int>();
//    internal List<int> SPeople = new List<int>();
//    internal List<int> WPeople = new List<int>();
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(int x, int y)
//    {
//      X = x;
//      Y = y;
//    }

//    internal int X;
//    internal int Y;


//    public override string ToString()
//    {
//      return $"{X} {Y}";
//    }
//  }

//}
//namespace ManhattanCrepeCart
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;
//  using Common;

//  public class CaseSolver
//  {
//    private static int numberOfCases;
//    private static IGoogleCodeJamCommunicator InOut;
//    public static void Run(IGoogleCodeJamCommunicator io = null)
//    {
//      InOut = io ?? new GoogleCodeJam2018Communicator();
//      var lines = InOut.ReadStringInput(out numberOfCases);
//      var cases = new CaseSplitter().Configure_TakingNFromFirstValPlusOne().GetCaseLines(lines);
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
//      var nCumulative = SetCumulative(input.NPeople, input.Q);
//      var eCumulative = SetCumulative(input.EPeople, input.Q);
//      var sCumulative = SetCumulative(input.SPeople, input.Q);
//      var wCumulative = SetCumulative(input.WPeople, input.Q);

//      var bestPopularity = 0;
//      var bestLocation = Tuple.Create(0,0);

//      for (int x = 0; x < input.Q+1; x++)
//      {
//        for (int y = 0; y < input.Q+1; y++)
//        {
//          var pop = Popularity(nCumulative, sCumulative, eCumulative, wCumulative, x, y, input.Q);
//          if (pop > bestPopularity)
//          {
//            bestLocation = Tuple.Create(x,y);
//            bestPopularity = pop;
//          }
//        }
//      }
//      return new CaseOutput(bestLocation.Item1, bestLocation.Item2);
//    }

//    public static int Popularity(
//      Dictionary<int, int> nPeopleDict,
//      Dictionary<int, int> sPeopleDict,
//      Dictionary<int, int> ePeopleDict,
//      Dictionary<int, int> wPeopleDict,
//      int x, int y,
//      int q)
//    {
//      return Popularity(ePeopleDict, wPeopleDict, x, q) + Popularity(nPeopleDict, sPeopleDict, y, q);
//    }

//    public static int Popularity(
//      Dictionary<int, int> posPeopleDict,
//      Dictionary<int, int> negPeopleDict,
//      int coord,
//      int q)
//    {
//      var totalPos = posPeopleDict[q];
//      var totalNeg = negPeopleDict[q];

//      // set for 0;
//      if (coord == 0)
//      {
//        return totalNeg;
//      }

//      // set for q;
//      if (coord == q)
//      {
//        return totalPos;
//      }

//      //set for 1 ... q-1
//      var posTowards = posPeopleDict[coord - 1];
//      var negTowards = totalNeg - negPeopleDict[coord];
//      return posTowards + negTowards;
//    }

//    public static Dictionary<int, int> SetCumulative(List<int> inputPeople, int max)
//    {
//      var cumDict = new Dictionary<int, int>();
//      var cumCount = 0;
//      foreach (var x in inputPeople)
//      {
//        cumCount++;
//        cumDict[x] = cumCount;
//      }
//      FillDictionary(cumDict, max);
//      return cumDict;
//    }

//    public static void FillDictionary(Dictionary<int, int> sparseDict, int max)
//    {
//      var dictLatest = 0;
//      for (int i = 0; i < max+1; i++)
//      {
//        if (sparseDict.ContainsKey(i))
//        {
//          dictLatest = sparseDict[i];
//        }
//        else
//        {
//          sparseDict[i] = dictLatest;
//        }
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
