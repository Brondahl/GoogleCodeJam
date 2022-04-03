namespace ChainReaction
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
            var initiationSequence = GetOptimalInitiationSequence();
            var clever = ScoreInitiationSequence(initiationSequence);

            return new CaseOutput(clever);
        }

        private List<GraphModule> GetOptimalInitiationSequence()
        {
            var sequence = new List<GraphModule>();
            
            foreach (var abyssalModule in input.AbyssalModules)
            {
                GetAndPopulateCheapestTriggerFunValues(abyssalModule);
                GetAndPopulateMaxFunValues(abyssalModule);
                sequence.AddRange(GetOptimalInitiationSequence(abyssalModule));
            }

            return sequence;
        }

        private long GetAndPopulateMaxFunValues(GraphModule module)
        {
            if (!module.Triggers.Any())
            {
                module.MaxAssociatedFunDuringCalculation = module.Fun;
            }
            else
            {
                var maxAmongTriggers = module.Triggers.Select(trig => GetAndPopulateMaxFunValues(trig)).Max();
                module.MaxAssociatedFunDuringCalculation = Math.Max(module.Fun, maxAmongTriggers);

            }
            return module.MaxAssociatedFunDuringCalculation;
        }

        private long GetAndPopulateCheapestTriggerFunValues(GraphModule module)
        {
            if (!module.Triggers.Any())
            {
                module.CheapestTriggerDuringCalculation = module.Fun;
            }
            else
            {
                var minAmongTriggers = module.Triggers.Select(trig => GetAndPopulateCheapestTriggerFunValues(trig)).Min();
                module.CheapestTriggerDuringCalculation = Math.Max(module.Fun, minAmongTriggers);
            }
            return module.CheapestTriggerDuringCalculation;
        }

        private List<GraphModule> GetOptimalInitiationSequence(GraphModule module)
        {
            return GetOptimalInitiationSequence_ByLowestMinValue(module);
        }

        private List<GraphModule> GetOptimalInitiationSequence_ByLowestMinValue(GraphModule module)
        {
            if (!module.Triggers.Any())
            {
                return new List<GraphModule> { module };
            }

            var orderedTriggers = module.Triggers.OrderBy(trig => trig.CheapestTriggerDuringCalculation).ToList();

            var result = new List<GraphModule>();
            foreach (var trigger in orderedTriggers)
            {
                result.AddRange(GetOptimalInitiationSequence(trigger));
            }

            return result;
        }

        private List<GraphModule> GetOptimalInitiationSequence_ByLowestMaxValue(GraphModule module)
        {
            if (!module.Triggers.Any())
            {
                return new List<GraphModule>{module};
            }

            var orderedTriggers = module.Triggers.OrderBy(trig => trig.MaxAssociatedFunDuringCalculation).ToList();

            var result = new List<GraphModule>();
            foreach (var trigger in orderedTriggers)
            {
                result.AddRange(GetOptimalInitiationSequence(trigger));
            }

            return result;
        }

        public long ScoreInitiationSequence(List<GraphModule> sequence)
        {
            var triggeredModules = new HashSet<GraphModule> { null };

            long total = 0;
            foreach (var module in sequence)
            {
                var score = Trigger(module, triggeredModules, 0);
                total += score;
            }

            return total;
        }

        private long Trigger(GraphModule module, HashSet<GraphModule> triggeredModules, long funToDate)
        {
            var updatedFun = Math.Max(module.Fun, funToDate);
            triggeredModules.Add(module);

            if (triggeredModules.Contains(module.Target))
            {
                return updatedFun;
            }

            var finalFun = Trigger(module.Target, triggeredModules, updatedFun);
            return finalFun;
        }
    }
}
