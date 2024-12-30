using System;

namespace JScy.NET.Physik.AttributesCustom
{
    /// <summary>
    /// Attribut für Elementsymbole.
    /// </summary>
    public class ElementSymbolAttribute : Attribute
    {
        /// <summary>
        /// Elementsymbol.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="val">Elementsymbol.</param>
        public ElementSymbolAttribute(string val) => Symbol = val;
    }
}