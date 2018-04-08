using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common
{
  public class Common2017 : CommonBase
  {
    private readonly string subFolderName;
    private readonly string fileName;

    public Common2017(string subFolderName, string fileName = null)
    {
      this.subFolderName = subFolderName;
      this.fileName = fileName;
    }
    protected override string folderPath => @"C:\Users\Brondahl\My Files\Programming\C#\Puzzles_And_Toys\GoogleCodeJam\GoogleCodeJam2017\";

    public override string[] ReadStringInput(out int numberOfCases)
    {
      var lines = File.ReadAllLines(folderPath + subFolderName + @"\" + (fileName ?? @"Data.in"));
      numberOfCases = int.Parse(lines.First());
      return lines.Skip(1).ToArray();
    }

    public override void WriteOutput(IEnumerable<string> lines)
    {
      File.WriteAllLines(folderPath + subFolderName + @"\Data.out", lines.ToArray());
    }
  }
}
