using JScience.Mathe.Vectors.BaseClasses;
using JScience.Mathe.Vectors.Enums;
using System;

namespace JScience.Mathe.Vectors.Classes
{
    public sealed class Vector3DComplex : Vector<System.Numerics.Complex>
    {
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

        public Vector3DComplex(string bezeichnung, System.Numerics.Complex x, System.Numerics.Complex y, System.Numerics.Complex z) : base(bezeichnung, EVecType.Column, new System.Numerics.Complex[] { x, y, z })
        {
        }

        public System.Numerics.Complex Skalarprodukt(Vector3DComplex secondVector)
        {
            System.Numerics.Complex d = 0;
            for (int i = 0; i < values.Length; i++)
                d += values[i] * secondVector.GetValue(i);
            return d;
        }

        public static Vector3DComplex operator +(Vector3DComplex a, Vector3DComplex b)
        {
            for (int i = 0; i < a.Dimensions; i++)
                a.SetValue(i, a.GetValue(i) + b.GetValue(i));
            return a;
        }

        public static Vector3DComplex operator -(Vector3DComplex a, Vector3DComplex b)
        {
            for (int i = 0; i < a.Dimensions; i++)
                a.SetValue(i, a.GetValue(i) - b.GetValue(i));
            return a;
        }
    }
}