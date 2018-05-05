namespace AntStack
{
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class Tests
  {
    [Test]
    [TestCase(new long[] { 1, 1 }, 2)]
    [TestCase(new long[] { 1, 6 }, 2)]
    [TestCase(new long[] { 1, 7 }, 2)]
    [TestCase(new long[] { 6, 1 }, 2)]
    [TestCase(new long[] { 7, 1 }, 1)]
    [TestCase(new long[] { 61, 10 }, 1)]
    [TestCase(new long[] { 60, 10 }, 2)]
    [TestCase(new long[] { 59, 10 }, 2)]


    [TestCase(new long[] { 9, 1 }, 1)]
    [TestCase(new long[] { 8, 4, 100 }, 3)]
    [TestCase(new long[] { 10, 10, 10, 10, 10, 10, 10, 10, 100 }, 8)]

    [TestCase(new long[] { 8, 2, 10, 3, 4, 1, 12 }, 5)]
    [TestCase(new long[] { 8, 2, 10, 3, 4, 1, 12, 3, 3, 2 }, 6)]
    public void Samples(long[] inputWeights, int expectedOutput)
    {
      var input = new CaseInput();
      input.N = inputWeights.Length;
      input.AntWeights = inputWeights;
      new CaseSolver(input).Solve().maxCount.Should().Be(expectedOutput);
    }
  }
}
