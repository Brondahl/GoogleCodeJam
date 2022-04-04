namespace ThreeDPrinting
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
            var cases = new CaseSplitter().Configure_ConstantMultiLineCases(3).GetCaseLines(lines);
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
            var target = 1000000;
            var minC = input.Printers.Select(p => p.C).Min();
            var minM = input.Printers.Select(p => p.M).Min();
            var minY = input.Printers.Select(p => p.Y).Min();
            var minK = input.Printers.Select(p => p.K).Min();

            if (minC + minM + minY + minK < target)
            {
                return new CaseOutput { IsImpossible = true };
            }

            var total = 0;
            var answer = new CaseOutput();

            answer.C = Math.Min(target-total, minC);
            total += answer.C;
            if (total == target) { return answer; }

            answer.M = Math.Min(target - total, minM);
            total += answer.M;
            if (total == target) { return answer; }

            answer.Y = Math.Min(target - total, minY);
            total += answer.Y;
            if (total == target) { return answer; }

            answer.K = Math.Min(target - total, minK);
            total += answer.K;
            if (total == target) { return answer; }

            throw new Exception("Booo");
        }

    }
}
