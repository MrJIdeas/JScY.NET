using JScy.NET.Mathe.Stochastik;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace JScy.NET.Mathe.Zahlentheorie
{
    /// <summary>
    /// Statische Klasse für Generierung von Funktionen.
    /// </summary>
    public static class Functions
    {
        private const int twoGig = 10000;

        /// <summary>
        /// Berechnung Riemannsche Zeta-Funktion.
        /// </summary>
        /// <param name="x">Wert.</param>
        /// <returns>Wert an Stelle x.</returns>
        public static double Zeta_Riemann(double x)
        {
            double[] calc = new double[twoGig];
            _ = Parallel.For(1, twoGig, (n, loopOP) =>
            {
                var erg2 = Math.Pow(n, -x);
                if (erg2 > double.Epsilon)
                    calc[n - 1] = erg2;
                else
                    loopOP.Break();
            });
            return calc.AsParallel().Sum(); ;
        }

        /// <summary>
        /// Berechnung Riemannsche Zeta-Funktion.
        /// </summary>
        /// <param name="x">Wert.</param>
        /// <returns>Wert an Stelle x.</returns>
        public static Complex Zeta_Riemann(Complex x)
        {
            double[] calcreal = new double[twoGig];
            double[] calcimag = new double[twoGig];
            _ = Parallel.For(1, twoGig, (n, loopOP) =>
            {
                double exp = x.Imaginary * Math.Log(n);
                double basis = Math.Pow(n, x.Real);
                double real = basis * Math.Cos(exp);
                double imag = basis * Math.Sin(exp);
                double wurzel = Math.Pow(real, 2) + Math.Pow(imag, 2);
                calcreal[n - 1] += real / wurzel;
                calcimag[n - 1] = -imag / wurzel;
            });
            return new Complex(calcreal.AsParallel().Sum(), calcimag.AsParallel().Sum());
        }

        /// <summary>
        /// Berechnung Hasse Zeta-Funktion.
        /// </summary>
        /// <param name="x">Wert.</param>
        /// <returns>Wert an Stelle x.</returns>
        public static double Zeta_Hasse(double x)
        {
            double erg = Math.Pow(x - 1, -1);
            double[] calc = new double[twoGig];
            _ = Parallel.For(0, twoGig, (n, loopOP) =>
            {
                double term = 0;
                for (ulong k = 0; k <= (ulong)n; k++)
                {
                    ulong koeff = Binom.Koeffizient((ulong)n, k);
                    double mid = koeff / Math.Pow(k + 1, x - 1);
                    if ((k - 1) % 2 == 1)
                    {
                        mid *= -1;
                    }
                    term += mid;
                }
                if (term > 0 && !double.IsInfinity(term))
                {
                    calc[n] = term * Math.Pow(n + 1, -1);
                }
            });
            return calc.AsParallel().Sum() + erg;
        }
    }
}