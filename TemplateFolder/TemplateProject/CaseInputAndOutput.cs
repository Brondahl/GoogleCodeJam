namespace TemplateProject
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseInput
  {
    internal CaseInput(List<string> lines)
    {
      N = int.Parse(lines[0]);
      IsEven = (N % 2 == 0);

      var V = lines[1].Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    }

    internal int N;
    internal bool IsEven;
  }

  class CaseOutput
  {
    internal CaseOutput(int errorIndex)
    {
      ErrorIndex = errorIndex;
    }

    internal int ErrorIndex;

    public override string ToString()
    {
      return ErrorIndex == -1 ? "OK" : ErrorIndex.ToString();
    }
  }

}
