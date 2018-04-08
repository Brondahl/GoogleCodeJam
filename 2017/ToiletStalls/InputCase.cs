namespace ToiletStalls
{
  class InputCase
  {
    internal InputCase(string line)
    {
      var split = line.Split(' ');
      Stalls = long.Parse(split[0]);
      Occupants = long.Parse(split[1]);
    }

    internal long Stalls;
    internal long Occupants;
  }
}
