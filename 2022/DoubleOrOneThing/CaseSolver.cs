namespace DoubleOrOneThing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;

    public class CaseSolver
    {
        private static int numberOfCases;
        private static IGoogleCodeJamCommunicator InOut;
        public static void Run(IGoogleCodeJamCommunicator io = null)
        {
            InOut = io ?? new GoogleCodeJam2018Communicator();
            var lines = InOut.ReadStringInput(out numberOfCases);
            var cases = new CaseSplitter().Configure_ConstantMultiLineCases(1).GetCaseLines(lines);
            var results = ProcessCases(cases);
            InOut.WriteOutput(results);
        }

        private static IEnumerable<string> ProcessCases(IEnumerable<List<string>> cases)
        {
            var currentCaseNumber = 0;
            foreach (var caseLines in cases)
            {
                currentCaseNumber++; //1-indexed.
                var parsedCase = new CaseInput(caseLines);
                var solver = new CaseSolver(parsedCase);
                var result = solver.Solve();

                var resultText = result.ToString();

                yield return $"Case #{currentCaseNumber}: {resultText}";
            }
        }

        private CaseInput input;

        internal CaseSolver(CaseInput inputCase)
        {
            input = inputCase;
        }

        internal CaseOutput Solve()
        {
            var output = "";
            for (int i = 0; i < input.text.Length-1; i++)
            {
                var c = input.text[i];
                var next = NextNotEqual(input.text, c, i);
                output += c;
                if (next > c)
                {
                    output += c;
                }
            }

            output += input.text.Last();
            return new CaseOutput(output);
        }

        private char NextNotEqual(char[] array, char current, int index)
        {
            for (int i = index+1; i < array.Length; i++)
            {
                if (array[i] != current)
                {
                    return array[i];
                }
            }

            return current;
        }
    }
}
