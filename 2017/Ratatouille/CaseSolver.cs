using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using Common;

namespace Ratatouille
{
  /*
   * TODO:
   *   - Namespace
   *   - References to Common and testing Frameworks
   *   - CodeJam Reference to here.
   *   - Program redirect to here.
   */
  public class CaseSolver
  {
    private static string subFolderName = @"Ratatouille";
    private static int numberOfCases;
    private static CommonBase Common = new Common2017(subFolderName);
    public static void Run()
    {
      var lines = Common.ReadStringInput(out numberOfCases).ToList();
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

      Common.WriteOutput(results);
    }



    private CaseInput input;

    internal CaseSolver(CaseInput inputCase)
    {
      input = inputCase;
    }

    internal CaseOutput Solve()
    {
      int[][] SortedIngredientPackages = new int[input.N][]; 
      for (int ingredient = 0; ingredient < input.N; ingredient++)
      {
        var packages = input.Packages[ingredient];
        SortedIngredientPackages[ingredient] = packages.OrderBy(x => x).ToArray();
      }
      var KitCount = 0;

      var multiplier = 0;

      var IngredientLowerBound = Enumerable.Repeat(0, input.Recipe.Count()).ToArray();
      var IngredientUpperBound = Enumerable.Repeat(0, input.Recipe.Count()).ToArray();

      var RecipeLowerBound = input.Recipe.Select(x => (int)(Math.Ceiling(x*0.9))).ToArray();
      var RecipeUpperBound = input.Recipe.Select(x => (int)(Math.Floor(x*1.1))).ToArray();

      var IngredientPackageIndex = Enumerable.Repeat(0, input.Recipe.Count()).ToArray();
      var moreKitsAreMakable = true;

      multiplier++;
      IngredientLowerBound = Enumerable.Zip(IngredientLowerBound, RecipeLowerBound, (d1, d2) => d1 + d2).ToArray();
      IngredientUpperBound = Enumerable.Zip(IngredientUpperBound, RecipeUpperBound, (d1, d2) => d1 + d2).ToArray();
      
      while (moreKitsAreMakable)
      {
        var thisKitIsMakable = true;

        for (int ingredient = 0; ingredient < input.N; ingredient++)
        {
          var currentIndex = IngredientPackageIndex[ingredient];
          var targetMin = IngredientLowerBound[ingredient];
          var targetMax = IngredientUpperBound[ingredient];
          while (currentIndex < input.P)
          {
            var currentPackage = SortedIngredientPackages[ingredient][currentIndex];

            if (currentPackage < targetMin)
            {
              currentIndex++;
              continue;
            }

            if (currentPackage > targetMax)
            {
              thisKitIsMakable = false;
            }

            break;
          }
          IngredientPackageIndex[ingredient] = currentIndex;
          if (currentIndex == input.P) //No matching Packages.
          {
            thisKitIsMakable = false;
            moreKitsAreMakable = false;
          }

          if (!thisKitIsMakable)
          {
            break;
          }
        }

        if (!moreKitsAreMakable)
        {
          break;
        }

        if (!thisKitIsMakable)
        {
          multiplier++;
          IngredientLowerBound = Enumerable.Zip(IngredientLowerBound, RecipeLowerBound, (d1, d2) => d1 + d2).ToArray();
          IngredientUpperBound = Enumerable.Zip(IngredientUpperBound, RecipeUpperBound, (d1, d2) => d1 + d2).ToArray();
          continue;
        }

        for (int ingredient = 0; ingredient < input.N; ingredient++)
        {
          var packageIndexToUse = IngredientPackageIndex[ingredient];
          SortedIngredientPackages[ingredient][packageIndexToUse] = 0;
        }

        KitCount++;

      }

      return new CaseOutput(KitCount);
    }
  }
}
