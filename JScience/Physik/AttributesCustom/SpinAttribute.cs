using System;

namespace JScience.Physik.AttributesCustom
{
    public class SpinAttribute : Attribute
    {
        public float val { get; private set; }

        public SpinAttribute(float val)
        {
            this.val = val;
        }
    }
}