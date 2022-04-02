namespace ManhattanCrepeCart
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      var line1 = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
      P = line1.First();
      Q = line1.Last();

      var people = lines.Skip(1).Select(person => person.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();

      foreach (var person in people)
      {
        switch (person[2].ToCharArray().Single())
        {
          case 'N':
            NPeople.Add(int.Parse(person[1]));
            break;
          case 'E':
            EPeople.Add(int.Parse(person[0]));
            break;
          case 'S':
            SPeople.Add(int.Parse(person[1]));
            break;
          case 'W':
            WPeople.Add(int.Parse(person[0]));
            break;
        }
      }
      NPeople = NPeople.OrderBy(x => x).ToList();
      EPeople = EPeople.OrderBy(x => x).ToList();
      SPeople = SPeople.OrderBy(x => x).ToList();
      WPeople = WPeople.OrderBy(x => x).ToList();
    }

    internal int P;
    internal int Q;
    internal List<int> NPeople = new List<int>();
    internal List<int> EPeople = new List<int>();
    internal List<int> SPeople = new List<int>();
    internal List<int> WPeople = new List<int>();
  }

  class CaseOutput
  {
    internal CaseOutput(int x, int y)
    {
      X = x;
      Y = y;
    }

    internal int X;
    internal int Y;


    public override string ToString()
    {
      return $"{X} {Y}";
    }
  }

}
