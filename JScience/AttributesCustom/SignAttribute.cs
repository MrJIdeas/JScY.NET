using System;

namespace JScience.AttributesCustom
{
    /// <summary>
    /// Attribut für Zeichen.
    /// </summary>
    public class SignAttribute : Attribute
    {
        /// <summary>
        /// Zeichen.
        /// </summary>
        public string Sign { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="sign">Zeichen.</param>
        public SignAttribute(string sign) => Sign = sign;
    }
}