namespace MysteriousRoadSigns
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class Tests
  {
    [Test]
    public void ParsingInput()
    {
      var input = @"3
3 7 4
4 5 2
7 3 2";
      var inputLines = input.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
      var caseInput = new CaseInput(inputLines.ToList());

      caseInput.Signs.Should().HaveCount(3);
      caseInput.S.Should().Be(3);

      caseInput.Signs[0].D.Should().Be(3);
      caseInput.Signs[1].D.Should().Be(4);
      caseInput.Signs[2].D.Should().Be(7);

      caseInput.Signs[0].A.Should().Be(7);
      caseInput.Signs[1].A.Should().Be(5);
      caseInput.Signs[2].A.Should().Be(3);

      caseInput.Signs[0].B.Should().Be(4);
      caseInput.Signs[1].B.Should().Be(2);
      caseInput.Signs[2].B.Should().Be(2);

      caseInput.Signs[0].M.Should().Be(10);
      caseInput.Signs[1].M.Should().Be(9);
      caseInput.Signs[2].M.Should().Be(10);

      caseInput.Signs[0].N.Should().Be(-1);
      caseInput.Signs[1].N.Should().Be(2);
      caseInput.Signs[2].N.Should().Be(5);
    }

    [Test]
    public void SetRule()
    {
      int x = 1;
      int y = 2;
      int a = 3;
      int b = 4;

      var xaRule = new CaseSolver.SetRule { Left = x, Right = a };
      var x_blank_Rule = new CaseSolver.SetRule { Left = x, Right = null };
      var blank_a_Rule = new CaseSolver.SetRule { Left = null, Right = a };
      var blank_blank_Rule = new CaseSolver.SetRule { Left = null, Right = null };

      var xaSign = new Sign(1, x, a);
      var xbSign = new Sign(1, x, b);
      var yaSign = new Sign(1, y, a);
      var ybSign = new Sign(1, y, b);

      xaRule.IsCompatibleWith(xaSign).Should().BeTrue();
      xaRule.IsCompatibleWith(xbSign).Should().BeTrue();
      xaRule.IsCompatibleWith(yaSign).Should().BeTrue();
      xaRule.IsCompatibleWith(ybSign).Should().BeFalse();

      xaRule.ActivelySupports(xaSign).Should().BeTrue();
      xaRule.ActivelySupports(xbSign).Should().BeTrue();
      xaRule.ActivelySupports(yaSign).Should().BeTrue();
      xaRule.ActivelySupports(ybSign).Should().BeFalse();



      x_blank_Rule.IsCompatibleWith(xaSign).Should().BeTrue();
      x_blank_Rule.IsCompatibleWith(xbSign).Should().BeTrue();
      x_blank_Rule.IsCompatibleWith(yaSign).Should().BeTrue();
      x_blank_Rule.IsCompatibleWith(ybSign).Should().BeTrue();

      x_blank_Rule.ActivelySupports(xaSign).Should().BeTrue();
      x_blank_Rule.ActivelySupports(xbSign).Should().BeTrue();
      x_blank_Rule.ActivelySupports(yaSign).Should().BeFalse();
      x_blank_Rule.ActivelySupports(ybSign).Should().BeFalse();



      blank_a_Rule.IsCompatibleWith(xaSign).Should().BeTrue();
      blank_a_Rule.IsCompatibleWith(xbSign).Should().BeTrue();
      blank_a_Rule.IsCompatibleWith(yaSign).Should().BeTrue();
      blank_a_Rule.IsCompatibleWith(ybSign).Should().BeTrue();

      blank_a_Rule.ActivelySupports(xaSign).Should().BeTrue();
      blank_a_Rule.ActivelySupports(xbSign).Should().BeFalse();
      blank_a_Rule.ActivelySupports(yaSign).Should().BeTrue();
      blank_a_Rule.ActivelySupports(ybSign).Should().BeFalse();



      blank_blank_Rule.IsCompatibleWith(xaSign).Should().BeTrue();
      blank_blank_Rule.IsCompatibleWith(xbSign).Should().BeTrue();
      blank_blank_Rule.IsCompatibleWith(yaSign).Should().BeTrue();
      blank_blank_Rule.IsCompatibleWith(ybSign).Should().BeTrue();

      blank_blank_Rule.ActivelySupports(xaSign).Should().BeFalse();
      blank_blank_Rule.ActivelySupports(xbSign).Should().BeFalse();
      blank_blank_Rule.ActivelySupports(yaSign).Should().BeFalse();
      blank_blank_Rule.ActivelySupports(ybSign).Should().BeFalse();

    }
  }
}
