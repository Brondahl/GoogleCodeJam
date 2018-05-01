namespace RoundingError
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
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = new CaseSplitter().GetCaseLines(lines, 2).ToArray();
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

    private List<double> currentPercentages;
    private int currentVotesRemaining;

    internal CaseOutput Solve()
    {
      currentVotesRemaining = input.RemainingVotes;
      currentPercentages = CalculateCurrentPercentages();

      if (input.RemainingVotes == 0)
      {
        var total = CalculateTotalImpliedByPercentages(currentPercentages);
        return new CaseOutput(total);
      }

      if (100 % input.SurveySize  == 0)
      {
        return new CaseOutput(100);
      }

      var fractionalOffsetsFromHalfWithIndex = currentPercentages.Select((perc, index) => new{ PercOffset = (perc - Math.Floor(perc) - 0.5), Index = index});
      foreach (var offsetObject in fractionalOffsetsFromHalfWithIndex.OrderByDescending(offset => offset.PercOffset))
      {
        var offset = offsetObject.PercOffset;
        if (offset > 0)
        {
          continue; //nothing to do here.
        }

        var votesToAssign = CalculateVotesToAssignToSurpassTarget(offset);
        AssignVotes(votesToAssign, offsetObject.Index);
      }

      var votesToAssignToANewLanguage = CalculateVotesToAssignToSurpassTarget(0.5);
      while (currentVotesRemaining > 0)
      {
        currentPercentages.Add(0);
        AssignVotes(votesToAssignToANewLanguage, currentPercentages.Count-1);
      }

      var modifiedTotal = CalculateTotalImpliedByPercentages(currentPercentages);
      return new CaseOutput(modifiedTotal);

    }

    private int CalculateVotesToAssignToSurpassTarget(double target)
    {
      var votesRequiredToGetPastOffset = (int) (Math.Ceiling(Math.Abs(target) / input.VoteFractionalValue));
      return votesRequiredToGetPastOffset;
    }

    private void AssignVotes(int desiredVotesToAssign, int index)
    {
      var votesToActuallyAssign = Math.Min(desiredVotesToAssign, currentVotesRemaining);
      currentPercentages[index] += votesToActuallyAssign * input.VoteValue;
      currentVotesRemaining -= votesToActuallyAssign;
    }

    private List<double> CalculateCurrentPercentages()
    {
      return input.InitialVoteCounts.Select(count => count * input.VoteValue).ToList();
    }

    private int CalculateTotalImpliedByPercentages(List<double> percentages)
    {
      return percentages.Select(doublePercentage => ((int)(Math.Round(doublePercentage, MidpointRounding.AwayFromZero)))).Sum();
    }
  }
}
