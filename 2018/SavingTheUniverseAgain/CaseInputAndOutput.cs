namespace SavingTheUniverseAgain
{
  class CaseInput
  {
    internal CaseInput(string line)
    {
      var split = line.Split(' ');
      D = int.Parse(split[0]);
      var P = split[1].ToCharArray();
      Shots = new int[30];

      var shotIndex = 0;
      foreach (var character in P)
      {
        if (character == 'C')
        {
          shotIndex++;
        }
        else
        {
          Shots[shotIndex]++;
          ShotCount++;
          MaxChargeFired = shotIndex;
       }
      }
    }

    internal long D;
    internal int[] Shots;
    internal int ShotCount;
    internal int MaxChargeFired;
  }

  class CaseOutput
  {
    internal CaseOutput(long hacks)
    {
      Hacks = hacks;
    }

    internal long Hacks;

    public override string ToString()
    {
      return Hacks == -1 ? "IMPOSSIBLE" : Hacks.ToString();
    }
  }

}
