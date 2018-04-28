using System.Collections.Generic;

namespace Common
{
  public interface IGoogleCodeJamCommunicator
  {
     IEnumerable<string> ReadStringInput(out int numberOfCases);
     void WriteOutput(IEnumerable<string> lines);
  }
}
