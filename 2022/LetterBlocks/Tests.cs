namespace LetterBlocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using FluentAssertions;
    using NUnit.Framework;

    public class TestIOStub : IGoogleCodeJamCommunicator
    {
        private readonly string input;
        public List<string> Output;
        public TestIOStub(string input)
        {
            this.input = input;
        }
        public IEnumerable<string> ReadStringInput(out int numberOfCases)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            numberOfCases = int.Parse(lines.First());
            return lines.Skip(1);
        }

        public void WriteOutput(IEnumerable<string> lines)
        {
            Output = lines.ToList();
        }
    }

    [TestFixture]
    public class Tests
    {

        [Test]
        public void Square()
        {
            var inputString = @"6
5
CODE JAM MIC EEL ZZZZZ
6
CODE JAM MIC EEL ZZZZZ EEK
2
OY YO
2
HASH CODE
6
A AA BB A BA BB
2
CAT TAX";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: ZZZZZJAMMICCODEEEL
Case #2: IMPOSSIBLE
Case #3: IMPOSSIBLE
Case #4: IMPOSSIBLE
Case #5: BBBBBAAAAA
Case #6: IMPOSSIBLE".Split(new []{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries)
            );
        }


    }
}
