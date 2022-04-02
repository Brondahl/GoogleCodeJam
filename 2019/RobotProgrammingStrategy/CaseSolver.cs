namespace RobotProgrammingStrategy
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using Common;

  public class CaseSolver
  {
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut;
    public static void Run(IGoogleCodeJamCommunicator io = null)
    {
      InOut = io ?? new GoogleCodeJam2018Communicator();
      var lines = InOut.ReadStringInput(out numberOfCases);
      var cases = new CaseSplitter().GetCaseLines_TakingNFromFirstValPlusOne(lines);
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
      long index = 0;
      var activeBots = input.Lines.ToList();
      var solution = "";

      while (true)
      {
        var pIsAvailable = true;
        var rIsAvailable = true;
        var sIsAvailable = true;

        var pKills = 0;
        var rKills = 0;
        var sKills = 0;

        var pBots = new List<string>();
        var rBots = new List<string>();
        var sBots = new List<string>();

        foreach (var activeBot in activeBots)
        {
          int moveIndex = (int)(index % activeBot.Length);
          var botMove = activeBot[moveIndex];
          switch (botMove)
          {
            case 'P': rIsAvailable = false; sKills++; pBots.Add(activeBot); break;
            case 'R': sIsAvailable = false; pKills++; rBots.Add(activeBot); break;
            case 'S': pIsAvailable = false; rKills++; sBots.Add(activeBot); break;
          }

          if (!rIsAvailable & !pIsAvailable & !sIsAvailable)
          {
            return new CaseOutput(false);
          }
        }

        char myMove = 'X';

        if (rIsAvailable && !pIsAvailable && !sIsAvailable) { myMove = 'R'; }
        if (!rIsAvailable && pIsAvailable && !sIsAvailable) { myMove = 'P'; }
        if (!rIsAvailable && !pIsAvailable && sIsAvailable) { myMove = 'S'; }

        if (rIsAvailable && pIsAvailable && !sIsAvailable) { myMove = rKills > pKills ? 'R' : 'P'; }
        if (rIsAvailable && !pIsAvailable && sIsAvailable) { myMove = rKills > sKills ? 'R' : 'S'; }
        if (!rIsAvailable && pIsAvailable && sIsAvailable) { myMove = pKills > sKills ? 'P' : 'S'; }

        solution += myMove;
        List<string> beatenBots = null;
        switch (myMove)
        {
          case 'P': beatenBots = rBots; break;
          case 'R': beatenBots = sBots; break;
          case 'S': beatenBots = pBots; break;
        }

        foreach (var deadBot in beatenBots)
        {
          activeBots.Remove(deadBot);
        }

        if (!activeBots.Any())
        {
          return new CaseOutput(solution);
        }

        index++;
      }
    }

  }
}
