using static System.Math;

namespace CubicUFO
{
  using System;

  class CaseInput
  {
    internal CaseInput(string line)
    {
      var split = line.Split(' ');
      Area = double.Parse(split[0]);
    }

    internal double Area;
  }

  class CaseOutput
  {
    internal CaseOutput(double theta, double phi = 0)
    {
      Theta = theta;
      Phi = phi;
    }

    internal double Theta;
    internal double Phi;

    internal double X;
    internal double Y;

    public override string ToString()
    {
      if (Phi != 0)
      {
        throw new InvalidOperationException();
      }

      X =  Cos(Theta) / 2;
      Y =  Sin(Theta) / 2;

      var coord1 = $"{-X} {Y} 0";
      var coord2 = $"{Y} {X} 0";
      var coord3 = "0 0 0.5";

      return Environment.NewLine + coord1 + Environment.NewLine + coord2 + Environment.NewLine + coord3;
    }
  }

}
