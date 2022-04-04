namespace TwistyPassages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<long> values)
        {
            N = (int)values[0];
            K = values[1];
        }

        internal int N;
        internal long K;
    }

    class CaseStepInput
    {
        internal CaseStepInput(List<long> values)
        {
            RoomNumber = values[0];
            NumberOfPassages = values[1];
        }

        internal long RoomNumber;
        internal long NumberOfPassages;
    }

    class CaseStepOutput
    {
        public static CaseStepOutput Walk()
        {
            return new CaseStepOutput("W");
        }

        public static CaseStepOutput Teleport(long room)
        {
            return new CaseStepOutput("T " + room);
        }

        public static CaseStepOutput FinalGuess(long guess)
        {
            return new CaseStepOutput("E " + guess);
        }

        private CaseStepOutput(string text)
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
