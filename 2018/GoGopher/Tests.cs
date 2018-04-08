using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GoGopher
{
  [TestFixture]
  public class Tests
  {
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    public void Simple3By3(int target)
    {
      var total = 0;
      for (int i = 0; i < 50; i++)
      {
        total += Solve_Once(target);
      }
      var average = total / 50.0;
    }

    [Test]
    [TestCase(10)]
    [TestCase(11)]
    [TestCase(12)]
    public void Simple3By4(int target)
    {
      var total = 0;
      for (int i = 0; i < 50; i++)
      {
        total += Solve_Once(target);
      }
      var average = total / 50.0;
    }

    [Test]
    [TestCase(13)]
    [TestCase(14)]
    [TestCase(15)]
    public void Simple3By5(int target)
    {
      var total = 0;
      for (int i = 0; i < 50; i++)
      {
        total += Solve_Once(target);
      }
      var average = total / 50.0;
    }

    public int Solve_Once(int target)
    {
      var testResponder = new TestGopherResponder();
      var solver = new GopherSolver(testResponder, true);

      solver.InteractiveSolveForTarget(target);
      return testResponder.AttemptsSoFar;
    }

    [Test]
    [TestCase(14)]
    [TestCase(18)]
    [TestCase(22)]
    [TestCase(60)]
    [TestCase(129)]
    [TestCase(157)]
    public void MiscIndividual(int target)
    {
      var testResponder = new TestGopherResponder();
      var solver = new GopherSolver(testResponder, true);

      solver.InteractiveSolveForTarget(target);
    }

    [Test]
    public void MaxConsistently()
    {
      var attempts = new List<int>();

      for (int i = 0; i < 1000; i++)
      {
        attempts.Add(Solve_Once(200));
      }
      var average = attempts.Average();
      var max = attempts.Max();
      var min = attempts.Min();
    }
  }
}
