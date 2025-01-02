using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.AttributesCustom
{
    /// <summary>
    /// Attribut für Zeichen.
    /// </summary>
    public class MathSignAttribute : Attribute
    {
        /// <summary>
        /// Zeichen.
        /// </summary>
        public short Sign { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="sign">Zeichen.</param>
        public MathSignAttribute(short sign) => Sign = sign;
    }
}