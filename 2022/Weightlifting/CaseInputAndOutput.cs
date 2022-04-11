namespace Weightlifting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            var lineOne = lines.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            Exercises = lineOne[0];
            MaxWeight = lineOne[1];

            ExerciseWeightCounts = lines.Skip(1).Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToArray();
        }

        internal long Exercises;
        internal long MaxWeight;

        internal long[][] ExerciseWeightCounts;
    }

    class CaseMidpoint
    {
        internal long Exercises;
        internal long MaxWeight;

        internal long[] WeightsPerExercise;
        internal long[][] ExerciseWeightPositions;
        internal long[] ExerciseStackLockPointInbound;
        internal long[][] ExerciseInboundLockedStackContents;
    }

    class CaseOutput
    {
        internal CaseOutput(long number)
        {
            Text = number.ToString();
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
