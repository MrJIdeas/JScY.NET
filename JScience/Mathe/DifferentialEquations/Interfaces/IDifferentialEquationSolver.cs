using System.Collections.Generic;

namespace JScience.Mathe.DifferentialEquations.Interfaces
{
    public interface IDifferentialEquationSolver
    {
        Dictionary<double, double> Solve(double x0, double y0, double h, double endX, IDifferentialEquation f);
    }
}