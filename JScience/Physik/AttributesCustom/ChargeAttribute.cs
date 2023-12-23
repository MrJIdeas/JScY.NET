using System;

namespace JScience.Physik.AttributesCustom
{
    public class ChargeAttribute : Attribute
    {
        public float Charge { get; private set; }

        public ChargeAttribute(float Charge)
        { this.Charge = Charge; }
    }
}