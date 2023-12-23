using JScience.Mathe.Vectors.Enums;
using JScience.Mathe.Vectors.Interfaces;
using System;

namespace JScience.Mathe.Vectors.BaseClasses
{
    public abstract class Vector<T> : IVector where T : struct
    {
        protected T[] values { get; private set; }

        public int Dimensions => values.Length;

        public double Abs2 => throw new NotImplementedException();

        public double Norm => Math.Sqrt(Abs2);

        public string Bezeichnung { get; private set; }

        public EVecType VectorType { get; private set; }

        protected Vector(string bezeichnung, EVecType vectorType, T[] values)
        {
            Bezeichnung = bezeichnung;
            VectorType = vectorType;
            this.values = values;
        }

        public T GetValue(int index) => values[index];

        public void SetValue(int index, T val) => values[index] = val;
    }
}