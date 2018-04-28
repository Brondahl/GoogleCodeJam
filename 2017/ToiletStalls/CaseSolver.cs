namespace ToiletStalls
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  class CaseSolver
  {
    private readonly InputCase input;
    internal static Dictionary<long, CacheBreakdownCollection> TupleCache
        = new Dictionary<long, CacheBreakdownCollection>();

    static CaseSolver()
    {
      PreLoadTuplesForOneAndTwo();
    }

    internal static void PreLoadTuplesForOneAndTwo()
    {
      var TupleCollectionForOne = new CacheBreakdownCollection();
      TupleCollectionForOne.Add(Tuple.Create<long, long>(0, 0), 1);

      var TupleCollectionForTwo = new CacheBreakdownCollection();
      TupleCollectionForTwo.Add(Tuple.Create<long, long>(1, 0), 1);
      TupleCollectionForTwo.Add(Tuple.Create<long, long>(0, 0), 1);

      TupleCache.Add(1, TupleCollectionForOne);
      TupleCache.Add(2, TupleCollectionForTwo);
    }

    internal CaseSolver(InputCase inputCase)
    {
      input = inputCase;
    }


    internal Tuple<long, long> Solve()
    {
      var tuples = GetTupleList(input.Stalls);

      var sortedTupleKeyValueEntriesQueue
        = new Queue<KeyValuePair<Tuple<long,long>, long>>(
            tuples.Tuples
              .OrderByDescending(tupleKVP => tupleKVP.Key.Item1)
              .ThenByDescending(tupleKVP => tupleKVP.Key.Item2)
          );

      long currentCount = 0;
      Tuple<long, long> currentTuple = null;

      while (currentCount < input.Occupants)
      {
        var nextKVP = sortedTupleKeyValueEntriesQueue.Dequeue();
        currentTuple = nextKVP.Key; //Tuple
        currentCount += nextKVP.Value; //TupleCount
      }
      //exit loop the first time we exceed the Occupants count, which means Occupants used that Tuple.
      return currentTuple;
    }

    internal CacheBreakdownCollection GetTupleList(long startValue)
    {
      if (TupleCache.ContainsKey(startValue))
      {
        return TupleCache[startValue];
      }

      //Identify the first split.
      var minusOne = startValue - 1;
      var isEvenSplit = minusOne % 2;
      var lowerHalf = minusOne/2;
      var upperHalf = lowerHalf + isEvenSplit;

      //Initialise the collection.
      var tupleCollectionToReturn = new CacheBreakdownCollection();
      tupleCollectionToReturn.Add(Tuple.Create(upperHalf, lowerHalf), 1);

      //Recurse and Recombine!
      var lowerTupleCollection = GetTupleList(lowerHalf);
      if (isEvenSplit == 0)
      {
        tupleCollectionToReturn.AddCollectionMultiplied(lowerTupleCollection, 2);
      }
      else
      {
        var upperTupleCollection = GetTupleList(lowerHalf + 1);
        tupleCollectionToReturn.AddCollection(upperTupleCollection);
        tupleCollectionToReturn.AddCollection(lowerTupleCollection);
      }

      //Store.
      TupleCache[startValue] = tupleCollectionToReturn;

      return tupleCollectionToReturn;
    }
  }
}
