namespace Common
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

  public static class Thrower
  {
    public enum ResponseType
    {
      Memory,
      Time
    }

    public static void TriggerMemLimit()
    {
      var size = 620;
      var newTooBigArray = new long[size, size, size];
      //Consumes about 1.9GB RAM
    }

    public static void TriggerTimeLimit()
    {
      System.Threading.Thread.Sleep(120000);
    }

    public static void TriggerResponseIfErrors(ResponseType response, Action doStuff)
    {
      Func<int> doStuffWithDummyReturn = () =>
      {
        doStuff();
        return 0;
      };
      TriggerResponseIfErrors(response, doStuffWithDummyReturn );
    }

    public static T TriggerResponseIfErrors<T>(ResponseType response, Func<T> doStuff)
    {
      try
      {
        return doStuff();
      }
      catch
      {
        if (response == ResponseType.Memory)
        {
          TriggerMemLimit();
        }
        else
        {
          TriggerTimeLimit();
        }
        throw;
      }
    }
  }
}
