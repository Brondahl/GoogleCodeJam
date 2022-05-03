namespace LetterBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            N = int.Parse(lines.First());
            Texts = lines.Last().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        internal long N;
        internal string[] Texts;
    }

    class CaseOutput
    {
        internal static CaseOutput Impossible => new CaseOutput("IMPOSSIBLE");
        internal CaseOutput(string text)
        {
            Text = text;
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
