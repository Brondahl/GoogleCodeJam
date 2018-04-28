namespace SavingTheUniverseAgain
{
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  /*
   * TODO:
   *   - Namespace
   *   - Copy in/out files.
   *   - References to Common and testing Frameworks
   *   - CodeJam Reference to here.
   *   - Program redirect to here.
   */
  public class CaseSolver
  {
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2018Communicator();
    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = lines.ToArray();
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new CaseInput(cases[ii]);
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();

        var resultText = result.ToString();

        results.Add($"Case #{ii + 1}: {resultText}");
      }

      InOut.WriteOutput(results);
    }



    private CaseInput input;
    private long currentMaxChargeDamage = 1;
    private long totalDamageDealt = 0;

    internal CaseSolver(CaseInput inputCase)
    {
      input = inputCase;
    }

    internal CaseOutput Solve()
    {
      if (input.ShotCount > input.D) { return new CaseOutput(-1); }
      ProcessSequence(input.Shots);

      long hacks = 0;
      while (totalDamageDealt > input.D)
      {
        var damageReduction = currentMaxChargeDamage / 2;

        //Move Shot down one level.
        input.Shots[input.MaxChargeFired]--;
        input.Shots[input.MaxChargeFired - 1]++;

        //Reduce Damage Tracker.
        totalDamageDealt -= damageReduction;
        
        //Record Hack
        hacks++;

        if (input.Shots[input.MaxChargeFired] == 0)
        {
          input.MaxChargeFired--;
          currentMaxChargeDamage = damageReduction;
        }
      }
      return new CaseOutput(hacks);
    }

    private long ProcessSequence(int[] sequence)
    {
      for (int charge = 0; charge < input.MaxChargeFired+1; charge++)
      {
        totalDamageDealt += currentMaxChargeDamage * sequence[charge];
        currentMaxChargeDamage *= 2;
      }
      currentMaxChargeDamage /= 2;
      return totalDamageDealt;
    }
  }
}
