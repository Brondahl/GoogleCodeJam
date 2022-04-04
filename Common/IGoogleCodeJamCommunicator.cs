namespace Common
{
  using System.Collections.Generic;

  public interface IGoogleCodeJamCommunicator
  {
      IEnumerable<string> ReadStringInput(out int numberOfCases);
      void WriteOutput(IEnumerable<string> lines);
  }

  public interface IGoogleCodeJamInteractiveCommunicator
  {
      long ReadSingleLongInput();
      List<long> ReadSingleLineOfLongsInput();
      string ReadSingleStringInput();
      List<string> ReadNextStringInputs(int linesToRead);
      void WriteSingleInteractiveOutput(string line);
      void WriteInteractiveOutput(List<string> lines);
  }
}
