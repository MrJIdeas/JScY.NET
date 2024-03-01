using System;

namespace JScience.AttributesCustom
{
    /// <summary>
    /// Einheitenattribut-Klasse
    /// </summary>
    public class UnitAttribute : Attribute
    {
        /// <summary>
        /// Bezeichnung der Einheit.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="UnitName">Einheit.</param>
        public UnitAttribute(string UnitName) => Name = UnitName;
    }
}