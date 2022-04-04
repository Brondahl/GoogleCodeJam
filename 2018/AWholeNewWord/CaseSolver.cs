namespace AWholeNewWord
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
      var cases = new CaseSplitter().Configure_TakingNFromFirstValPlusOne().GetCaseLines(lines);
      var results = new List<string>();
      var caseNumber = 0;

      foreach (var caseLines in cases)
      {
        caseNumber++; //1-indexed.
        var parsedCase = new CaseInput(caseLines);
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();

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
      if (input.NumberOfWords == 1)
      {
        return new CaseOutput(null);
      }

      if (input.WordLength == 1)
      {
        return new CaseOutput(null);
      }

      if (input.WordLength == 2)
      {
        return SolveSimpleCase();
      }
      else
      {
        return SolveHardCase();
      }
    }


    internal CaseOutput SolveHardCase()
    {
      var listOfSetsOfNthLetter = new HashSet<char>[input.WordLength];
      var dictOfNextLettersByWordStart = new Dictionary<string, HashSet<char>>();


      for (int i = 0; i < input.WordLength; i++)
      {
        listOfSetsOfNthLetter[i] = new HashSet<char>();
      }

      foreach (var charArray in input.CharArrays)
      {
        var wordSoFar = "";
        for (int i = 0; i < input.WordLength; i++)
        {
          var nextChar = charArray[i];
          listOfSetsOfNthLetter[i].Add(charArray[i]);

          if (dictOfNextLettersByWordStart.ContainsKey(wordSoFar))
          {
            dictOfNextLettersByWordStart[wordSoFar].Add(nextChar);
          }
          else
          {
            dictOfNextLettersByWordStart[wordSoFar] = new HashSet<char> { nextChar };
          }
          wordSoFar += nextChar;
        }
      }

      var found = AttemptMakeLonger("", listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
      return new CaseOutput(found);
    }

    private string AttemptMakeLonger(string wordSoFar, HashSet<char>[] listOfSetsOfNthLetter, Dictionary<string, HashSet<char>> dictOfNextLettersByWordStart)
    {
      var currentLength = wordSoFar.Length;
      if (wordSoFar.Length == input.WordLength)
      {
        return wordSoFar;
      }

      var nextChars = listOfSetsOfNthLetter[currentLength]; //0-indexing works nicely.
      if (!dictOfNextLettersByWordStart.ContainsKey(wordSoFar))
      {
        return AttemptMakeLonger(wordSoFar + nextChars.First(), listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
      }
      else
      {
        var knownNextLetters = dictOfNextLettersByWordStart[wordSoFar];
        var bestNextChars = nextChars.Except(knownNextLetters);
        foreach (var nextChar in bestNextChars)
        {
          var found = AttemptMakeLonger(wordSoFar + nextChar, listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
          if (found != null)
          {
            return found;
          }
        }

        if (wordSoFar.Length != input.WordLength - 1)
        {
          //OK to try known 'words'
          foreach (var nextChar in knownNextLetters)
          {
            var found = AttemptMakeLonger(wordSoFar + nextChar, listOfSetsOfNthLetter, dictOfNextLettersByWordStart);
            if (found != null)
            {
              return found;
            }
          }
        }
        else
        {
          return null;
        }
      }
      return null;
    }

    internal CaseOutput SolveSimpleCase()
    {
      // Simple Case:
      // WordLength = 2
      // NumberOfWords > 1

      var listOfFirstLetters = input.CharArrays.Select(arr => arr[0]).Distinct().ToList();
      var listOfSecondLetters = input.CharArrays.Select(arr => arr[1]).Distinct().ToList();
      var listOfSecondCharsPresentByFirstLetter = input.CharArrays.ToLookup(arr => arr[0], arr => arr[1]);

      foreach (var possibleFirstLetter in listOfFirstLetters)
      {
        foreach (var possibleSecondLetter in listOfSecondLetters)
        {
          var listOfSecondCharsAlreadyUsed = listOfSecondCharsPresentByFirstLetter[possibleFirstLetter];
          if (!listOfSecondCharsAlreadyUsed.Contains(possibleSecondLetter))
          {
            return new CaseOutput(possibleFirstLetter.ToString() + possibleSecondLetter.ToString());
          }
        }
      }
      //Simple Case: 

      return new CaseOutput(null);
    }

  }
}