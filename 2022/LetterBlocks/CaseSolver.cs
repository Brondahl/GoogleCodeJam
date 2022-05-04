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

        private DictionaryOfLists<char, Block> startBlockIndexes = new DictionaryOfLists<char, Block>();
        private DictionaryOfLists<char, Block> endBlockIndexes = new DictionaryOfLists<char, Block>();
        private DictionaryOfLists<char, Block> uniformBlockIndexes = new DictionaryOfLists<char, Block>();
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
            for (int i = 0; i < input.N; i++)
            {
                var block = new Block(input.Texts[i]);
                
                if (block.Start == block.End)
                {
                    uniformBlockIndexes.Add(block.Start, block);
                }
                else
                {
                    startBlockIndexes.Add(block.Start, block);
                    endBlockIndexes.Add(block.End, block);
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


            foreach (var uniformBlockListForChar in uniformBlockIndexes)
            {
                var uniformBlocks = uniformBlockListForChar.Value;
                if (uniformBlocks.Count > 1)
                {
                    JoinBlocks(uniformBlocks[0], uniformBlocks[1]);
                    return true;
                }
            }

            foreach (var uniformBlockListForChar in uniformBlockIndexes)
            {
                var uniformBlock = uniformBlockListForChar.Value.Single();
                var uniChar = uniformBlockListForChar.Key;
                if (startBlockIndexes.ContainsKey(uniChar))
                {
                    JoinBlocks(uniformBlock, startBlockIndexes[uniChar][0]);
                    return true;
                }
                if (endBlockIndexes.ContainsKey(uniChar))
                {
                    JoinBlocks(endBlockIndexes[uniChar][0], uniformBlock);
                    return true;
                }
            }

            foreach (var startBlockListForChar in startBlockIndexes)
            {
                var startChar = startBlockListForChar.Key;
                if (endBlockIndexes.ContainsKey(startChar))
                {
                    JoinBlocks(endBlockIndexes[startChar][0], startBlockListForChar.Value[0]);
                    return true;
                }
            }

            foreach (var endBlockListForChar in endBlockIndexes)
            {
                var endChar = endBlockListForChar.Key;
                if (startBlockIndexes.ContainsKey(endChar))
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
            startBlockIndexes.Remove(left.Start, left);
            startBlockIndexes.Remove(right.Start, right);
            endBlockIndexes.Remove(left.End, left);
            endBlockIndexes.Remove(right.End, right);
            if (left.IsUniform) { uniformBlockIndexes.Remove(left.Start, left); }
            if (right.IsUniform) { uniformBlockIndexes.Remove(right.Start, right); }

            blocks.Add(newBlock);

            if (newBlock.IsUniform)
            {
                uniformBlockIndexes.Add(newBlock.Start, newBlock);
            }
            else
            {
                startBlockIndexes.Add(newBlock.Start, newBlock);
                endBlockIndexes.Add(newBlock.End, newBlock);
            }
        }

    }
}
