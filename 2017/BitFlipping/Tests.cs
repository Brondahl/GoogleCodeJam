using FluentAssertions;
using NUnit.Framework;

namespace BitFlipping
{
  [TestFixture]
  class Tests
  {
    [Test]
    public static void testParser()
    {
      new CaseInput("+-+-++++--- 4").Sequence.Should().Equal(new[] { true, false, true, false, true, true, true, true, false, false, false });
      new CaseInput("+-+-++++--- 4").SequenceLength.Should().Be(11);
      new CaseInput("+-+-++++--- 4").FlipSize.Should().Be(4);

      new CaseInput("+++++++++++++ 9").Sequence.Should().Equal(new[] { true, true, true, true, true, true, true, true, true, true, true, true, true });
      new CaseInput("+++++++++++++ 9").SequenceLength.Should().Be(13);
      new CaseInput("+++++++++++++ 9").FlipSize.Should().Be(9);

      new CaseInput("---- 2").Sequence.Should().Equal(new[] { false, false, false, false });
      new CaseInput("---- 2").SequenceLength.Should().Be(4);
      new CaseInput("---- 2").FlipSize.Should().Be(2);
    }

    [Test]
    public static void testFlipping()
    {
      var input = new CaseInput("+-+-++++--- 4");
      new CaseSolver(input).MassFlipStartingAtIndex(0);
      new CaseSolver(input).MassFlipStartingAtIndex(4);

      input.Sequence.Should().Equal(new[] { false, true, false, true, false, false, false, false, false, false, false });
    }

    [Test]
    public static void testComplete()
    {
      new CaseSolver(new CaseInput("---+-++- 3")).Solve().Result.Should().Be(3);
      new CaseSolver(new CaseInput("+++++ 4")).Solve().Result.Should().Be(0);
      new CaseSolver(new CaseInput("-+-+- 4")).Solve().Result.Should().Be(-1);
    }

    [Test]
    public static void testOutputText()
    {
      new CaseSolver(new CaseInput("---+-++- 3")).Solve().ToString().Should().Be("3");
      new CaseSolver(new CaseInput("+++++ 4")).Solve().ToString().Should().Be("0");
      new CaseSolver(new CaseInput("-+-+- 4")).Solve().ToString().Should().Be("IMPOSSIBLE");
    }
  }
}
