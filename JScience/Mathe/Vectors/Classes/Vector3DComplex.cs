using JScience.Mathe.Vectors.BaseClasses;
using JScience.Mathe.Vectors.Enums;
using System;

namespace JScience.Mathe.Vectors.Classes
{
    public sealed class Vector3DComplex : Vector<System.Numerics.Complex>
    {
        ///<inheritdoc/>
        public new double Abs2
        {
            get
            {
                double d = 0;
                foreach (var item in values)
                    d += Math.Pow(item.Magnitude, 2);
                return d;
            }
        }

        ///<inheritdoc/>
        public Vector3DComplex(string bezeichnung, System.Numerics.Complex x, System.Numerics.Complex y, System.Numerics.Complex z) : base(bezeichnung, EVecType.Column, new System.Numerics.Complex[] { x, y, z })
        {
        }

        /// <summary>
        /// Methode für Skalarprodukt.
        /// </summary>
        /// <param name="secondVector">Zweiter Vektor.</param>
        /// <returns>Skalarprodukt.</returns>
        public System.Numerics.Complex Skalarprodukt(Vector3DComplex secondVector)
        {
            System.Numerics.Complex d = 0;
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
        public static Vector3DComplex operator +(Vector3DComplex a, Vector3DComplex b)
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
        public static Vector3DComplex operator -(Vector3DComplex a, Vector3DComplex b)
        {
            for (int i = 0; i < a.Dimensions; i++)
                a.SetValue(i, a.GetValue(i) - b.GetValue(i));
            return a;
        }
    }
}