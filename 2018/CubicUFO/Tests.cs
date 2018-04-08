using System;
using FluentAssertions;
using NUnit.Framework;
using static System.Math;

namespace CubicUFO
{
  [TestFixture]
  public class Tests
  {
    [Test]
    public void Sample1()
    {
      var input = new CaseInput("1.000000");
      var output = new CaseSolver(input).Solve();
      var stringOutputArray = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

      output.Phi.Should().BeApproximately(0, Pow(10,-10));
      output.Theta.Should().BeApproximately(0, Pow(10, -10));
      output.X.Should().BeApproximately(0.5, Pow(10, -10));
      output.Y.Should().BeApproximately(0, Pow(10, -10));

      //stringOutputArray[0].Should().Be("0.5 0 0");
      //stringOutputArray[1].Should().Be("0 0.5 0");
      stringOutputArray[2].Should().Be("0 0 0.5");
    }

    [Test]
    public void Sample2()
    {
      var input = new CaseInput("1.414213");
      var output = new CaseSolver(input).Solve();
      var stringOutputArray = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      var area = Sqrt(2) * Cos(PI / 4 - output.Theta);

      area.Should().BeApproximately(1.414213, Pow(10, -10));
      output.Phi.Should().BeApproximately(0, Pow(10, -3));
      output.Theta.Should().BeApproximately(PI / 4, Pow(10, -3));
      //output.X.Should().BeApproximately(0.3535533905932738, Pow(10, -10));
      //output.Y.Should().BeApproximately(0.3535533905932738, Pow(10, -10));

      //stringOutputArray[0].Should().Be("0.3535533905932738 0.3535533905932738 0");
      //stringOutputArray[1].Should().Be("-0.3535533905932738 0.3535533905932738 0");
      stringOutputArray[2].Should().Be("0 0 0.5");
    }

    [Test]
    public void Sample2_Exact()
    {
      var input = new CaseInput(Sqrt(2).ToString());
      var output = new CaseSolver(input).Solve();
      var stringOutputArray = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      var area = Sqrt(2) *Cos(PI / 4 - output.Theta);

      area.Should().BeApproximately(Sqrt(2), Pow(10, -10));
      output.Phi.Should().BeApproximately(0, Pow(10, -10));
      output.Theta.Should().BeApproximately(PI / 4, Pow(10, -10));
      output.X.Should().BeApproximately(0.3535533905932738, Pow(10, -10));
      output.Y.Should().BeApproximately(0.3535533905932738, Pow(10, -10));

      //stringOutputArray[0].Should().Be("0.3535533905932738 0.3535533905932738 0");
      //stringOutputArray[1].Should().Be("-0.3535533905932738 0.3535533905932738 0");
      stringOutputArray[2].Should().Be("0 0 0.5");
    }
  }
}
