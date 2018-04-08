using FluentAssertions;
using NUnit.Framework;

namespace SavingTheUniverseAgain
{
  [TestFixture]
  public class Tests
  {
    [Test]
    [TestCase("1 CS", 1)]
    [TestCase("2 CS",0)]
    [TestCase("1 SS",-1)]
    [TestCase("6 SCCSSC",2)]
    [TestCase("2 CC",0)]
    [TestCase("3 CSCSS",5)]
    public void Samples(string inputString, long expectedOutput)
    {
      new CaseSolver(new CaseInput(inputString)).Solve().Hacks.Should().Be(expectedOutput);
    }
  }
}
