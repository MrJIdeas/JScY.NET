using JScience.Physik.Enums;
using System;

namespace JScience.Physik.AttributesCustom
{
    public class ParticleFamilyAttribute : Attribute
    {
        public EParticleFamily Family { get; private set; }

        public ParticleFamilyAttribute(EParticleFamily family)
        {
            Family = family;
        }
    }
}