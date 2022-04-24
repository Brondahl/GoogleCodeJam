namespace PancakeDeque
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            var values = lines.Last().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            N = long.Parse(lines[0]);
            Values = values;
        }

        internal long N;
        internal List<long> Values;
    }

    class CaseOutput
    {
        internal CaseOutput(int answer)
        {
            Text = answer.ToString();
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
