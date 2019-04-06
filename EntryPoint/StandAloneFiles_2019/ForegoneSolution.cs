//using System;
//using System.Collections.Generic;

//class Program
//{
//  static void Main(string[] args)
//  {
//    //Run();
//    var output1 = Solve("345234");
//    var output2 = Solve("4");
//    var output3 = Solve("940");
//    var output4 = Solve("4444");
//  }

//  static void Run()
//  {
//    var ignoreFirstLine = Console.ReadLine();
//    var index = 1;
//    while (true)
//    {
//      var line = Console.ReadLine();
//      if (string.IsNullOrEmpty(line)) { break; }

//      var combinedAnswerString = Solve(line);
//      Console.WriteLine($"Case #{index}: {combinedAnswerString}");

//      index++;
//    }
//  }

//  private static string Solve(string line)
//  {
//    var chars = line.ToCharArray();
//    string answer1 = "";
//    string answer2 = "";

//    var found4 = false;
//    foreach (var digit in chars)
//    {
//      if (digit != '4')
//      {
//        answer1 += digit;
//        if (found4)
//        {
//          answer2 += '0';
//        }
//      }
//      else
//      {
//        found4 = true;
//        answer1 += '2';
//        answer2 += '2';
//      }
//    }
//    var combinedAnswerString = $"{answer1} {answer2}";
//    return combinedAnswerString;
//  }

//  public void WriteOutput(IEnumerable<string> lines)
//  {
//    foreach (var line in lines)
//    {
//      Console.WriteLine(line);
//    }
//  }
//}

