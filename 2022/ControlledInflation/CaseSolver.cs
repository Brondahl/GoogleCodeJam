namespace ControlledInflation
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
            var cases = new CaseSplitter().Configure_TakingNFromFirstValPlusOne().GetCaseLines(lines);
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
            var previousCustomer = new Customer(new long[] { 0 })
                { MaxTarget = 0, MinTarget = 0, OptimalClicksToMaxTarget = 0, OptimalClicksToMinTarget = 0 };

            for (int i = 0; i < input.Customers.Length; i++)
            {
                var nextCustomer = input.Customers[i];

                var prevMin = previousCustomer.OptimalClicksToMinTarget;
                var prevMax = previousCustomer.OptimalClicksToMaxTarget;

                var fromMinToMax = nextCustomer.AdditionalClicksStartingFromXEndingMax(previousCustomer.MinTarget);
                var fromMaxToMax = nextCustomer.AdditionalClicksStartingFromXEndingMax(previousCustomer.MaxTarget);
                var fromMinToMin = nextCustomer.AdditionalClicksStartingFromXEndingMin(previousCustomer.MinTarget);
                var fromMaxToMin = nextCustomer.AdditionalClicksStartingFromXEndingMin(previousCustomer.MaxTarget);

                nextCustomer.OptimalClicksToMaxTarget = Math.Min(prevMin + fromMinToMax, prevMax + fromMaxToMax);
                nextCustomer.OptimalClicksToMinTarget = Math.Min(prevMin + fromMinToMin, prevMax + fromMaxToMin);

                previousCustomer = nextCustomer;
            }

            var answer = Math.Min(previousCustomer.OptimalClicksToMaxTarget, previousCustomer.OptimalClicksToMinTarget);
            return new CaseOutput(answer);
        }

    }
}
