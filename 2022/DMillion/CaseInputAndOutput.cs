namespace DMillion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            N = int.Parse(lines[0]);
            values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        }

        internal int N;
        internal int[] values;
    }

    class CaseOutput
    {
        internal CaseOutput(int answer)
        {
            Answer = answer;
        }

        internal int Answer;

        public override string ToString()
        {
            return Answer.ToString();
        }
    }

}
