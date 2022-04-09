namespace DoubleOrOneThing
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
            var inputString = @"2
2 2
3 3";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1:
..+-+
..|.|
+-+-+
|.|.|
+-+-+",
                @"Case #2:
..+-+-+
..|.|.|
+-+-+-+
|.|.|.|
+-+-+-+
|.|.|.|
+-+-+-+"
            );
        }


        [Test]
        public void OffSquare()
        {
            var inputString = @"1
3 4";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1:
..+-+-+-+
..|.|.|.|
+-+-+-+-+
|.|.|.|.|
+-+-+-+-+
|.|.|.|.|
+-+-+-+-+"
            );
        }

    }
}
