namespace EdgyBaking
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class Tests
  {
    [Test]
    [TestCase(6.828427, "1 7", "1 1")]
    [TestCase(8, "2 7", "1 1", "1 1")]
    [TestCase(920, "2 920", "50 120", "50 120")]
    [TestCase(32, "1 32","7 4")]
    [TestCase(240, "3 240", "10 20", "20 30", "30 10")]
    public void Samples1(double expectedOutput, params string[] inputStrings)
    {
      new CaseSolver(new CaseInput(inputStrings)).Solve().Closest.Should().BeApproximately(expectedOutput, Math.Pow(10,-6));
    }

    [Test]
    public void Perf()
    {
      double expectedOutput = 4682.8427124745758;
      var inputStrings = new List<string>
      {
        "101 6399",
        "1000 1000"
      };
      inputStrings.AddRange(Enumerable.Repeat("1 1", 100));
      new CaseSolver(new CaseInput(inputStrings.ToArray())).Solve().Closest.Should().BeApproximately(expectedOutput, Math.Pow(10, -6));
    }
  }
}
