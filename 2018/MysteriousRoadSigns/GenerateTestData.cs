namespace MysteriousRoadSigns
{
  using System;
  using System.Collections.Generic;
  using System.IO;

  public class GenerateTestData
  {
    public static void GenerateData()
    {
      var allCaseLines = new List<string>{"100"};

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(1000, 50, 20));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 50, 20));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 100, 20));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 100, 3));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 10, 20));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 10, 3));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 10, 2));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 2, 2));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 50, 5));
      }

      for (int i = 0; i < 10; i++)
      {
        allCaseLines.AddRange(GenerateTestCase(100000, 50, 2));
      }

      var filePath = Path.Combine(@"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\2018\MysteriousRoadSigns\TestData", "data.txt");

      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }

      File.AppendAllLines(filePath, allCaseLines);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSigns">probably 100,000</param>
    /// <param name="probOfChangingMN">2-50</param>
    /// <param name="probOfUsingNewMN">2-20</param>
    private static List<string> GenerateTestCase(int numberOfSigns, int probOfChangingMN, int probOfUsingNewMN)
    {
      var caseLines = new List<string>{numberOfSigns.ToString()};
      var rand = new Random();
      var count = 0;
      var target = numberOfSigns;
      var d = 0;


      var m1 = GetM(rand, d);
      var m2 = GetM(rand, d);
      var n1 = GetN(rand, d);
      var n2 = GetN(rand, d);

      while (count < target)
      {
        count++;
        d = GetNextD(rand, d);

        var useNewM = rand.Next(probOfUsingNewMN) == 1;
        var useNewN = rand.Next(probOfUsingNewMN) == 1;

        var mToUse = useNewM ? m2 : m1;
        var nToUse = useNewN ? n2 : n1;

        var impliedA = nToUse - d;
        var impliedB = d - mToUse;

        caseLines.Add($"{d} {impliedA} {impliedB}");

        var changeM = rand.Next(probOfChangingMN) == 1;
        var changeN = rand.Next(probOfChangingMN) == 1;

        if (changeM)
        {
          m1 = m2;
          m2 = GetM(rand, d);
        }

        if (changeN)
        {
          n1 = n2;
          n2 = GetN(rand, d);
        }
      }
      return caseLines;
    }

    private static int GetM(Random rand, int d)
    {
      return rand.Next(-1000, d);
    }
    private static int GetN(Random rand, int d)
    {
      return rand.Next(d, d + 5000);
    }
    private static int GetNextD(Random rand, int d)
    {
      return d + rand.Next(100);
    }
  }
}

