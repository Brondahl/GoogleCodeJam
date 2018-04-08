using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToiletStalls
{
  class CacheBreakdownObject
  {
    internal CacheBreakdownObject(Tuple<long, long> tuple)
    {
      Count = 1;
      Max = tuple.Item1;
      Min = tuple.Item2;
    }

    internal void Multiply(long factor)
    {
      Count *= factor;
    }

    internal void Add(long summand)
    {
      Count += summand;
    }

    internal void Increment()
    {
      Count++;
    }

    internal long Count;
    internal long Max;
    internal long Min;
    internal Tuple<long, long> Tuple { get { return System.Tuple.Create(Max, Min); } }
  }
}
