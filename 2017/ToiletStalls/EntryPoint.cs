namespace ToiletStalls
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Common;

  public class EntryPoint
  {
    public static void RunFromFile()
    {
      new EntryPoint().Run();
    }

    public static void Test()
    {
      testDictionaryCreation();
    }

    #region Tests
    public static void testDictionaryCreation()
    {
      var x = new CaseSolver(null);
      x.GetTupleList(10);
      x.GetTupleList(67);
      x.GetTupleList(100);
      x.GetTupleList(100000000);
      x.GetTupleList(1000000000000000000);
      //Console.WriteLine(string.Join(",", new InputCase("+-+-++++--- 4").Sequence));
      //Console.WriteLine(new InputCase("+-+-++++--- 4").SequenceLength);
      //Console.WriteLine(new InputCase("+-+-++++--- 4").FlipSize);

      //Console.WriteLine(string.Join(",", new InputCase("+++++++++++++ 9").Sequence));
      //Console.WriteLine(new InputCase("+++++++++++++ 9").SequenceLength);
      //Console.WriteLine(new InputCase("+++++++++++++ 9").FlipSize);

      //Console.WriteLine(string.Join(",", new InputCase("---- 2").Sequence));
      //Console.WriteLine(new InputCase("---- 2").SequenceLength);
      //Console.WriteLine(new InputCase("---- 2").FlipSize);
    }

    public static void testFlipping()
    {
      //var input = new InputCase("+-+-++++--- 4");
      //new CaseSolver(input).MassFlipStartingAtIndex(0);
      //new CaseSolver(input).MassFlipStartingAtIndex(4);

      //Console.WriteLine(string.Join(",", input.Sequence));
    }


    public static void testComplete()
    {
      //Console.WriteLine(new CaseSolver(new InputCase("---+-++- 3")).Solve());
      //Console.WriteLine(new CaseSolver(new InputCase("+++++ 4")).Solve());
      //Console.WriteLine(new CaseSolver(new InputCase("-+-+- 4")).Solve());
    }


    #endregion

    private static string subFolderName = @"ToiletStalls";
    private int numberOfCases;
    private static IGoogleCodeJamCommunicator InOut = new GoogleCodeJam2017Communicator(subFolderName);

    public void Run()
    {
      var cases = InOut.ReadStringInput(out numberOfCases).ToList();
      var results = new List<string>();

      for (int ii = 0; ii < numberOfCases; ii++)
      {
        var parsedCase = new InputCase(cases[ii]);
        var solver = new CaseSolver(parsedCase);
        Tuple<long, long> resultTuple = solver.Solve();

        var resultText = resultTuple.Item1.ToString() + ' ' + resultTuple.Item2.ToString();

        results.Add(string.Format("Case #{0}: {1}", ii + 1, resultText));
      }

      InOut.WriteOutput(results);
    }

  }
}
