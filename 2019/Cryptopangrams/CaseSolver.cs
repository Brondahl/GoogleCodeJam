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
      var cases = new CaseSplitter().Configure_ConstantMultiLineCases(2).GetCaseLines(lines);
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

      lowestCompositeNumber = input.Numbers.First();
      locatedIndex = 0;

      Tuple<long, long> primes = null;
      primes = DecomposeSemiPrime(lowestCompositeNumber);
      var prime1 = primes.Item1;
      var prime2 = primes.Item2;


      long previousPrime, nextPrime;
      if (prime1 == prime2)
      {
        nextPrime = previousPrime = prime1;
      }
      else
      {

        long semiPrimeToCompare;
        int offsetRequired = 0;
        if (locatedIndex == 0)
        {
          semiPrimeToCompare = input.Numbers[locatedIndex + 1];

          int offset = 1;
          while (semiPrimeToCompare == input.Numbers[locatedIndex + offset])
          {
            offset++;
          }
          semiPrimeToCompare = input.Numbers[locatedIndex + offset];
        }
        else
        {
          semiPrimeToCompare = input.Numbers[locatedIndex - 1];

          int offset = 1;
          while (semiPrimeToCompare == input.Numbers[locatedIndex - offset])
          {
            offset++;
          }
          semiPrimeToCompare = input.Numbers[locatedIndex - offset];
        }

        if (locatedIndex == 0)
        {
          if (semiPrimeToCompare % prime1 == 0)
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
          if (semiPrimeToCompare % prime1 == 0)
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

        if (offsetRequired % 2 == 0)
        {
          var hold = nextPrime;
          nextPrime = previousPrime;
          previousPrime = hold;
        }
      }

      underlyingPrimes[locatedIndex] = previousPrime;
      underlyingPrimes[locatedIndex + 1] = nextPrime;

      for (int index = locatedIndex; index < input.Numbers.Length - 1; index++)
      {
        var nextSemiPrime = input.Numbers[index + 1];
        var identifiedPrimeFactor = underlyingPrimes[index + 1];
        underlyingPrimes[index + 2] = nextSemiPrime / identifiedPrimeFactor;
        if (nextSemiPrime != underlyingPrimes[index + 2] * identifiedPrimeFactor) { throw new Exception(); }
      }

      for (int index = locatedIndex; index > 0; index--)
      {
        var prevSemiPrime = input.Numbers[index - 1];
        var identifiedPrimeFactor = underlyingPrimes[index];
        underlyingPrimes[index - 1] = prevSemiPrime / identifiedPrimeFactor;
        if (prevSemiPrime != underlyingPrimes[index - 1] * identifiedPrimeFactor) { throw new Exception(); }
      }

      Dictionary<long, char> translationLookup = new Dictionary<long, char>();
      int index1 = 0;
      foreach (var x1 in underlyingPrimes.Distinct().OrderBy(x => x))
      {
        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (index1 < 0 || index1 >= alphabet.Length)
        {
          Thrower.TriggerTimeLimit();
        }
        var letter = Thrower.TriggerResponseIfErrors(Thrower.ResponseType.Time, () => alphabet[index1]);
        if (translationLookup.ContainsKey(x1)) { Thrower.TriggerTimeLimit(); }
        translationLookup.Add(x1, letter);
        index1++;
      }

      var array = underlyingPrimes.Select(prime => translationLookup[prime]).ToArray();

      string decryption = new string(array);

      return new CaseOutput(decryption);
    }

    private Tuple<long, long> DecomposeSemiPrime(long semiPrime)
    {
      if (semiPrime % 2 == 0)
      {
        var otherFactor = semiPrime / 2;
        if (semiPrime != 2 * otherFactor) { throw new Exception(); }
        return Tuple.Create(2L, semiPrime / 2);
      }

      for (long candidateDivisor = 3; candidateDivisor <= Math.Sqrt(semiPrime); candidateDivisor = candidateDivisor + 2)
      {
        if (semiPrime % candidateDivisor == 0)
        {
          var otherFactor = semiPrime / candidateDivisor;
          if (semiPrime != candidateDivisor * otherFactor) { throw new Exception(); }
          return Tuple.Create(candidateDivisor, otherFactor);
        }
      }
      throw new Exception("Impossible!");
    }
  }
}
