namespace Squary
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
            var cases = new CaseSplitter().Configure_ConstantMultiLineCases(2).GetCaseLines(lines);
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
            var oneOut = SolveWith1();
            if (oneOut.Text != "IMPOSSIBLE")
            {
                return oneOut;
            }
            else
            {
                if (input.K == 1)
                {
                    return CaseOutput.Impossible;
                }

                var twoOut = SolveWith2();
                return twoOut;
            }
        }

        internal CaseOutput SolveWith1()
        {
            var sum = input.inputs.Sum();
            var sumOfSquares = input.inputs.Select(Square).Sum();
            var squareOfSum = Square(sum);

            var top = sumOfSquares - squareOfSum;
            var bottom = 2 * sum;

            if (top == 0)
            {
                return new CaseOutput(0);
            }
            if (bottom == 0)
            {
                return CaseOutput.Impossible;
            }
            var answer = top / bottom;
            if (top.IsDivisibleBy(bottom))
            {
                return new CaseOutput(answer);
            }
            else
            {
                return CaseOutput.Impossible;
            }
        }
        internal CaseOutput SolveWith2()
        {
            var sum = input.inputs.Sum();
            var sumOfSquares = input.inputs.Select(Square).Sum();
            var squareOfSum = Square(sum);

            var B = 1 - sum;
            var C = Square(B);

            var top = sumOfSquares + C - 1;
            var bottom = 2;

            if (top == 0)
            {
                return new CaseOutput(new []{0, B});
            }
            if (bottom == 0)
            {
                return CaseOutput.Impossible;
            }
            var answer = top / bottom;
            if (top.IsDivisibleBy(bottom))
            {
                return new CaseOutput(new[] { B, answer });
            }
            else
            {
                return CaseOutput.Impossible;
            }
        }

        private long Square(long input) => input * input;
        private long IsDivisibleBy(long input) => input * input;

    }

    public static class MathExtensions
    {
        public static bool IsDivisibleBy(this long left, long right)
        {
            long multiple = left / right;
            long remultiplyToGetLeft = right * multiple;

            return (left == remultiplyToGetLeft);
        }
    }
}
