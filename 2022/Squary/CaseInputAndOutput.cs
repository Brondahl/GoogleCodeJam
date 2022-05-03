namespace Squary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            var values = lines.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            N = values[0];
            K = values[1];
            inputs = lines.Last().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }

        internal long N;
        internal long K;

        internal long[] inputs;
    }

    class CaseOutput
    {
        internal static CaseOutput Impossible => new CaseOutput(0){Text = "IMPOSSIBLE"};

        internal CaseOutput(long answer)
        {
            Text = answer.ToString();
        }

        internal CaseOutput(long[] answers)
        {
            Text = string.Join(" ", answers.Select(x => x.ToString()));
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
