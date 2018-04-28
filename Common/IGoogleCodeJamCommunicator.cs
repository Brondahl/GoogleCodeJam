namespace Common
{
  using System.Collections.Generic;

  public interface IGoogleCodeJamCommunicator
  {
     IEnumerable<string> ReadStringInput(out int numberOfCases);
     void WriteOutput(IEnumerable<string> lines);
  }
}
