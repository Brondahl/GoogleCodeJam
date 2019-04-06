//using System;
//using System.Collections.Generic;
//using System.Linq;


//namespace GoogleCodeJam2018_3
//{
//  class Solution
//  {
//    static void Main()
//    {
//      GoGopher.CaseSolver.Run();
//    }
//  }
//}


//namespace GoGopher
//{
//  public class CaseSolver
//  {
//    private static int numberOfCases;
//    public static void Run()
//    {
//      numberOfCases = int.Parse(Console.ReadLine());

//      for (int ii = 0; ii < numberOfCases; ii++)
//      {
//        var responder = new LiveStdOutStdInGopherResponder();
//        var solver = new GopherSolver(responder);

//        solver.InteractiveSolveForTarget(int.Parse(Console.ReadLine()));
//      }
//    }
//  }
//}

//namespace GoGopher
//{
//  public interface IGopherResponder
//  {
//    Tuple<int, int> SubmitGopherInstructionAndGetResponse(Tuple<int, int> target);
//    int AttemptsSoFar { get; }
//    List<Tuple<int, int>> HitsSoFar { get; }
//    List<Tuple<int, int>> DistinctSortedHitsSoFar { get; }
//  }

//  public class LiveStdOutStdInGopherResponder : IGopherResponder
//  {

//    public Tuple<int, int> SubmitGopherInstructionAndGetResponse(Tuple<int, int> target)
//    {
//      counter++;
//      Console.WriteLine($"{target.Item1} {target.Item2}");
//      var responseString = Console.ReadLine();

//      // Possible Responses:
//      // "x y" actual target hit.
//      // "0 0" area successfully solved.
//      // "-1 -1" Any Error.
//      var responseCoords = responseString.Split(' ').Select(int.Parse).ToArray();
//      return Tuple.Create(responseCoords[0], responseCoords[1]);
//    }

//    private int counter;
//    public int AttemptsSoFar => counter;
//    public List<Tuple<int, int>> HitsSoFar => null;
//    public List<Tuple<int, int>> DistinctSortedHitsSoFar => null;
//  }

//  public class TestGopherResponder : IGopherResponder
//  {
//    private static readonly Random Rand = new Random();
//    private static int Offset => Rand.Next(3) - 1;


//    public Tuple<int, int> SubmitGopherInstructionAndGetResponse(Tuple<int, int> target)
//    {
//      counter++;

//      if (counter > 1000)
//      {
//        return Tuple.Create(-1, -1);
//      }

//      var targetX = target.Item1;
//      var xOffset = Offset;
//      var targetY = target.Item2;
//      var yOffset = Offset;

//      var actualHit = Tuple.Create(targetX + xOffset, targetY + yOffset);

//      HitsSoFar.Add(actualHit);

//      //Doesn't report success as "0 0", handle this ourselves.
//      return actualHit;
//    }

//    private int counter;
//    public int AttemptsSoFar => counter;
//    public List<Tuple<int, int>> HitsSoFar { get; } = new List<Tuple<int, int>>();
//    public List<Tuple<int, int>> DistinctSortedHitsSoFar => HitsSoFar.Distinct().OrderBy(hit => hit.Item1).ThenBy(hit => hit.Item2).ToList();
//  }
//}

//namespace GoGopher
//{
//  public class GopherSolver
//  {
//    private readonly IGopherResponder responder;
//    private readonly bool testMode;
//    private bool[,] Grid;
//    private int GridCount;

//    private readonly Tuple<int, int> CompleteIndicator = Tuple.Create(0, 0);
//    private readonly Tuple<int, int> ErrorIndicator = Tuple.Create(-1, -1);
//    private readonly Tuple<int, int> initialTarget = Tuple.Create(2, 2);

//    private enum GopherResult
//    {
//      complete,
//      error,
//      newTarget,
//      repeatTarget
//    }

//    public GopherSolver(IGopherResponder responder, bool testMode = false)
//    {
//      this.responder = responder;
//      this.testMode = testMode;

//      Grid = null;
//      GridCount = 0;
//    }

//    public void InteractiveSolveForTarget(int target)
//    {
//      if (target <= 9)
//      {
//        InteractiveSolveForLessThanNine();
//      }
//      else
//      {
//        InteractiveSolveForMoreThanNine(target);
//      }
//    }

//    public void InteractiveSolveForLessThanNine()
//    {
//      Grid = new bool[3, 3];

//      GopherResult result = GopherResult.error; //Force the loop to run at least once.

//      while (result != GopherResult.complete)
//      {
//        result = SendAndReceive(initialTarget).Item2;
//        if (testMode && GridCount >= 9)
//        {
//          result = GopherResult.complete;
//        }
//      }
//    }


//    public void InteractiveSolveForMoreThanNine(int target)
//    {
//      var gridLength = target / 3;
//      if (target % 3 != 0)
//      {
//        gridLength++;
//      }
//      Grid = new bool[gridLength, 3];

//      GopherResult result = GopherResult.error; //Force the loop to run at least once.
//      int currentCentreStrip = 2;
//      int currentLeftStrip = currentCentreStrip - 1;
//      int leftStripCount = 0;
//      Tuple<int, int> currentTarget = Tuple.Create(currentCentreStrip, 2);

//      while (result != GopherResult.complete)
//      {
//        var output = SendAndReceive(currentTarget);
//        result = output.Item2;
//        var hit = output.Item1;

//        if (result == GopherResult.repeatTarget) { continue; }
//        if (result == GopherResult.complete) { continue; }

//        if (result == GopherResult.newTarget)
//        {
//          if (hit.Item1 == currentLeftStrip)
//          {
//            leftStripCount++;
//            while (leftStripCount == 3 && currentCentreStrip < gridLength - 1) //stop stepping forwards when either (there's a left hand cell to fill) or (we've reached the right hand limit of the grid)
//            {
//              currentCentreStrip++;
//              currentLeftStrip++;
//              currentTarget = Tuple.Create(currentCentreStrip, 2);
//              leftStripCount =
//                (GetGridBool(currentLeftStrip, 1) ? 1 : 0) +
//                (GetGridBool(currentLeftStrip, 2) ? 1 : 0) +
//                (GetGridBool(currentLeftStrip, 3) ? 1 : 0);
//            }
//          }
//        }

//        if (testMode)
//        {
//          var isComplete = (GridCount == 3 * gridLength);
//          result = isComplete ? GopherResult.complete : result;
//        }
//      }

//    }

//    private GopherResult InterpretGopherResponse(Tuple<int, int> response)
//    {
//      if (AreEqual(response, CompleteIndicator))
//      {
//        return GopherResult.complete;
//      }

//      if (AreEqual(response, ErrorIndicator))
//      {
//        throw new InvalidOperationException();
//        //return GopherResult.error;
//      }

//      if (GetGridBool(response))
//      {
//        return GopherResult.repeatTarget;
//      }

//      return GopherResult.newTarget;
//    }

//    private Tuple<Tuple<int, int>, GopherResult> SendAndReceive(Tuple<int, int> target)
//    {
//      var response = responder.SubmitGopherInstructionAndGetResponse(target);
//      var interp = InterpretGopherResponse(response);

//      if (interp == GopherResult.newTarget)
//      {
//        SetGridBool(response, true);
//        GridCount++;
//      }

//      return Tuple.Create(response, interp);
//    }

//    private bool GetGridBool(Tuple<int, int> target)
//    {
//      return GetGridBool(target.Item1, target.Item2);
//    }

//    private bool GetGridBool(int x, int y)
//    {
//      return Grid[x - 1, y - 1];
//    }

//    private void SetGridBool(Tuple<int, int> target, bool val)
//    {
//      SetGridBool(target.Item1, target.Item2, val);
//    }

//    private void SetGridBool(int x, int y, bool val)
//    {
//      Grid[x - 1, y - 1] = val;
//    }

//    public bool AreEqual(Tuple<int, int> actual, Tuple<int, int> test)
//    {
//      return actual.Item1 == test.Item1 && actual.Item2 == test.Item2;
//    }
//  }
//}
