using JScy.NET.Mathmatics.Zahlentheorie;
using System;
using System.Numerics;

namespace JScy.NET.Physics.Simulationen.Orbitals.Classes
{
    /// <summary>
    /// Klasse zur Berechnung der Kugelflächenfunktion.
    /// </summary>
    public static class SphericalHarmonics
    {
        /// <summary>
        /// Berechnet die Kugelflächenfunktion Y_l^m(\u03B8, \u03C6).
        /// </summary>
        /// <param name="l">Die Ordnung l (nicht-negativ).</param>
        /// <param name="m">Die Ordnung m (|m| <= l).</param>
        /// <param name="theta">Der Polwinkel \u03B8 (in Radianten).</param>
        /// <param name="phi">Der Azimutwinkel \u03C6 (in Radianten).</param>
        /// <returns>Der Wert von Y_l^m als komplexe Zahl.</returns>
        public static Complex Compute(int l, int m, double theta, double phi)
        {
            if (l < 0 || Math.Abs(m) > l)
            {
                throw new ArgumentException("Ungültige Quantenzahlen: l muss >= 0 sein und |m| <= l.");
            }

            double normalization = NormalizationFactor(l, m);
            double legendre = AssociatedLegendrePolynomial(l, m, Math.Cos(theta));
            Complex phase = Complex.Exp(new Complex(0, m * phi));

            return normalization * legendre * phase;
        }

        /// <summary>
        /// Berechnet die Normierungskonstante für Y_l^m.
        /// </summary>
        private static double NormalizationFactor(int l, int m)
        {
            m = Math.Abs(m);
            BigInteger numerator = (2 * l + 1) * Methods.Fakultaet(l - m);
            double denominator = 4 * Math.PI * (double)Methods.Fakultaet(l + m);
            return Math.Sqrt((double)numerator / denominator);
        }

        /// <summary>
        /// Berechnet das zugehörige Legendre-Polynom P_l^m(x) rekursiv.
        /// </summary>
        /// <param name="l">Die Ordnung l.</param>
        /// <param name="m">Die Ordnung m.</param>
        /// <param name="x">Der Wert von x (z.B. cos(\u03B8)).</param>
        /// <returns>Der Wert von P_l^m(x).</returns>
        private static double AssociatedLegendrePolynomial(int l, int m, double x)
        {
            if (m < 0)
            {
                // P_l^-m(x) = (-1)^m * (l-m)! / (l+m)! * P_l^m(x)
                return Math.Pow(-1, m) * (double)(Methods.Fakultaet(l - Math.Abs(m)) / Methods.Fakultaet(l + Math.Abs(m))) * AssociatedLegendrePolynomial(l, Math.Abs(m), x);
            }

            if (l == 0 && m == 0)
            {
                return 1.0;
            }
            else if (l == 1 && m == 0)
            {
                return x;
            }
            else if (l == 1 && m == 1)
            {
                return -Math.Sqrt(1 - x * x);
            }

            double pmm = 1.0;
            for (int i = 1; i <= m; i++)
            {
                pmm *= -(1.0 - x * x) * (2 * i - 1) / i;
            }

            if (l == m)
            {
                return pmm;
            }

            double pmm1 = x * (2 * m + 1) * pmm;
            if (l == m + 1)
            {
                return pmm1;
            }

            double pll = 0.0;
            for (int ll = m + 2; ll <= l; ll++)
            {
                pll = ((2 * ll - 1) * x * pmm1 - (ll + m - 1) * pmm) / (ll - m);
                pmm = pmm1;
                pmm1 = pll;
            }

            return pll;
        }
    }
}