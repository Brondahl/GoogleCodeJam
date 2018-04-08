using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TidyNumbers
{
  internal class CaseSolver
  {
    internal static readonly Dictionary<long, bool> dictionaryOfTidyness = new Dictionary<long, bool>();
    private const int hardCodedLimit = 10000;

    internal CaseSolver()
    {
      BuildDictForHardCodedNumbers();
    }

    internal long Solve(long input)
    {
      long forcedAnswer = 0;
      if (input <= hardCodedLimit)
      {
        forcedAnswer = ForceSolveSmall(input);
      }
      var goodAnswer = GoodSolve(input);

      if (forcedAnswer != 0 && goodAnswer != forcedAnswer)
      {
        throw new Exception();
      }

      return goodAnswer;
    }

    internal long ForceSolveSmall(long input)
    {
      var test = input;
      while (!NumberIsTidy(test))
      {
        test--;
      }
      return test;
    }

    internal long GoodSolve(long input)
    {
      if(input < 10)
      { return input; }

      var inputDigits = getDigitsLeftToRight(input);
      int[] outputDigits = new int[inputDigits.Count()];
      
      int digitIndex;
      outputDigits[0] = inputDigits[0];

      for (digitIndex = 1; digitIndex < inputDigits.Count(); digitIndex++)
      {
        if (inputDigits[digitIndex] >= inputDigits[digitIndex - 1])
        {
          //Increasing digit. Take and go onto next.
          outputDigits[digitIndex] = inputDigits[digitIndex];
          continue;
        }

        //We've Failed, so we need to back-track.
        digitIndex--;
        var proposedDigit = inputDigits[digitIndex] - 1;
        while (true)
        {
          if (digitIndex == 0)
          {
            outputDigits[digitIndex] = proposedDigit;
            break;
          }
          if (proposedDigit < outputDigits[digitIndex - 1])
          {
            digitIndex--;
          }
          else
          {
            outputDigits[digitIndex] = proposedDigit;
            break;
          }
            
        }

        break;
      }

      digitIndex++;

      //If we stopped early, then everything else gets filled in with 9s.
      while (digitIndex < inputDigits.Count())
      {
        outputDigits[digitIndex] = 9;
        digitIndex++;
      }

      return GetNumberFromDigitArrayLeftToRight(outputDigits);
    }



    #region tools
    internal int[] getDigitsLeftToRight(long input)
    {
      return input.ToString().ToCharArray().Select(digit => int.Parse(digit.ToString())).ToArray();
    }

    internal long GetNumberFromDigitArrayLeftToRight(int[] digits)
    {
      long output = 0;
      for (int ii = 0; ii < digits.Length; ii++)
      {
        output = 10 * output + digits[ii];
      }
      return output;
    }

    internal bool NumberIsTidy(long input)
    {
      if (dictionaryOfTidyness.ContainsKey(input))
      {
        return dictionaryOfTidyness[input];
      }
      else
      {
        return CalculateAndStoreNumberIsTidy(input);
      }
    }

    internal bool CalculateAndStoreNumberIsTidy(long input)
    {
      var isTidy = CalculateNumberIsTidy(input);
      dictionaryOfTidyness[input] = isTidy;
      return isTidy;
    }

    internal bool CalculateNumberIsTidy(long input)
    {
      var digits = getDigitsLeftToRight(input);
      for (int ii = 0; ii < digits.Count() - 1; ii++)
      {
        if (digits[ii + 1] < digits[ii])
        {
          return false;
        }
      }
      return true;
    }

    internal void BuildDictForHardCodedNumbers()
    {
      foreach (var ii in Enumerable.Range(0, hardCodedLimit + 1))
      {
        CalculateAndStoreNumberIsTidy(ii);
      }
    }
    #endregion
  }
}
