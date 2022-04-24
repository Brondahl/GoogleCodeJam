namespace PancakeDeque
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
        private long currentLimit = 0;
        
        internal CaseSolver(CaseInput inputCase)
        {
            input = inputCase;
        }

        internal CaseOutput Solve()
        {
            var max = input.Values.Max();
            var maxLocationScores = new List<int>();
            
            for (int i = 0; i < input.Values.Count; i++)
            {
                if (input.Values[i] == max)
                {
                    var score = EvaluateGivenCentrePointIndexWhichIsAMax(i);
                    maxLocationScores.Add(score);
                }
            }
            
            return new CaseOutput(maxLocationScores.Max());
        }

        internal Queue<long> SimplifyQueue(Queue<long> queueIn)
        {
            var last = (long)0;
            var queueOut = new Queue<long>();
            var length = queueIn.Count;
            for (int i = 0; i < length; i++)
            {
                var next = queueIn.Dequeue();
                if (next >= last)
                {
                    last = next;
                    queueOut.Enqueue(next);
                }
            }

            return queueOut;
        }

        internal int EvaluateGivenCentrePointIndexWhichIsAMax(int centre)
        {
            var lastValue = input.Values[centre];
            var queueLeft = SimplifyQueue(new Queue<long>(input.Values.Take(centre).ToList()));
            var queueRight = SimplifyQueue(new Queue<long>(input.Values.Skip(centre + 1).Reverse().ToList()));

            var score = queueLeft.Count + queueRight.Count + 1;
            //var limit = 0;
            ////var nextLeft = queueLeft.Peek();
            ////var nextRight = queueRight.Peek();

            ////while (queueLeft.Any() || queueRight.Any())
            ////{
            ////    if (nextLeft <= nextRight)
            ////    {
            ////        queueLeft.Dequeue();
            ////        score++;
            ////        nextLeft = queueLeft.Peek();
            ////        limit
            ////    }
            ////    else
            ////    {
            ////        queueRight.Dequeue();
            ////        score++;
            ////        nextRight = queueRight.Peek();
            ////    }
            ////}

            //if (limit < lastValue)
            //{
            //    score++;
            //}

            return score;
        }

        private Tuple<long, long> PeekLeftRight()
        {
            return Tuple.Create(input.Values.First(), input.Values.Last());
        }
    }
}
