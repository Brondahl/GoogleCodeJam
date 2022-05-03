namespace GoogleCodeJam
{
  using Squary;
  using Common;
  // See README.txt in sln root!!

  // Remember to add the new csproj,
  // and to add the proj ref and the
  // one-off fild to the EP project.
  class Program
  {
    static void Main(string[] args)
    {
      CaseSolver.Run();
    }
  }
}

namespace Squary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            var values = lines.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            N = values[0];
            K = values[1];
            inputs = lines.Last().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }

        internal long N;
        internal long K;

        internal long[] inputs;
    }

    class CaseOutput
    {
        internal static CaseOutput Impossible => new CaseOutput(0){Text = "IMPOSSIBLE"};

        internal CaseOutput(long answer)
        {
            Text = answer.ToString();
        }

        internal CaseOutput(long[] answers)
        {
            Text = string.Join(" ", answers.Select(x => x.ToString()));
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
namespace Squary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;

    public class CaseSolver
    {
        private static int numberOfCases;
        private static IGoogleCodeJamCommunicator InOut;
        public static void Run(IGoogleCodeJamCommunicator io = null)
        {
            InOut = io ?? new GoogleCodeJam2018Communicator();
            var lines = InOut.ReadStringInput(out numberOfCases);
            var cases = new CaseSplitter().Configure_ConstantMultiLineCases(2).GetCaseLines(lines);
            var results = ProcessCases(cases);
            InOut.WriteOutput(results);
        }

        private static IEnumerable<string> ProcessCases(IEnumerable<List<string>> cases)
        {
            var currentCaseNumber = 0;
            foreach (var caseLines in cases)
            {
                currentCaseNumber++; //1-indexed.
                var parsedCase = new CaseInput(caseLines);
                var solver = new CaseSolver(parsedCase);
                var result = solver.Solve();

                var resultText = result.ToString();

                yield return $"Case #{currentCaseNumber}: {resultText}";
            }
        }

        private CaseInput input;

        internal CaseSolver(CaseInput inputCase)
        {
            input = inputCase;
        }

        internal CaseOutput Solve()
        {
            var oneOut = SolveWith1();
            if (oneOut.Text != "IMPOSSIBLE")
            {
                return oneOut;
            }
            else
            {
                var twoOut = SolveWith2();
                return twoOut;
            }
        }

        internal CaseOutput SolveWith1()
        {
            var sum = input.inputs.Sum();
            var sumOfSquares = input.inputs.Select(Square).Sum();
            var squareOfSum = Square(sum);

            var top = sumOfSquares - squareOfSum;
            var bottom = 2 * sum;

            if (top == 0)
            {
                return new CaseOutput(0);
            }
            if (bottom == 0)
            {
                return CaseOutput.Impossible;
            }
            var answer = top / bottom;
            if (top.IsDivisibleBy(bottom))
            {
                return new CaseOutput(answer);
            }
            else
            {
                return CaseOutput.Impossible;
            }
        }
        internal CaseOutput SolveWith2()
        {
            var sum = input.inputs.Sum();
            var sumOfSquares = input.inputs.Select(Square).Sum();
            var squareOfSum = Square(sum);

            var B = 1 - sum;
            var C = Square(B);

            var top = sumOfSquares + C - 1;
            var bottom = 2;

            if (top == 0)
            {
                return new CaseOutput(new []{0, B});
            }
            if (bottom == 0)
            {
                return CaseOutput.Impossible;
            }
            var answer = top / bottom;
            if (top.IsDivisibleBy(bottom))
            {
                return new CaseOutput(new[] { answer, B });
            }
            else
            {
                return CaseOutput.Impossible;
            }
        }

        private long Square(long input) => input * input;
        private long IsDivisibleBy(long input) => input * input;

    }

    public static class MathExtensions
    {
        public static bool IsDivisibleBy(this long left, long right)
        {
            long multiple = left / right;
            long remultiplyToGetLeft = right * multiple;

            return (left == remultiplyToGetLeft);
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
      private Func<long[], long> getTotalNumberOfLinesInACase;
      private bool shouldParseFirstLineAsLongs = true;
    /// <summary>
    /// These assume that the first line of each case will be a sequence of longs,
    /// from which you can deduce the number of other lines in the case.
    /// In general the first line gets parsed, and those values are the passed to a function that calculates how many more lines to include in the Case.
    ///
    /// **N includes the first line, which provided N**, thus GetCaseLines_TakingNFromFirstValPlusOne is the most common variant.
    /// </summary>
    public CaseSplitter Configure_TakingNFromFirstVal()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First();
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_TakingNFromFirstValPlusOne()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First()+1;
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_TakingNFromFirstValPlusTwo()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First() + 2;
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_TakingNFromSecondVal()
    {
        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.Skip(1).First();
        return this;
    }

    /// <summary> See summary comment above </summary>
    public CaseSplitter Configure_ConstantMultiLineCases(long numberOfLinesInACase)
    {
        getTotalNumberOfLinesInACase = _ => numberOfLinesInACase;
        shouldParseFirstLineAsLongs = false;
        return this;
    }

    public CaseSplitter Configure_CustomMap(Func<long[], long> customFuncForTotalNumberOfLinesInACase)
    {
        getTotalNumberOfLinesInACase = customFuncForTotalNumberOfLinesInACase;
        shouldParseFirstLineAsLongs = true;
        return this;
    }

        public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
    {
        Configure_ConstantMultiLineCases(1);
        return GetCaseLines(lines).Select(caseLines => caseLines.Single());
    }

    /// <summary> See summary comment above </summary>
    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines)
    {
      var linesInCurrentCase = new List<string>();
      long numberOfLinesToPutInCurrentCase = 0;

      foreach (var line in lines)
      {
        linesInCurrentCase.Add(line);

        if (linesInCurrentCase.Count == 1)
        {
            if (shouldParseFirstLineAsLongs)
            {
                var firstLineLongs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                numberOfLinesToPutInCurrentCase = getTotalNumberOfLinesInACase(firstLineLongs);
            }
            else
            {
                numberOfLinesToPutInCurrentCase = getTotalNumberOfLinesInACase(new long[0]);
            }
        }

        if (linesInCurrentCase.Count < numberOfLinesToPutInCurrentCase)
        {
          continue;
        }

        // We've reached the end of the CaseSet.
        // Return this caseSet.
        yield return linesInCurrentCase.ToList();

        // And reset our counters
        linesInCurrentCase = new List<string>();
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
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class GoogleCodeJam2022CommunicatorWithInteractivity : IGoogleCodeJamCommunicator, IGoogleCodeJamInteractiveCommunicator
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

    public string ReadSingleStringInput()
    {
        return Console.ReadLine();
    }

    public long ReadSingleLongInput()
    {
        return long.Parse(ReadSingleStringInput());
    }

    public List<long> ReadSingleLineOfLongsInput()
    {
        return ReadSingleStringInput().Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
    }

    public List<string> ReadNextStringInputs(int linesToRead)
    {
        return ReadNextStringInputs_Internal(linesToRead).ToList();
    }

    private IEnumerable<string> ReadNextStringInputs_Internal(int linesToRead)
    {
        for (int i = 0; i < linesToRead; i++)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) { throw new Exception("Couldn't read expected number of lines"); }
            yield return (line);
        }
    }

    public void WriteSingleInteractiveOutput(string line)
    {
        WriteInteractiveOutput(new List<string>{line});
    }

    public void WriteInteractiveOutput(List<string> lines)
    {
        WriteOutput(lines);
        Console.Out.Flush();
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

  public interface IGoogleCodeJamInteractiveCommunicator
  {
      long ReadSingleLongInput();
      List<long> ReadSingleLineOfLongsInput();
      string ReadSingleStringInput();
      List<string> ReadNextStringInputs(int linesToRead);
      void WriteSingleInteractiveOutput(string line);
      void WriteInteractiveOutput(List<string> lines);
  }
}
namespace Common
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

  public static class Thrower
  {
    public enum ResponseType
    {
      Memory,
      Time
    }

    public static void TriggerMemLimit()
    {
      var size = 620;
      var newTooBigArray = new long[size, size, size];
      //Consumes about 1.9GB RAM
    }

    public static void TriggerTimeLimit()
    {
      System.Threading.Thread.Sleep(120000);
    }

    public static void TriggerResponseIfErrors(ResponseType response, Action doStuff)
    {
      Func<int> doStuffWithDummyReturn = () =>
      {
        doStuff();
        return 0;
      };
      TriggerResponseIfErrors(response, doStuffWithDummyReturn );
    }

    public static T TriggerResponseIfErrors<T>(ResponseType response, Func<T> doStuff)
    {
      try
      {
        return doStuff();
      }
      catch
      {
        if (response == ResponseType.Memory)
        {
          TriggerMemLimit();
        }
        else
        {
          TriggerTimeLimit();
        }
        throw;
      }
    }
  }
}
