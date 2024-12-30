using System;
using System.Collections.Generic;

namespace JScy.NET.Mathmatics.Zahlentheorie
{
    /// <summary>
    /// Statische Klasse zur Umrechnung in Zahlensysteme.
    /// </summary>
    public static class ZahlenSystem
    {
        #region Dezimal zu Zahlensystem

        /// <summary>
        /// Dezimalsystem zu n-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <param name="n">Gewünschtes Zahlensystem.</param>
        /// <returns>Ergebnis.</returns>
        public static string ToNBase(long Zahl10, long n)
        {
            long remainder;
            string result = string.Empty;
            while (Zahl10 > 0)
            {
                remainder = Zahl10 % n;
                Zahl10 /= n;
                result = remainder.ToString() + result;
            }

            return result;
        }

        /// <summary>
        /// Umwandlung in 2-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To2Base(long Zahl10) => ToNBase(Zahl10, 2);

        /// <summary>
        /// Umwandlung in 3-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To3Base(long Zahl10) => ToNBase(Zahl10, 3);

        /// <summary>
        /// Umwandlung in 4-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To4Base(long Zahl10) => ToNBase(Zahl10, 4);

        /// <summary>
        /// Umwandlung in 5-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To5Base(long Zahl10) => ToNBase(Zahl10, 5);

        /// <summary>
        /// Umwandlung in 6-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To6Base(long Zahl10) => ToNBase(Zahl10, 6);

        /// <summary>
        /// Umwandlung in 7-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To7Base(long Zahl10) => ToNBase(Zahl10, 7);

        /// <summary>
        /// Umwandlung in 8-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To8Base(long Zahl10) => ToNBase(Zahl10, 8);

        /// <summary>
        /// Umwandlung in 9-er-System.
        /// </summary>
        /// <param name="Zahl10">Dezimalzahl.</param>
        /// <returns>Ergebnis.</returns>
        public static string To9Base(long Zahl10) => ToNBase(Zahl10, 9);

        #endregion Dezimal zu Zahlensystem

        #region Basis irrational

        private static List<int> ConvertToIRRBase(double number, int maxPrecision, double IrrZahl)
        {
            List<int> coefficients = new List<int>(); // Speichert die Koeffizienten
            double remaining = number; // Restwert, der umgerechnet wird

            for (int exponent = (int)Math.Floor(System.Math.Log(number) / Math.Log(IrrZahl)); exponent >= -maxPrecision; exponent--)
            {
                double powerOfPi = Math.Pow(IrrZahl, exponent); // π^exponent
                int coefficient = (int)(remaining / powerOfPi); // Ganzzahliger Teil
                coefficients.Add(coefficient);
                remaining -= coefficient * powerOfPi; // Restwert reduzieren
            }

            return coefficients;
        }

        public static List<int> ConvertToPiBase(double number, int maxPrecision) => ConvertToIRRBase(number, maxPrecision, Math.PI);

        public static List<int> ConvertToEBase(double number, int maxPrecision) => ConvertToIRRBase(number, maxPrecision, Math.E);

        #endregion Basis irrational
    }
}