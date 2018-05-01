namespace RoundingError
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      var NLLine = lines[0];
      var NLArray = NLLine.Split(' ').Select(int.Parse).ToArray();
      SurveySize = NLArray[0];
      InitialLangCount = NLArray[1];

      var CurrentCountsLine = lines[1];
      InitialVoteCounts = CurrentCountsLine.Split(' ').Select(int.Parse).ToArray();

      VoteValue = 100.0 / SurveySize;
      VoteFractionalValue = VoteValue - Math.Floor(VoteValue);
      RemainingVotes = SurveySize - InitialVoteCounts.Sum();
    }

    internal int SurveySize;
    internal int InitialLangCount;
    internal int[] InitialVoteCounts;

    internal double VoteValue;
    internal double VoteFractionalValue;
    internal int RemainingVotes;
  }

  class CaseOutput
  {
    internal CaseOutput(int maxSum)
    {
      MaxSum = maxSum;
    }

    internal int MaxSum;

    public override string ToString()
    {
      return MaxSum.ToString();
    }
  }

}
