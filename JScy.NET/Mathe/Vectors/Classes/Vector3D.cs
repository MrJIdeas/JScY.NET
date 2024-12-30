using JScy.NET.Mathe.Vectors.BaseClasses;
using JScy.NET.Mathe.Vectors.Enums;
using System;

namespace JScy.NET.Mathe.Vectors.Classes
{
    /// <summary>
    /// Klasse für 3D-Vektor.
    /// </summary>
    public sealed class Vector3D : Vector<double>
    {
        ///<inheritdoc/>
        public new double Abs2
        {
            get
            {
                double d = 0;
                foreach (var item in values)
                    d += Math.Pow(item, 2);
                return d;
            }
        }

        ///<inheritdoc/>
        public Vector3D(string bezeichnung, double x, double y, double z) : base(bezeichnung, EVecType.Column, new double[] { x, y, z })
        {
        }

        /// <summary>
        /// Methode für Skalarprodukt.
        /// </summary>
        /// <param name="secondVector">Zweiter Vektor.</param>
        /// <returns>Skalarprodukt.</returns>
        public double Skalarprodukt(Vector3D secondVector)
        {
            double d = 0;
            for (int i = 0; i < values.Length; i++)
                d += values[i] * secondVector.GetValue(i);
            return d;
        }

        /// <summary>
        /// Addition Vektoren3D.
        /// </summary>
        /// <param name="a">1.</param>
        /// <param name="b">2.</param>
        /// <returns>Ergebnis</returns>
        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            for (int i = 0; i < a.Dimensions; i++)
                a.SetValue(i, a.GetValue(i) + b.GetValue(i));
            return a;
        }

        /// <summary>
        /// Subtraktion Vektoren3D.
        /// </summary>
        /// <param name="a">1.</param>
        /// <param name="b">2.</param>
        /// <returns>Ergebnis</returns>
        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            for (int i = 0; i < a.Dimensions; i++)
                a.SetValue(i, a.GetValue(i) - b.GetValue(i));
            return a;
        }
    }
}