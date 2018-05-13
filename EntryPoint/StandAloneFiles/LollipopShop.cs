//namespace GoogleCodeJam
//{
//  using System;
//  using System.Collections.Generic;
//  using System.IO;
//  using System.Linq;

//  class Program
//  {
//    static void Main(string[] args)
//    {
//      CaseSolver.Run();
//    }

//    public class CaseSolver
//    {
//      private static int numberOfCases;
//      private static bool[] lollipopSold = null;
//      private static int[] lollipopSeen = null;

//      public static void Run()
//      {
//        numberOfCases = int.Parse(Console.ReadLine());

//        for (int ii = 0; ii < numberOfCases; ii++)
//        {
//          var numberOfLollipopsAndCustomers = int.Parse(Console.ReadLine());

//          lollipopSold = new bool[numberOfLollipopsAndCustomers];
//          lollipopSeen = new int[numberOfLollipopsAndCustomers];

//          for (int jj = 0; jj < numberOfLollipopsAndCustomers; jj++)
//          {
//            var customerInput = Console.ReadLine();
//            var customerArray = customerInput.Split(' ').Select(int.Parse);
//            var D = customerArray.First();

//            if (D == 0)
//            {
//              Console.WriteLine("-1");
//              continue;
//            }

//            var customerLikes = customerArray.Skip(1).ToArray();

//            foreach (var likedLollipop in customerLikes)
//            {
//              lollipopSeen[likedLollipop]++;
//            }

//            var lollipopToSell = LeastSeenAvailablePermissibleLollipop(customerLikes);
//            if (lollipopToSell == -1)
//            {
//              Console.WriteLine("-1");
//              continue;
//            }

//            if (lollipopSold[lollipopToSell])
//            { throw new Exception("Bugger"); }

//            lollipopSold[lollipopToSell] = true;

//            Console.WriteLine(lollipopToSell.ToString());
//            continue;
//          }
//        }
//      }

//      public static int LeastSeenAvailablePermissibleLollipop(int[] permissibleLollipop)
//      {
//        if (!permissibleLollipop.Any())
//        {
//          return -1;
//        }

//        var availablePermissibleLollipops = AvailableLollipops(permissibleLollipop);
//        if (!availablePermissibleLollipops.Any())
//        {
//          return -1;
//        }

//        return LeastSeenLollipop(availablePermissibleLollipops);
//      }

//      public static List<int> AvailableLollipops(int[] lollipopsToConsider)
//      {
//        return lollipopsToConsider.Where(pop => !lollipopSold[pop]).ToList();
//      }

//      public static int LeastSeenLollipop(List<int> lollipopsToConsider)
//      {
//        return lollipopsToConsider.Select(pop => new { Pop = pop, Seen = lollipopSeen[pop] }).OrderBy(obj => obj.Seen).First().Pop;
//      }

//    }
//  }
//}

