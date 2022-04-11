namespace TemplateProject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            var values = lines.Single().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            R = values[0];
            C = values[1];
        }

        internal long R;
        internal long C;
    }

    class CaseOutput
    {
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
