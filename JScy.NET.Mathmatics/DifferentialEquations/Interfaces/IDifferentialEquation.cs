namespace JScy.NET.Mathmatics.DifferentialEquations.Interfaces
{
    /// <summary>
    /// Interface für Differentialgleichungen
    /// </summary>
    public interface IDifferentialEquation
    {
        /// <summary>
        /// Derivat berechnen
        /// </summary>
        /// <param name="x">x-Wert</param>
        /// <param name="y">y-Wert</param>
        /// <returns>Derivat-Grad.</returns>
        double Derivative(double x, double y);
    }
}