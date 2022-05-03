namespace ControlledInflation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        internal CaseInput(List<string> lines)
        {
            var values = lines.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            N = values[0];
            P = values[1];

            var dataLines = lines.Skip(1).Select(line => line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToArray();
            Customers = dataLines.Select(data => new Customer(data)).ToArray();
        }

        internal long N;
        internal long P;
        internal Customer[] Customers;
    }

    class Customer
    {
        internal Customer(long[] targets)
        {
            Targets = targets;
            MinTarget = targets.Min();
            MaxTarget = targets.Max();
            RangeWidth = MaxTarget - MinTarget;
        }
        internal long[] Targets;
        internal long MinTarget;
        internal long MaxTarget;
        internal long RangeWidth;

        internal long AdditionalClicksStartingFromXEndingMax(long x)
        {
            if (x == MaxTarget)
            {
                return 2 * RangeWidth;
            }
            if (x > MaxTarget)
            {
                return (x - MaxTarget) + (2 * RangeWidth);
            }
            if (x == MinTarget)
            {
                return RangeWidth;
            }
            if (x < MinTarget)
            {
                return (MinTarget - x) + (RangeWidth);
            }
            else
            {
                return (x - MinTarget) + (RangeWidth);
            }
        }

        internal long AdditionalClicksStartingFromXEndingMin(long x)
        {
            if (x == MaxTarget)
            {
                return RangeWidth;
            }
            if (x > MaxTarget)
            {
                return (x - MaxTarget) + (RangeWidth);
            }
            if (x == MinTarget)
            {
                return 2 * RangeWidth;
            }
            if (x < MinTarget)
            {
                return (MinTarget - x) + (2 * RangeWidth);
            }
            else
            {
                return (MaxTarget - x) + (RangeWidth);
            }
        }


        internal long OptimalClicksToMinTarget = long.MaxValue;
        internal long OptimalClicksToMaxTarget = long.MaxValue;
    }

    class CaseOutput
    {
        internal CaseOutput(long clicks)
        {
            Text = clicks.ToString();
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
