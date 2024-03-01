using JScience.Mathe.DifferentialEquations.Interfaces;
using System.Collections.Generic;

namespace JScience.Mathe.DifferentialEquations.Classes
{
    /// <summary>
    /// Klasse für Runge-Kutta-Verfahren.
    /// </summary>
    public class RungeKuttaSolver : IDifferentialEquationSolver
    {
        ///<inheritdoc/>
        public Dictionary<double, double> Solve(double x0, double y0, double h, double endX, IDifferentialEquation f)
        {
            var result = new Dictionary<double, double>();
            double x = x0;
            double y = y0;

            while (x <= endX)
            {
                if (result.ContainsKey(x))
                    result[x] = y;
                else
                    result.Add(x, y);
                double k1 = h * f.Derivative(x, y);
                double k2 = h * f.Derivative(x + h / 2, y + k1 / 2);
                double k3 = h * f.Derivative(x + h / 2, y + k2 / 2);
                double k4 = h * f.Derivative(x + h, y + k3);
                y += (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                x += h;
            }

            return result;
        }
    }
}