using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace RainbowUnicorns
{
  [TestFixture]
  class Tests
  {
    [Test]
    public static void caseSplitter()
    {
      var input = new CaseInput("11 4 0 4 0 4 0");
      var x = new CaseSolver(input).SingleColourSolve();

    }
  }
}
