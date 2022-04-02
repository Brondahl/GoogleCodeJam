namespace ThreeDPrinting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            Printers = lines.Select(l => new Printer(l)).ToArray();
        }

        internal Printer[] Printers;
    }

    class Printer
    {
        public Printer(string line)
        {
            var values = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            C = values[0];
            M = values[1];
            Y = values[2];
            K = values[3];
        }

        public int C;
        public int M;
        public int Y;
        public int K;
    }

    class CaseOutput
    {
        public int C;
        public int M;
        public int Y;
        public int K;

        public bool IsImpossible;

        public override string ToString()
        {
            return 
                IsImpossible 
                    ? "IMPOSSIBLE"
                    : $"{C} {M} {Y} {K}";
        }
    }

}
