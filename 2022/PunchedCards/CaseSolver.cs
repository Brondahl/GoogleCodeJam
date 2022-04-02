using System;

namespace PunchedCards
{
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  public class CaseSolver
  {
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut;
    public static void Run(IGoogleCodeJamCommunicator io = null)
    {
        InOut = io ?? new GoogleCodeJam2018Communicator();
        var lines = InOut.ReadStringInput(out numberOfCases);
        var cases = new CaseSplitter().GetSingleLineCases(lines);
        var results = ProcessCases(cases);
        InOut.WriteOutput(results);
    }

    private static IEnumerable<string> ProcessCases(IEnumerable<string> cases)
    {
        var currentCaseNumber = 0;
        foreach (var caseLines in cases)
        {
            currentCaseNumber++; //1-indexed.
            var parsedCase = new CaseInput(caseLines);
            var solver = new CaseSolver(parsedCase);
            var result = solver.Solve();

            var resultText = result.ToString();

            yield return $"Case #{currentCaseNumber}:{resultText}";
        }
    }

    private CaseInput input;

    internal CaseSolver(CaseInput inputCase)
    {
      input = inputCase;
    }

    internal CaseOutput Solve()
    {
        var cells = new char[2*input.R + 1, 2 * input.C + 1];
        for (int x = 0; x < 2 * input.R + 1; x++)
        {
            for (int y = 0; y < 2 * input.C + 1; y++)
            {
                if (x < 2 && y < 2)
                {
                    cells[x, y] = '.';
                    continue;
                }

                if (x % 2 == 1 && y % 2 == 0)
                {
                    cells[x, y] = '|';
                }
                if (x % 2 == 1 && y % 2 == 1)
                {
                    cells[x, y] = '.';
                }
                if (x % 2 == 0 && y % 2 == 0)
                {
                    cells[x, y] = '+';
                }
                if (x % 2 == 0 && y % 2 == 1)
                {
                    cells[x, y] = '-';
                }
            }
        }

        var text = "";
        for (int x = 0; x < 2 * input.R + 1; x++)
        {
            for (int y = 0; y < 2 * input.C + 1; y++)
            {
                text += cells[x, y];
            }

            text += Environment.NewLine;
        }

        return new CaseOutput(text);
    }

  }
}
