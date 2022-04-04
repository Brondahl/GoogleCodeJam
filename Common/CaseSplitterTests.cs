namespace Common
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  class CaseSplitterTests
  {
    private static readonly CaseSplitter splitter = new CaseSplitter();

    [Test]
    public static void CaseLinesFromSingleLines()
    {
      var inputFile =
        @"7
1
3
5
t
g
2
y
";
      var inputLines = inputFile.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);

      var output = splitter.GetSingleLineCases(inputLines);

      output.Should().BeEquivalentTo("1", "3", "5", "t", "g", "2", "y");
    }


    [Test]
    public static void CaseLinesFromConstantNumberOfLines()
    {
      var inputFile =
        @"3
3 3
G??
?C?
??J
3 4
CODE
????
?JAM
foo
CA
KE
ZZ".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);

      var output = splitter.Configure_ConstantMultiLineCases(4).GetCaseLines(inputFile).ToList();

      output.Should().HaveCount(3);

      output.First().Should().HaveCount(4);
      output.Skip(1).First().Should().HaveCount(4);
      output.Skip(2).First().Should().HaveCount(4);

      output.First().Skip(1).First().Should().Be("G??");

      output.Skip(1).First().Skip(1).First().Should().Be("CODE");

      output.Skip(2).First().First().Should().Be("foo");
      output.Skip(2).First().Skip(1).First().Should().Be("CA");
    }


    [Test]
    public static void CaseLineForNFromFirstValTest()
    {
      var inputFile = @"
3
3 3
G??
?C?
??J
3 4
CODE
????
?JAM
2 2
CA
KE
";
      var inputLines = inputFile.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);

      var output = splitter.Configure_TakingNFromFirstValPlusOne().GetCaseLines(inputLines).ToList();

      output.Should().HaveCount(3);

      output.First().Should().HaveCount(4);
      output.Skip(1).First().Should().HaveCount(4);
      output.Skip(2).First().Should().HaveCount(3);

      output.First().Skip(1).First().Should().Be("G??");

      output.Skip(1).First().Skip(1).First().Should().Be("CODE");

      output.Skip(2).First().Skip(1).First().Should().Be("CA");
    }
    [Test]
    public static void CaseLineForNFromFirstValPlusOneTest()
    {
      var inputFile =
@"3
3 3
foo
G??
?C?
??J
3 4
bar
CODE
????
?JAM
2 2
baz
CA
KE".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);

      var output = splitter.Configure_TakingNFromFirstValPlusTwo().GetCaseLines(inputFile).ToList();

      output.Should().HaveCount(3);

      output.First().Should().HaveCount(5);
      output.Skip(1).First().Should().HaveCount(5);
      output.Skip(2).First().Should().HaveCount(4);

      output.First().First().Should().Be("3 3");
      output.First().Skip(1).First().Should().Be("foo");
      output.First().Skip(2).First().Should().Be("G??");
      output.First().Skip(3).First().Should().Be("?C?");
      output.First().Skip(4).First().Should().Be("??J");

      output.Skip(1).First().First().Should().Be("3 4");
      output.Skip(1).First().Skip(1).First().Should().Be("bar");
      output.Skip(1).First().Skip(2).First().Should().Be("CODE");
      output.Skip(1).First().Skip(3).First().Should().Be("????");
      output.Skip(1).First().Skip(4).First().Should().Be("?JAM");
    }

    [Test]
    public static void CaseLineForNFromSecondVal()
    {
      var inputFile =
@"3
3 3
?C?
??J
3 4
CODE
????
?JAM
4 2
CA".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);

      var output = splitter.Configure_TakingNFromSecondVal().GetCaseLines(inputFile).ToList();

      output.Should().HaveCount(3);

      output.First().Should().HaveCount(3);
      output.Skip(1).First().Should().HaveCount(4);
      output.Skip(2).First().Should().HaveCount(2);

      output.First().First().Should().Be("3 3");
      output.First().Skip(1).First().Should().Be("?C?");
      output.First().Skip(2).First().Should().Be("??J");

      output.Skip(1).First().First().Should().Be("3 4");
      output.Skip(1).First().Skip(1).First().Should().Be("CODE");
      output.Skip(1).First().Skip(2).First().Should().Be("????");
      output.Skip(1).First().Skip(3).First().Should().Be("?JAM");

      output.Skip(2).First().First().Should().Be("4 2");
      output.Skip(2).First().Skip(1).First().Should().Be("CA");
    }

    [Test]
    public static void CustomFormat()
    {
      var inputFile =
@"3
1 3 3 7
foo
?C?
??J
foo
?C?
??J
1 3 4 6
bar
CODE
????
?JAM
foo
?C?
??J
1 4 2 4
bar
CODE
????
?JAM
baz
CA".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);

      var output = splitter.Configure_CustomMap(args => args[2] + args[1] + 1).GetCaseLines(inputFile).ToList();

      output.Should().HaveCount(3);

      output.First().Should().HaveCount(7);
      output.Skip(1).First().Should().HaveCount(8);
      output.Skip(2).First().Should().HaveCount(7);

      output.First().First().Should().Be("1 3 3 7");
      output.First().Skip(1).First().Should().Be("foo");
      output.First().Skip(2).First().Should().Be("?C?");
      output.First().Skip(3).First().Should().Be("??J");
      output.First().Skip(4).First().Should().Be("foo");
      output.First().Skip(5).First().Should().Be("?C?");
      output.First().Skip(6).First().Should().Be("??J");

      output.Skip(1).First().First().Should().Be("1 3 4 6");
      output.Skip(1).First().Skip(1).First().Should().Be("bar");
      output.Skip(1).First().Skip(2).First().Should().Be("CODE");
      output.Skip(1).First().Skip(3).First().Should().Be("????");
      output.Skip(1).First().Skip(4).First().Should().Be("?JAM");
      output.Skip(1).First().Skip(5).First().Should().Be("foo");
      output.Skip(1).First().Skip(6).First().Should().Be("?C?");
      output.Skip(1).First().Skip(7).First().Should().Be("??J");

      output.Skip(2).First().First().Should().Be("1 4 2 4");
      output.Skip(2).First().Skip(1).First().Should().Be("bar");
      output.Skip(2).First().Skip(2).First().Should().Be("CODE");
      output.Skip(2).First().Skip(3).First().Should().Be("????");
      output.Skip(2).First().Skip(4).First().Should().Be("?JAM");
      output.Skip(2).First().Skip(5).First().Should().Be("baz");
      output.Skip(2).First().Skip(6).First().Should().Be("CA");
    }
  }
}
