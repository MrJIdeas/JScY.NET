using JScience.Physik.Enums;
using System;

namespace JScience.Physik.AttributesCustom
{
    /// <summary>
    /// Attribut-Klasse für Elementtypen.
    /// </summary>
    public class ElementKategorieAttribute : Attribute
    {
        /// <summary>
        /// Kategorie.
        /// </summary>
        public EElementkategorie Kategorie { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="val">Kategorie.</param>
        public ElementKategorieAttribute(EElementkategorie val) => Kategorie = val;
    }
}