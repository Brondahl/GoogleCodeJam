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

      Tuple<long, long> primes = null;
      try
      {
        primes = DecomposeSemiPrime(lowestCompositeNumber);
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
      }
      catch (Exception e)
      {
        System.Threading.Thread.Sleep(25000);
      }


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

    private Tuple<long, long> DecomposeSemiPrime(long semiPrime)
    {
      if (semiPrime % 2 == 0)
      {
        return Tuple.Create(2L, semiPrime / 2);
      }

      for (long candidateDivisor = 3; candidateDivisor <= Math.Sqrt(semiPrime); candidateDivisor = candidateDivisor + 2)
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
