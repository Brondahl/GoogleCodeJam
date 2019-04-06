namespace Cryptopangrams
{
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class Tests
  {
    [Test]
    [TestCase(
      "103 31",
      "217 1891 4819 2291 2987 3811 1739 2491 4717 445 65 1079 8383 5353 901 187 649 1003 697 3239 7663 291 123 779 1007 3551 1943 2117 1679 989 3053",
      "CJQUIZKNOWBEVYOFDPFLUXALGORITHMS")]
    [TestCase(
      "10000 25",
      "3292937 175597 18779 50429 375469 1651121 2102 3722 2376497 611683 489059 2328901 3150061 829981 421301 76409 38477 291931 730241 959821 1664197 3057407 4267589 4729181 5335543",
      "SUBDERMATOGLYPHICFJKNQVWXZ")]
    public void Samples(string inputString1, string inputString2, string expectedOutput)
    {
      new CaseSolver(new CaseInput(new[] { inputString1, inputString2 }.ToList())).Solve().Decryption.Should().Be(expectedOutput);
    }
  }
}
