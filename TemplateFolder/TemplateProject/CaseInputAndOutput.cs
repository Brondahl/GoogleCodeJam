namespace TemplateProject
{
    using System;
    using Common;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal static CaseSplitter ConfigureSplitter(CaseSplitter inputSplitter)
            => inputSplitter.Configure_ConstantMultiLineCases(2);

        internal CaseInput(List<string> lines)
        {
            var values = lines.First().SplitToLongArray();

            R = values[0];
            C = values[1];

            var moreValues = lines.Last().SplitToLongArray();
        }

        internal long R;
        internal long C;
    }

    class CaseOutput
    {
        internal static CaseOutput Impossible()
        {
            return new CaseOutput("IMPOSSIBLE");
        }

        private CaseOutput(string text)
        {
            Text = text;
        }

        internal CaseOutput(int answer) : this(answer.ToString())
        {
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
