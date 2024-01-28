using System;

namespace JScience.Physik.AttributesCustom
{
    internal class ElementSymbolAttribute : Attribute
    {
        public string Symbol { get; private set; }

        public ElementSymbolAttribute(string val)
        {
            Symbol = val;
        }
    }
}