namespace GoogleCodeJam
{
  using System.Linq;

  class Program
  {
    static void Main(string[] args)
    {
      int numCases;
      var lines = new Common.GoogleCodeJam2018Communicator().ReadStringInput(out numCases);
      var lineList = lines.ToList();
    }
  }
}
