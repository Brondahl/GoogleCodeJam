namespace TwistyPassages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;

    public class CaseSolver
    {
        private static IGoogleCodeJamInteractiveCommunicator InOut;
        public static void Run(IGoogleCodeJamInteractiveCommunicator io = null)
        {
            InOut = io ?? new GoogleCodeJam2022CommunicatorWithInteractivity();
            var numberOfCases = InOut.ReadSingleLongInput();

            for (int i = 0; i < numberOfCases; i++)
            {
                new CaseSolver(InOut, i).SolveInteractively();
            }
        }

        private readonly IGoogleCodeJamInteractiveCommunicator commsStream;
        private CaseInput initialInput;
        private HashSet<long> knownRooms = new HashSet<long>();
        private long nextTeleportRoom = 1;
        private List<Sample> samples;

        internal CaseSolver(IGoogleCodeJamInteractiveCommunicator commsStream, int caseNumber)
        {
            this.commsStream = commsStream;
        }

        public class Sample
        {
            public long Value;
            public decimal Weight;
        }

        internal void SolveInteractively()
        {
            initialInput = new CaseInput(commsStream.ReadSingleLineOfLongsInput());

            var firstStepInput = new CaseStepInput(commsStream.ReadSingleLineOfLongsInput());
            samples = new List<Sample>();
            samples.Add(new Sample{Value = firstStepInput.NumberOfPassages, Weight = 1});
            knownRooms.Add(firstStepInput.RoomNumber);

            var maxSteps = Math.Min(initialInput.K, initialInput.N);
            for (int step = 1; step < maxSteps + 1; step++)
            {
                var nextAction = DetermineNextAction(step);
                commsStream.WriteSingleInteractiveOutput(nextAction.ToString());
                var stepInput = new CaseStepInput(commsStream.ReadSingleLineOfLongsInput());
                UpdateSamples(step, stepInput);
            }

            var integerGuess =  CalculateFinalGuess();
            commsStream.WriteSingleInteractiveOutput(CaseStepOutput.FinalGuess(integerGuess).ToString());
        }

        private long CalculateFinalGuess()
        {
            var decimalGuess = samples.Select(s => s.Value * s.Weight).Sum() / samples.Count;
            return (long)Math.Round(decimalGuess, 0);
        }

        private void UpdateSamples(int step, CaseStepInput stepInput)
        {
            if (step % 2 == 0)
            {
                samples.Add(new Sample
                {
                    Value = stepInput.NumberOfPassages,
                    Weight = 1
                });
            }
            else
            {
                var previousStepValue = samples.Last().Value;
                samples.Add(new Sample
                {
                    Value = stepInput.NumberOfPassages,
                    Weight = (decimal)previousStepValue / stepInput.NumberOfPassages
                });
            }
        }

        private CaseStepOutput DetermineNextAction(int step)
        {
            if (step % 2 == 1)
            {
                return CaseStepOutput.Walk();
            }
            else
            {
                while (knownRooms.Contains(nextTeleportRoom) && nextTeleportRoom < initialInput.N)
                {
                    nextTeleportRoom++;
                }
                var nextRoom = Math.Min(nextTeleportRoom, initialInput.N);
                return CaseStepOutput.Teleport(nextRoom);
                
            }
        }

    }
}
