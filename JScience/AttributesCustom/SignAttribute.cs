using System;

namespace JScience.AttributesCustom
{
    public class SignAttribute : Attribute
    {
        public string Sign { get; private set; }

        public SignAttribute(string sign)
        {
            Sign = sign;
        }
    }
}