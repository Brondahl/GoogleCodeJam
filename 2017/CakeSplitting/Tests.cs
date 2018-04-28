namespace CakeSplitting
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  class Tests
  {
    [Test]
    public static void caseSplitter()
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
2 2
CA
KE".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(1);

      var output = CaseInput.SubDivideInput(inputFile.GetEnumerator());

      output.Length.Should().Be(3);
      output[0].Length.Should().Be(4);
      output[1].Length.Should().Be(4);
      output[2].Length.Should().Be(3);
      output[0][1].Should().Be("G??");
      output[1][1].Should().Be("CODE");
      output[2][1].Should().Be("CA");
    }

    [Test]
    public static void caseParser()
    {
      var inputCase1 =
@"3 3
G??
?C?
??J".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
      
      var inputCase2 =
      @"3 4
CODE
????
?JAM".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
      
      var inputCase3 =
      @"2 2
CA
KE".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

      var output = new CaseInput(inputCase1);

      output.R.Should().Be(3);
      output.C.Should().Be(3);
      output.LettersInRow[0].Should().Be(1);
      output.LettersInRow[1].Should().Be(1);
      output.LettersInRow[2].Should().Be(1);
      output.LetterLocationsInRow[0].Should().Equal(0);
      output.LetterLocationsInRow[1].Should().Equal(1);
      output.LetterLocationsInRow[2].Should().Equal(2);

      output = new CaseInput(inputCase2);

      output.R.Should().Be(3);
      output.C.Should().Be(4);
      output.LettersInRow[0].Should().Be(4);
      output.LettersInRow[1].Should().Be(0);
      output.LettersInRow[2].Should().Be(3);
      output.LetterLocationsInRow[0].Should().Equal(0,1,2,3);
      output.LetterLocationsInRow[1].Should().BeEmpty();
      output.LetterLocationsInRow[2].Should().Equal(1,2,3);

      output = new CaseInput(inputCase3);

      output.R.Should().Be(2);
      output.C.Should().Be(2);
      output.LettersInRow[0].Should().Be(2);
      output.LettersInRow[1].Should().Be(2);
      output.LetterLocationsInRow[0].Should().Equal(0,1);
      output.LetterLocationsInRow[1].Should().Equal(0,1);
    }

    [Test]
    public static void Output()
    {
      var input = new CaseOutput(new char[,] { { 'a', 'b' }, { 'a', 'c' } }, 2, 2);
      input.ToString().Should().Be(@"
ab
ac");

      input = new CaseOutput(new char[,] { { 'a', 'b', 'b' }, { 'a', 'c', 'd' } }, 2, 3);
      input.ToString().Should().Be(@"
abb
acd");
    }

    [Test]
    public static void solverTest()
    {
      var inputCase1 =
@"3 3
G??
?C?
??J".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

      var inputCase2 =
      @"3 4
CODE
????
?JAM".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

      var inputCase3 =
      @"2 2
CA
KE".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

      var output = new CaseSolver(new CaseInput(inputCase1)).Solve();
      output.ToString().Should().Be(@"
GGG
CCC
JJJ");

      output = new CaseSolver(new CaseInput(inputCase2)).Solve();
      output.ToString().Should().Be(@"
CODE
JJAM
JJAM");

      output = new CaseSolver(new CaseInput(inputCase3)).Solve();
      output.ToString().Should().Be(@"
CA
KE");

    }

    [Test]
    public static void BlankLastRow_Square()
    {
      var inputCase1 =
        @"3 3
G??
?C?
???".Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
      var output = new CaseSolver(new CaseInput(inputCase1)).Solve();
      output.ToString().Should().Be(@"
GGG
CCC
CCC");
    }

    [Test]
    public static void BlankLastRow_RectA()
    {
      var inputCase1 =
        @"3 4
G???
?C??
????".Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
      var output = new CaseSolver(new CaseInput(inputCase1)).Solve();
      output.ToString().Should().Be(@"
GGGG
CCCC
CCCC");
    }

    [Test]
    public static void BlankLastRow_RectB()
    {
      var inputCase1 =
@"4 3
G??
?C?
???
???".Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
      var output = new CaseSolver(new CaseInput(inputCase1)).Solve();
      output.ToString().Should().Be(@"
GGG
CCC
CCC
CCC");
    }
  }
}
