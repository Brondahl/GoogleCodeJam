namespace GoogleCodeJam
{
  using Cryptopangrams;
  // See README.txt in sln root!!
  class Program
  {
    static void Main(string[] args)
    {
      CaseSolver.Run();
    }
  }
}

namespace Cryptopangrams
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      N = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).First();
      Numbers = lines[1].Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
    }

    internal int N;
    internal long[] Numbers;
  }

  class CaseOutput
  {
    public readonly string Decryption;

    internal CaseOutput(string decryption)
    {
      Decryption = decryption;
    }

    public override string ToString()
    {
      return Decryption;
    }
  }

}
namespace Cryptopangrams
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  public class CaseSolver
  {
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2018Communicator();
    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases);
      var cases = new CaseSplitter().GetConstantMultiLineCases(lines, 2);
      var results = new List<string>();
      var caseNumber = 0;

      foreach (var caseLines in cases)
      {
        caseNumber++; //1-indexed.
        var parsedCase = new CaseInput(caseLines);
        var solver = new CaseSolver(parsedCase);
        CaseOutput result = null;
        try
        {
          result = solver.Solve();
        }
        catch (Exception e)
        {
          System.Threading.Thread.Sleep(25000);
        }

        var resultText = result.ToString();

        results.Add($"Case #{caseNumber}: {resultText}");
      }

      InOut.WriteOutput(results);
    }

    private CaseInput input;

    internal CaseSolver(CaseInput inputCase)
    {
      input = inputCase;
    }

    internal CaseOutput Solve()
    {
      long lowestCompositeNumber = long.MaxValue;
      int locatedIndex = 0;
      long[] underlyingPrimes = new long[input.Numbers.Length + 1];

      for (int i = 0; i < input.Numbers.Length; i++)
      {
        if (input.Numbers[i] < lowestCompositeNumber)
        {
          lowestCompositeNumber = input.Numbers[i];
          locatedIndex = i;
        }
      }

      var primes = DecomposeSemiPrime(lowestCompositeNumber);
      var prime1 = primes.Item1;
      var prime2 = primes.Item2;

      long previousPrime, nextPrime;
      if (locatedIndex == 0)
      {
        if (input.Numbers[locatedIndex + 1] % prime1 == 0)
        {
          nextPrime = prime1;
          previousPrime = prime2;
        }
        else
        {
          previousPrime = prime1;
          nextPrime = prime2;
        }
      }
      else
      {
        if (input.Numbers[locatedIndex - 1] % prime1 == 0)
        {
          previousPrime = prime1;
          nextPrime = prime2;
        }
        else
        {
          nextPrime = prime1;
          previousPrime = prime2;
        }
      }

      underlyingPrimes[locatedIndex] = previousPrime;
      underlyingPrimes[locatedIndex + 1] = nextPrime;

      for (int index = locatedIndex; index < input.Numbers.Length - 1; index++)
      {
        var nextSemiPrime = input.Numbers[index + 1];
        var identifiedPrimeFactor = underlyingPrimes[index + 1];
        underlyingPrimes[index + 2] = nextSemiPrime / identifiedPrimeFactor;
      }

      for (int index = locatedIndex; index > 0; index--)
      {
        var prevSemiPrime = input.Numbers[index - 1];
        var identifiedPrimeFactor = underlyingPrimes[index];
        underlyingPrimes[index - 1] = prevSemiPrime / identifiedPrimeFactor;
      }

      var translationLookup = underlyingPrimes.Distinct().OrderBy(x => x).Select((x, index) => new {x, index}).ToDictionary(
        xWithIndex => xWithIndex.x,
        xWithIndex => "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[xWithIndex.index]
      );

      var decryption = new string(underlyingPrimes.Select(prime => translationLookup[prime]).ToArray());


      return new CaseOutput(decryption);
    }

    private Tuple<long,long> DecomposeSemiPrime(long semiPrime)
    {
      if (semiPrime % 2 == 0)
      {
        return Tuple.Create(2L, semiPrime / 2);
      }

      for (long candidateDivisor = 3; candidateDivisor < Math.Sqrt(semiPrime); candidateDivisor = candidateDivisor + 2)
      {
        if (semiPrime % candidateDivisor == 0)
        {
          return Tuple.Create(candidateDivisor, semiPrime / candidateDivisor);
        }
      }
      throw new Exception("Impossible!");
    }
  }
}
namespace Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class CaseSplitter
  {
    /// <summary>
    /// These assume that the first line of each case will be a sequence of longs,
    /// from which you can deduce the number of other lines in the case.
    /// In general the first line gets parsed, and those values are the passed to a function that calculates how many more lines to include in the Case.
    /// </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstVal(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusOne(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 1);
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromFirstValPlusTwo(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.First() + 2);
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines_TakingNFromSecondVal(IEnumerable<string> lines)
    {
      return GetCaseLines(lines, firstLineArray => firstLineArray.Skip(1).First());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
    {
      return GetConstantMultiLineCases(lines, 1).Select(caseLines => caseLines.Single());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetConstantMultiLineCases(IEnumerable<string> lines, long numberOfLinesInACase)
    {
      return GetCaseLines(lines, args => numberOfLinesInACase, false);
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines, Func<long[], long> totalNumberOfLinesInACase, bool parseArgsLineAsLongs = true)
    {
      var caseSet = new List<string>();
      long[] continueTestArgs = null;
      var currentLineCount = 0;
      long numberOfLinesToPutInCurrentCase = 0;

      foreach (var line in lines)
      {
        caseSet.Add(line);
        currentLineCount++;

        if (continueTestArgs == null)
        {
          continueTestArgs = parseArgsLineAsLongs ? line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray() : new long[0];
          numberOfLinesToPutInCurrentCase = totalNumberOfLinesInACase(continueTestArgs);
        }

        if (currentLineCount < numberOfLinesToPutInCurrentCase)
        {
          continue;
        }

        // We've reached the end of the CaseSet.
        // Return this caseSet.
        yield return caseSet.ToList();

        // And reset our counters
        caseSet = new List<string>();
        continueTestArgs = null;
        currentLineCount = 0;
        numberOfLinesToPutInCurrentCase = 0;
      }

    }
  }
}
namespace Common
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class GoogleCodeJam2017Communicator : IGoogleCodeJamCommunicator
  {
    private readonly string folderPath = @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\GoogleCodeJam2017\";
    private readonly string inputFileName;
    private readonly string inputFilePath;
    private readonly string outputFilePath;

    public GoogleCodeJam2017Communicator(bool indicator, string fullFolderOverride, string fileName = null)
    {
      inputFileName = fileName ?? @"Data.in";
      inputFilePath = Path.Combine(fullFolderOverride, inputFileName);
      outputFilePath = Path.Combine(fullFolderOverride, "Data.out");
    }

    public GoogleCodeJam2017Communicator(string subFolderName, string fileName = null)
    {
      inputFileName = fileName ?? @"Data.in";
      inputFilePath = Path.Combine(folderPath, subFolderName, inputFileName);
      outputFilePath = Path.Combine(folderPath, subFolderName, "Data.out");
    }

    public IEnumerable<string> ReadStringInput(out int numberOfCases)
    {
      var lines = File.ReadLines(inputFilePath);
      numberOfCases = int.Parse(lines.First());
      return lines.Skip(1);
    }

    public void WriteOutput(IEnumerable<string> lines)
    {
      File.WriteAllLines(outputFilePath, lines.ToArray());
    }
  }
}
namespace Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class GoogleCodeJam2018Communicator : IGoogleCodeJamCommunicator
  {
    public IEnumerable<string> ReadStringInput(out int numberOfCases)
    {
      var lines = ReadStringInputAsIterator();

      var firstLine = lines.Take(1).Single();
      numberOfCases = int.Parse(firstLine);

      return lines;
    }

    private IEnumerable<string> ReadStringInputAsIterator()
    {
      while (true)
      {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line)) { break; }
        yield return (line);
      }
    }

    public void WriteOutput(IEnumerable<string> lines)
    {
      foreach (var line in lines)
      {
        Console.WriteLine(line);
      }
    }
  }
}
namespace Common
{
  using System.Collections.Generic;

  public interface IGoogleCodeJamCommunicator
  {
     IEnumerable<string> ReadStringInput(out int numberOfCases);
     void WriteOutput(IEnumerable<string> lines);
  }
}
