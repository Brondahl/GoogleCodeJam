namespace MysteriousRoadSigns
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Common;

  public class CaseSolver
  {
    public static void GenerateAndSolveNewTestData()
    {
      GenerateTestData.GenerateData();
      CaseSolver.Run(true);
    }

    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator testFileInOut = new GoogleCodeJam2017Communicator(true, @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\2018\MysteriousRoadSigns\TestData", "data.txt");
    private static IGoogleCodeJamCommunicator liveInOut = new GoogleCodeJam2018Communicator();
    public static void Run(bool testingMode = false)
    {
      var activeIO = testingMode ? testFileInOut : liveInOut;
      var lines = activeIO.ReadStringInput(out numberOfCases);
      var cases = new CaseSplitter().GetCaseLines_TakingNFromFirstValPlusOne(lines);
      var results = new List<string>();
      var caseNumber = 0;
      var stopWatch = new Stopwatch();

      foreach (var caseLines in cases)
      {
        caseNumber++; //1-indexed.
        stopWatch.Reset();
        stopWatch.Start();

        var parsedCase = new CaseInput(caseLines);
        var solver = new CaseSolver(parsedCase);
        var result = solver.Solve();
        var resultText = result.ToString();

        stopWatch.Stop();
        results.Add($"Case #{caseNumber}: {resultText}");
        if (testingMode)
        {
          Console.WriteLine(stopWatch.ElapsedMilliseconds / 1000.0);
      }
      }

      activeIO.WriteOutput(results);
    }

    private CaseInput input;

    internal CaseSolver(CaseInput inputCase)
    {
      input = inputCase;
    }

    internal CaseOutput Solve()
    {
      var setLengthStartingAtI = new int[input.S];
      var rulesForSetStartingAtI = new SetRuleOptions[input.S];
      var setWithStartPointIIsActive = new bool[input.S];
      for (int i = 0; i < input.S; i++)
      {
        var sign_I = input.Signs[i];
        setLengthStartingAtI[i] = 1;
        rulesForSetStartingAtI[i] = new SetRuleOptions(sign_I);
        setWithStartPointIIsActive[i] = true;

        for (int j = 0; j < i; j++)
        {
          if (setWithStartPointIIsActive[j])
          {
            var existingRules = rulesForSetStartingAtI[j];
            var newRules = existingRules.AttemptIncludeSign(sign_I);

            if (newRules.IsBroken)
            {
              setWithStartPointIIsActive[j] = false;
            }
            else
            {
              setLengthStartingAtI[j]++;
              rulesForSetStartingAtI[j] = newRules;
            }
          }
        }
      }

      var maxSetLength = setLengthStartingAtI.Max();
      var numberOfSuchSets = setLengthStartingAtI.Count(x => x == maxSetLength);

      return new CaseOutput(maxSetLength, numberOfSuchSets);
    }

    class SetRuleOptions
    {
      public SetRule Rule1 = new SetRule();
      public SetRule Rule2 = new SetRule();
      public bool Considers2Rules = true;
      public bool IsBroken = false;

      public override string ToString()
      {
        var rule1Text = $"Rule1 = {Rule1}";
        var rule2Text = $"Rule2 = {Rule2}";
        return IsBroken ? "Broken" : Considers2Rules ? $"{rule1Text} | {rule2Text}" : rule1Text;
      }

      public SetRuleOptions(Sign initialSign)
      {
        Rule1 = new SetRule { Left = initialSign.M };
        Rule2 = new SetRule { Right = initialSign.N };
      }

      private SetRuleOptions()
      {
      }

      public SetRuleOptions AttemptIncludeSign(Sign sign)
      {
        if (Considers2Rules)
        {
          var rule1Compat = Rule1.IsCompatibleWith(sign);
          var rule1Support = Rule1.ActivelySupports(sign);

          var rule2Compat = Rule2.IsCompatibleWith(sign);
          var rule2Support = Rule2.ActivelySupports(sign);

          if (!Rule1.IsFixed)
          {
            if (rule1Support && rule2Support)
            {
              return this;
            }
            if (rule1Support)
            {
              return new SetRuleOptions { Considers2Rules = false, Rule1 = Rule1};
            }
            if (rule2Support)
            {
              return new SetRuleOptions { Considers2Rules = false, Rule1 = Rule2 };
            }
            return new SetRuleOptions
            {
              Considers2Rules = true,
              Rule1 = new SetRule { Left = Rule1.Left, Right = sign.N },
              Rule2 = new SetRule { Left = sign.M, Right = Rule2.Right },
            };
          }
          else
          {
            if (rule1Compat && rule2Compat)
            {
              return this;
            }

            if (rule1Compat)
            {
              return new SetRuleOptions { Considers2Rules = false, Rule1 = Rule1 };
            }

            if (rule2Compat)
            {
              return new SetRuleOptions { Considers2Rules = false, Rule1 = Rule2 };
            }
            return BrokenRule;
          }

        }
        else
        {
          var rule1Compat = Rule1.IsCompatibleWith(sign);
          var rule1Support = Rule1.ActivelySupports(sign);
          if (!rule1Compat)
          {
            return BrokenRule;
          }
          if (rule1Support)
          {
            return this;
          }
          else
          {
            return new SetRuleOptions {Considers2Rules = false, Rule1 = Rule1.ExpandToSupport(sign)};
          }
        }
      }

      private static SetRuleOptions BrokenRule => new SetRuleOptions { IsBroken = true };
    }

    public class SetRule
    {
      public int? Left;
      public int? Right;
      public bool IsFixed => Left.HasValue && Right.HasValue;

      public override string ToString()
      {
        return $"Rule = { (Left.HasValue ? Left.ToString() : "?")},{ (Right.HasValue ? Right.ToString() : "?")}";
      }

      public bool ActivelySupports(Sign sign)
      {
        var supportedOnLeft = Left.HasValue && sign.M == Left;
        var supportedOnRight = Right.HasValue && sign.N == Right;

        return supportedOnLeft || supportedOnRight;
      }

      public bool IsCompatibleWith(Sign sign)
      {
        var compatibleOnLeft = Left == null || sign.M == Left;
        var compatibleOnRight = Right == null || sign.N == Right;

        return compatibleOnLeft || compatibleOnRight;
      }

      public SetRule ExpandToSupport(Sign sign)
      {
        if (ActivelySupports(sign))
        {
          return this;
        }

        var compatibleOnLeft = Left == null || sign.M == Left;
        var compatibleOnRight = Right == null || sign.N == Right;

        if (compatibleOnLeft && Left == null)
        {
          return new SetRule {Left = sign.M, Right = Right};
        }
        else
        {
          return new SetRule { Left = Left, Right = sign.N };
        }
      }

    }
  }
}
