namespace GoGopher
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public interface IGopherResponder
  {
    Tuple<int, int> SubmitGopherInstructionAndGetResponse(Tuple<int, int> target);
    int AttemptsSoFar { get; }
    List<Tuple<int, int>> HitsSoFar { get; }
    List<Tuple<int, int>> DistinctSortedHitsSoFar { get; }
  }

  public class LiveStdOutStdInGopherResponder : IGopherResponder
  {

    public Tuple<int, int> SubmitGopherInstructionAndGetResponse(Tuple<int, int> target)
    {
      counter++;
      Console.WriteLine($"{target.Item1} {target.Item2}");
      var responseString = Console.ReadLine();

      // Possible Responses:
      // "x y" actual target hit.
      // "0 0" area successfully solved.
      // "-1 -1" Any Error.
      var responseCoords = responseString.Split(' ').Select(int.Parse).ToArray();
      return Tuple.Create(responseCoords[0], responseCoords[1]);
    }

    private int counter;
    public int AttemptsSoFar => counter;
    public List<Tuple<int, int>> HitsSoFar => null;
    public List<Tuple<int, int>> DistinctSortedHitsSoFar => null;
  }

  public class TestGopherResponder : IGopherResponder
  {
    private static readonly Random Rand = new Random();
    private static int Offset => Rand.Next(3) - 1;


    public Tuple<int, int> SubmitGopherInstructionAndGetResponse(Tuple<int, int> target)
    {
      counter++;

      if (counter > 1000)
      {
        return Tuple.Create(-1, -1);
      }

      var targetX = target.Item1;
      var xOffset = Offset;
      var targetY = target.Item2;
      var yOffset = Offset;

      var actualHit = Tuple.Create(targetX + xOffset, targetY + yOffset);

      HitsSoFar.Add(actualHit);

      //Doesn't report success as "0 0", handle this ourselves.
      return actualHit;
    }

    private int counter;
    public int AttemptsSoFar => counter;
    public List<Tuple<int, int>> HitsSoFar { get; } = new List<Tuple<int, int>>();
    public List<Tuple<int, int>> DistinctSortedHitsSoFar => HitsSoFar.Distinct().OrderBy(hit => hit.Item1).ThenBy(hit => hit.Item2).ToList();
  }
}

