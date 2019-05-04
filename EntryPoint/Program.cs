namespace GoogleCodeJam
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class Program
  {
    static void Main(string[] args)
    {
      var t_and_f = Console.ReadLine();

      var tfArray = t_and_f.Split(' ');
      var t = int.Parse(tfArray.First());

      for (int i = 0; i < t; i++)
      {
        var partialAnswer = SolveFirst4();
        var answer = CompleteAnswer(partialAnswer.ToList());
        Console.WriteLine(answer);
        var verdict = Console.ReadLine();
        if (verdict != "Y")
        {
          throw new Exception();
        }
      }
    }

    private static string CompleteAnswer(List<char> partialAnswer)
    {
      if (!partialAnswer.Contains('A')) { partialAnswer.Add('A'); }
      if (!partialAnswer.Contains('B')) { partialAnswer.Add('B'); }
      if (!partialAnswer.Contains('C')) { partialAnswer.Add('C'); }
      if (!partialAnswer.Contains('D')) { partialAnswer.Add('D'); }
      if (!partialAnswer.Contains('E')) { partialAnswer.Add('E'); }

      return new string(partialAnswer.ToArray());
    }

    private static char[] SolveFirst4()
    {
      var letterDictionaryArray = new Dictionary<char, List<int>>[4]
      {
        new Dictionary<char, List<int>>
        {
          {'A', new List<int>()},
          {'B', new List<int>()},
          {'C', new List<int>()},
          {'D', new List<int>()},
          {'E', new List<int>()}
        },
        new Dictionary<char, List<int>>
        {
          {'A', new List<int>()},
          {'B', new List<int>()},
          {'C', new List<int>()},
          {'D', new List<int>()},
          {'E', new List<int>()}
        },

        new Dictionary<char, List<int>>
        {
          {'A', new List<int>()},
          {'B', new List<int>()},
          {'C', new List<int>()},
          {'D', new List<int>()},
          {'E', new List<int>()}
        },

        new Dictionary<char, List<int>>
        {
          {'A', new List<int>()},
          {'B', new List<int>()},
          {'C', new List<int>()},
          {'D', new List<int>()},
          {'E', new List<int>()}
        },
      };

      for (int setLocation = 0; setLocation < 119; setLocation++)
      {
        var answer = ReadSetLocationPlace(setLocation, 0);
        var dict = letterDictionaryArray[0];
        dict[answer].Add(setLocation);
      }

      var missingFirst = DetermineMissingPosition(letterDictionaryArray, 0);
      var relevantSetsToCheck_1 = letterDictionaryArray[0][missingFirst];




      foreach (var relevantSetLocation in relevantSetsToCheck_1)
      {
        var answer = ReadSetLocationPlace(relevantSetLocation, 1);
        var dict = letterDictionaryArray[1];
        dict[answer].Add(relevantSetLocation);
      }

      var missingSecond = DetermineMissingPosition(letterDictionaryArray, 1, missingFirst);
      var relevantSetsToCheck_2 = letterDictionaryArray[1][missingSecond];




      foreach (var relevantSetLocation in relevantSetsToCheck_2)
      {
        var answer = ReadSetLocationPlace(relevantSetLocation, 2);
        var dict = letterDictionaryArray[2];
        dict[answer].Add(relevantSetLocation);
      }

      var missingThird = DetermineMissingPosition(letterDictionaryArray, 2, missingFirst, missingSecond);
      var relevantSetToCheck_3 = letterDictionaryArray[2][missingThird];




      foreach (var relevantSetLocation in relevantSetToCheck_3)
      {
        var answer = ReadSetLocationPlace(relevantSetLocation, 3);
        var dict = letterDictionaryArray[3];
        dict[answer].Add(relevantSetLocation);
      }

      var missingFourth = DetermineMissingPosition(letterDictionaryArray, 3, missingFirst, missingSecond, missingThird);

      return new []{ missingFirst, missingSecond, missingThird, missingFourth };
    }

    private static char DetermineMissingPosition(Dictionary<char, List<int>>[] letterDictionaryArray, int i, params char[] accountedFor)
    {
      var dict = letterDictionaryArray[i];
      var lengthsDict = dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count);
      return lengthsDict.Where(kvp => !accountedFor.Contains(kvp.Key)).OrderBy(kvp => kvp.Value).First().Key;
    }

    private static char ReadSetLocationPlace(int setLocation, int i)
    {
      Console.WriteLine(setLocation*5 + i + 1);
      return Console.ReadLine().ToCharArray().Single();
    }
  }
}