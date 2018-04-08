using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace TidyNumbers
{
    public class EntryPoint
    {
      public static void RunFromFile()
      {
        new EntryPoint().Run();
      }

      public static void Test()
      {
        testSolver_full();
      }

      #region test
      public static void testIsTidyCheck()
      {
        Console.WriteLine("? True ?");
        Console.WriteLine(new CaseSolver().NumberIsTidy(123));
        Console.WriteLine(new CaseSolver().NumberIsTidy(1123));
        Console.WriteLine(new CaseSolver().NumberIsTidy(1));
        Console.WriteLine(new CaseSolver().NumberIsTidy(0));

        Console.WriteLine("? True ?");
        Console.WriteLine(new CaseSolver().NumberIsTidy(8));
        Console.WriteLine(new CaseSolver().NumberIsTidy(123));
        Console.WriteLine(new CaseSolver().NumberIsTidy(555));
        Console.WriteLine(new CaseSolver().NumberIsTidy(224488));

        Console.WriteLine("? False ?");
        Console.WriteLine(new CaseSolver().NumberIsTidy(1352));
        Console.WriteLine(new CaseSolver().NumberIsTidy(20));
        Console.WriteLine(new CaseSolver().NumberIsTidy(321));
        Console.WriteLine(new CaseSolver().NumberIsTidy(495));
        Console.WriteLine(new CaseSolver().NumberIsTidy(999990));
      }

      public static void testDictBuild()
      {
        var solver = new CaseSolver();
        solver.BuildDictForHardCodedNumbers();
        var dict = CaseSolver.dictionaryOfTidyness;

        foreach (var key in dict.Keys.OrderBy(x => x))
        {
          string line = key.ToString() + "\t" + dict[key].ToString();
          Console.WriteLine(line);
        }
      }

      public static void testForceSolver()
      {
        var solver = new CaseSolver();

        Console.WriteLine(solver.ForceSolveSmall(132));
        Console.WriteLine(solver.ForceSolveSmall(1000));
        Console.WriteLine(solver.ForceSolveSmall(7));
      }

      public static void testForceSolverPerformance()
      {
        var solver = new CaseSolver();

        Console.WriteLine(solver.Solve(1000));
        Console.WriteLine(solver.Solve(5000));
        Console.WriteLine(solver.Solve(10000));
        Console.WriteLine(solver.Solve(50000));
        Console.WriteLine(solver.Solve(99999990));
      }

      public static void testDigitBreakDown()
      {
        Console.WriteLine(string.Join(",", new CaseSolver().getDigitsLeftToRight(123)));
        Console.WriteLine(string.Join(",", new CaseSolver().getDigitsLeftToRight(1123)));
        Console.WriteLine(string.Join(",", new CaseSolver().getDigitsLeftToRight(1)));
        Console.WriteLine(string.Join(",", new CaseSolver().getDigitsLeftToRight(0)));
        Console.WriteLine(string.Join(",", new CaseSolver().getDigitsLeftToRight(5868438)));
        Console.WriteLine(string.Join(",", new CaseSolver().getDigitsLeftToRight(0864068446810)));
      }

      public static void testDigitBuildUp()
      {
        Console.WriteLine(new CaseSolver().GetNumberFromDigitArrayLeftToRight(new[] { 5, 1, 8, 4, 3, 3 }));
        Console.WriteLine(new CaseSolver().GetNumberFromDigitArrayLeftToRight(new[] { 1 }));
        Console.WriteLine(new CaseSolver().GetNumberFromDigitArrayLeftToRight(new[] { 0 }));
        Console.WriteLine(new CaseSolver().GetNumberFromDigitArrayLeftToRight(new[] { 0, 5, 6, 8 }));
        Console.WriteLine(new CaseSolver().GetNumberFromDigitArrayLeftToRight(new[] { 0, 0, 9,9,9,0 }));
      }

      public static void testSolver_basic()
      {
        var solver = new CaseSolver();

        Console.WriteLine(solver.Solve(132));
        Console.WriteLine(solver.Solve(1000));
        Console.WriteLine(solver.Solve(7));

        Console.WriteLine(solver.Solve(1000));
        Console.WriteLine(solver.Solve(5000));
        Console.WriteLine(solver.Solve(10000));
        Console.WriteLine(solver.Solve(50000));
        Console.WriteLine(solver.Solve(99999990));

        Console.WriteLine(solver.Solve(7563));
      }

      public static void testSolver_more()
      {
        var solver = new CaseSolver();

        Console.WriteLine(new CaseSolver().Solve(332));
        Console.WriteLine(new CaseSolver().Solve(1332));
        Console.WriteLine(new CaseSolver().Solve(00332));
        Console.WriteLine(new CaseSolver().Solve(4332));
        Console.WriteLine(new CaseSolver().Solve(5332));
      }

      public static void testSolver_full()
      {
        var solver = new CaseSolver();

        for (int ii = 0; ii < 10000; ii++)
        {
          solver.Solve(ii);
        }
      }
      #endregion

      private int numberOfCases;
      private CommonBase Common = new Common2017(@"TidyNumbers", "Small.in");
      public void Run()
      {
      var cases = Common.ReadLongInput(out numberOfCases);
        var solver = new CaseSolver();
        var results = new List<string>();

        for (int ii = 0; ii < numberOfCases; ii++)
        {
          var answer = solver.Solve(cases[ii]);
          results.Add(string.Format("Case #{0}: {1}", ii+1, answer));
        }

        Common.WriteOutput(results);
      }
    }
}
