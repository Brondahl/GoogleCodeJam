namespace PowerArrangers
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

  [TestFixture, Ignore("Obsolete")]
  public class Tests
  {

    [Test]
    public void FullE2ETest()
    {
      var inputString = @"4
2
TARPOL
PROL
3
TARPOR
PROL
TARPRO
6
CODEJAM
JAM
HAM
NALAM
HUM
NOLOM
4
PI
HI
WI
FI";
      var io = new TestIOStub(inputString);
      CaseSolver.Run(io);

      io.Output.Should().BeEquivalentTo(
        "Case #1: 2",
        "Case #2: 0",
        "Case #3: 6",
        "Case #4: 2"
      );
    }

    [Test]
    [TestCase("5", "5 6 8 4 3", -1)]
    [TestCase("3", "8 9 7", 1)]
    public void Samples(string inputString1, string inputString2, int expectedOutput)
    {
      new CaseSolver(new CaseInput(new[] { inputString1, inputString2 }.ToList())).Solve().ErrorIndex.Should().Be(expectedOutput);
    }

    [Test]
    [TestCase("0", "", 0, new int[0], new int[0])]
    [TestCase("1", "1", 1, new[] { 1 }, new int[0])]
    [TestCase("2", "1 2", 1, new[] { 1 }, new[] { 2 })]
    [TestCase("2", "2 1", 1, new[] { 2 }, new[] { 1 })]
    [TestCase("2", "1 1", 1, new[] { 1 }, new[] { 1 })]
    [TestCase("3", "2 3 1", 2, new[] { 1, 2 }, new[] { 3 })]
    [TestCase("3", "1 3 2", 2, new[] { 1, 2 }, new[] { 3 })]
    [TestCase("3", "1 2 2", 2, new[] { 1, 2 }, new[] { 2 })]
    [TestCase("3", "1 2 3", 2, new[] { 1, 3 }, new[] { 2 })]
    [TestCase("4", "1 2 2 3", 2, new[] { 1, 2 }, new[] { 2, 3 })]
    [TestCase("4", "1 3 2 3", 2, new[] { 1, 2 }, new[] { 3, 3 })]
    [TestCase("5", "1 3 2 3 7", 3, new[] { 1, 2, 7 }, new[] { 3, 3 })]
    public void InputParser(string inputString1, string inputString2, int expectedShortLength, int[] expectedEvens, int[] expectedOdds)
    {
      //var input = new CaseInput(new[] { inputString1, inputString2 }.ToList());
      //input.LongestSubList.Should().Be(expectedShortLength);

      //input.EvenV.Should().BeEquivalentTo(expectedEvens);
      //input.OddV.Should().BeEquivalentTo(expectedOdds, options => options.WithStrictOrdering());
    }


    [Test]
    [TestCase("0", "", -1)]
    [TestCase("1", "1", -1)]
    [TestCase("2", "1 2", -1)]
    [TestCase("2", "2 1", 0)]
    [TestCase("2", "1 1", -1)]
    [TestCase("3", "1 1 1", -1)]
    [TestCase("3", "2 1 1", -1)]
    [TestCase("3", "1 2 1", 1)]
    [TestCase("3", "1 2 2", -1)]
    [TestCase("3", "2 1 2", 0)]
    [TestCase("3", "2 2 1", -1)]
    [TestCase("3", "1 2 3", -1)]
    [TestCase("3", "1 3 2", 1)]
    [TestCase("3", "2 1 3", 0)]
    [TestCase("3", "2 3 1", 1)]
    [TestCase("3", "3 1 2", 0)]
    [TestCase("3", "3 2 1", -1)]
    public void OrderingCheckerSmallCases(string inputString1, string inputString2, int expectedOutput)
    {
      new CaseSolver(new CaseInput(new[] { inputString1, inputString2 }.ToList())).Solve().ErrorIndex.Should().Be(expectedOutput);
    }

    [TestCase("4", "1 1 1 1", -1)]

    [TestCase("4", "2 1 1 1", 2)]
    [TestCase("4", "1 2 1 1", -1)]
    [TestCase("4", "1 1 2 1", 2)]
    [TestCase("4", "1 1 1 2", -1)]

    [TestCase("4", "2 2 2 1", 0)]
    [TestCase("4", "2 2 1 2", -1)]
    [TestCase("4", "2 1 2 2", 0)]
    [TestCase("4", "1 2 2 2", -1)]

    [TestCase("4", "1 1 2 2", -1)]
    [TestCase("4", "1 2 1 2", 1)]
    [TestCase("4", "1 2 2 1", -1)]
    [TestCase("4", "2 1 1 2", -1)]
    [TestCase("4", "2 1 2 1", 0)]
    [TestCase("4", "2 2 1 1", -1)]

    [TestCase("4", "1 2 3 4", -1)]
    [TestCase("4", "1 2 4 3", 2)]
    [TestCase("4", "1 3 2 4", 1)]
    [TestCase("4", "1 4 2 3", 1)]
    [TestCase("4", "1 3 4 2", 2)]
    [TestCase("4", "1 4 3 2", -1)]

    [TestCase("4", "2 1 3 4", 0)]
    [TestCase("4", "2 1 4 3", 0)]
    [TestCase("4", "2 3 1 4", 1)]
    [TestCase("4", "2 4 1 3", 1)]
    [TestCase("4", "2 3 4 1", 0)]
    [TestCase("4", "2 4 3 1", 0)]

    [TestCase("4", "3 2 1 4", -1)]
    [TestCase("4", "3 2 4 1", 0)]
    [TestCase("4", "3 1 2 4", 0)]
    [TestCase("4", "3 4 2 1", 0)]
    [TestCase("4", "3 1 4 2", 0)]
    [TestCase("4", "3 4 1 2", -1)]

    [TestCase("4", "4 2 3 1", 0)]
    [TestCase("4", "4 2 1 3", 2)]
    [TestCase("4", "4 3 2 1", 0)]
    [TestCase("4", "4 1 2 3", 0)]
    [TestCase("4", "4 3 1 2", 2)]
    [TestCase("4", "4 1 3 2", 0)]


    [TestCase("4", "1 1 2 3", -1)]
    [TestCase("4", "1 1 3 2", 2)]
    [TestCase("4", "1 2 1 3", 1)]
    [TestCase("4", "1 2 3 1", 2)]
    [TestCase("4", "1 3 1 2", 1)]
    [TestCase("4", "1 3 2 1", -1)]

    [TestCase("4", "2 1 2 3", 0)]
    [TestCase("4", "2 1 3 2", 0)]
    [TestCase("4", "2 2 1 3", -1)]
    [TestCase("4", "2 2 3 1", 0)]
    [TestCase("4", "2 3 1 2", -1)]
    [TestCase("4", "2 3 2 1", 0)]

    [TestCase("4", "3 1 2 3", 0)]
    [TestCase("4", "3 1 3 2", 0)]
    [TestCase("4", "3 2 1 3", -1)]
    [TestCase("4", "3 2 3 1", 0)]
    [TestCase("4", "3 3 1 2", -1)]
    [TestCase("4", "3 3 2 1", 0)]

    [TestCase("4", "1 1 2 3", -1)]
    [TestCase("4", "1 1 3 2", 2)]
    [TestCase("4", "2 1 1 3", -1)]
    [TestCase("4", "2 1 3 1", 0)]
    [TestCase("4", "3 1 1 2", 2)]
    [TestCase("4", "3 1 2 1", 0)]

    [TestCase("4", "1 2 2 3", -1)]
    [TestCase("4", "1 2 3 2", 2)]
    [TestCase("4", "2 2 1 3", -1)]
    [TestCase("4", "2 2 3 1", 0)]
    [TestCase("4", "3 2 1 2", 2)]
    [TestCase("4", "3 2 2 1", 0)]

    [TestCase("4", "1 3 2 3", 1)]
    [TestCase("4", "1 3 3 2", -1)]
    [TestCase("4", "2 3 1 3", 1)]
    [TestCase("4", "2 3 3 1", 0)]
    [TestCase("4", "3 3 1 2", -1)]
    [TestCase("4", "3 3 2 1", 0)]

    [TestCase("4", "1 2 1 3", 1)]
    [TestCase("4", "1 3 1 2", 1)]
    [TestCase("4", "2 1 1 3", -1)]
    [TestCase("4", "2 3 1 1", -1)]
    [TestCase("4", "3 1 1 2", 2)]
    [TestCase("4", "3 2 1 1", 2)]

    [TestCase("4", "1 2 2 3", -1)]
    [TestCase("4", "1 3 2 2", -1)]
    [TestCase("4", "2 1 2 3", 0)]
    [TestCase("4", "2 3 2 1", 0)]
    [TestCase("4", "3 1 2 2", 0)]
    [TestCase("4", "3 2 2 1", 0)]

    [TestCase("4", "1 2 3 3", -1)]
    [TestCase("4", "1 3 3 2", -1)]
    [TestCase("4", "2 1 3 3", 0)]
    [TestCase("4", "2 3 3 1", 0)]
    [TestCase("4", "3 1 3 2", 0)]
    [TestCase("4", "3 2 3 1", 0)]

    [TestCase("4", "1 2 3 1", 2)]
    [TestCase("4", "1 3 2 1", -1)]
    [TestCase("4", "2 1 3 1", 0)]
    [TestCase("4", "2 3 1 1", -1)]
    [TestCase("4", "3 1 2 1", 0)]
    [TestCase("4", "3 2 1 1", 2)]

    [TestCase("4", "1 2 3 2", 2)]
    [TestCase("4", "1 3 2 2", -1)]
    [TestCase("4", "2 1 3 2", 0)]
    [TestCase("4", "2 3 1 2", -1)]
    [TestCase("4", "3 1 2 2", 0)]
    [TestCase("4", "3 2 1 2", 2)]

    [TestCase("4", "1 2 3 3", -1)]
    [TestCase("4", "1 3 2 3", 1)]
    [TestCase("4", "2 1 3 3", 0)]
    [TestCase("4", "2 3 1 3", 1)]
    [TestCase("4", "3 1 2 3", 0)]
    [TestCase("4", "3 2 1 3", -1)]
    public void OrderingChecker4ValueCases(string inputString1, string inputString2, int expectedOutput)
    {
      new CaseSolver(new CaseInput(new[] { inputString1, inputString2 }.ToList())).Solve().ErrorIndex.Should().Be(expectedOutput);
    }

    [TestCase("5", "1 3 2 3 7", 1)]
    public void OrderingCheckerOtherCases(string inputString1, string inputString2, int expectedOutput)
    {
      new CaseSolver(new CaseInput(new[] { inputString1, inputString2 }.ToList())).Solve().ErrorIndex.Should().Be(expectedOutput);
    }
  }
}
