using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
  public class GoogleCodeJam2017Communicator : IGoogleCodeJamCommunicator
  {
    private readonly string folderPath = @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\GoogleCodeJam2017\";
    private readonly string inputFileName;
    private readonly string inputFilePath;
    private readonly string outputFilePath;

    public GoogleCodeJam2017Communicator(string subFolderName, string fileName = null)
    {
      inputFileName = fileName ?? @"Data.in";
      inputFilePath = Path.Combine(folderPath, subFolderName, inputFileName);
      outputFilePath = Path.Combine(folderPath, subFolderName, "Data.out");
    }

    public IEnumerable<string> ReadStringInput(out int numberOfCases)
    {
      var lines = File.ReadAllLines(inputFilePath);
      numberOfCases = int.Parse(lines.First());
      return lines.Skip(1).ToArray();
    }

    public void WriteOutput(IEnumerable<string> lines)
    {
      File.WriteAllLines(outputFilePath, lines.ToArray());
    }
  }
}
