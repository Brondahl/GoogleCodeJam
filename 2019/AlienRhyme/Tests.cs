namespace AlienRhyme
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

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
HIFI",
      2)]
    [TestCase(
      @"4
PI
HI
WI
HIWI",
      4)]
    public void Samples(string inputString1, int expectedOutput)
    {
      new CaseSolver(new CaseInput(inputString1.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList())).Solve().Pairs.Should().Be(expectedOutput);
    }
  }
}
