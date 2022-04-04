using System.IO;

namespace ChainReaction
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
            var lines = input.Split(new[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
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
            var inputString = @"3
4
60 20 40 50
0 1 1 2
5
3 2 1 4 5
0 1 1 1 0
8
100 100 100 90 80 100 90 100
0 1 2 1 2 3 1 3";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: 110",
                "Case #2: 14",
                "Case #3: 490"
            );
        }

        [Test]
        public void Custom()
        {
            var inputString = @"1
8
110 120 140 190 80 130 90 150
0 1 2 1 2 3 1 3";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: 710"//11+19+12+14+15 = 7,4,5,6,8
            );
        }

        [Test]
        public void Custom2()
        {
            var inputString = @"1
5
10 2 11 1 15
0 1 1 2 2";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: 36"//10+11+15 = 4,3,5
            );
        }

        [Test, Repeat(10)]
        public void Random()
        {
            var rand = new Random();
            var x = 10;
            var funs = new int[x];
            var targets = new int[x];

            for (int i = 0; i < x; i++)
            {
                funs[i] = rand.Next(1000);
                targets[i] = rand.Next(i/2);
            }

            var inputString = $@"1
{x}
{string.Join(" ",funs.Select(y => y.ToString()))}
{string.Join(" ", targets.Select(z => z.ToString()))}";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            //io.Output.Should().BeEquivalentTo(
            //    @"Case #1: 36"//10+11+15 = 4,3,5
            //);
        }

        [Test]
        public void Final()
        {
            var inputString = File.ReadAllText(@"..\..\test_set_1\ts1_input.txt");
            var expectedOutputStrings = File.ReadAllLines(@"..\..\test_set_1\ts1_output.txt");
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(expectedOutputStrings);
        }

        [Test]
        public void FailedFinal()
        {
            var inputString = @"1
9
601543813 362455031 504985097 57861633 426966649 170032630 319023699 251514672 645648527
0 0 1 1 1 2 2 3 7";
            var io = new TestIOStub(inputString);
            CaseSolver.Run(io);

            io.Output.Should().BeEquivalentTo(
                @"Case #1: 2541599117"
            );
        }


    }
}
