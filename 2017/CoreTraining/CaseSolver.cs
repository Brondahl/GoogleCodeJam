namespace CoreTraining
{
  using System;
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
    private static string subFolderName = @"CoreTraining";
    private static int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2017Communicator(subFolderName);

    public static void Run()
    {
      var lines = InOut.ReadStringInput(out numberOfCases).ToList();
      var cases = new CaseSplitter().GetCaseLines(lines, 3).ToArray();
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
        decimal successChance = 0;
        CaseInput tempInput;

        //Check possibilities, based on training top x cores, upwards
        for (int i = 1; i <= input.N; i++)
        {
          tempInput = input.DeepClone();
          var selectedCores = tempInput.Cores.OrderByDescending(core => core.Success).Take(i).ToList();
          var remainingTraining = TrainWeakestFirstEvenly(selectedCores, tempInput.U);
          TrainStrongestFirstUnevenly(tempInput.Cores, remainingTraining);
          var result = EvaluateCoresSuccessRate(tempInput.Cores.ToList(), tempInput.K);

          if (result > successChance)
          {
            successChance = result;
          }
        }

        //Check possibilities, based on training bottom x cores, upwards
        for (int i = 1; i <= input.N; i++)
        {
          tempInput = input.DeepClone();
          var selectedCores = tempInput.Cores.OrderBy(core => core.Success).Take(i).ToList();
          var remainingTraining = TrainWeakestFirstEvenly(selectedCores, tempInput.U);
          TrainStrongestFirstUnevenly(tempInput.Cores, remainingTraining);
          var result = EvaluateCoresSuccessRate(tempInput.Cores.ToList(), tempInput.K);

          if (result > successChance)
          {
            successChance = result;
          }
        }

        //Train all cores, from the top, downwards
        {
          tempInput = input.DeepClone();
          TrainStrongestFirstUnevenly(tempInput.Cores, tempInput.U);
          var result = EvaluateCoresSuccessRate(tempInput.Cores.ToList(), tempInput.K);

          if (result > successChance)
          {
            successChance = result;
          }
        }

        return new CaseOutput(successChance, input);
    }

    internal decimal TrainWeakestFirstEvenly(IEnumerable<Core> cores, decimal trainingAmount)
    {
      var coreQ = new Queue<Core>(cores.OrderBy(core => core.Success));
      var remainingTraining = trainingAmount;

      if (!coreQ.Any())
      {
        throw new Exception();
      }

      var coresBeingTrained = new List<Core> {coreQ.Dequeue()};

      while (remainingTraining > 0 && coreQ.Any())
      {

        var nextCore = coreQ.Peek();
        var successDiff = nextCore.Success - coresBeingTrained.First().Success;

        if (successDiff != 0)
        {
          remainingTraining = TrainCoresByAmountOrMax(coresBeingTrained, successDiff, remainingTraining);
        }

        coresBeingTrained.Add(coreQ.Dequeue());
      }

      if (remainingTraining > 0)
      {
        var currentSuccess = coresBeingTrained.First().Success;
        remainingTraining = TrainCoresByAmountOrMax(coresBeingTrained, 1 - currentSuccess, remainingTraining);
      }

      return remainingTraining;
    }

    internal decimal TrainStrongestFirstUnevenly(IEnumerable<Core> cores, decimal trainingAmount)
    {
      var remainingTrainingAvailable = trainingAmount;
      while (remainingTrainingAvailable > 0 && cores.Any(core => core.Success < 1))
      {
        var strongestCore = cores.Where(core => core.Success < 1).OrderByDescending(core => core.Success).First();
        var coreRemainingTrainingWanted = strongestCore.Failure;
        if(coreRemainingTrainingWanted >= remainingTrainingAvailable)
        {
          strongestCore.Success += remainingTrainingAvailable;
          return 0;
        }
        else
        {
          strongestCore.Success = 1;
          remainingTrainingAvailable -= coreRemainingTrainingWanted;
        }
      }
      return remainingTrainingAvailable;
    }

    private static decimal TrainCoresByAmountOrMax(List<Core> trainingCoreSet, decimal trainingAmountGoal, decimal totalTrainingLimit)
    {
      var trainingCoreCount = trainingCoreSet.Count();
      var fullTrainCost = trainingAmountGoal * trainingCoreCount;

      if (fullTrainCost <= totalTrainingLimit)
      {
        TrainCoresByAmount(trainingCoreSet, trainingAmountGoal);
        totalTrainingLimit -= fullTrainCost;
      }
      else
      {
        TrainCoresByAmount(trainingCoreSet, totalTrainingLimit / trainingCoreCount);
        totalTrainingLimit = 0;
      }
      return totalTrainingLimit;
    }

    private static void TrainCoresByAmount(List<Core> trainingCoreSet, decimal amount)
    {
      foreach (var core in trainingCoreSet)
      {
        core.Success += amount;
      }
    }

    private decimal EvaluateCoresSuccessRate(List<Core> cores, int successesRequired)
    {
        var N = cores.Count;

        /* In normal space */
        var probOfPSuccessesAfterCoreQ = new decimal[N+1, N];

        //Handle q = 0 differently.
        var firstCore = cores[0];
        probOfPSuccessesAfterCoreQ[0, 0] = firstCore.Failure;
        probOfPSuccessesAfterCoreQ[1, 0] = firstCore.Success;

        for (int q = 1; q < N; q++)
        {
          var qthCore = cores[q];

          //Handle p = 0 differently.
          probOfPSuccessesAfterCoreQ[0, q] = probOfPSuccessesAfterCoreQ[0, q-1] * qthCore.Failure;

          for (int p = 1; p < q + 2; p++)
          {
            // Prob(Successes = p) = Prob(q succeeds)*Prob(p-1 from q-1 Cores) + Prob(q fails)*Prob(p from q-1 Cores)
            var probPsuccesses = qthCore.Success * probOfPSuccessesAfterCoreQ[p - 1, q - 1] +
                                 qthCore.Failure * probOfPSuccessesAfterCoreQ[p, q - 1];

            probOfPSuccessesAfterCoreQ[p, q] = probPsuccesses;
          }
        }

        /* In log space */
        var logProbOfPSuccessesAfterCoreQ = new double[N+1, N];
        for (int i = 0; i < N+1; i++)
        {
          for (int j = 0; j < N; j++)
          {
            logProbOfPSuccessesAfterCoreQ[i, j] = double.NegativeInfinity;
          } 
        }

        //Handle q = 0 differently.
        //var firstCore = input.Cores[0];
        logProbOfPSuccessesAfterCoreQ[0, 0] = firstCore.LogFailure;
        logProbOfPSuccessesAfterCoreQ[1, 0] = firstCore.LogSuccess;

        for (int q = 1; q < N; q++)
        {
          var qthCore = cores[q];

          //Handle p = 0 differently.
          logProbOfPSuccessesAfterCoreQ[0, q] = logProbOfPSuccessesAfterCoreQ[0, q - 1] + qthCore.LogFailure;

          for (int p = 1; p < q + 2; p++)
          {
            // Prob(Successes = p) = Prob(q succeeds)*Prob(p-1 from q-1 Cores) + Prob(q fails)*Prob(p from q-1 Cores)
            // P = A*B + C*D
            // Log(P) = Log(A*B + C*D)
            // Log(P) = Log(B*(A + C*D/B))
            // Log(P) = Log(B) + Log(A + C*D/B))
            //
            // Observe B = e^(Log(B)), and D similar.
            //
            // Log(P) = Log(B) + Log(A + C*[e^Log(D)]/[e^Log(B)])
            //
            // Log(P) = Log(B) + Log(A + C*e^[Log(D)-Log(B)])
            //
            // The initial equation is symmetric in A,B <=> C,D
            // so wlog we can choose B & D such that 0 > Log(D) > Log(B), then 0 > Log(D)-Log(B) > Log(D)
            // to do this, start from P = w*x + y*z as Success, then Fail, and map A,B,C,D as necessary.

            double A = (double)qthCore.Success;
            double log_B = logProbOfPSuccessesAfterCoreQ[p - 1, q - 1];
            double C = (double)qthCore.Failure;
            double log_D = logProbOfPSuccessesAfterCoreQ[p, q - 1];

            if (log_B > log_D)
            {
              double holder;
              holder = log_D;
              log_D = log_B;
              log_B = holder;

              holder = C;
              C = A;
              A = holder;
            }

            double logProbPsuccesses;

            if (A == 0 || double.IsNegativeInfinity(log_B) || double.IsNaN(log_B))
            {
              logProbPsuccesses = Math.Log(C) + log_D;
            }
            else if (C == 0 || double.IsNegativeInfinity(log_D) || double.IsNaN(log_D))
            {
              logProbPsuccesses = Math.Log(A) + log_B;
            }
            else
            {
              logProbPsuccesses = log_B + Math.Log(A + C*Math.Exp(log_D - log_B));
            }

            logProbOfPSuccessesAfterCoreQ[p, q] = logProbPsuccesses;
          }
        }


        double probOfInputSuccess_Log = 0;
        decimal probOfInputSuccess_NonLog = 0;
        for (int k = successesRequired; k < N+1; k++)
        {
          probOfInputSuccess_NonLog += probOfPSuccessesAfterCoreQ[k, N-1];
          probOfInputSuccess_Log += Math.Exp(logProbOfPSuccessesAfterCoreQ[k, N-1]);
          var diff = probOfInputSuccess_Log - (double) probOfInputSuccess_NonLog;
          if (diff > Math.Pow(10,-15))
          { 
            Console.WriteLine(diff);
          }
        }

        return (decimal)probOfInputSuccess_Log;
    }

    private double?[] TValues;
    private double TValue(IEnumerable<Core> cores, int x)
    {
      if (TValues[x] == null)
      {
        TValues[x] = CalcTValue(cores, x);
      }
      return TValues[x].Value;
    }

    private double CalcTValue(IEnumerable<Core> cores, int x)
    {
      return cores.Sum(core => (Math.Pow((double) (core.Success/core.Failure), x)));
    }

    private decimal?[] Probs;
    private decimal ProbNumOfSuccessesIsX(IEnumerable<Core> cores, int x)
    {
      if (Probs[x] == null)
      {
        if (x == 0)
        {
          Probs[0] = CalcProbNumOfSuccessesIs0(cores);
        }
        else
        {
          Probs[x] = CalcProbNumOfSuccessesIsX(cores, x);
        }
      }
      return Probs[x].Value;
    }

    private decimal CalcProbNumOfSuccessesIs0(IEnumerable<Core> cores)
    {
      return cores.Aggregate(1.0m, (acc, core) => acc * core.Failure);
    }

    private decimal CalcProbNumOfSuccessesIsX(IEnumerable<Core> cores, int x)
    {
      double sum = 0;
      for (int i = 1; i <= x; i++)
      {
        double prob_XMinusI = (double)ProbNumOfSuccessesIsX(cores, x - i);
        double t_I = TValue(cores, i);
        double parity = (i % 2 == 0) ? -1 : 1;

        sum += (parity * prob_XMinusI * t_I);
      }

      return (decimal)sum/x;
    }

  }
}