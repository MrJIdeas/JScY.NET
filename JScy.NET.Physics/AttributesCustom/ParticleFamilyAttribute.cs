using System;
using JScy.NET.Physics.Enums;

namespace JScy.NET.Physics.AttributesCustom
{
    /// <summary>
    /// Attribut für Partikelfamilie.
    /// </summary>
    public class ParticleFamilyAttribute : Attribute
    {
        /// <summary>
        /// Partikelfamilie.
        /// </summary>
        public EParticleFamily Family { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="family">Elementfamilie.</param>
        public ParticleFamilyAttribute(EParticleFamily family) => Family = family;
    }
}