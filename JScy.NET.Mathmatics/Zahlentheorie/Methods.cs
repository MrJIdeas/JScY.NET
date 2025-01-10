using System;
using System.Numerics;

namespace JScy.NET.Mathmatics.Zahlentheorie
{
    /// <summary>
    /// Methodensammlung.
    /// </summary>
    public static class Methods
    {
        /// <summary>
        /// Fakultät für BigInteger berechnen.
        /// </summary>
        /// <param name="x">Wert.</param>
        /// <returns>Ergebnis.</returns>
        public static BigInteger Fakultaet(BigInteger x)
        {
            if (x < 0)
            {
                throw new ArgumentException("n muss >= 0 sein.");
            }

            BigInteger erg = 1;
            for (BigInteger i = 2; i <= x; i++)
            {
                erg *= i;
            }
            return erg;
        }
    }
}