using System;

namespace JScy.NET.Physics.AttributesCustom
{
    /// <summary>
    /// Attribut für Spins.
    /// </summary>
    public class SpinAttribute : Attribute
    {
        /// <summary>
        /// Spin-Betrag.
        /// </summary>
        public float val { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="val">Spin.</param>
        public SpinAttribute(float val) => this.val = val;
    }
}