namespace Weightlifting
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

        [Test, Ignore("Not Solved")]
        public void Square()
        {
            var inputString = @"3
3 1
1
2
1
2 3
1 2 1
2 1 2
3 3
3 1 1
3 3 3
2 3 3
";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            string.Join(Environment.NewLine,io.Output).Should().Be(
                @"Case #1: 4
Case #2: 12
Case #3: 20"
            );
        }

    }
}
