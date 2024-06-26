﻿using JScience.Physik.Enums;
using System;

namespace JScience.Physik.AttributesCustom
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