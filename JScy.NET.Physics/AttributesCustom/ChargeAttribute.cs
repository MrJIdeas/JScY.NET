using System;

namespace JScy.NET.Physics.AttributesCustom
{
    /// <summary>
    /// Attribute for charges
    /// </summary>
    public class ChargeAttribute : Attribute
    {
        /// <summary>
        /// Charge.
        /// </summary>
        public float Charge { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Charge">Charge.</param>
        public ChargeAttribute(float Charge) => this.Charge = Charge;
    }
}