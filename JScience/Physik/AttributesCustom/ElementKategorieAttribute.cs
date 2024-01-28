using JScience.Physik.Enums;
using System;

namespace JScience.Physik.AttributesCustom
{
    internal class ElementKategorieAttribute : Attribute
    {
        public EElementkategorie Kategorie { get; private set; }

        public ElementKategorieAttribute(EElementkategorie val)
        {
            Kategorie = val;
        }
    }
}