namespace ChainReaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CaseInput
    {
        public readonly List<string> lines;

        internal CaseInput(List<string> lines)
        {
            this.lines = lines;
            N = int.Parse(lines[0]);
            var funs = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var targets = lines[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            for (int arrayIndex = 0; arrayIndex < N; arrayIndex++)
            {
                var targetGraphModule = targets[arrayIndex] > 0 ? Modules[targets[arrayIndex]] : null;

                var newGraphModule = new GraphModule
                {
                    Id = arrayIndex+1,
                    Fun = funs[arrayIndex],
                    Target = targetGraphModule
                };

                targetGraphModule?.Triggers.Add(newGraphModule);
                Modules.Add(arrayIndex+1, newGraphModule);
            }

            foreach (var module in Modules.Values)
            {
                if (module.Target == null)
                {
                    AbyssalModules.Add(module);
                }

                if (!module.Triggers.Any())
                {
                    Initiators.Add(module);
                }
            }
        }

        internal int N;
        internal Dictionary<int, GraphModule> Modules = new Dictionary<int, GraphModule>();
        internal List<GraphModule> Initiators = new List<GraphModule>();
        internal List<GraphModule> AbyssalModules = new List<GraphModule>();
    }


    public class GraphModule
    {
        public int Id;
        public long Fun;
        public GraphModule Target;
        public List<GraphModule> Triggers = new List<GraphModule>();
        public long MaxAssociatedFunDuringCalculation = 0;
        public long CheapestTriggerDuringCalculation = 0;
    }


    class CaseOutput
    {
        internal CaseOutput(long answer)
        {
            Text = answer.ToString();
        }

        internal string Text;

        public override string ToString()
        {
            return Text;
        }
    }

}
