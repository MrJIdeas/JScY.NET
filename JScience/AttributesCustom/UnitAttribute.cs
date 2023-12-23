using System;

namespace JScience.AttributesCustom
{
    public class UnitAttribute : Attribute
    {
        public string Name { get; private set; }

        public UnitAttribute(string UnitName)
        {
            Name = UnitName;
        }
    }
}