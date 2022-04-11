namespace Weightlifting
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

        #region perms
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();

            var factorials = Enumerable.Range(0, array.Length + 1)
                .Select(Factorial)
                .ToArray();

            for (var i = 0L; i < factorials[array.Length]; i++)
            {
                var sequence = GenerateSequence(i, array.Length - 1, factorials);

                yield return GeneratePermutation(array, sequence);
            }
        }

        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var clone = (T[])array.Clone();

            for (int i = 0; i < clone.Length - 1; i++)
            {
                Swap(ref clone[i], ref clone[i + sequence[i]]);
            }

            return clone;
        }

        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var sequence = new int[size];

            for (var j = 0; j < sequence.Length; j++)
            {
                var facto = factorials[sequence.Length - j];

                sequence[j] = (int)(number / facto);
                number = (int)(number % facto);
            }

            return sequence;
        }

        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        private static long Factorial(int n)
        {
            long result = n;

            for (int i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }
        #endregion

        internal CaseOutput Solve()
        {
            var weightsPerExercise = input.ExerciseWeightCounts.Select(ex => ex.Sum()).ToArray();
            var exerciseWeightPositions = weightsPerExercise.Select(count => Enumerable.Repeat((long)-1, (int)count).ToArray()).ToArray();
            var mid = new CaseMidpoint
            {
                Exercises = input.Exercises,
                MaxWeight = input.MaxWeight,
                WeightsPerExercise = weightsPerExercise,
                ExerciseWeightPositions = exerciseWeightPositions,
                ExerciseStackLockPointInbound = new long[input.Exercises],
                ExerciseInboundLockedStackContents = new long[input.Exercises][]
            };

            var isFirstLoop = true;
            for (long ex = mid.Exercises - 1; ex >= 1; ex--)
            {
                var thisExWeightCounts = input.ExerciseWeightCounts[ex];
                var prevExWeightCounts = input.ExerciseWeightCounts[ex - 1];
                var outboundLockPoint = isFirstLoop ? mid.WeightsPerExercise[ex] : mid.ExerciseStackLockPointInbound[ex + 1];
                var thisExFlexibleWeights = 0;
                var minWeightsCounts = Enumerable.Zip(thisExWeightCounts, prevExWeightCounts, Math.Min).ToArray();
                var lockPoint = minWeightsCounts.Sum() - 1;

                var inboundUnlockedWeightCounts = Enumerable.Zip(prevExWeightCounts, minWeightsCounts, (prev, min) => prev - min).ToArray();
                var inboundUnlockedWeightsTotalCount = inboundUnlockedWeightCounts.Sum();
                if (lockPoint + 1 + inboundUnlockedWeightsTotalCount != mid.WeightsPerExercise[ex - 1]) { Thrower.TriggerMemLimit(); }

                var outboundUnlockedWeightCounts = Enumerable.Zip(thisExWeightCounts, minWeightsCounts, (th, min) => th - min).ToArray();
                var outboundUnlockedWeightsTotalCount = outboundUnlockedWeightCounts.Sum();
                if (lockPoint + 1 + outboundUnlockedWeightsTotalCount != mid.WeightsPerExercise[ex]) { Thrower.TriggerMemLimit(); }

                var inboundUnlockedWeightPositions = inboundUnlockedWeightCounts.SelectMany((count, index) => Enumerable.Repeat(index, (int)count)).ToArray();
                var outboundUnlockedWeightPositions = outboundUnlockedWeightCounts.SelectMany((count, index) => Enumerable.Repeat(index, (int)count)).ToArray();

                mid.ExerciseStackLockPointInbound[ex] = lockPoint;
                mid.ExerciseInboundLockedStackContents[ex] = minWeightsCounts;

                for (int i = 0; i < inboundUnlockedWeightsTotalCount; i++)
                {
                    mid.ExerciseWeightPositions[ex - 1][lockPoint + 1 + i] = inboundUnlockedWeightPositions[i];
                }
                mid.ExerciseInboundLockedStackContents[ex] = minWeightsCounts;

                isFirstLoop = false;
            }

            return OutputFromMidPoint(mid);
        }

        internal CaseOutput OutputFromMidPoint(CaseMidpoint solution)
        {
            solution.ExerciseStackLockPointInbound[0] = -1;

            for (int ex = 0; ex < solution.Exercises - 1; ex++)
            {
                var thisEx = solution.ExerciseWeightPositions[ex];
                var nextEx = solution.ExerciseWeightPositions[ex+1];

                var minLength = Math.Min(thisEx.LongLength, nextEx.LongLength);

                var calculatedLockPoint = minLength - 1;
                for (int i = 0; i < minLength; i++)
                {
                    if (thisEx[i] != nextEx[i])
                    {
                        calculatedLockPoint = i - 1;
                        break;
                    }
                }

                if (calculatedLockPoint != solution.ExerciseStackLockPointInbound[ex + 1]) { Thrower.TriggerMemLimit(); }

                //solution.ExerciseStackLockPointInbound[ex + 1] = calculatedLockPoint;
            }

            long currentStackCount = 0;
            long operationCount = 0;

            for (int ex = 0; ex < solution.Exercises; ex++)
            {
                var removeOps = currentStackCount - (solution.ExerciseStackLockPointInbound[ex] + 1);
                currentStackCount = solution.ExerciseStackLockPointInbound[ex] + 1;
                operationCount += removeOps;
                
                var addOps = solution.WeightsPerExercise[ex] - currentStackCount;
                operationCount += addOps;
                currentStackCount = solution.WeightsPerExercise[ex];
            }

            var finalRemoveOps = currentStackCount;
            currentStackCount = 0;
            operationCount += finalRemoveOps;

            return new CaseOutput(operationCount);
        }
    }
}
