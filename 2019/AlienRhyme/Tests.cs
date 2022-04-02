namespace AlienRhyme
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
      var lines = input.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
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
    [TestCase(
@"2
TARPOL
PROL",
      2)]
    [TestCase(
@"3
TARPOR
PROL
TARPRO",
      0)]
    [TestCase(
@"6
CODEJAM
JAM
HAM
NALAM
HUM
NOLOM",
      6)]
    [TestCase(
      @"4
PI
HI
WI
FI",
      2)]
    [TestCase(
      @"4
PI
HI
WI
FI
GI",
      2)]
    [TestCase(
      @"4
PI
HI
WI
HIFI",
      2)]
    [TestCase(
      @"4
PI
HI
WI
HIWI",
      4)]
    [TestCase(
      @"5
ABCC
ADCC
CCB
DCB
EDCC",
      4)]
    [TestCase(
      @"5
ABCC
ADCC
CCB
DCC
EDCC",
      4)]
    public void Samples(string inputString1, int expectedOutput)
    {
      new CaseSolver(new CaseInput(inputString1.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList())).Solve().Pairs.Should().Be(expectedOutput);
    }

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
  }
}
