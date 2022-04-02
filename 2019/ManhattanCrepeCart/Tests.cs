namespace ManhattanCrepeCart
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;
  using FluentAssertions;
  using NUnit.Framework;

  public class TestIOStub : IGoogleCodeJamCommunicator
  {
    private readonly string input;
    public List<string> Output;
    public TestIOStub(string input)
    {
      this.input = input;
    }
    public IEnumerable<string> ReadStringInput(out int numberOfCases)
    {
      var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      numberOfCases = int.Parse(lines.First());
      return lines.Skip(1);
    }

    public void WriteOutput(IEnumerable<string> lines)
    {
      Output = lines.ToList();
    }
  }

  [TestFixture]
  public class Tests
  {

    [Test]
    public void FullE2ETest()
    {
      var inputString = @"3
1 10
5 5 N
4 10
2 4 N
2 6 S
1 5 E
3 5 W
8 10
0 2 S
0 3 N
0 3 N
0 4 N
0 5 S
0 5 S
0 8 S
1 5 W";
      var io = new TestIOStub(inputString);
      CaseSolver.Run(io);

      io.Output.Should().BeEquivalentTo(
        "Case #1: 0 6",
        "Case #2: 2 5",
        "Case #3: 0 4"
      );
    }

    [Test]
    [TestCase(
@"1 10
5 5 N",
0, 6)]
    [TestCase(
@"4 10
2 4 N
2 6 S
1 5 E
3 5 W",
2, 5)]
    [TestCase(
@"8 10
0 2 S
0 3 N
0 3 N
0 4 N
0 5 S
0 5 S
0 8 S
1 5 W",
0, 4)]
    [TestCase(
@"3 5
1 2 S
3 2 N
2 3 E",
3, 0)]
    [TestCase(
@"4 2
0 2 S
1 0 N
0 1 E
2 0 W",
1, 1)]
    [TestCase(
@"6 2
0 2 S
1 0 N
0 1 E
2 0 W
1 1 W
1 1 S",
0, 0)]
    [TestCase(
      @"6 2
0 2 S
1 0 N
0 1 E
2 0 W
1 2 W
1 1 S",
0, 0)]
    [TestCase(
      @"8 2
0 2 S
1 0 N
1 0 N
0 1 E
0 1 E
2 0 W
1 1 W
1 1 S",
      0, 0)]

    public void Samples(string inputString, int x, int y)
    {
      var inputStringArr = inputString.Split(new []{Environment.NewLine}, StringSplitOptions.None);
      var solution = new CaseSolver(new CaseInput(inputStringArr.ToList())).Solve();
      solution.ToString().Should().Be($"{x} {y}");
    }

    [Test]
    [TestCase(new[] { 2 }, 3, new[] { 0, 0, 1, 1 })]
    [TestCase(new[] { 2,2,3,4,4 }, 4, new[] { 0, 0, 2, 3,5 })]
    [TestCase(new[] { 0 }, 1, new[] { 1, 1 })]
    public void DictFiller(int[] listIn, int max, int[] expectedDict)
    {
      var output = CaseSolver.SetCumulative(listIn.ToList(), max);
      output.Keys.Should().HaveCount(expectedDict.Length);

      foreach (var kvp in output)
      {
        kvp.Value.Should().Be(expectedDict[kvp.Key]);
      }
    }

  }
}
