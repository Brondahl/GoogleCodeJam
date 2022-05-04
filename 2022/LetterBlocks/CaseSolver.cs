namespace LetterBlocks
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

        private class Block
        {

            internal Block(string input)
            {
                Text = input;
                Start = input[0];
                End = input[input.Length-1];
                IsUniform = Start == End;
            }

            public bool CheckValidity()
            {
                char currentChain = ' ';
                for (int i = 0; i < Text.Length; i++)
                {
                    var thisChar = Text[i];
                    if (thisChar == currentChain)
                    {
                        continue;
                    }

                    currentChain = thisChar;
                    var previouslySeen = !Contents.Add(thisChar);
                    if (previouslySeen)
                    {
                        return false;
                    }
                }

                return true;
            }

            public HashSet<char> Contents = new HashSet<char>();
            public string Text;
            public char Start;
            public char End;
            public bool IsUniform;
        }
        private CaseInput input;

        internal CaseSolver(CaseInput inputCase)
        {
            input = inputCase;
        }

        private Dictionary<char, List<Block>> startBlockIndexes = new Dictionary<char, List<Block>>();
        private Dictionary<char, List<Block>> endBlockIndexes = new Dictionary<char, List<Block>>();
        private Dictionary<char, List<Block>> uniformBlockIndexes = new Dictionary<char, List<Block>>();
        private List<Block> blocks = new List<Block>();

        private List<Block> AllUniformBlocks => uniformBlockIndexes.Values.SelectMany(l => l).ToList();

        internal CaseOutput Solve()
        {
            try
            {
                return SolveWithAssumptionOfValidity();
            }
            catch (Exception)
            {
                return CaseOutput.Impossible;
            }
        }

        internal CaseOutput SolveWithAssumptionOfValidity()
        {
            foreach (var capChar in "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray())
            {
                startBlockIndexes[capChar] = new List<Block>();
                endBlockIndexes[capChar] = new List<Block>();
                uniformBlockIndexes[capChar] = new List<Block>();
            }

            for (int i = 0; i < input.N; i++)
            {
                var block = new Block(input.Texts[i]);
                
                if (block.Start == block.End)
                {
                    uniformBlockIndexes[block.Start].Add(block);
                }
                else
                {
                    startBlockIndexes[block.Start].Add(block);
                    endBlockIndexes[block.End].Add(block);
                }

                blocks.Add(block);
            }

            var foundReduction = true;
            while (foundReduction)
            {
                foundReduction = Reduce();
            }

            var answer = SolveUnreducible();

            if (answer.CheckValidity())
            {
                return new CaseOutput(answer.Text);
            }
            else
            {
                return CaseOutput.Impossible;
            }

        }

        private Block SolveUnreducible()
        {
            var fullString = string.Concat(blocks.Select(b => b.Text));
            return new Block(fullString);
        }

        private bool Reduce()
        {
            if (blocks.Count == 1) { return false; }


            foreach (var uniformBlockListForChar in uniformBlockIndexes.ToList())
            {
                if (!uniformBlockListForChar.Value.Any())
                {
                    uniformBlockIndexes.Remove(uniformBlockListForChar.Key);
                    continue;
                }

                var uniformBlocks = uniformBlockListForChar.Value;
                if (uniformBlocks.Count > 1)
                {
                    JoinBlocks(uniformBlocks[0], uniformBlocks[1]);
                    return true;
                }
            }

            foreach (var uniformBlockListForChar in uniformBlockIndexes.ToList())
            {
                if (!uniformBlockListForChar.Value.Any())
                {
                    uniformBlockIndexes.Remove(uniformBlockListForChar.Key);
                    continue;
                }

                var uniformBlock = uniformBlockListForChar.Value.Single();
                var uniChar = uniformBlockListForChar.Key;
                if (startBlockIndexes[uniChar].Any())
                {
                    JoinBlocks(uniformBlock, startBlockIndexes[uniChar][0]);
                    return true;
                }
                if (endBlockIndexes[uniChar].Any())
                {
                    JoinBlocks(endBlockIndexes[uniChar][0], uniformBlock);
                    return true;
                }
            }

            foreach (var startBlockListForChar in startBlockIndexes.ToList())
            {
                if(!startBlockListForChar.Value.Any()){continue;}
                var startChar = startBlockListForChar.Key;
                if (endBlockIndexes[startChar].Any())
                {
                    JoinBlocks(endBlockIndexes[startChar][0], startBlockListForChar.Value[0]);
                    return true;
                }
            }

            foreach (var endBlockListForChar in endBlockIndexes.ToList())
            {
                if (!endBlockListForChar.Value.Any()) { continue; }
                var endChar = endBlockListForChar.Key;
                if (startBlockIndexes[endChar].Any())
                {
                    JoinBlocks(endBlockListForChar.Value[0], startBlockIndexes[endChar][0]);
                    return true;
                }
            }
            
            return false;
        }

        private void JoinBlocks(Block left, Block right)
        {
            var newBlock = new Block(left.Text + right.Text);

            blocks.Remove(left);
            blocks.Remove(right);
            startBlockIndexes[left.Start].Remove(left);
            startBlockIndexes[right.Start].Remove(right);
            endBlockIndexes[left.End].Remove(left);
            endBlockIndexes[right.End].Remove(right);
            if (left.IsUniform) { uniformBlockIndexes[left.Start].Remove(left); }
            if (right.IsUniform) { uniformBlockIndexes[right.Start].Remove(right); }


            blocks.Add(newBlock);

            if (newBlock.IsUniform)
            {
                uniformBlockIndexes[newBlock.Start].Add(newBlock);
            }
            else
            {
                startBlockIndexes[newBlock.Start].Add(newBlock);
                endBlockIndexes[newBlock.End].Add(newBlock);
            }
        }

    }
}
