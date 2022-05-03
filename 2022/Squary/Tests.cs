namespace Squary
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
            var inputString = @"4
2 1
-2 6
2 1
-10 10
1 1
0
3 1
2 -2 2";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: 3
Case #2: IMPOSSIBLE
Case #3: 0
Case #4: 2".Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            );
        }

        [Test]
        public void Harder()
        {
            var inputString = @"3
3 10
-2 3 6
6 2
-2 2 1 -2 4 -1
1 12
-5";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: 0
Case #2: -1 15
Case #3: 1 1 1 1 1 1 1 1 1 1 1".Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            );
        }


    }
}
