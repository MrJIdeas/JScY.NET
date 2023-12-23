using JScience.Mathe.Vectors.BaseClasses;
using JScience.Mathe.Vectors.Enums;
using System;

namespace JScience.Mathe.Vectors.Classes
{
    public sealed class Vector3D : Vector<double>
    {
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

        public Vector3D(string bezeichnung, double x, double y, double z) : base(bezeichnung, EVecType.Column, new double[] { x, y, z })
        {
        }

        public double Skalarprodukt(Vector3D secondVector)
        {
            double d = 0;
            for (int i = 0; i < values.Length; i++)
                d += values[i] * secondVector.GetValue(i);
            return d;
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            for (int i = 0; i < a.Dimensions; i++)
                a.SetValue(i, a.GetValue(i) + b.GetValue(i));
            return a;
        }

        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            for (int i = 0; i < a.Dimensions; i++)
                a.SetValue(i, a.GetValue(i) - b.GetValue(i));
            return a;
        }
    }
}