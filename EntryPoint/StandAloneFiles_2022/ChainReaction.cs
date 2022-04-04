//namespace GoogleCodeJam
//{
//  using ChainReaction;
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

//namespace ChainReaction
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;

//    class CaseInput
//    {
//        public readonly List<string> lines;

//        internal CaseInput(List<string> lines)
//        {
//            this.lines = lines;
//            N = int.Parse(lines[0]);
//            var funs = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
//            var targets = lines[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

//            for (int arrayIndex = 0; arrayIndex < N; arrayIndex++)
//            {
//                var targetGraphModule = targets[arrayIndex] > 0 ? Modules[targets[arrayIndex]] : null;

//                var newGraphModule = new GraphModule
//                {
//                    Id = arrayIndex+1,
//                    Fun = funs[arrayIndex],
//                    Target = targetGraphModule
//                };

//                targetGraphModule?.Triggers.Add(newGraphModule);
//                Modules.Add(arrayIndex+1, newGraphModule);
//            }

//            foreach (var module in Modules.Values)
//            {
//                if (module.Target == null)
//                {
//                    AbyssalModules.Add(module);
//                }

//                if (!module.Triggers.Any())
//                {
//                    Initiators.Add(module);
//                }
//            }
//        }

//        internal int N;
//        internal Dictionary<int, GraphModule> Modules = new Dictionary<int, GraphModule>();
//        internal List<GraphModule> Initiators = new List<GraphModule>();
//        internal List<GraphModule> AbyssalModules = new List<GraphModule>();
//    }


//    public class GraphModule
//    {
//        public int Id;
//        public long Fun;
//        public GraphModule Target;
//        public List<GraphModule> Triggers = new List<GraphModule>();
//        public long MaxAssociatedFunDuringCalculation = 0;
//        public long CheapestTriggerDuringCalculation = 0;
//    }


//    class CaseOutput
//    {
//        internal CaseOutput(long answer)
//        {
//            Text = answer.ToString();
//        }

//        internal string Text;

//        public override string ToString()
//        {
//            return Text;
//        }
//    }

//}
//namespace ChainReaction
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using Common;

//    public class CaseSolver
//    {
//        private static int numberOfCases;
//        private static IGoogleCodeJamCommunicator InOut;
//        public static void Run(IGoogleCodeJamCommunicator io = null)
//        {
//            InOut = io ?? new GoogleCodeJam2018Communicator();
//            var lines = InOut.ReadStringInput(out numberOfCases);
//            var cases = new CaseSplitter().Configure_ConstantMultiLineCases(3).GetCaseLines(lines);
//            var results = ProcessCases(cases);
//            InOut.WriteOutput(results);
//        }

//        private static IEnumerable<string> ProcessCases(IEnumerable<List<string>> cases)
//        {
//            var currentCaseNumber = 0;
//            foreach (var caseLines in cases)
//            {
//                currentCaseNumber++; //1-indexed.
//                var parsedCase = new CaseInput(caseLines);
//                var solver = new CaseSolver(parsedCase);
//                var result = solver.Solve();

//                var resultText = result.ToString();

//                yield return $"Case #{currentCaseNumber}: {resultText}";
//            }
//        }

//        private CaseInput input;

//        internal CaseSolver(CaseInput inputCase)
//        {
//            input = inputCase;
//        }

//        internal CaseOutput Solve()
//        {
//            var brute = BruteForce();
//            var initiationSequence = GetOptimalInitiationSequence();
//            var clever = ScoreInitiationSequence(initiationSequence);

//            if (brute != clever)
//            {
//                throw new Exception(string.Join(Environment.NewLine ,input.lines));
//            }

//            return new CaseOutput(clever);
//        }

//        private long BruteForce()
//        {
//            var inits = input.Initiators.ToList();
//            var perms = GetPermutations(inits).Select(perm => perm.ToList()).ToList();
//            return perms.Select(possibleAnswer => ScoreInitiationSequence(possibleAnswer.ToList())).Max();
//        }

//        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> enumerable)
//        {
//            var array = enumerable as T[] ?? enumerable.ToArray();

//            var factorials = Enumerable.Range(0, array.Length + 1)
//                .Select(Factorial)
//                .ToArray();

//            for (var i = 0L; i < factorials[array.Length]; i++)
//            {
//                var sequence = GenerateSequence(i, array.Length - 1, factorials);

//                yield return GeneratePermutation(array, sequence);
//            }
//        }

//        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
//        {
//            var clone = (T[])array.Clone();

//            for (int i = 0; i < clone.Length - 1; i++)
//            {
//                Swap(ref clone[i], ref clone[i + sequence[i]]);
//            }

//            return clone;
//        }

//        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
//        {
//            var sequence = new int[size];

//            for (var j = 0; j < sequence.Length; j++)
//            {
//                var facto = factorials[sequence.Length - j];

//                sequence[j] = (int)(number / facto);
//                number = (int)(number % facto);
//            }

//            return sequence;
//        }

//        static void Swap<T>(ref T a, ref T b)
//        {
//            T temp = a;
//            a = b;
//            b = temp;
//        }

//        private static long Factorial(int n)
//        {
//            long result = n;

//            for (int i = 1; i < n; i++)
//            {
//                result = result * i;
//            }

//            return result;
//        }
//        private List<GraphModule> GetOptimalInitiationSequence()
//        {
//            var sequence = new List<GraphModule>();
            
//            foreach (var abyssalModule in input.AbyssalModules)
//            {
//                GetAndPopulateCheapestTriggerFunValues(abyssalModule);
//                GetAndPopulateMaxFunValues(abyssalModule);
//                sequence.AddRange(GetOptimalInitiationSequence(abyssalModule));
//            }

//            return sequence;
//        }

//        private long GetAndPopulateMaxFunValues(GraphModule module)
//        {
//            if (!module.Triggers.Any())
//            {
//                module.MaxAssociatedFunDuringCalculation = module.Fun;
//            }
//            else
//            {
//                var maxAmongTriggers = module.Triggers.Select(trig => GetAndPopulateMaxFunValues(trig)).Max();
//                module.MaxAssociatedFunDuringCalculation = Math.Max(module.Fun, maxAmongTriggers);

//            }
//            return module.MaxAssociatedFunDuringCalculation;
//        }

//        private long GetAndPopulateCheapestTriggerFunValues(GraphModule module)
//        {
//            if (!module.Triggers.Any())
//            {
//                module.CheapestTriggerDuringCalculation = module.Fun;
//            }
//            else
//            {
//                var minAmongTriggers = module.Triggers.Select(trig => GetAndPopulateCheapestTriggerFunValues(trig)).Min();
//                module.CheapestTriggerDuringCalculation = Math.Max(module.Fun, minAmongTriggers);
//            }
//            return module.CheapestTriggerDuringCalculation;
//        }

//        private List<GraphModule> GetOptimalInitiationSequence(GraphModule module)
//        {
//            return GetOptimalInitiationSequence_ByLowestMinValue(module);
//        }

//        private List<GraphModule> GetOptimalInitiationSequence_ByLowestMinValue(GraphModule module)
//        {
//            if (!module.Triggers.Any())
//            {
//                return new List<GraphModule> { module };
//            }

//            var orderedTriggers = module.Triggers.OrderBy(trig => trig.CheapestTriggerDuringCalculation).ToList();

//            var result = new List<GraphModule>();
//            foreach (var trigger in orderedTriggers)
//            {
//                result.AddRange(GetOptimalInitiationSequence(trigger));
//            }

//            return result;
//        }

//        private List<GraphModule> GetOptimalInitiationSequence_ByLowestMaxValue(GraphModule module)
//        {
//            if (!module.Triggers.Any())
//            {
//                return new List<GraphModule>{module};
//            }

//            var orderedTriggers = module.Triggers.OrderBy(trig => trig.MaxAssociatedFunDuringCalculation).ToList();

//            var result = new List<GraphModule>();
//            foreach (var trigger in orderedTriggers)
//            {
//                result.AddRange(GetOptimalInitiationSequence(trigger));
//            }

//            return result;
//        }

//        public long ScoreInitiationSequence(List<GraphModule> sequence)
//        {
//            var triggeredModules = new HashSet<GraphModule> { null };

//            long total = 0;
//            foreach (var module in sequence)
//            {
//                var score = Trigger(module, triggeredModules, 0);
//                total += score;
//            }

//            return total;
//        }

//        private long Trigger(GraphModule module, HashSet<GraphModule> triggeredModules, long funToDate)
//        {
//            var updatedFun = Math.Max(module.Fun, funToDate);
//            triggeredModules.Add(module);

//            if (triggeredModules.Contains(module.Target))
//            {
//                return updatedFun;
//            }

//            var finalFun = Trigger(module.Target, triggeredModules, updatedFun);
//            return finalFun;
//        }
//    }
//}
//namespace Common
//{
//  using System;
//  using System.Collections.Generic;
//  using System.Linq;

//  public class CaseSplitter
//  {
//      private Func<long[], long> getTotalNumberOfLinesInACase;
//      private bool shouldParseFirstLineAsLongs = true;
//    /// <summary>
//    /// These assume that the first line of each case will be a sequence of longs,
//    /// from which you can deduce the number of other lines in the case.
//    /// In general the first line gets parsed, and those values are the passed to a function that calculates how many more lines to include in the Case.
//    ///
//    /// **N includes the first line, which provided N**, thus GetCaseLines_TakingNFromFirstValPlusOne is the most common variant.
//    /// </summary>
//    public CaseSplitter Configure_TakingNFromFirstVal()
//    {
//        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First();
//        return this;
//    }

//    /// <summary> See summary comment above </summary>
//    public CaseSplitter Configure_TakingNFromFirstValPlusOne()
//    {
//        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First()+1;
//        return this;
//    }

//    /// <summary> See summary comment above </summary>
//    public CaseSplitter Configure_TakingNFromFirstValPlusTwo()
//    {
//        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.First() + 2;
//        return this;
//    }

//    /// <summary> See summary comment above </summary>
//    public CaseSplitter Configure_TakingNFromSecondVal()
//    {
//        getTotalNumberOfLinesInACase = firstLineArray => firstLineArray.Skip(1).First();
//        return this;
//    }

//    /// <summary> See summary comment above </summary>
//    public CaseSplitter Configure_ConstantMultiLineCases(long numberOfLinesInACase)
//    {
//        getTotalNumberOfLinesInACase = _ => numberOfLinesInACase;
//        shouldParseFirstLineAsLongs = false;
//        return this;
//    }

//    public CaseSplitter Configure_CustomMap(Func<long[], long> customFuncForTotalNumberOfLinesInACase)
//    {
//        getTotalNumberOfLinesInACase = customFuncForTotalNumberOfLinesInACase;
//        shouldParseFirstLineAsLongs = true;
//        return this;
//    }

//        public IEnumerable<string> GetSingleLineCases(IEnumerable<string> lines)
//    {
//        Configure_ConstantMultiLineCases(1);
//        return GetCaseLines(lines).Select(caseLines => caseLines.Single());
//    }

//    /// <summary> See summary comment above </summary>
//    public IEnumerable<List<string>> GetCaseLines(IEnumerable<string> lines)
//    {
//      var linesInCurrentCase = new List<string>();
//      long numberOfLinesToPutInCurrentCase = 0;

//      foreach (var line in lines)
//      {
//        linesInCurrentCase.Add(line);

//        if (linesInCurrentCase.Count == 1)
//        {
//            if (shouldParseFirstLineAsLongs)
//            {
//                var firstLineLongs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
//                numberOfLinesToPutInCurrentCase = getTotalNumberOfLinesInACase(firstLineLongs);
//            }
//            else
//            {
//                numberOfLinesToPutInCurrentCase = getTotalNumberOfLinesInACase(new long[0]);
//            }
//        }

//        if (linesInCurrentCase.Count < numberOfLinesToPutInCurrentCase)
//        {
//          continue;
//        }

//        // We've reached the end of the CaseSet.
//        // Return this caseSet.
//        yield return linesInCurrentCase.ToList();

//        // And reset our counters
//        linesInCurrentCase = new List<string>();
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
