using System.Collections.Generic;

namespace JScience.Mathe.DifferentialEquations.Interfaces
{
    /// <summary>
    /// Interface für Lösungstool für Differentialgleichungen.
    /// </summary>
    public interface IDifferentialEquationSolver
    {
        /// <summary>
        /// Methode zum Lösen.
        /// </summary>
        /// <param name="x0">Startwert x-Achse.</param>
        /// <param name="y0">Startwert y-Achse.</param>
        /// <param name="h">Schrittweite.</param>
        /// <param name="endX">Endstelle x.</param>
        /// <param name="f">Funktion.</param>
        /// <returns>Wertepaare.</returns>
        Dictionary<double, double> Solve(double x0, double y0, double h, double endX, IDifferentialEquation f);
    }
}