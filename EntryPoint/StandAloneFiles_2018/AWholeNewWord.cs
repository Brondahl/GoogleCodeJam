//namespace AWholeNewWord
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
//      NumberOfWords = NLArray[0];
//      WordLength = NLArray[1];

//      var Words = lines.Skip(1).ToArray();
//      CharArrays = Words.Select(word => word.ToCharArray()).ToArray();
//    }

//    internal int NumberOfWords;
//    internal int WordLength;
//    internal char[][] CharArrays;
//  }

//  class CaseOutput
//  {
//    internal CaseOutput(string wordFound)
//    {
//      this.wordFound = wordFound;
//    }

//    internal string wordFound;

//    public override string ToString()
//    {
//      return wordFound ?? "-";
//    }
//  }

//}
//namespace AWholeNewWord
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
//      var cases = new CaseSplitter().Configure_TakingNFromFirstValPlusOne().GetCaseLines(lines).ToArray();
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
//      if (input.NumberOfWords == 1)
//      {
//        return new CaseOutput(null);
//      }

//      if (input.WordLength == 1)
//      {
//        return new CaseOutput(null);
//      }

//      if (input.WordLength == 2)
//      {
//        return SolveSimpleCase();
//      }
//      else
//      {
//        return SolveHardCase();
//      }
//    }


//    internal CaseOutput SolveHardCase()
//    {
//      var listOfSetsOfNthLetter = new HashSet<char>[input.WordLength];
//      var dictOfNextLettersByWordStart = new Dictionary<string, HashSet<char>>();


//      for (int i = 0; i < input.WordLength; i++)
//      {
//        listOfSetsOfNthLetter[i] = new HashSet<char>();
//      }

//      foreach (var charArray in input.CharArrays)
//      {
//        var wordSoFar = "";
//        for (int i = 0; i < input.WordLength; i++)
//        {
//          var nextChar = charArray[i];
//          listOfSetsOfNthLetter[i].Add(charArray[i]);

//          if (dictOfNextLettersByWordStart.ContainsKey(wordSoFar))
//          {
//            dictOfNextLettersByWordStart[wordSoFar].Add(nextChar);
//          }
//          else
//          {
//            dictOfNextLettersByWordStart[wordSoFar] = new HashSet<char> { nextChar };
//          }
//          wordSoFar += nextChar;
//        }
//      }

//      var found = AttemptMakeLonger("", listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
//      return new CaseOutput(found);
//    }

//    private string AttemptMakeLonger(string wordSoFar, HashSet<char>[] listOfSetsOfNthLetter, Dictionary<string, HashSet<char>> dictOfNextLettersByWordStart)
//    {
//      var currentLength = wordSoFar.Length;
//      if (wordSoFar.Length == input.WordLength)
//      {
//        return wordSoFar;
//      }

//      var nextChars = listOfSetsOfNthLetter[currentLength]; //0-indexing works nicely.
//      if (!dictOfNextLettersByWordStart.ContainsKey(wordSoFar))
//      {
//        return AttemptMakeLonger(wordSoFar + nextChars.First(), listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
//      }
//      else
//      {
//        var knownNextLetters = dictOfNextLettersByWordStart[wordSoFar];
//        var bestNextChars = nextChars.Except(knownNextLetters);
//        foreach (var nextChar in bestNextChars)
//        {
//          var found = AttemptMakeLonger(wordSoFar + nextChar, listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
//          if (found != null)
//          {
//            return found;
//          }
//        }

//        if (wordSoFar.Length != input.WordLength - 1)
//        {
//          //OK to try known 'words'
//          foreach (var nextChar in knownNextLetters)
//          {
//            var found = AttemptMakeLonger(wordSoFar + nextChar, listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
//            if (found != null)
//            {
//              return found;
//            }
//          }
//        }
//        else
//        {
//          return null;
//        }
//      }
//      return null;
//    }

//    internal CaseOutput SolveSimpleCase()
//    {
//      // Simple Case:
//      // WordLength = 2
//      // NumberOfWords > 1

//      var listOfFirstLetters = input.CharArrays.Select(arr => arr[0]).Distinct().ToList();
//      var listOfSecondLetters = input.CharArrays.Select(arr => arr[1]).Distinct().ToList();
//      var listOfSecondCharsPresentByFirstLetter = input.CharArrays.ToLookup(arr => arr[0], arr => arr[1]);

//      foreach (var possibleFirstLetter in listOfFirstLetters)
//      {
//        foreach (var possibleSecondLetter in listOfSecondLetters)
//        {
//          var listOfSecondCharsAlreadyUsed = listOfSecondCharsPresentByFirstLetter[possibleFirstLetter];
//          if (!listOfSecondCharsAlreadyUsed.Contains(possibleSecondLetter))
//          {
//            return new CaseOutput(possibleFirstLetter.ToString() + possibleSecondLetter.ToString());
//          }
//        }
//      }
//      //Simple Case: 

//      return new CaseOutput(null);
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
//  using AWholeNewWord;

//  class Program
//  {
//    static void Main(string[] args)
//    {
//      CaseSolver.Run();
//    }

//  }
//}
