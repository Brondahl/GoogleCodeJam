namespace GoGopher
{
  using System;

  public class CaseSolver
  {
    private static int numberOfCases;
    public static void Run()
    {
      numberOfCases = int.Parse(Console.ReadLine());

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var responder = new LiveStdOutStdInGopherResponder();
        var solver = new GopherSolver(responder);

        solver.InteractiveSolveForTarget(int.Parse(Console.ReadLine()));
      }
    }
  }
}
