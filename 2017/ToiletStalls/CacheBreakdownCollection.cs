namespace ToiletStalls
{
  using System;
  using System.Collections.Generic;

  class CacheBreakdownCollection
  {
    internal Dictionary<Tuple<long, long>, long> Tuples
      = new Dictionary<Tuple<long, long>, long>();

    //internal void Add(CacheBreakdownObject newEntry)
    //{
    //  var tupleToInsert = newEntry.Tuple;
    //  Add(tupleToInsert, newEntry.Count);
    //}

    internal void Add(Tuple<long,long> tuple, long count)
    {
      if (Tuples.ContainsKey(tuple))
      {
        Tuples[tuple] += count;
      }
      else
      {
        Tuples[tuple] = count;
      }
    }

    internal void AddCollection(CacheBreakdownCollection newCollection)
    {
      AddCollectionMultiplied(newCollection, 1);
    }

    internal void AddCollectionMultiplied(CacheBreakdownCollection newCollection, long factor)
    {
      foreach (var keyValueEntry in newCollection.Tuples)
      {
        var tuple = keyValueEntry.Key;
        var count = keyValueEntry.Value;
        Add(tuple, count * factor);
      }
    }


  }
}
