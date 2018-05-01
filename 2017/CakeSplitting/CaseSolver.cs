namespace CakeSplitting
{
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  public class CaseSolver
  {
    private static string subFolderName = @"Problem1";
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2017Communicator(subFolderName);

    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = CaseInput.SubDivideInput(lines.GetEnumerator());
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new CaseInput(cases[ii]);
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();

        var resultText = result.ToString();

        results.Add(string.Format("Case #{0}: {1}", ii + 1, resultText));
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
      var output = new CaseOutput(input.Grid, input.R, input.C);

      var rowBackFill = 0;
      for (int rr = 0; rr < input.R; rr++)
      {
        if (input.LettersInRow[rr] == 0)
        {
          rowBackFill++;
          continue;
        }
        var localRowBackFill = rowBackFill;
        rowBackFill = 0;

        var previousLetterCol = -1;
        for (int xx = 0; xx < input.LettersInRow[rr]; xx++)
        {
          var currentLetterCol = input.LetterLocationsInRow[rr][xx];
          var currentLetter = input.Grid[rr, currentLetterCol];

          int endColForThis = (xx == input.LettersInRow[rr] - 1) ? input.C - 1 : currentLetterCol;

          for (int jj = previousLetterCol + 1; jj < endColForThis+1; jj++)
          {
            for (int backfill = 0; backfill < localRowBackFill+1; backfill++)
            {
              output.Grid[rr - backfill, jj] = currentLetter;
            }
          }

          previousLetterCol = currentLetterCol;
        }
      }

      
      if (rowBackFill > 0)
      {
        for (int ii = 0; ii < rowBackFill; ii++)
        {
          for (int jj = 0; jj < input.C; jj++)
          {
            output.Grid[input.R - 1 - ii, jj] = output.Grid[input.R - 1 - rowBackFill, jj];
          }
        }
      }

      return output;
    }

    //internal void MassFlipStartingAtIndex(int index)
    //{
    //  for (int jj = 0; jj < input.FlipSize; jj++)
    //  {
    //    SingleBitFlip(index + jj);
    //  }
    //}

    //internal void SingleBitFlip(int index)
    //{
    //  input.Sequence[index] = !input.Sequence[index];
    //}

  }
}
