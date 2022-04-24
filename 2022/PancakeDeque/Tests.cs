namespace PancakeDeque
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
2
1 5
4
1 4 2 3
5
10 10 10 10 10
4
7 1 3 1000000";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: 2",
"Case #2: 3",
"Case #3: 5",
"Case #4: 2");
        }


    }
}
