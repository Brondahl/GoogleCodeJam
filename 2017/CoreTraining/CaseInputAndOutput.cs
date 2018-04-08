using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreTraining
{
  class CaseInput
  {
    private CaseInput()
    {
    }

    internal CaseInput(IEnumerable<string> linesIn)
    {
      var lines = linesIn.ToList();
      var NKline = lines.First();
      var split = NKline.Split(' ');
      N = int.Parse(split[0]);
      K = int.Parse(split[1]);

      U = decimal.Parse(lines[1]);

      var lastLine = lines.Last();
      Cores = lastLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse).Select((val, index) => new Core(val, index)).ToArray();
    }

    internal int N;
    internal int K;
    internal decimal U;
    internal Core[] Cores;

    internal CaseInput DeepClone()
    {
      return new CaseInput
      {
        N = N,
        K = K,
        U = U,
        Cores = Cores.Select(core => new Core(core.Success, core.n)).ToArray()
      };
    }
  }

  class Core
  {
    internal Core(decimal success, int index)
    {
      Success = success;
      n = index;
    }

    public int n;
    public decimal Success;
    public decimal Failure { get { return 1 - Success; } }

    public double LogSuccess { get { return Math.Log((double)Success); } }
    public double LogFailure { get { return Math.Log((double)Failure); } }
  }

  class CaseOutput
  {
    internal CaseOutput(decimal success, CaseInput input)
    {
      Success = success;
      Input = input;
    }

    internal decimal Success;
    internal CaseInput Input;

    public override string ToString()
    {
      //return Input.N + "|" + Input.K + "|" + Input.U;
      return Success.ToString("0.#######");
    }
  }

}
