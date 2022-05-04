namespace TemplateProject
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
        public void BasicExample()
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
            var expectedOutputString = @"Case #1: 2
Case #2: 2
Case #3: 1
Case #4: 3";

            var outputStringsSeparated = expectedOutputString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(outputStringsSeparated);
        }

        [Test]
        public void SingleCase()
        {
            var inputString = @"1
2 1
-2 6";
            var expectedOutputString = @"Case #1: 2";

            var outputStringsSeparated = expectedOutputString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(outputStringsSeparated);
        }

        [Test, Ignore("Not Used")]
        public void HarderExample()
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
            var expectedOutputString = @"Case #1: 3
Case #2: IMPOSSIBLE
Case #3: 0
Case #4: 2";

            var outputStringsSeparated = expectedOutputString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(outputStringsSeparated);
        }

    }
}