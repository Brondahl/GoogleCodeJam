namespace DoubleOrOneThing
{
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            text = lines.Single().ToCharArray();
        }

        internal char[] text;
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
