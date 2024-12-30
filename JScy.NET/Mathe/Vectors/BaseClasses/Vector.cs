using JScy.NET.Mathe.Vectors.Enums;
using JScy.NET.Mathe.Vectors.Interfaces;
using System;

namespace JScy.NET.Mathe.Vectors.BaseClasses
{
    /// <summary>
    /// Basis-Klasse für Vektoren.
    /// </summary>
    /// <typeparam name="T">Generischer Typ für structs.</typeparam>
    public abstract class Vector<T> : IVector where T : struct
    {
        /// <summary>
        /// Werte des Vektors.
        /// </summary>
        protected T[] values { get; private set; }

        ///<inheritdoc/>
        public int Dimensions => values.Length;

        ///<inheritdoc/>
        public double Abs2 => throw new NotImplementedException();

        ///<inheritdoc/>

        public double Norm => Math.Sqrt(Abs2);

        ///<inheritdoc/>
        public string Bezeichnung { get; private set; }

        ///<inheritdoc/>
        public EVecType VectorType { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="bezeichnung">Vektorbezeichnung.</param>
        /// <param name="vectorType">Vektortyp.</param>
        /// <param name="values">Werte.</param>
        protected Vector(string bezeichnung, EVecType vectorType, T[] values)
        {
            Bezeichnung = bezeichnung;
            VectorType = vectorType;
            this.values = values;
        }

        /// <summary>
        /// Methode zur Ausgabe eines Wertes an der Stelle.
        /// </summary>
        /// <param name="index">Stelle.</param>
        /// <returns>Wert.</returns>
        public T GetValue(int index) => values[index];

        /// <summary>
        /// Setzt Wert an einer Stelle.
        /// </summary>
        /// <param name="index">Stelle.</param>
        /// <param name="val">Wert.</param>
        public void SetValue(int index, T val) => values[index] = val;
    }
}